# Plugin.Maui.NavigationAware

[![NuGet](https://img.shields.io/nuget/v/Plugin.Maui.NavigationAware.svg)](https://www.nuget.org/packages/Plugin.Maui.NavigationAware/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

`Plugin.Maui.NavigationAware` provides navigation awareness for .NET MAUI applications, similar to Prism's INavigationAware interface. This plugin allows your pages to be notified when navigation events occur, enabling you to handle incoming and outgoing navigation with parameters.

## Features

- ✅ **Navigation Awareness**: Receive notifications when navigating to or from a page
- ✅ **Parameter Passing**: Pass and receive strongly-typed parameters during navigation
- ✅ **ViewModel Locator**: Automatic View-ViewModel binding with convention-based or explicit registration
- ✅ **String-Based Navigation**: Navigate using page names/keys like Prism (e.g., `NavigateToAsync("PageName")`)
- ✅ **URI-Based Navigation**: Navigate using URI paths (e.g., `NavigateAsync("/NavigationPage/DetailPage")`)
- ✅ **Instance-Based Navigation**: Navigate using page instances (e.g., `NavigateToAsync(new MyPage())`)
- ✅ **Navigation Stack Control**: Navigate back to specific pages or root with `GoBackToAsync()` and `GoBackToRootAsync()`
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
    
    // Option 1: Navigate using page instance
    await navigationService.NavigateToAsync(new DetailsPage(), parameters);
    
    // Option 2: Navigate using string (requires page registration - see below)
    await navigationService.NavigateToAsync("DetailsPage", parameters);
    // or using nameof for type safety:
    await navigationService.NavigateToAsync(nameof(DetailsPage), parameters);
}
```

### String-Based Navigation (Optional)

For developers migrating from Prism or preferring string-based navigation, you can register pages in your `MauiProgram.cs`:

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // Register pages for string-based navigation
        builder.Services.RegisterPage<MainPage>();
        builder.Services.RegisterPage<DetailsPage>();
        
        // You can also use custom keys:
        builder.Services.RegisterPage<DetailsPage>("CustomDetailsKey");

        return builder.Build();
    }
}
```

Now you can navigate using strings:
```csharp
await navigationService.NavigateToAsync("DetailsPage", parameters);
await navigationService.NavigateToAsync(nameof(DetailsPage), parameters);
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

### ViewModel Locator

The plugin includes a powerful ViewModel Locator that automates the binding between Views and ViewModels, reducing boilerplate code and making it easier to maintain the MVVM pattern.

#### Automatic ViewModel Binding in XAML

Use the `AutoWireViewModel` attached property to automatically bind a ViewModel to your View:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<local:NavigationAwarePage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:Plugin.Maui.NavigationAware;assembly=Plugin.Maui.NavigationAware"
             xmlns:nav="clr-namespace:Plugin.Maui.NavigationAware;assembly=Plugin.Maui.NavigationAware"
             x:Class="MyApp.MainPage"
             nav:ViewModelLocator.AutoWireViewModel="True"
             Title="Main Page">
    <!-- Your content here -->
</local:NavigationAwarePage>
```

The ViewModel Locator will automatically find and create a ViewModel based on naming conventions:
- `MainPage` → `MainPageViewModel`
- `DetailsPage` → `DetailsPageViewModel`
- `MainView` → `MainViewModel`

#### Convention-Based ViewModel Resolution

By default, the ViewModel Locator uses convention-based naming to find ViewModels. For a page named `MainPage`, it will look for:
1. `MainPageViewModel` in the same namespace
2. `MainViewModel` (if page ends with "Page")

You can customize the convention by setting the `DefaultViewTypeToViewModelTypeResolver`:

```csharp
ViewModelLocationProvider.DefaultViewTypeToViewModelTypeResolver = viewType =>
{
    // Your custom logic to resolve ViewModel type from View type
    var viewName = viewType.Name;
    var viewModelTypeName = $"{viewType.Namespace}.ViewModels.{viewName}ViewModel";
    return viewType.Assembly.GetType(viewModelTypeName);
};
```

#### Explicit ViewModel Registration

You can explicitly register ViewModels for specific Views in your `MauiProgram.cs`:

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // Register navigation service
        builder.Services.AddNavigationAware();
        
        // Register pages and ViewModels
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<MainPageViewModel>();
        
        // Explicitly register ViewModel for a View
        builder.Services.RegisterViewModel<MainPage, MainPageViewModel>();
        
        // Or use a factory method
        builder.Services.RegisterViewModel<DetailsPage>(sp => 
            new DetailsPageViewModel(sp.GetRequiredService<IDataService>()));

        return builder.Build();
    }
}
```

#### Programmatic ViewModel Binding

You can also wire up ViewModels programmatically:

```csharp
public partial class MyPage : NavigationAwarePage
{
    public MyPage()
    {
        InitializeComponent();
        
        // Automatically wire up ViewModel based on convention
        ViewModelLocationProvider.AutoWireViewModel(this);
        
        // Or specify a specific ViewModel type
        // ViewModelLocationProvider.AutoWireViewModel(this, typeof(MyCustomViewModel));
    }
}
```

#### Using ViewModels with Navigation

When using ViewModels, you can update them in the navigation lifecycle methods:

```csharp
public partial class MyPage : NavigationAwarePage
{
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        
        if (BindingContext is MyPageViewModel viewModel)
        {
            // Access parameters and update ViewModel
            if (parameters.TryGetValue<int>("userId", out var userId))
            {
                viewModel.LoadUser(userId);
            }
        }
    }
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
    Task NavigateToAsync(string pageKey, INavigationParameters? parameters = null);
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
    
    // Register a page type for string-based navigation
    public static IServiceCollection RegisterPage<TPage>(this IServiceCollection services, string? key = null) 
        where TPage : Page;
    
    // Register a ViewModel for a specific View type
    public static IServiceCollection RegisterViewModel<TView, TViewModel>(this IServiceCollection services)
        where TView : BindableObject;
    
    // Register a ViewModel using a factory method
    public static IServiceCollection RegisterViewModel<TView>(this IServiceCollection services, 
        Func<IServiceProvider, object> factory)
        where TView : BindableObject;
}
```

### ViewModel Locator Classes

#### ViewModelLocator

Provides attached properties for automatic ViewModel location and binding in XAML.

```csharp
public static class ViewModelLocator
{
    // Attached property to enable automatic ViewModel wiring
    public static readonly BindableProperty AutoWireViewModelProperty;
    
