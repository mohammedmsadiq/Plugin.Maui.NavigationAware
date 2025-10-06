# Plugin.Maui.NavigationAware

[![NuGet](https://img.shields.io/nuget/v/Plugin.Maui.NavigationAware.svg)](https://www.nuget.org/packages/Plugin.Maui.NavigationAware/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

`Plugin.Maui.NavigationAware` provides navigation awareness for .NET MAUI applications, similar to Prism's INavigationAware interface. This plugin allows your pages to be notified when navigation events occur, enabling you to handle incoming and outgoing navigation with parameters.

## Features

- ✅ **Navigation Awareness**: Receive notifications when navigating to or from a page
- ✅ **Parameter Passing**: Pass and receive strongly-typed parameters during navigation
- ✅ **Prism-like API**: Familiar interface for developers coming from Prism
- ✅ **Easy Integration**: Simple base class or interface implementation
- ✅ **Cross-platform**: Works on all .NET MAUI supported platforms (iOS, Android, macOS, Windows)
- ✅ **Zero Dependencies**: No external dependencies beyond .NET MAUI

## Installation

Install the package via NuGet:

```bash
dotnet add package Plugin.Maui.NavigationAware
```

Or via the NuGet Package Manager:

```
Install-Package Plugin.Maui.NavigationAware
```

## Getting Started

### Basic Usage

There are two ways to use this plugin:

#### Option 1: Inherit from NavigationAwarePage

The easiest way to use navigation awareness is to inherit from `NavigationAwarePage`:

```csharp
using Plugin.Maui.NavigationAware;

public partial class MyPage : NavigationAwarePage
{
    public MyPage()
    {
        InitializeComponent();
    }

    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        // Handle incoming navigation
        
        // Access parameters
        if (parameters.TryGetValue<string>("message", out var message))
        {
            // Use the message
            Console.WriteLine($"Received: {message}");
        }
    }

    public override void OnNavigatedFrom(INavigationParameters parameters)
    {
        base.OnNavigatedFrom(parameters);
        // Handle outgoing navigation
    }
}
```

**XAML File:**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<local:NavigationAwarePage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:Plugin.Maui.NavigationAware;assembly=Plugin.Maui.NavigationAware"
             x:Class="YourNamespace.MyPage"
             Title="My Page">
    <!-- Your content here -->
</local:NavigationAwarePage>
```

#### Option 2: Implement INavigationAware

Alternatively, you can implement the `INavigationAware` interface on any `ContentPage`:

```csharp
using Plugin.Maui.NavigationAware;

public partial class MyPage : ContentPage, INavigationAware
{
    public MyPage()
    {
        InitializeComponent();
    }

    public void OnNavigatedTo(INavigationParameters parameters)
    {
        // Handle incoming navigation
    }

    public void OnNavigatedFrom(INavigationParameters parameters)
    {
        // Handle outgoing navigation
    }
}
```

### Navigation with Parameters

Use the `NavigationService` to navigate with parameters:

```csharp
private async void OnNavigateClicked(object sender, EventArgs e)
{
    // Get the navigation service from the current page
    var navigationService = this.GetNavigationService();
    
    // Create navigation parameters
    var parameters = new NavigationParameters
    {
        { "userId", 123 },
        { "message", "Hello from previous page!" },
        { "timestamp", DateTime.Now }
    };
    
    // Navigate to the next page
    await navigationService.NavigateToAsync(new DetailsPage(), parameters);
}
```

### Receiving Parameters

On the destination page, access the parameters in the `OnNavigatedTo` method:

```csharp
public override void OnNavigatedTo(INavigationParameters parameters)
{
    base.OnNavigatedTo(parameters);
    
    // Access strongly-typed parameters
    if (parameters.TryGetValue<int>("userId", out var userId))
    {
        LoadUserData(userId);
    }
    
    if (parameters.TryGetValue<string>("message", out var message))
    {
        DisplayMessage(message);
    }
    
    // Or use GetValue with default
    var timestamp = parameters.GetValue<DateTime>("timestamp");
}
```

### Going Back with Parameters

You can also pass parameters when navigating back:

```csharp
private async void OnGoBackClicked(object sender, EventArgs e)
{
    var navigationService = this.GetNavigationService();
    
    var parameters = new NavigationParameters
    {
        { "result", "Success" },
        { "data", someData }
    };
    
    await navigationService.GoBackAsync(parameters);
}
```

## API Reference

### Core Interfaces

#### INavigationAware

Interface for pages that need to be notified of navigation events.

```csharp
public interface INavigationAware
{
    void OnNavigatedTo(INavigationParameters parameters);
    void OnNavigatedFrom(INavigationParameters parameters);
}
```

#### INavigationParameters

Represents navigation parameters passed during navigation.

```csharp
public interface INavigationParameters : IDictionary<string, object>
{
    T? GetValue<T>(string key);
    bool TryGetValue<T>(string key, out T? value);
}
```

#### INavigationService

Service for performing navigation operations.

```csharp
public interface INavigationService
{
    Task NavigateToAsync(Page page, INavigationParameters? parameters = null);
    Task GoBackAsync(INavigationParameters? parameters = null);
}
```

### Base Classes

#### NavigationAwarePage

Base `ContentPage` implementation with navigation awareness built-in.

```csharp
public abstract class NavigationAwarePage : ContentPage, INavigationAware
{
    public virtual void OnNavigatedTo(INavigationParameters parameters);
    public virtual void OnNavigatedFrom(INavigationParameters parameters);
}
```

### Extension Methods

#### NavigationExtensions

```csharp
public static class NavigationExtensions
{
    // Get navigation service from a page
    public static INavigationService GetNavigationService(this Page page);
    
    // Register navigation service with DI (optional)
    public static IServiceCollection AddNavigationAware(this IServiceCollection services);
}
```

## Advanced Usage

### Dependency Injection

You can optionally register the navigation service with the DI container:

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

        // Register navigation service
        builder.Services.AddNavigationAware();
        
        // Register your pages
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<DetailsPage>();

        return builder.Build();
    }
}
```

Then inject it into your pages:

```csharp
public partial class MyPage : NavigationAwarePage
{
    private readonly INavigationService _navigationService;

    public MyPage(INavigationService navigationService)
    {
        InitializeComponent();
        _navigationService = navigationService;
    }
}
```

## Sample Application

The repository includes a sample application demonstrating all features of the plugin. Check out the [samples folder](samples/) to see it in action.

## Comparison with Prism

If you're familiar with Prism, this plugin provides similar functionality:

| Prism | Plugin.Maui.NavigationAware |
|-------|----------------------------|
| `INavigationAware` | `INavigationAware` |
| `INavigationParameters` | `INavigationParameters` |
| `NavigationParameters` | `NavigationParameters` |
| `OnNavigatedTo()` | `OnNavigatedTo()` |
| `OnNavigatedFrom()` | `OnNavigatedFrom()` |

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Inspired by [Prism Library](https://prismlibrary.com/) for Xamarin.Forms and .NET MAUI
- Built with ❤️ for the .NET MAUI community

## Support

If you encounter any issues or have questions, please [open an issue](https://github.com/mohammedmsadiq/Plugin.Maui.NavigationAware/issues) on GitHub.
