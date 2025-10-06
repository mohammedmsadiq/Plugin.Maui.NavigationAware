# API Reference

Complete API reference for Plugin.Maui.NavigationAware.

## Namespaces

- `Plugin.Maui.NavigationAware`

## Interfaces

### INavigationAware

Interface for pages that need to be notified of navigation events.

```csharp
public interface INavigationAware
{
    void OnNavigatedTo(INavigationParameters parameters);
    void OnNavigatedFrom(INavigationParameters parameters);
}
```

#### Methods

##### OnNavigatedTo

Called when the page is being navigated to.

```csharp
void OnNavigatedTo(INavigationParameters parameters)
```

**Parameters:**
- `parameters` (INavigationParameters): Navigation parameters passed to this page

**Remarks:**
Override this method to handle incoming navigation and access any parameters passed during navigation.

##### OnNavigatedFrom

Called when navigating away from this page.

```csharp
void OnNavigatedFrom(INavigationParameters parameters)
```

**Parameters:**
- `parameters` (INavigationParameters): Navigation parameters passed during navigation

**Remarks:**
Override this method to handle outgoing navigation, save state, or clean up resources.

---

### INavigationParameters

Represents navigation parameters passed during navigation events.

```csharp
public interface INavigationParameters : IDictionary<string, object>
{
    T? GetValue<T>(string key);
    bool TryGetValue<T>(string key, out T? value);
}
```

#### Methods

##### GetValue&lt;T&gt;

Gets the value associated with the specified key.

```csharp
T? GetValue<T>(string key)
```

**Type Parameters:**
- `T`: The type of the value

**Parameters:**
- `key` (string): The key of the value to get

**Returns:**
The value associated with the key, or default if not found.

**Example:**
```csharp
var userId = parameters.GetValue<int>("userId");
var message = parameters.GetValue<string>("message");
```

##### TryGetValue&lt;T&gt;

Tries to get the value associated with the specified key.

```csharp
bool TryGetValue<T>(string key, out T? value)
```

**Type Parameters:**
- `T`: The type of the value

**Parameters:**
- `key` (string): The key of the value to get
- `value` (T): The value associated with the key

**Returns:**
True if the value was found, false otherwise.

**Example:**
```csharp
if (parameters.TryGetValue<string>("message", out var message))
{
    Console.WriteLine(message);
}
```

---

### INavigationService

Service for performing navigation operations.

```csharp
public interface INavigationService
{
    Task NavigateToAsync(Page page, INavigationParameters? parameters = null);
    Task NavigateToAsync(string pageKey, INavigationParameters? parameters = null);
    Task GoBackAsync(INavigationParameters? parameters = null);
    Task NavigateAsync(string uri, INavigationParameters? parameters = null);
    Task GoBackToAsync(string pageKey, INavigationParameters? parameters = null);
    Task GoBackToRootAsync(INavigationParameters? parameters = null);
}
```

#### Methods

##### NavigateToAsync(Page, INavigationParameters?)

Navigate to a page using a page instance.

```csharp
Task NavigateToAsync(Page page, INavigationParameters? parameters = null)
```

**Parameters:**
- `page` (Page): The page to navigate to
- `parameters` (INavigationParameters, optional): Navigation parameters

**Returns:**
Task representing the navigation operation.

**Example:**
```csharp
var navigationService = this.GetNavigationService();
await navigationService.NavigateToAsync(
    new DetailsPage(), 
    new NavigationParameters { { "id", 123 } }
);
```

##### NavigateToAsync(string, INavigationParameters?)

Navigate to a page using its registered key.

```csharp
Task NavigateToAsync(string pageKey, INavigationParameters? parameters = null)
```

**Parameters:**
- `pageKey` (string): The key of the page to navigate to
- `parameters` (INavigationParameters, optional): Navigation parameters

**Returns:**
Task representing the navigation operation.

**Exceptions:**
- ArgumentException: Thrown when pageKey is null or empty
- InvalidOperationException: Thrown when the page key is not registered

**Remarks:**
Pages must be registered using `PageRegistry.Register<TPage>()` or `services.RegisterPage<TPage>()` before using string-based navigation.

