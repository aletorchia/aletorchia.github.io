namespace MealWise.Mobile.Models;

public sealed record RecipeDetail(
    string MealId,
    string Name,
    string? ThumbnailUrl,
    string? Category,
    string? Area,
    string? Instructions,
    IReadOnlyList<RecipeIngredient> Ingredients);
