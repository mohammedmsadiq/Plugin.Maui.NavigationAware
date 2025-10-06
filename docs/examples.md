# Examples

This page provides practical examples of using Plugin.Maui.NavigationAware in various scenarios.

## Table of Contents

1. [Basic Navigation](#basic-navigation)
2. [String-Based Navigation](#string-based-navigation)
3. [Passing Multiple Parameters](#passing-multiple-parameters)
4. [Complex Object Parameters](#complex-object-parameters)
5. [Navigation with Return Values](#navigation-with-return-values)
6. [Master-Detail Navigation](#master-detail-navigation)
7. [Modal Navigation](#modal-navigation)
8. [Dependency Injection](#dependency-injection)
9. [ViewModel Locator](#viewmodel-locator)
10. [URI-Based Navigation](#uri-based-navigation)
11. [Navigate Back To Specific Page](#navigate-back-to-specific-page)
12. [Navigate Back To Root](#navigate-back-to-root)

---

## Basic Navigation

Simple navigation between two pages.

### MainPage.xaml.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class MainPage : NavigationAwarePage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnNavigateClicked(object sender, EventArgs e)
    {
        var navigationService = this.GetNavigationService();
        await navigationService.NavigateToAsync(new SecondPage());
    }

    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        StatusLabel.Text = "On Main Page";
    }
}
```

### SecondPage.xaml.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class SecondPage : NavigationAwarePage
{
    public SecondPage()
    {
        InitializeComponent();
    }

    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        StatusLabel.Text = "On Second Page";
    }

    private async void OnGoBackClicked(object sender, EventArgs e)
    {
        var navigationService = this.GetNavigationService();
        await navigationService.GoBackAsync();
    }
}
```

---

## String-Based Navigation

Navigate using page names/keys instead of instances, similar to Prism.

### MauiProgram.cs

First, register your pages:

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

        // Register pages for string-based navigation
        builder.Services.RegisterPage<MainPage>();
        builder.Services.RegisterPage<DetailsPage>();
        builder.Services.RegisterPage<SettingsPage>();
        
        // You can also register with custom keys
        builder.Services.RegisterPage<DetailsPage>("ProductDetails");

        return builder.Build();
    }
}
```

### MainPage.xaml.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class MainPage : NavigationAwarePage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnNavigateClicked(object sender, EventArgs e)
    {
        var navigationService = this.GetNavigationService();
        
        // Navigate using page name
        await navigationService.NavigateToAsync("DetailsPage");
        
        // Or use nameof for type safety
        await navigationService.NavigateToAsync(nameof(DetailsPage));
        
        // Or use custom key
        await navigationService.NavigateToAsync("ProductDetails");
    }

    private async void OnNavigateWithParametersClicked(object sender, EventArgs e)
    {
        var navigationService = this.GetNavigationService();
        var parameters = new NavigationParameters
        {
            { "userId", 123 },
            { "userName", "John Doe" }
        };
        
        await navigationService.NavigateToAsync(nameof(DetailsPage), parameters);
    }
}
```

### DetailsPage.xaml.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class DetailsPage : NavigationAwarePage
{
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        
        if (parameters.TryGetValue<int>("userId", out var userId))
        {
            var userName = parameters.GetValue<string>("userName");
            StatusLabel.Text = $"User: {userName} (ID: {userId})";
        }
    }
}
```

---

## Passing Multiple Parameters

Pass multiple parameters of different types.

### ListPage.xaml.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class ListPage : NavigationAwarePage
{
    private async void OnItemSelected(object sender, ItemTappedEventArgs e)
    {
        var selectedItem = e.Item as Product;
        
        var navigationService = this.GetNavigationService();
        var parameters = new NavigationParameters
        {
            { "productId", selectedItem.Id },
            { "productName", selectedItem.Name },
            { "price", selectedItem.Price },
            { "isAvailable", selectedItem.IsAvailable }
        };
        
        await navigationService.NavigateToAsync(new DetailsPage(), parameters);
    }
}
```

### DetailsPage.xaml.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class DetailsPage : NavigationAwarePage
{
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        
        var productId = parameters.GetValue<int>("productId");
        var productName = parameters.GetValue<string>("productName");
        var price = parameters.GetValue<decimal>("price");
        var isAvailable = parameters.GetValue<bool>("isAvailable");
        
        // Update UI
        ProductIdLabel.Text = productId.ToString();
        ProductNameLabel.Text = productName;
        PriceLabel.Text = $"${price:F2}";
        AvailabilityLabel.Text = isAvailable ? "In Stock" : "Out of Stock";
    }
}
```

---

## Complex Object Parameters

Pass complex objects as parameters.

### Product Model

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public List<string> Images { get; set; }
}
```

### ProductListPage.xaml.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class ProductListPage : NavigationAwarePage
{
    private async void OnProductSelected(object sender, SelectionChangedEventArgs e)
    {
        var product = e.CurrentSelection.FirstOrDefault() as Product;
        if (product == null) return;
        
        var navigationService = this.GetNavigationService();
        var parameters = new NavigationParameters
        {
            { "product", product },
            { "source", "ProductList" }
        };
        
        await navigationService.NavigateToAsync(new ProductDetailsPage(), parameters);
    }
}
```

### ProductDetailsPage.xaml.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class ProductDetailsPage : NavigationAwarePage
{
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        
        if (parameters.TryGetValue<Product>("product", out var product))
        {
            DisplayProduct(product);
        }
        
        var source = parameters.GetValue<string>("source");
        Console.WriteLine($"Navigated from: {source}");
    }
    
    private void DisplayProduct(Product product)
    {
        TitleLabel.Text = product.Name;
        DescriptionLabel.Text = product.Description;
        PriceLabel.Text = $"${product.Price:F2}";
        
        // Display images
        ProductImage.Source = product.Images.FirstOrDefault();
    }
}
```

---

## Navigation with Return Values

Pass data back when navigating back.

### EditPage.xaml.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class EditPage : NavigationAwarePage
{
    private string _originalValue;
    
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        
        if (parameters.TryGetValue<string>("value", out var value))
        {
            _originalValue = value;
            ValueEntry.Text = value;
        }
    }
    
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var navigationService = this.GetNavigationService();
        var parameters = new NavigationParameters
        {
            { "result", "saved" },
            { "value", ValueEntry.Text },
            { "changed", ValueEntry.Text != _originalValue }
        };
        
        await navigationService.GoBackAsync(parameters);
    }
    
    private async void OnCancelClicked(object sender, EventArgs e)
    {
        var navigationService = this.GetNavigationService();
        var parameters = new NavigationParameters
        {
            { "result", "cancelled" }
        };
        
        await navigationService.GoBackAsync(parameters);
    }
}
```

### MainPage.xaml.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class MainPage : NavigationAwarePage
{
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        
        // Check if returning from edit page
        if (parameters.TryGetValue<string>("result", out var result))
        {
            if (result == "saved")
            {
                var newValue = parameters.GetValue<string>("value");
                var changed = parameters.GetValue<bool>("changed");
                
                if (changed)
                {
                    ValueLabel.Text = newValue;
                    await DisplayAlert("Success", "Value updated successfully", "OK");
                }
            }
            else if (result == "cancelled")
            {
                await DisplayAlert("Info", "Edit cancelled", "OK");
            }
        }
    }
    
    private async void OnEditClicked(object sender, EventArgs e)
    {
        var navigationService = this.GetNavigationService();
        var parameters = new NavigationParameters
        {
            { "value", ValueLabel.Text }
        };
        
        await navigationService.NavigateToAsync(new EditPage(), parameters);
    }
}
```

---

## Master-Detail Navigation

Navigate from a master list to detail pages.

### ContactListPage.xaml.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class ContactListPage : NavigationAwarePage
{
    private readonly ObservableCollection<Contact> _contacts = new();
    
    public ContactListPage()
    {
        InitializeComponent();
        ContactsListView.ItemsSource = _contacts;
    }
    
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        
        // Refresh list if a contact was modified
        if (parameters.TryGetValue<bool>("refresh", out var refresh) && refresh)
        {
            LoadContacts();
        }
    }
    
    private async void OnContactTapped(object sender, ItemTappedEventArgs e)
    {
        var contact = e.Item as Contact;
        
        var navigationService = this.GetNavigationService();
        var parameters = new NavigationParameters
        {
            { "contact", contact },
            { "mode", "view" }
        };
        
        await navigationService.NavigateToAsync(new ContactDetailsPage(), parameters);
    }
    
    private async void OnAddContactClicked(object sender, EventArgs e)
    {
        var navigationService = this.GetNavigationService();
        var parameters = new NavigationParameters
        {
            { "mode", "create" }
        };
        
        await navigationService.NavigateToAsync(new ContactDetailsPage(), parameters);
    }
}
```

### ContactDetailsPage.xaml.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class ContactDetailsPage : NavigationAwarePage
{
    private Contact _contact;
    private string _mode;
    
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        
        _mode = parameters.GetValue<string>("mode");
        
        if (_mode == "create")
        {
            _contact = new Contact();
            Title = "New Contact";
        }
        else if (_mode == "view" && parameters.TryGetValue<Contact>("contact", out var contact))
        {
            _contact = contact;
            Title = "Contact Details";
            DisplayContact(contact);
        }
    }
    
    private void DisplayContact(Contact contact)
    {
        NameEntry.Text = contact.Name;
        EmailEntry.Text = contact.Email;
        PhoneEntry.Text = contact.Phone;
    }
    
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        _contact.Name = NameEntry.Text;
        _contact.Email = EmailEntry.Text;
        _contact.Phone = PhoneEntry.Text;
        
        // Save to database...
        
        var navigationService = this.GetNavigationService();
        var parameters = new NavigationParameters
        {
            { "refresh", true },
            { "contact", _contact },
            { "action", _mode == "create" ? "created" : "updated" }
        };
        
        await navigationService.GoBackAsync(parameters);
    }
}
```

---

## Modal Navigation

Although this plugin focuses on standard navigation, you can still use it with modal pages.

### MainPage.xaml.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class MainPage : NavigationAwarePage
{
    private async void OnShowModalClicked(object sender, EventArgs e)
    {
        var modalPage = new SettingsPage();
        
        // Pass parameters before showing modal
        var parameters = new NavigationParameters
        {
            { "theme", CurrentTheme },
            { "fontSize", CurrentFontSize }
        };
        
        // Trigger OnNavigatedTo manually for modal
        if (modalPage is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedTo(parameters);
        }
        
        await Navigation.PushModalAsync(modalPage);
    }
    
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        
        // Check if returning from modal
        if (parameters.TryGetValue<string>("theme", out var theme))
        {
            ApplyTheme(theme);
        }
    }
}
```

---

## Dependency Injection

Use dependency injection with the navigation service.

### MauiProgram.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

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
        
        // Register services
        builder.Services.AddSingleton<IDataService, DataService>();

        return builder.Build();
    }
}
```

### MainPage.xaml.cs with DI

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class MainPage : NavigationAwarePage
{
    private readonly INavigationService _navigationService;
    private readonly IDataService _dataService;
    
    public MainPage(INavigationService navigationService, IDataService dataService)
    {
        InitializeComponent();
        _navigationService = navigationService;
        _dataService = dataService;
    }
    
    private async void OnLoadDataClicked(object sender, EventArgs e)
    {
        var data = await _dataService.GetDataAsync();
        
        var parameters = new NavigationParameters
        {
            { "data", data }
        };
        
        await _navigationService.NavigateToAsync(new DetailsPage(), parameters);
    }
}
```

---

## ViewModel Locator

The ViewModel Locator provides automatic ViewModel binding for your Views, eliminating boilerplate code and making MVVM easier to implement.

### Quick Start Guide

To use the ViewModel Locator, follow these steps:

1. **Register navigation services** in `MauiProgram.cs`:

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // IMPORTANT: Register navigation services first
        builder.Services.AddNavigationAware();
        
        // Register your ViewModels in DI (recommended)
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<DetailsPageViewModel>();
        
        return builder.Build();
    }
}
```

2. **Create your ViewModel** following naming conventions:
   - For `MainPage`, create `MainPageViewModel`
   - For `DetailsPage`, create `DetailsPageViewModel`
   - For `ProfileView`, create `ProfileViewModel`

3. **Use the attached property** in your XAML:

```xml
<local:NavigationAwarePage 
    xmlns:nav="clr-namespace:Plugin.Maui.NavigationAware;assembly=Plugin.Maui.NavigationAware"
    nav:ViewModelLocator.AutoWireViewModel="True">
```

### Using Automatic ViewModel Binding in XAML

Automatically wire up ViewModels using the `AutoWireViewModel` attached property.

#### MainPage.xaml

```xml
<?xml version="1.0" encoding="utf-8" ?>
<local:NavigationAwarePage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:Plugin.Maui.NavigationAware;assembly=Plugin.Maui.NavigationAware"
             xmlns:nav="clr-namespace:Plugin.Maui.NavigationAware;assembly=Plugin.Maui.NavigationAware"
             x:Class="MyApp.MainPage"
             nav:ViewModelLocator.AutoWireViewModel="True"
             Title="Main Page">

    <VerticalStackLayout Padding="30" Spacing="25">
        <Label Text="Welcome!" FontSize="32" FontAttributes="Bold" />
        
        <Label Text="{Binding StatusMessage}" FontSize="16" />
        
        <Entry Text="{Binding InputMessage}" Placeholder="Enter message" />
        
        <Button Text="Navigate" Command="{Binding NavigateCommand}" />
    </VerticalStackLayout>

</local:NavigationAwarePage>
```

#### MainPageViewModel.cs

```csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MyApp;

public class MainPageViewModel : INotifyPropertyChanged
{
    private string _statusMessage = "Ready";
    private string _inputMessage = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            if (_statusMessage != value)
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }
    }

    public string InputMessage
    {
        get => _inputMessage;
        set
        {
            if (_inputMessage != value)
            {
                _inputMessage = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand NavigateCommand { get; }

    public MainPageViewModel()
    {
        NavigateCommand = new Command(async () => await OnNavigate());
    }

    private async Task OnNavigate()
    {
        StatusMessage = "Navigating...";
        // Navigation logic here
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

#### MainPage.xaml.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class MainPage : NavigationAwarePage
{
    public MainPage()
    {
        InitializeComponent();
        // ViewModel is automatically bound via ViewModelLocator
    }

    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        
        // Access the ViewModel to update with navigation parameters
        if (BindingContext is MainPageViewModel viewModel)
        {
            if (parameters.TryGetValue<string>("message", out var message))
            {
                viewModel.StatusMessage = message;
            }
        }
    }
}
```

---

### Explicit ViewModel Registration

Register ViewModels explicitly in your DI container.

#### MauiProgram.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // Register navigation service
        builder.Services.AddNavigationAware();
        
        // Register pages
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<DetailsPage>();
        
        // Register ViewModels
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<DetailsPageViewModel>();
        
        // Explicitly register ViewModel for View
        builder.Services.RegisterViewModel<MainPage, MainPageViewModel>();
        builder.Services.RegisterViewModel<DetailsPage, DetailsPageViewModel>();
        
        // Register with factory for dependency injection
        builder.Services.RegisterViewModel<ProfilePage>(sp => 
            new ProfilePageViewModel(
                sp.GetRequiredService<IUserService>(),
                sp.GetRequiredService<INavigationService>()
            ));

        return builder.Build();
    }
}
```

---

### Custom ViewModel Resolution Convention

Customize the naming convention for ViewModel resolution.

#### App.xaml.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Customize ViewModel resolution convention
        ViewModelLocationProvider.DefaultViewTypeToViewModelTypeResolver = viewType =>
        {
            // Example: Look for ViewModels in a separate namespace
            var viewName = viewType.Name;
            var viewModelTypeName = $"{viewType.Namespace}.ViewModels.{viewName}ViewModel";
            
            var viewModelType = viewType.Assembly.GetType(viewModelTypeName);
            if (viewModelType != null)
                return viewModelType;
            
            // Fallback to default convention
            viewModelTypeName = $"{viewType.Namespace}.{viewName}ViewModel";
            return viewType.Assembly.GetType(viewModelTypeName);
        };

        MainPage = new AppShell();
    }
}
```