**Example:**
```csharp
var navigationService = this.GetNavigationService();

// Using page name
await navigationService.NavigateToAsync(
    "DetailsPage",
    new NavigationParameters { { "id", 123 } }
);

// Using nameof for type safety
await navigationService.NavigateToAsync(
    nameof(DetailsPage),
    new NavigationParameters { { "id", 123 } }
);
```

##### GoBackAsync

Navigate back.

```csharp
Task GoBackAsync(INavigationParameters? parameters = null)
```

**Parameters:**
- `parameters` (INavigationParameters, optional): Navigation parameters

**Returns:**
Task representing the navigation operation.

**Example:**
```csharp
var navigationService = this.GetNavigationService();
await navigationService.GoBackAsync(
    new NavigationParameters { { "result", "success" } }
);
```

##### NavigateAsync

Navigate to a page using a URI path.

```csharp
Task NavigateAsync(string uri, INavigationParameters? parameters = null)
```

**Parameters:**
- `uri` (string): The URI path representing the navigation hierarchy (e.g., "/NavigationPage/DetailPage")
- `parameters` (INavigationParameters, optional): Navigation parameters

**Returns:**
Task representing the navigation operation.

**Exceptions:**
- ArgumentException: Thrown when uri is null or empty
- InvalidOperationException: Thrown when a page in the URI is not registered

**Remarks:**
The URI is parsed and each segment is treated as a page key. Pages are navigated to in sequence.

**Example:**
```csharp
var navigationService = this.GetNavigationService();
await navigationService.NavigateAsync("/NavigationPage/MasterTabbedPage");
```

##### GoBackToAsync

Navigate back to a specific page in the navigation stack.

```csharp
Task GoBackToAsync(string pageKey, INavigationParameters? parameters = null)
```

**Parameters:**
- `pageKey` (string): The key of the page to navigate back to
- `parameters` (INavigationParameters, optional): Navigation parameters

**Returns:**
Task representing the navigation operation.

**Exceptions:**
- ArgumentException: Thrown when pageKey is null or empty
- InvalidOperationException: Thrown when the page is not found in the navigation stack

**Remarks:**
Pops all pages from the navigation stack until the specified page is reached.

**Example:**
```csharp
var navigationService = this.GetNavigationService();
await navigationService.GoBackToAsync("MainPage");
```

##### GoBackToRootAsync

Navigate back to the root page.

```csharp
Task GoBackToRootAsync(INavigationParameters? parameters = null)
```

**Parameters:**
- `parameters` (INavigationParameters, optional): Navigation parameters

**Returns:**
Task representing the navigation operation.

**Remarks:**
Pops all pages from the navigation stack, returning to the root page.

**Example:**
```csharp
var navigationService = this.GetNavigationService();
await navigationService.GoBackToRootAsync();
```

---

## Classes

### NavigationAwarePage

Base ContentPage implementation that provides navigation awareness.

```csharp
public abstract class NavigationAwarePage : ContentPage, INavigationAware
{
    public virtual void OnNavigatedTo(INavigationParameters parameters);
    public virtual void OnNavigatedFrom(INavigationParameters parameters);
    protected override void OnAppearing();
    protected override void OnDisappearing();
}
```

#### Inheritance
- `ContentPage` → `NavigationAwarePage`
- Implements: `INavigationAware`

#### Methods

##### OnNavigatedTo

Called when the page is being navigated to.

```csharp
public virtual void OnNavigatedTo(INavigationParameters parameters)
```

**Parameters:**
- `parameters` (INavigationParameters): Navigation parameters passed to this page

**Remarks:**
Override this method to handle incoming navigation. The base implementation does nothing.

**Example:**
```csharp
public override void OnNavigatedTo(INavigationParameters parameters)
{
    base.OnNavigatedTo(parameters);
    
    if (parameters.TryGetValue<int>("userId", out var userId))
    {
        LoadUser(userId);
    }
}
```

##### OnNavigatedFrom

Called when navigating away from this page.

```csharp
public virtual void OnNavigatedFrom(INavigationParameters parameters)
```

**Parameters:**
- `parameters` (INavigationParameters): Navigation parameters

**Remarks:**
Override this method to handle outgoing navigation. The base implementation does nothing.

