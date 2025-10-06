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
    /// Navigate back
    /// </summary>
    /// <param name="parameters">Navigation parameters</param>
    /// <returns>Task representing the navigation operation</returns>
    Task GoBackAsync(INavigationParameters? parameters = null);
}
