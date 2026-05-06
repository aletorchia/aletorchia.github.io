namespace MealWise.Mobile.Views;

public partial class BrowsePage : ContentPage
{
    public BrowsePage(ViewModels.BrowseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
