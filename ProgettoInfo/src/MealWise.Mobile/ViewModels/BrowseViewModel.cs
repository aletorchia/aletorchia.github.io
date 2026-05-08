using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MealWise.Mobile.Models;
using MealWise.Mobile.Services;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace MealWise.Mobile.ViewModels;

public partial class BrowseViewModel : ObservableObject
{
    private readonly IRecipeSearchService recipeSearchService;

    public BrowseViewModel(IRecipeSearchService recipeSearchService)
    {
        this.recipeSearchService = recipeSearchService;
        _ = LoadRandomAsync();
    }

    public ObservableCollection<RecipeSearchResult> RandomRecipes { get; } = new();

    [ObservableProperty]
    private string title = "Browse";

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string? errorMessage;

    [ObservableProperty]
    private bool hasData;

    [ObservableProperty]
    private bool isEmptyState;

    [ObservableProperty]
    private RecipeSearchResult? selectedRecipe;

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    [RelayCommand]
    private async Task LoadRandomAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        ErrorMessage = null;
        HasData = false;
        IsEmptyState = false;
        RandomRecipes.Clear();

        try
        {
            var recipes = await recipeSearchService.GetRandomRecipesAsync(8);
            foreach (var recipe in recipes)
            {
                RandomRecipes.Add(recipe);
            }

            HasData = RandomRecipes.Count > 0;
            IsEmptyState = RandomRecipes.Count == 0;
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Non riesco a caricare ricette casuali. Controlla la connessione e riprova.";
            HasData = false;
            IsEmptyState = false;
        }
        catch (JsonException)
        {
            ErrorMessage = "Il servizio ha restituito dati non validi. Riprova piu' tardi.";
            HasData = false;
            IsEmptyState = false;
        }
        catch (TaskCanceledException)
        {
            ErrorMessage = "Il caricamento ha impiegato troppo tempo. Riprova.";
            HasData = false;
            IsEmptyState = false;
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnSelectedRecipeChanged(RecipeSearchResult? value)
    {
        if (value is null)
        {
            return;
        }

        _ = OpenRecipeAsync(value);
    }

    private async Task OpenRecipeAsync(RecipeSearchResult recipe)
    {
        try
        {
            var mealId = Uri.EscapeDataString(recipe.MealId);
            await Shell.Current.GoToAsync($"recipe-detail?mealId={mealId}");
        }
        finally
        {
            SelectedRecipe = null;
        }
    }
}
