using MealWise.Mobile.Models;

namespace MealWise.Mobile.Services;

public interface IRecipeSearchService
{
    Task<IReadOnlyList<RecipeSearchResult>> SearchAsync(
        string query,
        RecipeSearchMode mode,
        CancellationToken cancellationToken = default);

    Task<RecipeDetail?> GetDetailAsync(
        string mealId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RecipeSearchResult>> GetRandomRecipesAsync(
        int count,
        CancellationToken cancellationToken = default);
}
