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
    private readonly IMealPlanService mealPlanService;
    private string mealId = string.Empty;
    private RecipeDetail? currentDetail;

    public RecipeDetailViewModel(
        IRecipeSearchService recipeSearchService,
        IMealPlanService mealPlanService)
    {
        this.recipeSearchService = recipeSearchService;
        this.mealPlanService = mealPlanService;
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

    [ObservableProperty]
    private bool isIngredientListEmpty;

    [ObservableProperty]
    private DateTime selectedPlanDate = DateTime.Today;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasPlanningMessage))]
    private string? planningMessage;

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    public bool HasPlanningMessage => !string.IsNullOrWhiteSpace(PlanningMessage);

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue("mealId", out var value))
        {
            ClearDetail();
            HasData = false;
            IsEmptyState = false;
            ErrorMessage = "Ricetta non valida.";
            return;
        }

        mealId = Uri.UnescapeDataString(value?.ToString() ?? string.Empty);
        _ = LoadCommand.ExecuteAsync(null);
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
            ClearDetail();
            ErrorMessage = "Ricetta non valida.";
            HasData = false;
            IsEmptyState = false;
            return;
        }

        IsBusy = true;
        ErrorMessage = null;
        HasData = false;
        IsEmptyState = false;
        ClearDetail();

        try
        {
            var detail = await recipeSearchService.GetDetailAsync(mealId);
            if (detail is null)
            {
                IsEmptyState = true;
                return;
            }

            currentDetail = detail;
            Title = detail.Name;
            MealName = detail.Name;
            ThumbnailUrl = detail.ThumbnailUrl;
            Category = string.IsNullOrWhiteSpace(detail.Category) ? "Non indicata" : detail.Category.Trim();
            Area = string.IsNullOrWhiteSpace(detail.Area) ? "Non indicata" : detail.Area.Trim();
            Instructions = string.IsNullOrWhiteSpace(detail.Instructions)
                ? "Istruzioni non disponibili."
                : detail.Instructions.Trim();

            foreach (var ingredient in detail.Ingredients)
            {
                Ingredients.Add(ingredient);
            }

            IsIngredientListEmpty = Ingredients.Count == 0;
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

    [RelayCommand]
    private async Task AddToCalendarAsync()
    {
        if (IsBusy)
        {
            return;
        }

        if (currentDetail is null)
        {
            PlanningMessage = "Carica il dettaglio prima di aggiungere la ricetta al calendario.";
            return;
        }

        IsBusy = true;
        PlanningMessage = null;

        try
        {
            await mealPlanService.AddMealAsync(SelectedPlanDate, currentDetail);
            PlanningMessage = $"Ricetta aggiunta al calendario per il {SelectedPlanDate:dd/MM/yyyy}.";
        }
        catch (InvalidOperationException ex)
        {
            PlanningMessage = ex.Message;
        }
        catch (JsonException)
        {
            PlanningMessage = "Non riesco ad aggiornare il calendario locale.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ClearDetail()
    {
        currentDetail = null;
        Title = "Dettaglio";
        MealName = null;
        ThumbnailUrl = null;
        Category = null;
        Area = null;
        Instructions = null;
        IsIngredientListEmpty = false;
        PlanningMessage = null;
        Ingredients.Clear();
    }
}
