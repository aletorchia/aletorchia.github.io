using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MealWise.Mobile.Models;
using MealWise.Mobile.Services;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace MealWise.Mobile.ViewModels;

public partial class SearchViewModel : ObservableObject
{
    private readonly IRecipeSearchService recipeSearchService;
    private string lastQuery = string.Empty;
    private string lastSearchMode = "Ingrediente";

    public SearchViewModel(IRecipeSearchService recipeSearchService)
    {
        this.recipeSearchService = recipeSearchService;
    }

    public IReadOnlyList<string> SearchModes { get; } = new[]
    {
        "Ingrediente",
        "Categoria"
    };

    public ObservableCollection<RecipeSearchResult> Results { get; } = new();

    [ObservableProperty]
    private string title = "Search";

    [ObservableProperty]
    private string query = string.Empty;

    [ObservableProperty]
    private string selectedSearchMode = "Ingrediente";

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string? errorMessage;

    [ObservableProperty]
    private bool hasData;

    [ObservableProperty]
    private bool isEmptyState;

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    [RelayCommand]
    private Task SearchAsync()
    {
        return SearchCoreAsync(Query, SelectedSearchMode);
    }

    [RelayCommand]
    private Task RetryAsync()
    {
        return SearchCoreAsync(lastQuery, lastSearchMode);
    }

    private async Task SearchCoreAsync(string? query, string? searchMode)
    {
        if (IsBusy)
        {
            return;
        }

        var normalizedQuery = query?.Trim() ?? string.Empty;
        var normalizedMode = string.IsNullOrWhiteSpace(searchMode)
            ? "Ingrediente"
            : searchMode.Trim();

        Results.Clear();
        HasData = false;
        IsEmptyState = false;
        ErrorMessage = null;

        if (string.IsNullOrWhiteSpace(normalizedQuery))
        {
            ErrorMessage = "Inserisci un ingrediente o una categoria.";
            return;
        }

        IsBusy = true;
        lastQuery = normalizedQuery;
        lastSearchMode = normalizedMode;

        try
        {
            var mode = normalizedMode.Equals("Categoria", StringComparison.OrdinalIgnoreCase)
                ? RecipeSearchMode.Category
                : RecipeSearchMode.Ingredient;

            var results = await recipeSearchService.SearchAsync(normalizedQuery, mode);

            foreach (var result in results)
            {
                Results.Add(result);
            }

            HasData = Results.Count > 0;
            IsEmptyState = Results.Count == 0;
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Servizio ricette non raggiungibile. Controlla la connessione e riprova.";
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
            ErrorMessage = "La ricerca ha impiegato troppo tempo. Riprova.";
            HasData = false;
            IsEmptyState = false;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
