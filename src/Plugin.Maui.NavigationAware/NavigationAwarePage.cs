namespace Plugin.Maui.NavigationAware;

/// <summary>
/// Base ContentPage implementation that provides navigation awareness
/// </summary>
public abstract class NavigationAwarePage : ContentPage, INavigationAware
{
    /// <summary>
    /// Called when the page is being navigated to.
    /// Override this method to handle incoming navigation.
    /// </summary>
    /// <param name="parameters">Navigation parameters passed to this page</param>
    public virtual void OnNavigatedTo(INavigationParameters parameters)
    {
        // Default implementation - override in derived classes
    }

    /// <summary>
    /// Called when navigating away from this page.
    /// Override this method to handle outgoing navigation.
    /// </summary>
    /// <param name="parameters">Navigation parameters passed during navigation</param>
    public virtual void OnNavigatedFrom(INavigationParameters parameters)
    {
        // Default implementation - override in derived classes
    }

    /// <inheritdoc/>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Trigger navigation awareness when page appears
        OnNavigatedTo(new NavigationParameters());
    }

    /// <inheritdoc/>
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Trigger navigation awareness when page disappears
        OnNavigatedFrom(new NavigationParameters());
    }
}
