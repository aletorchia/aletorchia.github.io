using System.Text.Json;
using MealWise.Mobile.Models;
using Microsoft.Maui.Storage;
using SQLite;

namespace MealWise.Mobile.Services;

public sealed class MealPlanService : IMealPlanService
{
    private const string DatabaseFileName = "mealwise-local.db3";
    private const string PlannedMealsKey = "mealwise.plannedMeals.v1";
    private const string ShoppingStateKey = "mealwise.shoppingState.v1";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly string databasePath;
    private readonly SQLiteAsyncConnection database;
    private readonly SemaphoreSlim initSemaphore = new(1, 1);
    private bool isInitialized;

    public MealPlanService()
    {
        databasePath = Path.Combine(FileSystem.AppDataDirectory, DatabaseFileName);
        database = new SQLiteAsyncConnection(databasePath);
    }

    public async Task<IReadOnlyList<PlannedMeal>> GetPlannedMealsAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await EnsureInitializedAsync(cancellationToken);

        var rows = await database.Table<PlannedMealRow>().ToListAsync();
        var meals = rows
            .Select(ToPlannedMeal)
            .OrderBy(meal => meal.Date.Date)
            .ThenBy(meal => meal.RecipeName, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return meals;
    }

    public async Task AddMealAsync(
        DateTime date,
        RecipeDetail recipe,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await EnsureInitializedAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(recipe.MealId) || string.IsNullOrWhiteSpace(recipe.Name))
        {
            throw new InvalidOperationException("La ricetta non contiene dati sufficienti per il calendario.");
        }

        var normalizedDate = date.Date;
        var normalizedMealId = recipe.MealId.Trim();

        var duplicate = await database.Table<PlannedMealRow>()
            .Where(row => row.Date == normalizedDate.Ticks && row.MealId == normalizedMealId)
            .FirstOrDefaultAsync();

        if (duplicate is not null)
        {
            return;
        }

        var row = new PlannedMealRow
        {
            Id = Guid.NewGuid().ToString("N"),
            Date = normalizedDate.Ticks,
            MealId = normalizedMealId,
            RecipeName = recipe.Name.Trim(),
            ThumbnailUrl = string.IsNullOrWhiteSpace(recipe.ThumbnailUrl) ? null : recipe.ThumbnailUrl.Trim(),
            IngredientsJson = JsonSerializer.Serialize(recipe.Ingredients, JsonOptions),
            CreatedAtUtc = DateTime.UtcNow.Ticks
        };

