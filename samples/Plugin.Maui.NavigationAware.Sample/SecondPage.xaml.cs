using Plugin.Maui.NavigationAware;

namespace Plugin.Maui.NavigationAware.Sample;

public partial class SecondPage : NavigationAwarePage
{
	public SecondPage()
	{
		InitializeComponent();
	}

	public override void OnNavigatedTo(INavigationParameters parameters)
	{
		base.OnNavigatedTo(parameters);
		// Handle incoming navigation
		StatusLabel.Text = "SecondPage: Navigated To";
		
		// Access parameters if provided
		if (parameters.TryGetValue<string>("message", out var message))
		{
			StatusLabel.Text += $"\nReceived: {message}";
		}
		
		if (parameters.TryGetValue<DateTime>("timestamp", out var timestamp))
		{
			StatusLabel.Text += $"\nTimestamp: {timestamp:HH:mm:ss}";
		}
	}

	public override void OnNavigatedFrom(INavigationParameters parameters)
	{
		base.OnNavigatedFrom(parameters);
		// Handle outgoing navigation
		StatusLabel.Text = "SecondPage: Navigated From";
	}

	private async void OnGoBackClicked(object sender, EventArgs e)
	{
		var navigationService = this.GetNavigationService();
		var parameters = new NavigationParameters
		{
			{ "message", "Returning from SecondPage" }
		};
		
		await navigationService.GoBackAsync(parameters);
	}
}
