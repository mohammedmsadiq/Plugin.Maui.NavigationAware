namespace Plugin.Maui.NavigationAware;

/// <summary>
/// Provides a mechanism for resolving ViewModels based on Views using naming conventions
/// </summary>
public static class ViewModelLocationProvider
{
    private static readonly Dictionary<Type, Type> _viewModelTypeCache = new();
    private static readonly Dictionary<Type, Func<object>> _viewModelFactoryCache = new();
    private static IServiceProvider? _serviceProvider;
    
    /// <summary>
    /// Default convention for resolving ViewModel type names from View type names
    /// Converts "MainPage" to "MainPageViewModel" or "MainViewModel"
    /// </summary>
    public static Func<Type, Type?> DefaultViewTypeToViewModelTypeResolver { get; set; } = 
        viewType =>
        {
            var viewName = viewType.Name;
            var viewModelTypeName = viewType.Namespace + "." + viewName + "ViewModel";
            
            // Try with "ViewModel" suffix first
            var viewModelType = viewType.Assembly.GetType(viewModelTypeName);
            if (viewModelType != null)
                return viewModelType;
            
            // Try replacing "Page" with "ViewModel"
            if (viewName.EndsWith("Page"))
            {
                var baseName = viewName.Substring(0, viewName.Length - 4);
                viewModelTypeName = viewType.Namespace + "." + baseName + "ViewModel";
                viewModelType = viewType.Assembly.GetType(viewModelTypeName);
                if (viewModelType != null)
                    return viewModelType;
            }
            
            // Try replacing "View" with "ViewModel"
            if (viewName.EndsWith("View"))
            {
                var baseName = viewName.Substring(0, viewName.Length - 4);
                viewModelTypeName = viewType.Namespace + "." + baseName + "ViewModel";
                viewModelType = viewType.Assembly.GetType(viewModelTypeName);
                if (viewModelType != null)
                    return viewModelType;
            }
            
            return null;
        };

    /// <summary>
    /// Sets the service provider for ViewModel resolution
    /// </summary>
    /// <param name="serviceProvider">The service provider</param>
    public static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Registers a ViewModel type for a specific View type
    /// </summary>
    /// <typeparam name="TView">The View type</typeparam>
    /// <typeparam name="TViewModel">The ViewModel type</typeparam>
    public static void Register<TView, TViewModel>() 
        where TView : BindableObject
    {
        _viewModelTypeCache[typeof(TView)] = typeof(TViewModel);
    }

    /// <summary>
    /// Registers a ViewModel type for a specific View type
    /// </summary>
    /// <param name="viewType">The View type</param>
    /// <param name="viewModelType">The ViewModel type</param>
    public static void Register(Type viewType, Type viewModelType)
    {
        if (!typeof(BindableObject).IsAssignableFrom(viewType))
            throw new ArgumentException($"Type {viewType.Name} must inherit from BindableObject", nameof(viewType));

        _viewModelTypeCache[viewType] = viewModelType;
    }

    /// <summary>
    /// Registers a factory method for creating ViewModels for a specific View type
    /// </summary>
    /// <typeparam name="TView">The View type</typeparam>
    /// <param name="factory">Factory method to create the ViewModel</param>
    public static void Register<TView>(Func<object> factory) 
        where TView : BindableObject
    {
        _viewModelFactoryCache[typeof(TView)] = factory;
    }

    /// <summary>
    /// Automatically wires up the ViewModel for a View
    /// </summary>
    /// <param name="view">The view to wire up</param>
    /// <param name="viewModelType">Optional specific ViewModel type to use</param>
    public static void AutoWireViewModel(BindableObject view, Type? viewModelType = null)
    {
        if (view == null)
            throw new ArgumentNullException(nameof(view));

        var viewType = view.GetType();

        // Try factory first
        if (_viewModelFactoryCache.TryGetValue(viewType, out var factory))
        {
            SetBindingContext(view, factory());
            return;
        }

        // Determine ViewModel type
        Type? resolvedViewModelType = viewModelType;
        
        if (resolvedViewModelType == null && !_viewModelTypeCache.TryGetValue(viewType, out resolvedViewModelType))
        {
            resolvedViewModelType = DefaultViewTypeToViewModelTypeResolver(viewType);
        }

        if (resolvedViewModelType == null)
        {
            // No ViewModel found - Log a debug message to help developers
            System.Diagnostics.Debug.WriteLine(
                $"[ViewModelLocationProvider] Could not resolve ViewModel for View type '{viewType.FullName}'. " +
                $"Ensure the ViewModel follows naming conventions (e.g., {viewType.Name}ViewModel) or is explicitly registered.");
            return;
        }

        // Create ViewModel instance
        var viewModel = CreateViewModel(resolvedViewModelType);
        SetBindingContext(view, viewModel);
    }

    /// <summary>
    /// Creates an instance of the specified ViewModel type
    /// </summary>
    /// <param name="viewModelType">The ViewModel type to create</param>
    /// <returns>An instance of the ViewModel</returns>
    private static object CreateViewModel(Type viewModelType)
    {
        // Try to resolve from DI container first
        if (_serviceProvider != null)
        {
            var viewModel = _serviceProvider.GetService(viewModelType);
            if (viewModel != null)
                return viewModel;
        }

        // Fall back to Activator.CreateInstance
        try
        {
            return Activator.CreateInstance(viewModelType)!;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to create instance of ViewModel type '{viewModelType.Name}'. " +
                $"Ensure the ViewModel has a parameterless constructor or is registered in the DI container.", ex);
        }
    }

    /// <summary>
    /// Sets the BindingContext of a view
    /// </summary>
    /// <param name="view">The view</param>
    /// <param name="viewModel">The ViewModel to set as BindingContext</param>
    private static void SetBindingContext(BindableObject view, object viewModel)
    {
        if (view is Element element)
        {
            element.BindingContext = viewModel;
            System.Diagnostics.Debug.WriteLine(
                $"[ViewModelLocationProvider] Successfully set BindingContext for '{view.GetType().Name}' " +
                $"to ViewModel of type '{viewModel.GetType().Name}'");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine(
                $"[ViewModelLocationProvider] Warning: Cannot set BindingContext for type '{view.GetType().Name}' " +
                $"because it does not inherit from Element");
        }
    }

    /// <summary>
    /// Clears all registered ViewModels and factories
    /// </summary>
    public static void Clear()
    {
        _viewModelTypeCache.Clear();
        _viewModelFactoryCache.Clear();
    }
}