---

### ViewModel with Navigation Parameters

Handle navigation parameters in your ViewModel.

#### DetailsPageViewModel.cs

```csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyApp;

public class DetailsPageViewModel : INotifyPropertyChanged
{
    private string _title = string.Empty;
    private int _itemId;
    private string _description = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Title
    {
        get => _title;
        set
        {
            if (_title != value)
            {
                _title = value;
                OnPropertyChanged();
            }
        }
    }

    public int ItemId
    {
        get => _itemId;
        set
        {
            if (_itemId != value)
            {
                _itemId = value;
                OnPropertyChanged();
                LoadItem(value);
            }
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            if (_description != value)
            {
                _description = value;
                OnPropertyChanged();
            }
        }
    }

    private async void LoadItem(int itemId)
    {
        // Load item data
        Title = $"Item {itemId}";
        Description = $"Details for item {itemId}";
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

#### DetailsPage.xaml.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class DetailsPage : NavigationAwarePage
{
    public DetailsPage()
    {
        InitializeComponent();
    }

    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        
        // Update ViewModel with navigation parameters
        if (BindingContext is DetailsPageViewModel viewModel)
        {
            if (parameters.TryGetValue<int>("itemId", out var itemId))
            {
                viewModel.ItemId = itemId;
            }
            
            if (parameters.TryGetValue<string>("title", out var title))
            {
                viewModel.Title = title;
            }
        }
    }
}
```

