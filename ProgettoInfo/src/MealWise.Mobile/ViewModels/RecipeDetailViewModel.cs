using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MealWise.Mobile.Models;
using MealWise.Mobile.Services;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace MealWise.Mobile.ViewModels;

public partial class RecipeDetailViewModel : ObservableObject, IQueryAttributable
{
    private readonly IRecipeSearchService recipeSearchService;
    private string mealId = string.Empty;

    public RecipeDetailViewModel(IRecipeSearchService recipeSearchService)
    {
        this.recipeSearchService = recipeSearchService;
    }

    public ObservableCollection<RecipeIngredient> Ingredients { get; } = new();

    [ObservableProperty]
    private string title = "Dettaglio";

    [ObservableProperty]
    private string? mealName;

    [ObservableProperty]
    private string? thumbnailUrl;

    [ObservableProperty]
    private string? category;

    [ObservableProperty]
    private string? area;

    [ObservableProperty]
    private string? instructions;

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

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue("mealId", out var value))
        {
            ErrorMessage = "Ricetta non valida.";
            return;
        }

        mealId = Uri.UnescapeDataString(value?.ToString() ?? string.Empty);
        LoadCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(mealId))
        {
            ErrorMessage = "Ricetta non valida.";
            HasData = false;
            IsEmptyState = false;
            return;
        }

        IsBusy = true;
        ErrorMessage = null;
        HasData = false;
        IsEmptyState = false;
        Ingredients.Clear();

        try
        {
            var detail = await recipeSearchService.GetDetailAsync(mealId);
            if (detail is null)
            {
                MealName = null;
                ThumbnailUrl = null;
                Category = null;
                Area = null;
                Instructions = null;
                IsEmptyState = true;
                return;
            }

            Title = detail.Name;
            MealName = detail.Name;
            ThumbnailUrl = detail.ThumbnailUrl;
            Category = detail.Category;
            Area = detail.Area;
            Instructions = detail.Instructions;

            foreach (var ingredient in detail.Ingredients)
            {
                Ingredients.Add(ingredient);
            }

            HasData = true;
            IsEmptyState = false;
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Non riesco a caricare il dettaglio. Controlla la connessione e riprova.";
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
}
