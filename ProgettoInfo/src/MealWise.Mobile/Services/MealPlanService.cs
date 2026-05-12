using System.Text.Json;
using MealWise.Mobile.Models;
using Microsoft.Maui.Storage;

namespace MealWise.Mobile.Services;

public sealed class MealPlanService : IMealPlanService
{
    private const string PlannedMealsKey = "mealwise.plannedMeals.v1";
    private const string ShoppingStateKey = "mealwise.shoppingState.v1";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public Task<IReadOnlyList<PlannedMeal>> GetPlannedMealsAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var meals = LoadPlannedMeals()
            .OrderBy(meal => meal.Date.Date)
            .ThenBy(meal => meal.RecipeName, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return Task.FromResult<IReadOnlyList<PlannedMeal>>(meals);
    }

    public Task AddMealAsync(
        DateTime date,
        RecipeDetail recipe,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(recipe.MealId) || string.IsNullOrWhiteSpace(recipe.Name))
        {
            throw new InvalidOperationException("La ricetta non contiene dati sufficienti per il calendario.");
        }

        var meals = LoadPlannedMeals();
        var normalizedDate = date.Date;
        var duplicate = meals.Any(meal =>
            meal.Date.Date == normalizedDate
            && meal.MealId.Equals(recipe.MealId, StringComparison.OrdinalIgnoreCase));

        if (duplicate)
        {
            return Task.CompletedTask;
        }

        meals.Add(new PlannedMeal
        {
            Id = Guid.NewGuid().ToString("N"),
            Date = normalizedDate,
            MealId = recipe.MealId.Trim(),
            RecipeName = recipe.Name.Trim(),
            ThumbnailUrl = string.IsNullOrWhiteSpace(recipe.ThumbnailUrl) ? null : recipe.ThumbnailUrl.Trim(),
            Ingredients = recipe.Ingredients.ToList(),
            CreatedAtUtc = DateTime.UtcNow
        });

        SavePlannedMeals(meals);
        return Task.CompletedTask;
    }

    public Task RemoveMealAsync(string plannedMealId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(plannedMealId))
        {
            return Task.CompletedTask;
        }

        var meals = LoadPlannedMeals();
        meals.RemoveAll(meal => meal.Id.Equals(plannedMealId, StringComparison.OrdinalIgnoreCase));
        SavePlannedMeals(meals);

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<ShoppingListItem>> GetShoppingListAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var meals = LoadPlannedMeals();
        var state = LoadShoppingState();
        var items = BuildDerivedItems(meals, state)
            .Concat(state.ManualItems.Select(ToShoppingListItem))
            .OrderBy(item => item.IsChecked)
            .ThenBy(item => item.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return Task.FromResult<IReadOnlyList<ShoppingListItem>>(items);
    }

    public Task ToggleShoppingItemAsync(string itemId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(itemId))
        {
            return Task.CompletedTask;
        }

        var state = LoadShoppingState();
        var manualItem = state.ManualItems.FirstOrDefault(item =>
            item.Id.Equals(itemId, StringComparison.OrdinalIgnoreCase));

        if (manualItem is not null)
        {
            manualItem.IsChecked = !manualItem.IsChecked;
            SaveShoppingState(state);
            return Task.CompletedTask;
        }

        var overrideItem = GetOrCreateOverride(state, itemId);
        overrideItem.IsChecked = !overrideItem.IsChecked;
        SaveShoppingState(state);

        return Task.CompletedTask;
    }

    public Task AddManualShoppingItemAsync(
        string name,
        string? measure,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var normalizedName = name.Trim();
        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            throw new InvalidOperationException("Inserisci un elemento da aggiungere alla lista spesa.");
        }

        var state = LoadShoppingState();
        state.ManualItems.Add(new ManualShoppingItem
        {
            Id = $"manual:{Guid.NewGuid():N}",
            Name = normalizedName,
            Measure = measure?.Trim() ?? string.Empty,
            IsChecked = false
        });

        SaveShoppingState(state);
        return Task.CompletedTask;
    }

    public Task RemoveShoppingItemAsync(string itemId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(itemId))
        {
            return Task.CompletedTask;
        }

        var state = LoadShoppingState();
        var removedManualItems = state.ManualItems.RemoveAll(item =>
            item.Id.Equals(itemId, StringComparison.OrdinalIgnoreCase));

        if (removedManualItems == 0)
        {
            var overrideItem = GetOrCreateOverride(state, itemId);
            overrideItem.IsSuppressed = true;
        }

        SaveShoppingState(state);
        return Task.CompletedTask;
    }

    private static List<PlannedMeal> LoadPlannedMeals()
    {
        var json = Preferences.Default.Get(PlannedMealsKey, string.Empty);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<PlannedMeal>();
        }

        return JsonSerializer.Deserialize<List<PlannedMeal>>(json, JsonOptions) ?? new List<PlannedMeal>();
    }

    private static void SavePlannedMeals(List<PlannedMeal> meals)
    {
        Preferences.Default.Set(PlannedMealsKey, JsonSerializer.Serialize(meals, JsonOptions));
    }

    private static ShoppingState LoadShoppingState()
    {
        var json = Preferences.Default.Get(ShoppingStateKey, string.Empty);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new ShoppingState();
        }

        return JsonSerializer.Deserialize<ShoppingState>(json, JsonOptions) ?? new ShoppingState();
    }

    private static void SaveShoppingState(ShoppingState state)
    {
        Preferences.Default.Set(ShoppingStateKey, JsonSerializer.Serialize(state, JsonOptions));
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

    private static ShoppingOverride GetOrCreateOverride(ShoppingState state, string itemId)
    {
        var overrideItem = state.Overrides.FirstOrDefault(item =>
            item.Id.Equals(itemId, StringComparison.OrdinalIgnoreCase));

        if (overrideItem is not null)
        {
            return overrideItem;
        }

        overrideItem = new ShoppingOverride { Id = itemId };
        state.Overrides.Add(overrideItem);
        return overrideItem;
    }

    private static string NormalizeIngredient(string value)
    {
        return value.Trim().ToLowerInvariant();
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