---

### ViewModel Locator Troubleshooting

This section helps you diagnose and fix common issues with the ViewModel Locator.

#### Issue: ViewModel is not being set (BindingContext is null)

**Check the Debug Output**

The ViewModelLocationProvider logs debug messages. Check your IDE's debug output window for messages like:
```
[ViewModelLocationProvider] Could not resolve ViewModel for View type 'MyApp.MainPage'...
```

**Common Causes and Solutions:**

1. **Naming Convention Not Followed**

   Problem: Your View is named `MainPage` but ViewModel is named `MainViewModel` (not `MainPageViewModel`)
   
   Solution A - Rename your ViewModel:
   ```csharp
   // Change from:
   public class MainViewModel { }
   
   // To:
   public class MainPageViewModel { }
   ```
   
   Solution B - Register explicitly:
   ```csharp
   // In MauiProgram.cs
   builder.Services.RegisterViewModel<MainPage, MainViewModel>();
   ```

2. **Different Namespace**

   Problem: View is in `MyApp.Pages` but ViewModel is in `MyApp.ViewModels`
   
   Solution: Customize the resolver in your `App.xaml.cs`:
   ```csharp
   public App()
   {
       InitializeComponent();
       
       // Configure custom namespace resolution
       ViewModelLocationProvider.DefaultViewTypeToViewModelTypeResolver = viewType =>
       {
           var viewName = viewType.Name;
           
           // Try ViewModels namespace first
           var viewModelTypeName = $"MyApp.ViewModels.{viewName}ViewModel";
           var viewModelType = viewType.Assembly.GetType(viewModelTypeName);
           
           if (viewModelType != null)
               return viewModelType;
           
           // Fallback to same namespace
           viewModelTypeName = $"{viewType.Namespace}.{viewName}ViewModel";
           return viewType.Assembly.GetType(viewModelTypeName);
       };
       
       MainPage = new AppShell();
   }
   ```