        await database.InsertAsync(row);
    }

    public async Task RemoveMealAsync(string plannedMealId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await EnsureInitializedAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(plannedMealId))
        {
            return;
        }

        await database.DeleteAsync<PlannedMealRow>(plannedMealId);
    }

    public async Task<IReadOnlyList<ShoppingListItem>> GetShoppingListAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await EnsureInitializedAsync(cancellationToken);

        var meals = await GetPlannedMealsAsync(cancellationToken);
        var overrides = await database.Table<ShoppingOverrideRow>().ToListAsync();
        var manualRows = await database.Table<ManualShoppingItemRow>().ToListAsync();

        var state = new ShoppingState
        {
            Overrides = overrides
                .Select(row => new ShoppingOverride
                {
                    Id = row.Id,
                    IsChecked = row.IsChecked,
                    IsSuppressed = row.IsSuppressed
                })
                .ToList(),
            ManualItems = manualRows
                .Select(row => new ManualShoppingItem
                {
                    Id = row.Id,
                    Name = row.Name,
                    Measure = row.Measure,
                    IsChecked = row.IsChecked
                })
                .ToList()
        };

        var items = BuildDerivedItems(meals, state)
            .Concat(state.ManualItems.Select(ToShoppingListItem))
            .OrderBy(item => item.IsChecked)
            .ThenBy(item => item.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return items;
    }

    public async Task ToggleShoppingItemAsync(string itemId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await EnsureInitializedAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(itemId))
        {
            return;
        }

        if (itemId.StartsWith("manual:", StringComparison.OrdinalIgnoreCase))
        {
            var manualRow = await database.Table<ManualShoppingItemRow>()
                .Where(row => row.Id == itemId)
                .FirstOrDefaultAsync();

            if (manualRow is null)
            {
                return;
            }

            manualRow.IsChecked = !manualRow.IsChecked;
            await database.UpdateAsync(manualRow);
            return;
        }

        var overrideRow = await database.Table<ShoppingOverrideRow>()
            .Where(row => row.Id == itemId)
            .FirstOrDefaultAsync();

        if (overrideRow is null)
        {
            overrideRow = new ShoppingOverrideRow
            {
                Id = itemId,
                IsChecked = true,
                IsSuppressed = false
            };

            await database.InsertAsync(overrideRow);
            return;
        }

        overrideRow.IsChecked = !overrideRow.IsChecked;
        await database.UpdateAsync(overrideRow);
    }

    public async Task AddManualShoppingItemAsync(
        string name,
        string? measure,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await EnsureInitializedAsync(cancellationToken);

        var normalizedName = name.Trim();
        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            throw new InvalidOperationException("Inserisci un elemento da aggiungere alla lista spesa.");
        }

        var row = new ManualShoppingItemRow
        {
            Id = $"manual:{Guid.NewGuid():N}",
            Name = normalizedName,
            Measure = measure?.Trim() ?? string.Empty,
            IsChecked = false
        };

        await database.InsertAsync(row);
    }

    public async Task AddRecipeIngredientsToShoppingListAsync(
        RecipeDetail recipe,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await EnsureInitializedAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(recipe.MealId) || string.IsNullOrWhiteSpace(recipe.Name))
        {
            throw new InvalidOperationException("La ricetta non contiene dati sufficienti per la lista spesa.");
        }

        var ingredients = recipe.Ingredients
            .Where(ingredient => !string.IsNullOrWhiteSpace(ingredient.Name))
            .Select(ingredient => new
            {
                Name = ingredient.Name.Trim(),
                Measure = ingredient.Measure?.Trim() ?? string.Empty
            })
            .ToList();

        if (ingredients.Count == 0)
        {
            throw new InvalidOperationException("La ricetta non contiene ingredienti da aggiungere.");
        }

        var existingManualRows = await database.Table<ManualShoppingItemRow>().ToListAsync();

        foreach (var ingredient in ingredients)
        {
            var exists = existingManualRows.Any(row =>
                row.Name.Equals(ingredient.Name, StringComparison.OrdinalIgnoreCase)
                && row.Measure.Equals(ingredient.Measure, StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                continue;
            }

            var row = new ManualShoppingItemRow
            {
                Id = $"manual:{Guid.NewGuid():N}",
                Name = ingredient.Name,
                Measure = ingredient.Measure,
                IsChecked = false
            };

            await database.InsertAsync(row);
            existingManualRows.Add(row);
        }
    }

    public async Task RemoveShoppingItemAsync(string itemId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await EnsureInitializedAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(itemId))
        {
            return;
        }

        if (itemId.StartsWith("manual:", StringComparison.OrdinalIgnoreCase))
        {
            await database.DeleteAsync<ManualShoppingItemRow>(itemId);
            return;
        }

        var overrideRow = await database.Table<ShoppingOverrideRow>()
            .Where(row => row.Id == itemId)
            .FirstOrDefaultAsync();

        if (overrideRow is null)
        {
            overrideRow = new ShoppingOverrideRow { Id = itemId };
            await database.InsertAsync(overrideRow);
        }

        overrideRow.IsSuppressed = true;
        await database.UpdateAsync(overrideRow);
    }

    private async Task EnsureInitializedAsync(CancellationToken cancellationToken)
    {
        if (isInitialized)
        {
            return;
        }

        await initSemaphore.WaitAsync(cancellationToken);

        try
        {
            if (isInitialized)
            {
                return;
            }

            await database.CreateTableAsync<PlannedMealRow>();
            await database.CreateTableAsync<ShoppingOverrideRow>();
            await database.CreateTableAsync<ManualShoppingItemRow>();

            await MigrateFromPreferencesIfNeededAsync();

            isInitialized = true;
        }
        finally
        {
            initSemaphore.Release();
        }
    }

    private async Task MigrateFromPreferencesIfNeededAsync()
    {
        var existingRows = await database.Table<PlannedMealRow>().CountAsync();
        if (existingRows > 0)
        {
            return;
        }

        var legacyMealsJson = Preferences.Default.Get(PlannedMealsKey, string.Empty);
        var legacyStateJson = Preferences.Default.Get(ShoppingStateKey, string.Empty);

        if (!string.IsNullOrWhiteSpace(legacyMealsJson))
        {
            var meals = JsonSerializer.Deserialize<List<PlannedMeal>>(legacyMealsJson, JsonOptions) ?? new List<PlannedMeal>();
            foreach (var meal in meals)
            {
                var row = new PlannedMealRow
                {
                    Id = string.IsNullOrWhiteSpace(meal.Id) ? Guid.NewGuid().ToString("N") : meal.Id,
                    Date = meal.Date.Date.Ticks,
                    MealId = meal.MealId,
                    RecipeName = meal.RecipeName,
                    ThumbnailUrl = meal.ThumbnailUrl,
                    IngredientsJson = JsonSerializer.Serialize(meal.Ingredients, JsonOptions),
                    CreatedAtUtc = meal.CreatedAtUtc == default ? DateTime.UtcNow.Ticks : meal.CreatedAtUtc.Ticks
                };

                await database.InsertOrReplaceAsync(row);
            }
        }

        if (!string.IsNullOrWhiteSpace(legacyStateJson))
        {
            var state = JsonSerializer.Deserialize<ShoppingState>(legacyStateJson, JsonOptions) ?? new ShoppingState();

            foreach (var item in state.Overrides)
            {
                await database.InsertOrReplaceAsync(new ShoppingOverrideRow
                {
                    Id = item.Id,
                    IsChecked = item.IsChecked,
                    IsSuppressed = item.IsSuppressed
                });
            }

            foreach (var item in state.ManualItems)
            {
                await database.InsertOrReplaceAsync(new ManualShoppingItemRow
                {
                    Id = item.Id,
                    Name = item.Name,
                    Measure = item.Measure,
                    IsChecked = item.IsChecked
                });
            }
        }
    }

    private static PlannedMeal ToPlannedMeal(PlannedMealRow row)
    {
        return new PlannedMeal
        {
            Id = row.Id,
            Date = new DateTime(row.Date, DateTimeKind.Unspecified),
            MealId = row.MealId,
            RecipeName = row.RecipeName,
            ThumbnailUrl = row.ThumbnailUrl,
            Ingredients = JsonSerializer.Deserialize<List<RecipeIngredient>>(row.IngredientsJson, JsonOptions)
                ?? new List<RecipeIngredient>(),
            CreatedAtUtc = new DateTime(row.CreatedAtUtc, DateTimeKind.Utc)
        };
    }

    private static IEnumerable<ShoppingListItem> BuildDerivedItems(
        IReadOnlyList<PlannedMeal> meals,
        ShoppingState state)
    {
        return meals
            .SelectMany(meal => meal.Ingredients.Select(ingredient => new
            {
                Ingredient = ingredient,
                meal.Id
            }))
            .Where(entry => !string.IsNullOrWhiteSpace(entry.Ingredient.Name))
            .GroupBy(entry => NormalizeIngredient(entry.Ingredient.Name))
            .Where(group => !string.IsNullOrWhiteSpace(group.Key))
            .Select(group =>
            {
                var id = $"derived:{group.Key}";
                var overrideItem = state.Overrides.FirstOrDefault(item =>
                    item.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

                if (overrideItem?.IsSuppressed == true)
                {
                    return null;
                }

                var names = group
                    .Select(entry => entry.Ingredient.Name.Trim())
                    .Where(value => !string.IsNullOrWhiteSpace(value))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                var measures = group
                    .Select(entry => entry.Ingredient.Measure.Trim())
                    .Where(value => !string.IsNullOrWhiteSpace(value))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                return new ShoppingListItem
                {
                    Id = id,
                    Name = names.FirstOrDefault() ?? group.Key,
                    Measure = string.Join(", ", measures),
                    IsChecked = overrideItem?.IsChecked == true,
                    IsManual = false,
                    SourceCount = group.Select(entry => entry.Id).Distinct().Count()
                };
            })
            .Where(item => item is not null)
            .Select(item => item!);
    }

    private static ShoppingListItem ToShoppingListItem(ManualShoppingItem item)
    {
        return new ShoppingListItem
        {
            Id = item.Id,
            Name = item.Name,
            Measure = item.Measure,
            IsChecked = item.IsChecked,
            IsManual = true,
            SourceCount = 0
        };
    }

    private static string NormalizeIngredient(string value)
    {
        return value.Trim().ToLowerInvariant();
    }

    [Table("planned_meals")]
    private sealed class PlannedMealRow
    {
        [PrimaryKey]
        [Column("id")]
        public string Id { get; set; } = string.Empty;

        [Indexed]
        [Column("date_ticks")]
        public long Date { get; set; }

        [Column("meal_id")]
        public string MealId { get; set; } = string.Empty;

        [Column("recipe_name")]
        public string RecipeName { get; set; } = string.Empty;

        [Column("thumbnail_url")]
        public string? ThumbnailUrl { get; set; }

        [Column("ingredients_json")]
        public string IngredientsJson { get; set; } = "[]";

        [Column("created_at_utc_ticks")]
        public long CreatedAtUtc { get; set; }
    }

    [Table("shopping_overrides")]
    private sealed class ShoppingOverrideRow
    {
        [PrimaryKey]
        [Column("id")]
        public string Id { get; set; } = string.Empty;

        [Column("is_checked")]
        public bool IsChecked { get; set; }

        [Column("is_suppressed")]
        public bool IsSuppressed { get; set; }
    }

    [Table("manual_shopping_items")]
    private sealed class ManualShoppingItemRow
    {
        [PrimaryKey]
        [Column("id")]
        public string Id { get; set; } = string.Empty;

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("measure")]
        public string Measure { get; set; } = string.Empty;

        [Column("is_checked")]
        public bool IsChecked { get; set; }
    }

    private sealed class ShoppingState
    {
        public List<ShoppingOverride> Overrides { get; set; } = new();

        public List<ManualShoppingItem> ManualItems { get; set; } = new();
    }

    private sealed class ShoppingOverride
    {
        public string Id { get; set; } = string.Empty;

        public bool IsChecked { get; set; }

        public bool IsSuppressed { get; set; }
    }

    private sealed class ManualShoppingItem
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Measure { get; set; } = string.Empty;

        public bool IsChecked { get; set; }
    }
}
