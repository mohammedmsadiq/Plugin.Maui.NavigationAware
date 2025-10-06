# Examples

This page provides practical examples of using Plugin.Maui.NavigationAware in various scenarios.

## Table of Contents

1. [Basic Navigation](#basic-navigation)
2. [Passing Multiple Parameters](#passing-multiple-parameters)
3. [Complex Object Parameters](#complex-object-parameters)
4. [Navigation with Return Values](#navigation-with-return-values)
5. [Master-Detail Navigation](#master-detail-navigation)
6. [Modal Navigation](#modal-navigation)
7. [Dependency Injection](#dependency-injection)

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

## More Examples

For more examples, check out the [sample application](https://github.com/mohammedmsadiq/Plugin.Maui.NavigationAware/tree/main/samples) in the repository.
