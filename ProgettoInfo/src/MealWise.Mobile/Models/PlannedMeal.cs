using System.Globalization;

namespace MealWise.Mobile.Models;

public sealed class PlannedMeal
{
    public string Id { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string MealId { get; set; } = string.Empty;

    public string RecipeName { get; set; } = string.Empty;

    public string? ThumbnailUrl { get; set; }

    public List<RecipeIngredient> Ingredients { get; set; } = new();

    public DateTime CreatedAtUtc { get; set; }

    public string DateText => Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

    public string IngredientSummary => Ingredients.Count == 1
        ? "1 ingrediente"
        : $"{Ingredients.Count} ingredienti";
}
