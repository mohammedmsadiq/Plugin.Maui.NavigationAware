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
		builder.Services.AddTransient<ThirdPage>();
		
		// Register ViewModels
		builder.Services.AddTransient<MainPageViewModel>();
		builder.Services.AddTransient<SecondPageViewModel>();
		builder.Services.AddTransient<ThirdPageViewModel>();
		
		// Register pages for string-based navigation
		builder.Services.RegisterPage<MainPage>();
		builder.Services.RegisterPage<SecondPage>();
		builder.Services.RegisterPage<ThirdPage>();
		
		// Register ViewModels for automatic binding (optional - can use convention-based naming instead)
		// builder.Services.RegisterViewModel<MainPage, MainPageViewModel>();
		// builder.Services.RegisterViewModel<SecondPage, SecondPageViewModel>();
		// builder.Services.RegisterViewModel<ThirdPage, ThirdPageViewModel>();
		
		// Register navigation service (optional - can also use extension method)
		// builder.Services.AddNavigationAware();

		return builder.Build();
	}
}