using MealWise.Mobile.ViewModels;
using MealWise.Mobile.Services;
using MealWise.Mobile.Views;
using Microsoft.Extensions.Logging;

namespace MealWise.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<AppShell>();
		builder.Services.AddSingleton(_ => new HttpClient
		{
			BaseAddress = new Uri("https://www.themealdb.com/api/json/v1/1/"),
			Timeout = TimeSpan.FromSeconds(15)
		});
		builder.Services.AddTransient<IRecipeSearchService, RecipeSearchService>();
		builder.Services.AddTransient<HomeViewModel>();
		builder.Services.AddTransient<SearchViewModel>();
		builder.Services.AddTransient<BrowseViewModel>();
		builder.Services.AddTransient<RecipeDetailViewModel>();
		builder.Services.AddTransient<HomePage>();
		builder.Services.AddTransient<SearchPage>();
		builder.Services.AddTransient<BrowsePage>();
		builder.Services.AddTransient<RecipeDetailPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