**Example:**
```csharp
public override void OnNavigatedFrom(INavigationParameters parameters)
{
    base.OnNavigatedFrom(parameters);
    SaveState();
}
```

##### OnAppearing

Called when the page is about to appear.

```csharp
protected override void OnAppearing()
```

**Remarks:**
Automatically triggers `OnNavigatedTo` with empty parameters. You should not override this unless you need custom behavior.

##### OnDisappearing

Called when the page is about to disappear.

```csharp
protected override void OnDisappearing()
```

**Remarks:**
Automatically triggers `OnNavigatedFrom` with empty parameters. You should not override this unless you need custom behavior.

---

### NavigationParameters

Default implementation of INavigationParameters.

```csharp
public class NavigationParameters : INavigationParameters
{
    public NavigationParameters();
    public NavigationParameters(IDictionary<string, object>? parameters);
}
```

#### Constructors

##### NavigationParameters()

Initializes a new instance of NavigationParameters.

```csharp
public NavigationParameters()
```

**Example:**
```csharp
var parameters = new NavigationParameters();
parameters["userId"] = 123;
parameters["message"] = "Hello";
```

##### NavigationParameters(IDictionary&lt;string, object&gt;?)

Initializes a new instance of NavigationParameters with initial values.

```csharp
public NavigationParameters(IDictionary<string, object>? parameters)
```

**Parameters:**
- `parameters` (IDictionary<string, object>): Initial parameters

**Example:**
```csharp
var dict = new Dictionary<string, object>
{
    { "userId", 123 },
    { "message", "Hello" }
};
var parameters = new NavigationParameters(dict);
```

#### Collection Initialization

You can use collection initializer syntax:

```csharp
var parameters = new NavigationParameters
{
    { "userId", 123 },
    { "message", "Hello" },
    { "timestamp", DateTime.Now }
};
```

---

### NavigationService

Default implementation of INavigationService.

```csharp
public class NavigationService : INavigationService
{
    public NavigationService(INavigation navigation);
    public Task NavigateToAsync(Page page, INavigationParameters? parameters = null);
    public Task NavigateToAsync(string pageKey, INavigationParameters? parameters = null);
    public Task GoBackAsync(INavigationParameters? parameters = null);
    public Task NavigateAsync(string uri, INavigationParameters? parameters = null);
    public Task GoBackToAsync(string pageKey, INavigationParameters? parameters = null);
    public Task GoBackToRootAsync(INavigationParameters? parameters = null);
}
```

#### Constructors

##### NavigationService(INavigation)

Initializes a new instance of NavigationService.

```csharp
public NavigationService(INavigation navigation)
```

**Parameters:**
- `navigation` (INavigation): The MAUI navigation object

**Remarks:**
Usually you don't create this directly. Use the `GetNavigationService()` extension method instead.

---

### PageRegistry

Static registry for page types to enable string-based navigation.

```csharp
public static class PageRegistry
{
    public static void SetServiceProvider(IServiceProvider serviceProvider);
    public static void Register<TPage>(string? key = null) where TPage : Page;
    public static void Register(Type pageType, string? key = null);
    public static Page CreatePage(string key);
    public static bool IsRegistered(string key);
    public static Type? GetPageType(string key);
    public static void Clear();
}
```

#### Methods

##### SetServiceProvider

Sets the service provider for page resolution.

```csharp
public static void SetServiceProvider(IServiceProvider serviceProvider)
```

**Parameters:**
- `serviceProvider` (IServiceProvider): The service provider to use for resolving pages

**Remarks:**
This is automatically called by `AddNavigationAware()`. You usually don't need to call this directly.

##### Register&lt;TPage&gt;

Registers a page type with an optional key.

```csharp
public static void Register<TPage>(string? key = null) where TPage : Page
```

**Type Parameters:**
- `TPage`: The page type to register

**Parameters:**
- `key` (string, optional): The key to register the page with. If null, uses the type name.

**Example:**
```csharp
// Register with type name as key
PageRegistry.Register<MainPage>();

// Register with custom key
PageRegistry.Register<DetailsPage>("ProductDetails");
```

##### Register(Type, string?)

Registers a page type with an optional key (non-generic version).

```csharp
public static void Register(Type pageType, string? key = null)
```

