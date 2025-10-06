namespace Plugin.Maui.NavigationAware;

/// <summary>
/// Extension methods for navigation awareness
/// </summary>
public static class NavigationExtensions
{
    /// <summary>
    /// Registers navigation aware services with the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddNavigationAware(this IServiceCollection services)
    {
        services.AddSingleton<INavigationService, NavigationService>(sp =>
        {
            var navigation = Application.Current?.MainPage?.Navigation 
                ?? throw new InvalidOperationException("Navigation not available");
            return new NavigationService(navigation);
        });

        return services;
    }

    /// <summary>
    /// Creates a navigation service from a page's navigation
    /// </summary>
    /// <param name="page">The page</param>
    /// <returns>A navigation service</returns>
    public static INavigationService GetNavigationService(this Page page)
    {
        if (page == null)
            throw new ArgumentNullException(nameof(page));

        return new NavigationService(page.Navigation);
    }
}
