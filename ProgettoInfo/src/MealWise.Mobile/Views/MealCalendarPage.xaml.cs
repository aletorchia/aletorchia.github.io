namespace MealWise.Mobile.Views;

public partial class MealCalendarPage : ContentPage
{
    public MealCalendarPage(ViewModels.MealCalendarViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
