namespace MealWise.Mobile;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("recipe-detail", typeof(Views.RecipeDetailPage));
	}
}
