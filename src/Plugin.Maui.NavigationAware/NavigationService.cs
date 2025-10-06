namespace Plugin.Maui.NavigationAware;

/// <summary>
/// Default implementation of INavigationService
/// </summary>
public class NavigationService : INavigationService
{
    private readonly INavigation _navigation;

    /// <summary>
    /// Initializes a new instance of NavigationService
    /// </summary>
    /// <param name="navigation">The MAUI navigation object</param>
    public NavigationService(INavigation navigation)
    {
        _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
    }

    /// <inheritdoc/>
    public async Task NavigateToAsync(Page page, INavigationParameters? parameters = null)
    {
        if (page == null)
            throw new ArgumentNullException(nameof(page));

        // Notify current page of navigation away
        if (_navigation.NavigationStack.Count > 0)
        {
            var currentPage = _navigation.NavigationStack.Last();
            if (currentPage is INavigationAware currentNavigationAware)
            {
                currentNavigationAware.OnNavigatedFrom(parameters ?? new NavigationParameters());
            }
        }

        // Navigate to the new page
        await _navigation.PushAsync(page);

        // Notify new page of navigation to
        if (page is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedTo(parameters ?? new NavigationParameters());
        }
    }

    /// <inheritdoc/>
    public async Task NavigateToAsync(string pageKey, INavigationParameters? parameters = null)
    {
        if (string.IsNullOrWhiteSpace(pageKey))
            throw new ArgumentException("Page key cannot be null or empty", nameof(pageKey));

        var page = PageRegistry.CreatePage(pageKey);
        await NavigateToAsync(page, parameters);
    }

    /// <inheritdoc/>
    public async Task GoBackAsync(INavigationParameters? parameters = null)
    {
        if (_navigation.NavigationStack.Count > 0)
        {
            var currentPage = _navigation.NavigationStack.Last();
            if (currentPage is INavigationAware currentNavigationAware)
            {
                currentNavigationAware.OnNavigatedFrom(parameters ?? new NavigationParameters());
            }
        }

        await _navigation.PopAsync();

        if (_navigation.NavigationStack.Count > 0)
        {
            var previousPage = _navigation.NavigationStack.Last();
            if (previousPage is INavigationAware previousNavigationAware)
            {
                previousNavigationAware.OnNavigatedTo(parameters ?? new NavigationParameters());
            }
        }
    }
}
