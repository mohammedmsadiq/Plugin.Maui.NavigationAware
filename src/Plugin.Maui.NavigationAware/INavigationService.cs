namespace Plugin.Maui.NavigationAware;

/// <summary>
/// Interface for navigation service
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Navigate to a page
    /// </summary>
    /// <param name="page">The page to navigate to</param>
    /// <param name="parameters">Navigation parameters</param>
    /// <returns>Task representing the navigation operation</returns>
    Task NavigateToAsync(Page page, INavigationParameters? parameters = null);

    /// <summary>
    /// Navigate to a page by its registered key
    /// </summary>
    /// <param name="pageKey">The key of the page to navigate to</param>
    /// <param name="parameters">Navigation parameters</param>
    /// <returns>Task representing the navigation operation</returns>
    Task NavigateToAsync(string pageKey, INavigationParameters? parameters = null);

    /// <summary>
    /// Navigate back
    /// </summary>
    /// <param name="parameters">Navigation parameters</param>
    /// <returns>Task representing the navigation operation</returns>
    Task GoBackAsync(INavigationParameters? parameters = null);

    /// <summary>
    /// Navigate to a page using a URI path (e.g., "/NavigationPage/DetailPage")
    /// </summary>
    /// <param name="uri">The URI path representing the navigation hierarchy</param>
    /// <param name="parameters">Navigation parameters</param>
    /// <returns>Task representing the navigation operation</returns>
    Task NavigateAsync(string uri, INavigationParameters? parameters = null);

    /// <summary>
    /// Navigate back to a specific page in the navigation stack
    /// </summary>
    /// <param name="pageKey">The key of the page to navigate back to</param>
    /// <param name="parameters">Navigation parameters</param>
    /// <returns>Task representing the navigation operation</returns>
    Task GoBackToAsync(string pageKey, INavigationParameters? parameters = null);

    /// <summary>
    /// Navigate back to the root page
    /// </summary>
    /// <param name="parameters">Navigation parameters</param>
    /// <returns>Task representing the navigation operation</returns>
    Task GoBackToRootAsync(INavigationParameters? parameters = null);
}