3. **ViewModel Not Registered in DI**

   Problem: ViewModel has dependencies that can't be resolved
   
   Solution: Register the ViewModel and its dependencies:
   ```csharp
   // In MauiProgram.cs
   builder.Services.AddNavigationAware();
   
   // Register dependencies
   builder.Services.AddSingleton<IDataService, DataService>();
   
   // Register ViewModel
   builder.Services.AddTransient<MainPageViewModel>();
   ```

#### Issue: ViewModel constructor throws exception

**Problem**: ViewModel has dependencies but they're not registered

**Solution**: Ensure all dependencies are registered in DI before the ViewModel:

```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder.UseMauiApp<App>();
    
    // 1. Register navigation services FIRST
    builder.Services.AddNavigationAware();
    
    // 2. Register services/dependencies
    builder.Services.AddSingleton<IUserService, UserService>();
    builder.Services.AddSingleton<IDataService, DataService>();
    
    // 3. Register ViewModels LAST
    builder.Services.AddTransient<MainPageViewModel>();
    
    return builder.Build();
}
```

**Alternative**: Use parameterless constructor and property injection:

```csharp
public class MainPageViewModel : INotifyPropertyChanged
{
    private IUserService? _userService;
    
    // Parameterless constructor for AutoWireViewModel
    public MainPageViewModel()
    {
        // Get service from DI when needed
        _userService = Application.Current?.Handler?.MauiContext?.Services
            .GetService<IUserService>();
    }
    
    // Or use property injection
    public IUserService UserService { get; set; } = null!;
}
```

