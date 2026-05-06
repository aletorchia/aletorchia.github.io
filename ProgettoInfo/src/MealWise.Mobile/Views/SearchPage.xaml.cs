namespace MealWise.Mobile.Views;

public partial class SearchPage : ContentPage
{
    public SearchPage(ViewModels.SearchViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
