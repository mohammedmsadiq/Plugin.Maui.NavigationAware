using Microsoft.Extensions.DependencyInjection;
using Plugin.Maui.NavigationAware;

namespace Plugin.Maui.NavigationAware.Sample;

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

		// Register pages
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<SecondPage>();
		
		// Register navigation service (optional - can also use extension method)
		// builder.Services.AddNavigationAware();

		return builder.Build();
	}
}