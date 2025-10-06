using Plugin.Maui.NavigationAware;

namespace Plugin.Maui.NavigationAware.Sample;

public partial class MainPage : NavigationAwarePage
{
	public MainPage()
	{
		InitializeComponent();
	}

	public override void OnNavigatedTo(INavigationParameters parameters)
	{
		base.OnNavigatedTo(parameters);
		// Handle incoming navigation
		StatusLabel.Text = "MainPage: Navigated To";
		
		// Access parameters if provided
		if (parameters.TryGetValue<string>("message", out var message))
		{
			StatusLabel.Text += $"\nReceived: {message}";
		}
	}

	public override void OnNavigatedFrom(INavigationParameters parameters)
	{
		base.OnNavigatedFrom(parameters);
		// Handle outgoing navigation
		StatusLabel.Text = "MainPage: Navigated From";
	}

	private async void OnNavigateClicked(object sender, EventArgs e)
	{
		var navigationService = this.GetNavigationService();
		var parameters = new NavigationParameters
		{
			{ "message", "Hello from MainPage!" },
			{ "timestamp", DateTime.Now }
		};
		
		// Option 1: Navigate using page instance (original approach)
		// await navigationService.NavigateToAsync(new SecondPage(), parameters);
		
		// Option 2: Navigate using string-based navigation (new approach)
		await navigationService.NavigateToAsync("SecondPage", parameters);
	}

	private async void OnNavigateToViewModelPageClicked(object sender, EventArgs e)
	{
		var navigationService = this.GetNavigationService();
		var parameters = new NavigationParameters
		{
			{ "message", "Hello from MainPage with ViewModel!" },
			{ "timestamp", DateTime.Now }
		};
		
		await navigationService.NavigateToAsync("ThirdPage", parameters);
	}
}

