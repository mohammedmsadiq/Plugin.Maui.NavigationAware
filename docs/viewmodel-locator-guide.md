# ViewModel Locator Quick Reference Guide

This guide provides a quick reference for using the ViewModel Locator feature in Plugin.Maui.NavigationAware.

## Quick Start (3 Steps)

### Step 1: Setup in MauiProgram.cs

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // STEP 1: Register navigation services
        builder.Services.AddNavigationAware();
        
        // STEP 2: Register your ViewModels (optional but recommended for DI)
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<DetailsPageViewModel>();
        
        return builder.Build();
    }
}
```

### Step 2: Create ViewModel

Create a ViewModel that follows naming conventions:

```csharp
// For MainPage, create MainPageViewModel
public class MainPageViewModel : INotifyPropertyChanged
{
    private string _message = "Hello from ViewModel!";
    
    public string Message
    {
        get => _message;
        set
        {
            if (_message != value)
            {
                _message = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

### Step 3: Use in XAML

```xml
<?xml version="1.0" encoding="utf-8" ?>
<local:NavigationAwarePage 
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:Plugin.Maui.NavigationAware;assembly=Plugin.Maui.NavigationAware"
    xmlns:nav="clr-namespace:Plugin.Maui.NavigationAware;assembly=Plugin.Maui.NavigationAware"
    x:Class="MyApp.MainPage"
    nav:ViewModelLocator.AutoWireViewModel="True"
    Title="Main Page">
    
    <VerticalStackLayout>
        <Label Text="{Binding Message}" />
    </VerticalStackLayout>
</local:NavigationAwarePage>
```

**That's it!** Your ViewModel will be automatically created and bound to the page.

---

## Naming Conventions

The ViewModel Locator uses these conventions by default:

| View Name | ViewModel Name |
|-----------|----------------|
| `MainPage` | `MainPageViewModel` |
| `DetailsPage` | `DetailsPageViewModel` or `DetailsViewModel` |
| `ProfileView` | `ProfileViewModel` |
| `SettingsPage` | `SettingsPageViewModel` or `SettingsViewModel` |

**Rule**: Append `ViewModel` to the view name, OR replace `Page`/`View` suffix with `ViewModel`.

---

## Common Scenarios

### Scenario 1: ViewModel with Dependencies

When your ViewModel has constructor dependencies:

```csharp
public class MainPageViewModel
{
    private readonly IUserService _userService;
    
    // Constructor with dependency
    public MainPageViewModel(IUserService userService)
    {
        _userService = userService;
    }
}
```

**Register both in DI:**

```csharp
// In MauiProgram.cs
builder.Services.AddNavigationAware();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddTransient<MainPageViewModel>();
```

### Scenario 2: ViewModels in Different Namespace

If your ViewModels are in `MyApp.ViewModels` but Views are in `MyApp.Pages`:

```csharp
// In App.xaml.cs constructor
ViewModelLocationProvider.DefaultViewTypeToViewModelTypeResolver = viewType =>
{
    var viewName = viewType.Name;
    var viewModelTypeName = $"MyApp.ViewModels.{viewName}ViewModel";
    return viewType.Assembly.GetType(viewModelTypeName);
};
```

### Scenario 3: Explicit Registration

When naming conventions don't work:

```csharp
// In MauiProgram.cs
builder.Services.RegisterViewModel<MainPage, MyCustomViewModel>();
```

### Scenario 4: Factory-based Registration

For complex ViewModel creation:

```csharp
builder.Services.RegisterViewModel<ProfilePage>(sp => 
    new ProfilePageViewModel(
        sp.GetRequiredService<IUserService>(),
        sp.GetRequiredService<INavigationService>()
    ));
```

---

## Troubleshooting

### ✗ BindingContext is null

**Check:**
1. ✓ Did you call `builder.Services.AddNavigationAware()` in `MauiProgram.cs`?
2. ✓ Does your ViewModel follow naming conventions?
3. ✓ Is the ViewModel in the same namespace as the View?
4. ✓ Check Debug output for `[ViewModelLocationProvider]` messages

**Solution:** See [Troubleshooting Guide](./examples.md#viewmodel-locator-troubleshooting)

### ✗ ViewModel constructor exception

**Check:**
1. ✓ Are all dependencies registered in DI?
2. ✓ Is the ViewModel registered in DI?
3. ✓ Does the ViewModel have a parameterless constructor?

**Solution:**
```csharp
// Register dependencies BEFORE ViewModel
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddTransient<MainPageViewModel>();
```

### ✗ ViewModel not found

**Check Debug Output:**
```
[ViewModelLocationProvider] Could not resolve ViewModel for View type 'MyApp.MainPage'...
```

**Solutions:**
- Ensure ViewModel is named `MainPageViewModel`
- Or register explicitly: `builder.Services.RegisterViewModel<MainPage, MyViewModel>()`
- Or customize the resolver (see Scenario 2 above)

---

## Best Practices

1. **Always register navigation services first:**
   ```csharp
   builder.Services.AddNavigationAware();
   ```

2. **Follow naming conventions** for automatic resolution:
   - `MainPage` → `MainPageViewModel`
   - Same namespace as the View

3. **Register ViewModels in DI** when they have dependencies:
   ```csharp
   builder.Services.AddTransient<MainPageViewModel>();
   ```

4. **Use the Debug output** to diagnose issues:
   - Look for `[ViewModelLocationProvider]` messages

5. **Test ViewModel creation** separately:
   ```csharp
   var vm = new MainPageViewModel();
   // If this throws, fix dependencies first
   ```

---

## Advanced: Programmatic Wiring

Instead of using XAML, you can wire up ViewModels in code:

```csharp
public partial class MainPage : NavigationAwarePage
{
    public MainPage()
    {
        InitializeComponent();
        
        // Auto-wire using convention
        ViewModelLocationProvider.AutoWireViewModel(this);
        
        // Or specify a specific type
        // ViewModelLocationProvider.AutoWireViewModel(this, typeof(CustomViewModel));
    }
}
```

---

## See Also

- [Complete Examples](./examples.md#viewmodel-locator)
- [API Reference](./api-reference.md#viewmodellocator)
- [Troubleshooting Guide](./examples.md#viewmodel-locator-troubleshooting)
