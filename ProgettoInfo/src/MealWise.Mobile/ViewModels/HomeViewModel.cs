using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MealWise.Mobile.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = "Home";

    [RelayCommand]
    private Task GoToSearchAsync()
    {
        return Shell.Current.GoToAsync("//search");
    }

    [RelayCommand]
    private Task GoToBrowseAsync()
    {
        return Shell.Current.GoToAsync("//browse");
    }
}
