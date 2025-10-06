using Plugin.Maui.NavigationAware;

namespace Plugin.Maui.NavigationAware.Sample;

public partial class ThirdPage : NavigationAwarePage
{
    public ThirdPage()
    {
        InitializeComponent();
    }

    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        
        // ViewModel is automatically bound via ViewModelLocator
        if (BindingContext is ThirdPageViewModel viewModel)
        {
            // Handle incoming navigation parameters
            if (parameters.TryGetValue<string>("message", out var message))
            {
                viewModel.ReceivedMessage = message;
            }
            
            if (parameters.TryGetValue<DateTime>("timestamp", out var timestamp))
            {
                viewModel.ReceivedTimestamp = timestamp;
            }
        }
    }

    public override void OnNavigatedFrom(INavigationParameters parameters)
    {
        base.OnNavigatedFrom(parameters);
    }

    private void OnUpdateStatusClicked(object sender, EventArgs e)
    {
        if (BindingContext is ThirdPageViewModel viewModel)
        {
            viewModel.StatusMessage = $"Updated: {viewModel.InputMessage}";
        }
    }

    private async void OnGoBackClicked(object sender, EventArgs e)
    {
        var navigationService = this.GetNavigationService();
        var parameters = new NavigationParameters
        {
            { "message", "Returning from ThirdPage with ViewModel" }
        };
        
        await navigationService.GoBackAsync(parameters);
    }
}
