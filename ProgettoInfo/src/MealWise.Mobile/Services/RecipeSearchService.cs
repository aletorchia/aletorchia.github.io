using System.Text.Json;
using System.Text.Json.Serialization;
using MealWise.Mobile.Models;

namespace MealWise.Mobile.Services;

public sealed class RecipeSearchService : IRecipeSearchService
{
    private readonly HttpClient httpClient;
    private readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web);

    public RecipeSearchService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IReadOnlyList<RecipeSearchResult>> SearchAsync(
        string query,
        RecipeSearchMode mode,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return Array.Empty<RecipeSearchResult>();
        }

        var parameter = mode == RecipeSearchMode.Ingredient ? "i" : "c";
        var endpoint = $"filter.php?{parameter}={Uri.EscapeDataString(query.Trim())}";

        using var response = await httpClient.GetAsync(endpoint, cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var payload = await JsonSerializer.DeserializeAsync<MealDbSearchResponse>(
            stream,
            jsonOptions,
            cancellationToken);

        if (payload?.Meals is null)
        {
            return Array.Empty<RecipeSearchResult>();
        }

        return payload.Meals
            .Where(meal => !string.IsNullOrWhiteSpace(meal.IdMeal)
                && !string.IsNullOrWhiteSpace(meal.StrMeal))
            .Select(ToSearchResult)
            .ToList();
    }

    public async Task<RecipeDetail?> GetDetailAsync(
        string mealId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(mealId))
        {
            return null;
        }

        var endpoint = $"lookup.php?i={Uri.EscapeDataString(mealId.Trim())}";
        using var response = await httpClient.GetAsync(endpoint, cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

        if (!document.RootElement.TryGetProperty("meals", out var meals)
            || meals.ValueKind != JsonValueKind.Array
            || meals.GetArrayLength() == 0)
        {
            return null;
        }

        var meal = meals[0];
        var id = GetString(meal, "idMeal");
        var name = GetString(meal, "strMeal");

        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var ingredients = new List<RecipeIngredient>();
        for (var index = 1; index <= 20; index++)
        {
            var ingredient = GetString(meal, $"strIngredient{index}");
            if (string.IsNullOrWhiteSpace(ingredient))
            {
                continue;
            }

            var measure = GetString(meal, $"strMeasure{index}") ?? string.Empty;
            ingredients.Add(new RecipeIngredient(ingredient.Trim(), measure.Trim()));
        }

        return new RecipeDetail(
            id.Trim(),
            name.Trim(),
            GetString(meal, "strMealThumb"),
            GetString(meal, "strCategory"),
            GetString(meal, "strArea"),
            GetString(meal, "strInstructions"),
            ingredients);
    }

    public async Task<IReadOnlyList<RecipeSearchResult>> GetRandomRecipesAsync(
        int count,
        CancellationToken cancellationToken = default)
    {
        var requestedCount = Math.Clamp(count, 1, 12);
        var results = new Dictionary<string, RecipeSearchResult>();

        for (var attempt = 0; attempt < requestedCount * 2 && results.Count < requestedCount; attempt++)
        {
            using var response = await httpClient.GetAsync("random.php", cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var payload = await JsonSerializer.DeserializeAsync<MealDbSearchResponse>(
                stream,
                jsonOptions,
                cancellationToken);

            var item = payload?.Meals?.FirstOrDefault();
            if (item is null || string.IsNullOrWhiteSpace(item.IdMeal) || string.IsNullOrWhiteSpace(item.StrMeal))
            {
                continue;
            }

            results.TryAdd(item.IdMeal.Trim(), ToSearchResult(item));
        }

        return results.Values.ToList();
    }

    private static RecipeSearchResult ToSearchResult(MealDbSearchItem meal)
    {
        return new RecipeSearchResult(
            meal.IdMeal!.Trim(),
            meal.StrMeal!.Trim(),
            string.IsNullOrWhiteSpace(meal.StrMealThumb) ? null : meal.StrMealThumb.Trim());
    }

    private static string? GetString(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property)
            || property.ValueKind == JsonValueKind.Null
            || property.ValueKind == JsonValueKind.Undefined)
        {
            return null;
        }

        return property.GetString();
    }

    private sealed class MealDbSearchResponse
    {
        [JsonPropertyName("meals")]
        public List<MealDbSearchItem>? Meals { get; set; }
    }

    private sealed class MealDbSearchItem
    {
        [JsonPropertyName("idMeal")]
        public string? IdMeal { get; set; }

        [JsonPropertyName("strMeal")]
        public string? StrMeal { get; set; }

        [JsonPropertyName("strMealThumb")]
        public string? StrMealThumb { get; set; }
    }
}
