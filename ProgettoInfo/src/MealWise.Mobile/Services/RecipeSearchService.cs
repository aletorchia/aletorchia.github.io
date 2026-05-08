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
            .Select(meal => new RecipeSearchResult(
                meal.IdMeal!.Trim(),
                meal.StrMeal!.Trim(),
                string.IsNullOrWhiteSpace(meal.StrMealThumb) ? null : meal.StrMealThumb.Trim()))
            .ToList();
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
