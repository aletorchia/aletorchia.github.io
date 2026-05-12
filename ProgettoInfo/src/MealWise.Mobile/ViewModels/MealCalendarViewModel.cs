using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MealWise.Mobile.Models;
using MealWise.Mobile.Services;

namespace MealWise.Mobile.ViewModels;

public partial class MealCalendarViewModel : ObservableObject
{
    private readonly IMealPlanService mealPlanService;

    public MealCalendarViewModel(IMealPlanService mealPlanService)
    {
        this.mealPlanService = mealPlanService;
        _ = LoadAsync();
    }

    public ObservableCollection<PlannedMeal> PlannedMeals { get; } = new();

    [ObservableProperty]
    private string title = "Calendario";

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
    private async Task LoadAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            await RefreshCoreAsync();
        }
        catch (JsonException)
        {
            ErrorMessage = "I dati locali del calendario non sono leggibili.";
            HasData = false;
            IsEmptyState = false;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RemoveMealAsync(PlannedMeal? meal)
    {
        if (meal is null || IsBusy)
        {
            return;
        }

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            await mealPlanService.RemoveMealAsync(meal.Id);
            await RefreshCoreAsync();
        }
        catch (JsonException)
        {
            ErrorMessage = "Non riesco ad aggiornare il calendario locale.";
            HasData = false;
            IsEmptyState = false;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task RefreshCoreAsync()
    {
        var meals = await mealPlanService.GetPlannedMealsAsync();

        PlannedMeals.Clear();
        foreach (var meal in meals)
        {
            PlannedMeals.Add(meal);
        }

        HasData = PlannedMeals.Count > 0;
        IsEmptyState = PlannedMeals.Count == 0;
    }
}
