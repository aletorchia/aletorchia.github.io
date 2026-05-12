using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MealWise.Mobile.Models;
using MealWise.Mobile.Services;

namespace MealWise.Mobile.ViewModels;

public partial class ShoppingListViewModel : ObservableObject
{
    private readonly IMealPlanService mealPlanService;

    public ShoppingListViewModel(IMealPlanService mealPlanService)
    {
        this.mealPlanService = mealPlanService;
        _ = LoadAsync();
    }

    public ObservableCollection<ShoppingListItem> Items { get; } = new();

    [ObservableProperty]
    private string title = "Spesa";

    [ObservableProperty]
    private string newItemName = string.Empty;

    [ObservableProperty]
    private string newItemMeasure = string.Empty;

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
            ErrorMessage = "I dati locali della lista spesa non sono leggibili.";
            HasData = false;
            IsEmptyState = false;
        }
        catch (Exception)
        {
            ErrorMessage = "Non riesco a leggere la lista spesa locale.";
            HasData = false;
            IsEmptyState = false;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddManualItemAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            await mealPlanService.AddManualShoppingItemAsync(NewItemName, NewItemMeasure);
            NewItemName = string.Empty;
            NewItemMeasure = string.Empty;
            await RefreshCoreAsync();
        }
        catch (InvalidOperationException ex)
        {
            ErrorMessage = ex.Message;
            HasData = Items.Count > 0;
            IsEmptyState = Items.Count == 0;
        }
        catch (JsonException)
        {
            ErrorMessage = "Non riesco ad aggiornare la lista spesa locale.";
            HasData = false;
            IsEmptyState = false;
        }
        catch (Exception)
        {
            ErrorMessage = "Non riesco ad aggiornare la lista spesa locale.";
            HasData = false;
            IsEmptyState = false;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ToggleItemAsync(ShoppingListItem? item)
    {
        if (item is null || IsBusy)
        {
            return;
        }

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            await mealPlanService.ToggleShoppingItemAsync(item.Id);
            await RefreshCoreAsync();
        }
        catch (JsonException)
        {
            ErrorMessage = "Non riesco ad aggiornare lo stato della lista spesa.";
            HasData = false;
            IsEmptyState = false;
        }
        catch (Exception)
        {
            ErrorMessage = "Non riesco ad aggiornare lo stato della lista spesa.";
            HasData = false;
            IsEmptyState = false;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RemoveItemAsync(ShoppingListItem? item)
    {
        if (item is null || IsBusy)
        {
            return;
        }

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            await mealPlanService.RemoveShoppingItemAsync(item.Id);
            await RefreshCoreAsync();
        }
        catch (JsonException)
        {
            ErrorMessage = "Non riesco a rimuovere l'elemento dalla lista spesa.";
            HasData = false;
            IsEmptyState = false;
        }
        catch (Exception)
        {
            ErrorMessage = "Non riesco a rimuovere l'elemento dalla lista spesa.";
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
        var items = await mealPlanService.GetShoppingListAsync();

        Items.Clear();
        foreach (var item in items)
        {
            Items.Add(item);
        }

        HasData = Items.Count > 0;
        IsEmptyState = Items.Count == 0;
    }
}