**Parameters:**
- `pageType` (Type): The page type to register. Must inherit from Page.
- `key` (string, optional): The key to register the page with. If null, uses the type name.

**Exceptions:**
- ArgumentException: Thrown when pageType does not inherit from Page

##### CreatePage

Creates an instance of a page from its registered key.

```csharp
public static Page CreatePage(string key)
```

**Parameters:**
- `key` (string): The key of the page to create

**Returns:**
A new instance of the page.

**Exceptions:**
- InvalidOperationException: Thrown when the page key is not registered or page cannot be instantiated

**Remarks:**
Attempts to resolve the page using the service provider first. If that fails, uses Activator.CreateInstance.

##### IsRegistered

Checks if a page key is registered.

```csharp
public static bool IsRegistered(string key)
```

**Parameters:**
- `key` (string): The key to check

**Returns:**
True if the key is registered, false otherwise.

##### GetPageType

Gets the page type for a registered key.

```csharp
public static Type? GetPageType(string key)
```

**Parameters:**
- `key` (string): The key to look up

**Returns:**
The page type if found, null otherwise.

**Example:**
```csharp
var pageType = PageRegistry.GetPageType("MainPage");
if (pageType != null)
{
    // Page is registered
}
```

##### Clear

Clears all registered pages.

```csharp
public static void Clear()
```

**Remarks:**
This is mainly useful for testing scenarios.

---

### ViewModelLocator

Provides attached properties for automatic ViewModel location and binding in XAML.

```csharp
public static class ViewModelLocator
{
    public static readonly BindableProperty AutoWireViewModelProperty;
    public static readonly BindableProperty ViewModelTypeProperty;
    
    public static bool GetAutoWireViewModel(BindableObject bindable);
    public static void SetAutoWireViewModel(BindableObject bindable, bool value);
    public static Type? GetViewModelType(BindableObject bindable);
    public static void SetViewModelType(BindableObject bindable, Type? value);
}
```

#### Attached Properties

##### AutoWireViewModelProperty

Attached property to enable automatic ViewModel wiring.

**Usage in XAML:**
```xml
<local:NavigationAwarePage 
    xmlns:nav="clr-namespace:Plugin.Maui.NavigationAware;assembly=Plugin.Maui.NavigationAware"
    nav:ViewModelLocator.AutoWireViewModel="True">
    <!-- Content -->
</local:NavigationAwarePage>
```

**Remarks:**
When set to true, automatically resolves and binds a ViewModel to the View's BindingContext using convention-based naming or explicit registration.

##### ViewModelTypeProperty

Attached property to specify a specific ViewModel type.

**Usage in XAML:**
```xml
<local:NavigationAwarePage 
    xmlns:nav="clr-namespace:Plugin.Maui.NavigationAware;assembly=Plugin.Maui.NavigationAware"
    xmlns:vm="clr-namespace:MyApp.ViewModels"
    nav:ViewModelLocator.ViewModelType="{x:Type vm:CustomViewModel}">
    <!-- Content -->
</local:NavigationAwarePage>
```

**Remarks:**
Allows you to explicitly specify which ViewModel type to use instead of relying on convention-based resolution.

#### Methods

##### GetAutoWireViewModel

Gets the AutoWireViewModel attached property value.

```csharp
public static bool GetAutoWireViewModel(BindableObject bindable)
```

**Parameters:**
- `bindable` (BindableObject): The bindable object

**Returns:**
True if auto-wiring is enabled, false otherwise.

##### SetAutoWireViewModel

Sets the AutoWireViewModel attached property value.

```csharp
public static void SetAutoWireViewModel(BindableObject bindable, bool value)
```

**Parameters:**
- `bindable` (BindableObject): The bindable object
- `value` (bool): True to enable auto-wiring, false otherwise

##### GetViewModelType

Gets the ViewModelType attached property value.

```csharp
public static Type? GetViewModelType(BindableObject bindable)
```

**Parameters:**
- `bindable` (BindableObject): The bindable object

**Returns:**
The ViewModel type, or null if not set.

##### SetViewModelType

Sets the ViewModelType attached property value.

```csharp
public static void SetViewModelType(BindableObject bindable, Type? value)
```

**Parameters:**
- `bindable` (BindableObject): The bindable object
- `value` (Type, optional): The ViewModel type