#### Issue: AutoWireViewModel doesn't work with ContentView

**Problem**: ContentView doesn't support ViewModel binding

**Solution**: Use UserControl or ContentPage instead, or set BindingContext manually:

```csharp
public partial class MyContentView : ContentView
{
    public MyContentView()
    {
        InitializeComponent();
        
        // Manually wire up ViewModel
        ViewModelLocationProvider.AutoWireViewModel(this);
    }
}
```

#### Issue: Multiple pages share the same ViewModel type

**Problem**: You want different ViewModel instances for different pages

**Solution**: Use factory registration:

```csharp
builder.Services.RegisterViewModel<Page1>(sp => 
    new SharedViewModel { Title = "Page 1" });
    
builder.Services.RegisterViewModel<Page2>(sp => 
    new SharedViewModel { Title = "Page 2" });
```

#### Debugging Tips

1. **Enable verbose logging**: Check the Debug output window for `[ViewModelLocationProvider]` messages

2. **Verify ViewModel can be created**:
   ```csharp
   // Test in your page constructor
   try
   {
       var vm = new MainPageViewModel();
       Console.WriteLine($"ViewModel created: {vm != null}");
   }
   catch (Exception ex)
   {
       Console.WriteLine($"Error creating ViewModel: {ex.Message}");
   }
   ```

