namespace Plugin.Maui.NavigationAware;

/// <summary>
/// Registry for page types to enable string-based navigation
/// </summary>
public static class PageRegistry
{
    private static readonly Dictionary<string, Type> _pageTypes = new();
    private static IServiceProvider? _serviceProvider;

    /// <summary>
    /// Sets the service provider for page resolution
    /// </summary>
    /// <param name="serviceProvider">The service provider</param>
    public static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Registers a page type with a key
    /// </summary>
    /// <typeparam name="TPage">The page type</typeparam>
    /// <param name="key">The key to register the page with (defaults to type name)</param>
    public static void Register<TPage>(string? key = null) where TPage : Page
    {
        var pageType = typeof(TPage);
        var registrationKey = key ?? pageType.Name;
        
        // Also register with the full name for better compatibility
        _pageTypes[registrationKey] = pageType;
        
        // If custom key was provided, also register with type name
        if (key != null && key != pageType.Name)
        {
            _pageTypes[pageType.Name] = pageType;
        }
    }

    /// <summary>
    /// Registers a page type with a key
    /// </summary>
    /// <param name="pageType">The page type</param>
    /// <param name="key">The key to register the page with (defaults to type name)</param>
    public static void Register(Type pageType, string? key = null)
    {
        if (!typeof(Page).IsAssignableFrom(pageType))
            throw new ArgumentException($"Type {pageType.Name} must inherit from Page", nameof(pageType));

        var registrationKey = key ?? pageType.Name;
        _pageTypes[registrationKey] = pageType;
    }

    /// <summary>
    /// Creates an instance of a page from its registered key
    /// </summary>
    /// <param name="key">The key of the page to create</param>
    /// <returns>A new instance of the page</returns>
    /// <exception cref="InvalidOperationException">Thrown when the page key is not registered</exception>
    public static Page CreatePage(string key)
    {
        if (!_pageTypes.TryGetValue(key, out var pageType))
        {
            throw new InvalidOperationException(
                $"Page with key '{key}' is not registered. Use PageRegistry.Register<TPage>() to register the page type.");
        }

        // Try to create using service provider first (if available)
        if (_serviceProvider != null)
        {
            var page = _serviceProvider.GetService(pageType) as Page;
            if (page != null)
                return page;
        }

        // Fall back to Activator.CreateInstance
        try
        {
            return (Page)Activator.CreateInstance(pageType)!;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to create instance of page type '{pageType.Name}'. " +
                $"Ensure the page has a parameterless constructor or is registered in the DI container.", ex);
        }
    }

    /// <summary>
    /// Checks if a page key is registered
    /// </summary>
    /// <param name="key">The key to check</param>
    /// <returns>True if the key is registered, false otherwise</returns>
    public static bool IsRegistered(string key)
    {
        return _pageTypes.ContainsKey(key);
    }

    /// <summary>
    /// Clears all registered pages
    /// </summary>
    public static void Clear()
    {
        _pageTypes.Clear();
    }
}