    // Gets/Sets the AutoWireViewModel attached property value
    public static bool GetAutoWireViewModel(BindableObject bindable);
    public static void SetAutoWireViewModel(BindableObject bindable, bool value);
    
    // Attached property to specify a specific ViewModel type
    public static readonly BindableProperty ViewModelTypeProperty;
    
    // Gets/Sets the ViewModelType attached property value
    public static Type? GetViewModelType(BindableObject bindable);
    public static void SetViewModelType(BindableObject bindable, Type? value);
}
```

#### ViewModelLocationProvider

Provides a mechanism for resolving ViewModels based on Views using naming conventions.

```csharp
public static class ViewModelLocationProvider
{
    // Default convention for resolving ViewModel type names from View type names
    public static Func<Type, Type?> DefaultViewTypeToViewModelTypeResolver { get; set; }
    
    // Sets the service provider for ViewModel resolution
    public static void SetServiceProvider(IServiceProvider serviceProvider);
    
    // Registers a ViewModel type for a specific View type
    public static void Register<TView, TViewModel>() where TView : BindableObject;
    public static void Register(Type viewType, Type viewModelType);
    
    // Registers a factory method for creating ViewModels
    public static void Register<TView>(Func<object> factory) where TView : BindableObject;
    
    // Automatically wires up the ViewModel for a View
    public static void AutoWireViewModel(BindableObject view, Type? viewModelType = null);
    
    // Clears all registered ViewModels and factories
    public static void Clear();
}
```

## Advanced Usage

### URI-Based Navigation

Navigate to multiple pages in sequence using a URI path:

```csharp
var navigationService = this.GetNavigationService();

// Navigate through multiple pages
await navigationService.NavigateAsync("/NavigationPage/MasterTabbedPage");

// With parameters
await navigationService.NavigateAsync(
    "/NavigationPage/DetailPage",
    new NavigationParameters { { "id", 123 } }
);
```

### Navigate Back to Specific Page

Pop the navigation stack to a specific page:

```csharp
var navigationService = this.GetNavigationService();

// Navigate back to a specific page
await navigationService.GoBackToAsync("SettingsPage");

// With parameters
await navigationService.GoBackToAsync(
    "MainPage",
    new NavigationParameters { { "result", "updated" } }
);
```

### Navigate Back to Root

Pop all pages and return to the root:

```csharp
var navigationService = this.GetNavigationService();

// Navigate back to root
await navigationService.GoBackToRootAsync();

// With parameters
await navigationService.GoBackToRootAsync(
    new NavigationParameters { { "taskCompleted", true } }
);
```

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
| `NavigateAsync(string uri)` | `NavigateAsync(string uri)` |
| `GoBackAsync()` | `GoBackAsync()` |
| `GoBackToRootAsync()` | `GoBackToRootAsync()` |
| Not available | `GoBackToAsync(string pageKey)` |

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Inspired by [Prism Library](https://prismlibrary.com/) for Xamarin.Forms and .NET MAUI
- Built with ❤️ for the .NET MAUI community

## Support

If you encounter any issues or have questions, please [open an issue](https://github.com/mohammedmsadiq/Plugin.Maui.NavigationAware/issues) on GitHub.
