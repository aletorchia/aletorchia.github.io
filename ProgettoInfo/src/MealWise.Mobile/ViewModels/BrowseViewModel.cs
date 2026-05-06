using CommunityToolkit.Mvvm.ComponentModel;

namespace MealWise.Mobile.ViewModels;

public partial class BrowseViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = "Browse";
}
