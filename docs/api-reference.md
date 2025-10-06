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
    Task GoBackAsync(INavigationParameters? parameters = null);
}
```

#### Methods

##### NavigateToAsync

Navigate to a page.

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
    public Task GoBackAsync(INavigationParameters? parameters = null);
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

## Extension Methods

### NavigationExtensions

Extension methods for navigation awareness.

```csharp
public static class NavigationExtensions
{
    public static IServiceCollection AddNavigationAware(this IServiceCollection services);
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

**Example:**
```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder.Services.AddNavigationAware();
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
