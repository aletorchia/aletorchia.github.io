namespace MealWise.Mobile.Models;

public sealed record RecipeSearchResult(
    string MealId,
    string Name,
    string? ThumbnailUrl);