---

### ViewModelLocationProvider

Provides a mechanism for resolving ViewModels based on Views using naming conventions.

```csharp
public static class ViewModelLocationProvider
{
    public static Func<Type, Type?> DefaultViewTypeToViewModelTypeResolver { get; set; }
    
    public static void SetServiceProvider(IServiceProvider serviceProvider);
    public static void Register<TView, TViewModel>() where TView : BindableObject;
    public static void Register(Type viewType, Type viewModelType);
    public static void Register<TView>(Func<object> factory) where TView : BindableObject;
    public static void AutoWireViewModel(BindableObject view, Type? viewModelType = null);
    public static void Clear();
}
```

#### Properties

##### DefaultViewTypeToViewModelTypeResolver

Default convention for resolving ViewModel type names from View type names.

```csharp
public static Func<Type, Type?> DefaultViewTypeToViewModelTypeResolver { get; set; }
```

**Default Behavior:**
- `MainPage` → `MainPageViewModel`
- `MainView` → `MainViewModel`
- `DetailsPage` → `DetailsPageViewModel`

**Example:**
```csharp
ViewModelLocationProvider.DefaultViewTypeToViewModelTypeResolver = viewType =>
{
    var viewName = viewType.Name;
    var viewModelTypeName = $"{viewType.Namespace}.ViewModels.{viewName}ViewModel";
    return viewType.Assembly.GetType(viewModelTypeName);
};
```

#### Methods

##### SetServiceProvider

Sets the service provider for ViewModel resolution.

```csharp
public static void SetServiceProvider(IServiceProvider serviceProvider)
```

**Parameters:**
- `serviceProvider` (IServiceProvider): The service provider

**Remarks:**
Called automatically by `AddNavigationAware()`. ViewModels will be resolved from the service provider if registered.

##### Register&lt;TView, TViewModel&gt;

Registers a ViewModel type for a specific View type.

```csharp
public static void Register<TView, TViewModel>() where TView : BindableObject
```

**Type Parameters:**
- `TView`: The View type
- `TViewModel`: The ViewModel type

**Example:**
```csharp
ViewModelLocationProvider.Register<MainPage, MainPageViewModel>();
```

##### Register(Type, Type)

Registers a ViewModel type for a specific View type (non-generic version).

```csharp
public static void Register(Type viewType, Type viewModelType)
```

**Parameters:**
- `viewType` (Type): The View type. Must inherit from BindableObject.
- `viewModelType` (Type): The ViewModel type

**Exceptions:**
- ArgumentException: Thrown when viewType does not inherit from BindableObject

##### Register&lt;TView&gt;(Func&lt;object&gt;)

Registers a factory method for creating ViewModels for a specific View type.

```csharp
public static void Register<TView>(Func<object> factory) where TView : BindableObject
```

**Type Parameters:**
- `TView`: The View type

**Parameters:**
- `factory` (Func&lt;object&gt;): Factory method to create the ViewModel

**Example:**
```csharp
ViewModelLocationProvider.Register<DetailsPage>(() => 
    new DetailsPageViewModel(serviceLocator.Get<IDataService>()));
```

##### AutoWireViewModel

Automatically wires up the ViewModel for a View.

```csharp
public static void AutoWireViewModel(BindableObject view, Type? viewModelType = null)
```

**Parameters:**
- `view` (BindableObject): The view to wire up
- `viewModelType` (Type, optional): Optional specific ViewModel type to use

**Remarks:**
Resolves the ViewModel using the following order:
1. Registered factory (if exists)
2. Explicitly registered ViewModel type
3. Convention-based resolution using DefaultViewTypeToViewModelTypeResolver
4. Service provider resolution
5. Activator.CreateInstance

**Example:**
```csharp
ViewModelLocationProvider.AutoWireViewModel(this);
// or
ViewModelLocationProvider.AutoWireViewModel(this, typeof(CustomViewModel));
```

##### Clear

Clears all registered ViewModels and factories.

```csharp
public static void Clear()
```

**Remarks:**
This is mainly useful for testing scenarios.

---

## Extension Methods

### NavigationExtensions

Extension methods for navigation awareness.

