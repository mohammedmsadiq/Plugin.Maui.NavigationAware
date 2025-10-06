# Getting Started with Plugin.Maui.NavigationAware

This guide will help you get started with Plugin.Maui.NavigationAware in your .NET MAUI application.

## Prerequisites

- .NET 8.0 or later
- .NET MAUI workload installed
- A .NET MAUI application project

## Installation

### Install via NuGet

Install the package using one of the following methods:

**Using .NET CLI:**
```bash
dotnet add package Plugin.Maui.NavigationAware
```

**Using Package Manager Console:**
```powershell
Install-Package Plugin.Maui.NavigationAware
```

**Using Visual Studio:**
1. Right-click on your project in Solution Explorer
2. Select "Manage NuGet Packages"
3. Search for "Plugin.Maui.NavigationAware"
4. Click "Install"

## Basic Setup

### Step 1: Choose Your Approach

There are two ways to use navigation awareness:

1. **Inherit from NavigationAwarePage** (Recommended)
2. **Implement INavigationAware interface**

### Step 2: Update Your XAML

If using `NavigationAwarePage`, update your XAML file:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<local:NavigationAwarePage 
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:Plugin.Maui.NavigationAware;assembly=Plugin.Maui.NavigationAware"
    x:Class="YourApp.MainPage"
    Title="Main Page">
    
    <VerticalStackLayout Padding="30" Spacing="25">
        <Label Text="Welcome!" FontSize="24" />
    </VerticalStackLayout>
    
</local:NavigationAwarePage>
```

### Step 3: Update Your Code-Behind

Update your code-behind to inherit from `NavigationAwarePage`:

```csharp
using Plugin.Maui.NavigationAware;

namespace YourApp;

public partial class MainPage : NavigationAwarePage
{
    public MainPage()
    {
        InitializeComponent();
    }

    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        
        // This is called when the page is navigated to
        Console.WriteLine("Navigated to MainPage");
        
        // Access any parameters passed during navigation
        if (parameters.TryGetValue<string>("message", out var message))
        {
            Console.WriteLine($"Message: {message}");
        }
    }

    public override void OnNavigatedFrom(INavigationParameters parameters)
    {
        base.OnNavigatedFrom(parameters);
        
        // This is called when navigating away from the page
        Console.WriteLine("Navigated from MainPage");
    }
}
```

## Using the Navigation Service

### Step 1: Navigate with Parameters

Create a button click handler to navigate to another page:

```csharp
private async void OnNavigateClicked(object sender, EventArgs e)
{
    // Get the navigation service
    var navigationService = this.GetNavigationService();
    
    // Create parameters to pass
    var parameters = new NavigationParameters
    {
        { "userId", 123 },
        { "userName", "John Doe" }
    };
    
    // Navigate to the next page
    await navigationService.NavigateToAsync(new DetailsPage(), parameters);
}
```

### Step 2: Receive Parameters

In your destination page (DetailsPage), receive the parameters:

```csharp
public partial class DetailsPage : NavigationAwarePage
{
    public DetailsPage()
    {
        InitializeComponent();
    }

    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        
        // Retrieve the parameters
        var userId = parameters.GetValue<int>("userId");
        var userName = parameters.GetValue<string>("userName");
        
        Console.WriteLine($"User ID: {userId}, Name: {userName}");
        
        // Update UI with the data
        UserIdLabel.Text = userId.ToString();
        UserNameLabel.Text = userName;
    }
}
```

## Alternative: Using INavigationAware Interface

If you cannot inherit from `NavigationAwarePage` (e.g., your page already has a different base class), you can implement the interface directly:

```csharp
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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Manually trigger navigation awareness
        OnNavigatedTo(new NavigationParameters());
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Manually trigger navigation awareness
        OnNavigatedFrom(new NavigationParameters());
    }
}
```

## Dependency Injection (Optional)

You can optionally register the navigation service with the DI container in `MauiProgram.cs`:

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
        
        // Register pages
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

    private async void OnNavigateClicked(object sender, EventArgs e)
    {
        await _navigationService.NavigateToAsync(
            new DetailsPage(), 
            new NavigationParameters { { "key", "value" } }
        );
    }
}
```

## Next Steps

- Learn more about the [API Reference](api-reference.md)
- Check out [Examples](examples.md) for more usage scenarios
- See the [Migration Guide](migration-guide.md) if you're coming from Prism

## Common Issues

### Issue: Navigation events not firing

**Solution**: Ensure your page inherits from `NavigationAwarePage` or implements `INavigationAware` and manually calls the methods in `OnAppearing`/`OnDisappearing`.

### Issue: Parameters are null

**Solution**: Make sure you're passing parameters when calling `NavigateToAsync()`:

```csharp
// ✅ Correct
await navigationService.NavigateToAsync(new Page(), new NavigationParameters { { "key", "value" } });

// ❌ Incorrect (parameters will be empty)
await navigationService.NavigateToAsync(new Page());
```

### Issue: XAML cannot find NavigationAwarePage

**Solution**: Add the XML namespace declaration:

```xml
xmlns:local="clr-namespace:Plugin.Maui.NavigationAware;assembly=Plugin.Maui.NavigationAware"
```

## Support

If you encounter any issues, please:
1. Check the [Examples](examples.md) for common scenarios
2. Review the [API Reference](api-reference.md)
3. [Open an issue](https://github.com/mohammedmsadiq/Plugin.Maui.NavigationAware/issues) on GitHub