3. **Check if AutoWireViewModel is called**:
   ```csharp
   public partial class MainPage : NavigationAwarePage
   {
       public MainPage()
       {
           InitializeComponent();
           
           // Check if BindingContext was set
           Console.WriteLine($"BindingContext after init: {BindingContext?.GetType().Name ?? "null"}");
       }
   }
   ```

---

## URI-Based Navigation

Navigate to pages using a URI path, similar to Prism's navigation syntax.

### MauiProgram.cs

First, register your pages:

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // Register pages
        builder.Services.RegisterPage<NavigationPage>();
        builder.Services.RegisterPage<MasterTabbedPage>();
        builder.Services.RegisterPage<DetailPage>();

        return builder.Build();
    }
}
```

### MainPage.xaml.cs

Use URI-based navigation:

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class MainPage : NavigationAwarePage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnNavigateToDetailsClicked(object sender, EventArgs e)
    {
        var navigationService = this.GetNavigationService();
        
        // Navigate using URI path
        await navigationService.NavigateAsync("/NavigationPage/MasterTabbedPage");
    }

    private async void OnNavigateWithParametersClicked(object sender, EventArgs e)
    {
        var navigationService = this.GetNavigationService();
        var parameters = new NavigationParameters
        {
            { "userId", 123 },
            { "mode", "edit" }
        };
        
        await navigationService.NavigateAsync("/NavigationPage/DetailPage", parameters);
    }
}
```

