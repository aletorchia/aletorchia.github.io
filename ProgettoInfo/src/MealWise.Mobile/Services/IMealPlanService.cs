using MealWise.Mobile.Models;

namespace MealWise.Mobile.Services;

public interface IMealPlanService
{
    Task<IReadOnlyList<PlannedMeal>> GetPlannedMealsAsync(CancellationToken cancellationToken = default);

    Task AddMealAsync(DateTime date, RecipeDetail recipe, CancellationToken cancellationToken = default);

    Task RemoveMealAsync(string plannedMealId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ShoppingListItem>> GetShoppingListAsync(CancellationToken cancellationToken = default);

    Task ToggleShoppingItemAsync(string itemId, CancellationToken cancellationToken = default);

    Task AddManualShoppingItemAsync(string name, string? measure, CancellationToken cancellationToken = default);

    Task AddRecipeIngredientsToShoppingListAsync(RecipeDetail recipe, CancellationToken cancellationToken = default);

    Task RemoveShoppingItemAsync(string itemId, CancellationToken cancellationToken = default);
}
