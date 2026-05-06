using CommunityToolkit.Mvvm.ComponentModel;

namespace MealWise.Mobile.ViewModels;

public partial class SearchViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = "Search";
}