**Note:** The URI path is parsed and each segment is treated as a page key. Pages are navigated to in sequence.

---

## Navigate Back To Specific Page

Navigate back to a specific page in the navigation stack.

### Example Scenario

You have a navigation stack like: MainPage -> SettingsPage -> ProfilePage -> DetailPage

You want to navigate back to SettingsPage from DetailPage, skipping ProfilePage.

### DetailPage.xaml.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class DetailPage : NavigationAwarePage
{
    public DetailPage()
    {
        InitializeComponent();
    }

    private async void OnBackToSettingsClicked(object sender, EventArgs e)
    {
        var navigationService = this.GetNavigationService();
        
        // Navigate back to SettingsPage, removing all pages in between
        await navigationService.GoBackToAsync("SettingsPage");
    }

    private async void OnBackToSettingsWithDataClicked(object sender, EventArgs e)
    {
        var navigationService = this.GetNavigationService();
        var parameters = new NavigationParameters
        {
            { "result", "updated" },
            { "timestamp", DateTime.Now }
        };
        
        await navigationService.GoBackToAsync("SettingsPage", parameters);
    }
}
```

### SettingsPage.xaml.cs

Receive the parameters when navigated back to:

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class SettingsPage : NavigationAwarePage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        
        // Handle return from detail page
        if (parameters.TryGetValue<string>("result", out var result))
        {
            if (result == "updated")
            {
                StatusLabel.Text = "Settings were updated";
                RefreshSettings();
            }
        }
    }

    private void RefreshSettings()
    {
        // Refresh settings UI
    }
}
```

---

## Navigate Back To Root

Navigate back to the root page of the navigation stack.

### DeepNestedPage.xaml.cs

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class DeepNestedPage : NavigationAwarePage
{
    public DeepNestedPage()
    {
        InitializeComponent();
    }

    private async void OnBackToRootClicked(object sender, EventArgs e)
    {
        var navigationService = this.GetNavigationService();
        
        // Navigate back to the root page (usually MainPage)
        await navigationService.GoBackToRootAsync();
    }

    private async void OnBackToRootWithResultClicked(object sender, EventArgs e)
    {
        var navigationService = this.GetNavigationService();
        var parameters = new NavigationParameters
        {
            { "taskCompleted", true },
            { "result", "success" }
        };
        
        await navigationService.GoBackToRootAsync(parameters);
    }
}
```

### MainPage.xaml.cs (Root Page)

Handle the parameters when returning to root:

```csharp
using Plugin.Maui.NavigationAware;

namespace MyApp;

public partial class MainPage : NavigationAwarePage
{
    public MainPage()
    {
        InitializeComponent();
    }

    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        
        // Handle return to root
        if (parameters.TryGetValue<bool>("taskCompleted", out var taskCompleted))
        {
            if (taskCompleted)
            {
                var result = parameters.GetValue<string>("result");
                StatusLabel.Text = $"Task completed: {result}";
            }
        }
    }
}
```

---

## More Examples

For more examples, check out the [sample application](https://github.com/mohammedmsadiq/Plugin.Maui.NavigationAware/tree/main/samples) in the repository.
