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

    /// <inheritdoc/>
    public async Task NavigateAsync(string uri, INavigationParameters? parameters = null)
    {
        if (string.IsNullOrWhiteSpace(uri))
            throw new ArgumentException("URI cannot be null or empty", nameof(uri));

        // Parse the URI and extract page segments
        var segments = uri.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        
        if (segments.Length == 0)
            throw new ArgumentException("URI must contain at least one page segment", nameof(uri));

        // Navigate to each page in the URI path
        foreach (var segment in segments)
        {
            var page = PageRegistry.CreatePage(segment);
            await NavigateToAsync(page, parameters);
        }
    }

    /// <inheritdoc/>
    public async Task GoBackToAsync(string pageKey, INavigationParameters? parameters = null)
    {
        if (string.IsNullOrWhiteSpace(pageKey))
            throw new ArgumentException("Page key cannot be null or empty", nameof(pageKey));

        // Find the page in the navigation stack
        var targetPageIndex = -1;
        var registeredPageType = PageRegistry.GetPageType(pageKey);
        
        for (int i = _navigation.NavigationStack.Count - 1; i >= 0; i--)
        {
            var page = _navigation.NavigationStack[i];
            var pageType = page.GetType();
            
            // Match by type name or registered type
            if (pageType.Name == pageKey || (registeredPageType != null && pageType == registeredPageType))
            {
                targetPageIndex = i;
                break;
            }
        }

        if (targetPageIndex == -1)
            throw new InvalidOperationException($"Page with key '{pageKey}' not found in navigation stack");

        // Pop all pages until we reach the target page
        var pagesToPop = _navigation.NavigationStack.Count - targetPageIndex - 1;
        
        if (pagesToPop == 0)
            return; // Already on the target page

        // Notify current page of navigation away
        if (_navigation.NavigationStack.Count > 0)
        {
            var currentPage = _navigation.NavigationStack.Last();
            if (currentPage is INavigationAware currentNavigationAware)
            {
                currentNavigationAware.OnNavigatedFrom(parameters ?? new NavigationParameters());
            }
        }

        // Pop pages to reach the target
        for (int i = 0; i < pagesToPop; i++)
        {
            await _navigation.PopAsync(false); // Animated = false for intermediate pops
        }

        // Notify target page of navigation to
        if (_navigation.NavigationStack.Count > 0)
        {
            var targetPage = _navigation.NavigationStack.Last();
            if (targetPage is INavigationAware targetNavigationAware)
            {
                targetNavigationAware.OnNavigatedTo(parameters ?? new NavigationParameters());
            }
        }
    }

    /// <inheritdoc/>
    public async Task GoBackToRootAsync(INavigationParameters? parameters = null)
    {
        if (_navigation.NavigationStack.Count <= 1)
            return; // Already at root or empty stack

        // Notify current page of navigation away
        if (_navigation.NavigationStack.Count > 0)
        {
            var currentPage = _navigation.NavigationStack.Last();
            if (currentPage is INavigationAware currentNavigationAware)
            {
                currentNavigationAware.OnNavigatedFrom(parameters ?? new NavigationParameters());
            }
        }

        // Pop to root
        await _navigation.PopToRootAsync();

        // Notify root page of navigation to
        if (_navigation.NavigationStack.Count > 0)
        {
            var rootPage = _navigation.NavigationStack.First();
            if (rootPage is INavigationAware rootNavigationAware)
            {
                rootNavigationAware.OnNavigatedTo(parameters ?? new NavigationParameters());
            }
        }
    }
}
