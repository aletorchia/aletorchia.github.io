namespace MealWise.Mobile.Views;

public partial class RecipeDetailPage : ContentPage, IQueryAttributable
{
    public RecipeDetailPage(ViewModels.RecipeDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is IQueryAttributable queryTarget)
        {
            queryTarget.ApplyQueryAttributes(query);
        }
    }
}
