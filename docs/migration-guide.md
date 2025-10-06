# Migration Guide

This guide helps developers migrate from other navigation frameworks to Plugin.Maui.NavigationAware.

## Table of Contents

1. [Migrating from Prism](#migrating-from-prism)
2. [Migrating from Xamarin.Forms](#migrating-from-xamarinforms)
3. [Migrating from Standard MAUI Navigation](#migrating-from-standard-maui-navigation)

---

## Migrating from Prism

If you're coming from Prism, you'll find Plugin.Maui.NavigationAware very familiar. The API is intentionally similar to make migration easier.

### Key Similarities

| Prism | Plugin.Maui.NavigationAware |
|-------|----------------------------|
| `INavigationAware` | `INavigationAware` ✅ Same |
| `INavigationParameters` | `INavigationParameters` ✅ Same |
| `NavigationParameters` | `NavigationParameters` ✅ Same |
| `OnNavigatedTo()` | `OnNavigatedTo()` ✅ Same |
| `OnNavigatedFrom()` | `OnNavigatedFrom()` ✅ Same |

### Key Differences

| Feature | Prism | Plugin.Maui.NavigationAware |
|---------|-------|----------------------------|
| **Base Class** | No base class | `NavigationAwarePage` available |
| **URI Navigation** | `NavigateAsync("MyPage")` | Direct page instance: `NavigateToAsync(new MyPage())` |
| **Container Integration** | Required | Optional |
| **ViewModel Support** | Built-in MVVM | You implement your own |
| **Dialog Service** | Included | Not included |

### Migration Steps

#### Step 1: Update Package References

Remove Prism packages:
```xml
<!-- Remove these -->
<PackageReference Include="Prism.Maui" Version="..." />
<PackageReference Include="Prism.DryIoc.Maui" Version="..." />
```

Add Plugin.Maui.NavigationAware:
```xml
<PackageReference Include="Plugin.Maui.NavigationAware" Version="1.0.0" />
```

#### Step 2: Update Page Implementation

**Before (Prism):**
```csharp
using Prism.Navigation;

public partial class MyPage : ContentPage, INavigationAware
{
    public void OnNavigatedTo(INavigationParameters parameters)
    {
        // Handle navigation
    }

    public void OnNavigatedFrom(INavigationParameters parameters)
    {
        // Handle navigation
    }
}
```

**After (Plugin.Maui.NavigationAware):**
```csharp
using Plugin.Maui.NavigationAware;

public partial class MyPage : NavigationAwarePage
{
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        // Handle navigation
    }

    public override void OnNavigatedFrom(INavigationParameters parameters)
    {
        base.OnNavigatedFrom(parameters);
        // Handle navigation
    }
}
```

#### Step 3: Update Navigation Calls

**Before (Prism):**
```csharp
private readonly INavigationService _navigationService;

public MyPage(INavigationService navigationService)
{
    _navigationService = navigationService;
}

private async void OnNavigateClicked(object sender, EventArgs e)
{
    var parameters = new NavigationParameters
    {
        { "id", 123 }
    };
    
    await _navigationService.NavigateAsync("DetailsPage", parameters);
}
```

**After (Plugin.Maui.NavigationAware):**
```csharp
// Option 1: Use extension method (no DI required)
private async void OnNavigateClicked(object sender, EventArgs e)
{
    var navigationService = this.GetNavigationService();
    var parameters = new NavigationParameters
    {
        { "id", 123 }
    };
    
    await navigationService.NavigateToAsync(new DetailsPage(), parameters);
}

// Option 2: Use DI (if configured)
private readonly INavigationService _navigationService;

public MyPage(INavigationService navigationService)
{
    _navigationService = navigationService;
}

private async void OnNavigateClicked(object sender, EventArgs e)
{
    var parameters = new NavigationParameters
    {
        { "id", 123 }
    };
    
    await _navigationService.NavigateToAsync(new DetailsPage(), parameters);
}
```

#### Step 4: Update XAML

**Before (Prism):**
```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             x:Class="MyApp.MyPage">
    <!-- Content -->
</ContentPage>
```

**After (Plugin.Maui.NavigationAware):**
```xml
<local:NavigationAwarePage 
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:local="clr-namespace:Plugin.Maui.NavigationAware;assembly=Plugin.Maui.NavigationAware"
    x:Class="MyApp.MyPage">
    <!-- Content -->
</local:NavigationAwarePage>
```

#### Step 5: Update Parameter Access

Parameter access remains the same:

```csharp
public override void OnNavigatedTo(INavigationParameters parameters)
{
    base.OnNavigatedTo(parameters);
    
    // Both work the same way
    var id = parameters.GetValue<int>("id");
    
    if (parameters.TryGetValue<string>("name", out var name))
    {
        // Use name
    }
}
```

### What's Not Supported

Plugin.Maui.NavigationAware is a lightweight navigation awareness library. The following Prism features are **not** included:

- ❌ URI-based navigation
- ❌ Built-in MVVM support
- ❌ Dialog service
- ❌ RegionManager
- ❌ Automatic ViewModel creation and binding
- ❌ Navigation journal

If you need these features, continue using Prism or implement them separately.

---

## Migrating from Xamarin.Forms

### Key Changes

1. **Update to .NET MAUI** first
2. **Add navigation awareness** with this plugin

### Before (Xamarin.Forms with MessagingCenter)

```csharp
// Sender page
MessagingCenter.Send(this, "UserSelected", userId);
await Navigation.PushAsync(new DetailsPage());

// Receiver page
MessagingCenter.Subscribe<MainPage, int>(this, "UserSelected", (sender, userId) =>
{
    LoadUser(userId);
});
```

### After (MAUI with Plugin.Maui.NavigationAware)

```csharp
// Sender page
var navigationService = this.GetNavigationService();
var parameters = new NavigationParameters
{
    { "userId", userId }
};
await navigationService.NavigateToAsync(new DetailsPage(), parameters);

// Receiver page
public override void OnNavigatedTo(INavigationParameters parameters)
{
    base.OnNavigatedTo(parameters);
    
    if (parameters.TryGetValue<int>("userId", out var userId))
    {
        LoadUser(userId);
    }
}
```

### Benefits of Migration

- ✅ Type-safe parameter passing
- ✅ No need for magic strings
- ✅ Automatic lifecycle management
- ✅ No memory leaks from forgotten unsubscribe
- ✅ Better testability

---

## Migrating from Standard MAUI Navigation

### Before (Standard MAUI)

```csharp
// Passing data via constructor
private async void OnNavigateClicked(object sender, EventArgs e)
{
    var detailsPage = new DetailsPage(userId: 123, userName: "John");
    await Navigation.PushAsync(detailsPage);
}

public partial class DetailsPage : ContentPage
{
    public DetailsPage(int userId, string userName)
    {
        InitializeComponent();
        LoadUser(userId, userName);
    }
}
```

**Problems:**
- ❌ Tight coupling
- ❌ Hard to pass data back
- ❌ No lifecycle awareness
- ❌ DI container issues with constructor parameters

### After (Plugin.Maui.NavigationAware)

```csharp
// Passing data via parameters
private async void OnNavigateClicked(object sender, EventArgs e)
{
    var navigationService = this.GetNavigationService();
    var parameters = new NavigationParameters
    {
        { "userId", 123 },
        { "userName", "John" }
    };
    await navigationService.NavigateToAsync(new DetailsPage(), parameters);
}

public partial class DetailsPage : NavigationAwarePage
{
    public DetailsPage()
    {
        InitializeComponent();
    }
    
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        
        var userId = parameters.GetValue<int>("userId");
        var userName = parameters.GetValue<string>("userName");
        LoadUser(userId, userName);
    }
}
```

**Benefits:**
- ✅ Loose coupling
- ✅ Easy to pass data back
- ✅ Lifecycle awareness
- ✅ Works with DI

---

## Common Migration Issues

### Issue 1: "Cannot resolve NavigationAwarePage in XAML"

**Solution:** Add the XML namespace:

```xml
xmlns:local="clr-namespace:Plugin.Maui.NavigationAware;assembly=Plugin.Maui.NavigationAware"
```

### Issue 2: "Parameters are always empty"

**Solution:** Make sure you're passing parameters:

```csharp
// ❌ Wrong
await navigationService.NavigateToAsync(new Page());

// ✅ Correct
await navigationService.NavigateToAsync(new Page(), new NavigationParameters { { "key", "value" } });
```

### Issue 3: "OnNavigatedTo not called"

**Solution:** 
1. Inherit from `NavigationAwarePage`, or
2. If implementing `INavigationAware`, manually call it in `OnAppearing`

### Issue 4: "DI constructor parameters conflict with navigation parameters"

**Solution:** Use DI for services, navigation parameters for data:

```csharp
// ✅ Correct pattern
public MyPage(IDataService dataService) // DI for services
{
    InitializeComponent();
    _dataService = dataService;
}

public override void OnNavigatedTo(INavigationParameters parameters) // Parameters for data
{
    base.OnNavigatedTo(parameters);
    var userId = parameters.GetValue<int>("userId");
}
```

---

## Need Help?

If you're having trouble migrating, please:

1. Check the [Examples](examples.md) page
2. Review the [API Reference](api-reference.md)
3. [Open an issue](https://github.com/mohammedmsadiq/Plugin.Maui.NavigationAware/issues) on GitHub