```csharp
public static class NavigationExtensions
{
    public static IServiceCollection AddNavigationAware(this IServiceCollection services);
    public static IServiceCollection RegisterPage<TPage>(this IServiceCollection services, string? key = null) where TPage : Page;
    public static IServiceCollection RegisterViewModel<TView, TViewModel>(this IServiceCollection services) where TView : BindableObject;
    public static IServiceCollection RegisterViewModel<TView>(this IServiceCollection services, Func<IServiceProvider, object> factory) where TView : BindableObject;
    public static INavigationService GetNavigationService(this Page page);
}
```

#### Methods

##### AddNavigationAware

Registers navigation aware services with the service collection.

```csharp
public static IServiceCollection AddNavigationAware(this IServiceCollection services)
```

**Parameters:**
- `services` (IServiceCollection): The service collection

**Returns:**
The service collection for chaining.

**Remarks:**
This also sets the service provider on PageRegistry for DI-based page resolution.

**Example:**
```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder.Services.AddNavigationAware();
    return builder.Build();
}
```

##### RegisterPage&lt;TPage&gt;

Registers a page type for string-based navigation.

```csharp
public static IServiceCollection RegisterPage<TPage>(this IServiceCollection services, string? key = null) where TPage : Page
```

**Type Parameters:**
- `TPage`: The page type to register

**Parameters:**
- `services` (IServiceCollection): The service collection
- `key` (string, optional): The key to register the page with. If null, uses the type name.

**Returns:**
The service collection for chaining.

**Example:**
```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    
    // Register pages for string-based navigation
    builder.Services.RegisterPage<MainPage>();
    builder.Services.RegisterPage<DetailsPage>();
    
    // Register with custom key
    builder.Services.RegisterPage<DetailsPage>("ProductDetails");
    
    return builder.Build();
}
```

##### RegisterViewModel&lt;TView, TViewModel&gt;

Registers a ViewModel for a specific View type.

```csharp
public static IServiceCollection RegisterViewModel<TView, TViewModel>(this IServiceCollection services) 
    where TView : BindableObject
```

**Type Parameters:**
- `TView`: The View type
- `TViewModel`: The ViewModel type

**Parameters:**
- `services` (IServiceCollection): The service collection

**Returns:**
The service collection for chaining.

**Example:**
```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    
    // Register pages and ViewModels
    builder.Services.AddTransient<MainPage>();
    builder.Services.AddTransient<MainPageViewModel>();
    
    // Register ViewModel mapping
    builder.Services.RegisterViewModel<MainPage, MainPageViewModel>();
    
    return builder.Build();
}
```

##### RegisterViewModel&lt;TView&gt;(Func)

Registers a ViewModel for a specific View type using a factory method.

```csharp
public static IServiceCollection RegisterViewModel<TView>(
    this IServiceCollection services, 
    Func<IServiceProvider, object> factory) 
    where TView : BindableObject
```

**Type Parameters:**
- `TView`: The View type

**Parameters:**
- `services` (IServiceCollection): The service collection
- `factory` (Func&lt;IServiceProvider, object&gt;): Factory method to create the ViewModel

**Returns:**
The service collection for chaining.

**Example:**
```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    
    // Register ViewModel with dependencies
    builder.Services.RegisterViewModel<DetailsPage>(sp => 
        new DetailsPageViewModel(
            sp.GetRequiredService<IDataService>(),
            sp.GetRequiredService<INavigationService>()
        ));
    
    return builder.Build();
}
```

##### GetNavigationService

Creates a navigation service from a page's navigation.

```csharp
public static INavigationService GetNavigationService(this Page page)
```

**Parameters:**
- `page` (Page): The page

**Returns:**
A navigation service.

**Example:**
```csharp
var navigationService = this.GetNavigationService();
await navigationService.NavigateToAsync(new DetailsPage());
```

---

## Obsolete Members

The following members are obsolete and maintained only for backwards compatibility:

### IFeature (Obsolete)

```csharp
[Obsolete("This interface is deprecated. Use INavigationAware and related interfaces instead.")]
public interface IFeature
```

### Feature (Obsolete)

```csharp
[Obsolete("This class is deprecated. Use NavigationService and INavigationAware instead.")]
public static class Feature
```

**Migration:** Use `INavigationAware`, `INavigationService`, and related types instead.
