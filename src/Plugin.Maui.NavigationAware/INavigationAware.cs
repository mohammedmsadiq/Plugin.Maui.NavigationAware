namespace Plugin.Maui.NavigationAware;

/// <summary>
/// Interface for pages/views that need to be notified of navigation events
/// </summary>
public interface INavigationAware
{
    /// <summary>
    /// Called when the page is being navigated to
    /// </summary>
    /// <param name="parameters">Navigation parameters passed to this page</param>
    void OnNavigatedTo(INavigationParameters parameters);

    /// <summary>
    /// Called when navigating away from this page
    /// </summary>
    /// <param name="parameters">Navigation parameters passed during navigation</param>
    void OnNavigatedFrom(INavigationParameters parameters);
}
