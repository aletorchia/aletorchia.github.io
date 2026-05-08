using MealWise.Mobile.Models;

namespace MealWise.Mobile.Services;

public interface IRecipeSearchService
{
    Task<IReadOnlyList<RecipeSearchResult>> SearchAsync(
        string query,
        RecipeSearchMode mode,
        CancellationToken cancellationToken = default);
}
