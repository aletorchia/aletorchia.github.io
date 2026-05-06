using MealWise.Mobile.ViewModels;
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
		builder.Services.AddTransient<SearchViewModel>();
		builder.Services.AddTransient<BrowseViewModel>();
		builder.Services.AddTransient<SearchPage>();
		builder.Services.AddTransient<BrowsePage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
