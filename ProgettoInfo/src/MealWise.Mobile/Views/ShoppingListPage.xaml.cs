namespace MealWise.Mobile.Views;

public partial class ShoppingListPage : ContentPage
{
    public ShoppingListPage(ViewModels.ShoppingListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
