using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Plugin.Maui.NavigationAware.Sample;

/// <summary>
/// ViewModel for SecondPage demonstrating ViewModel Locator
/// </summary>
public class SecondPageViewModel : INotifyPropertyChanged
{
    private string _statusMessage = "SecondPage ViewModel Loaded";
    private string _receivedMessage = string.Empty;
    private DateTime? _receivedTimestamp;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            if (_statusMessage != value)
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }
    }

    public string ReceivedMessage
    {
        get => _receivedMessage;
        set
        {
            if (_receivedMessage != value)
            {
                _receivedMessage = value;
                OnPropertyChanged();
                UpdateStatusMessage();
            }
        }
    }

    public DateTime? ReceivedTimestamp
    {
        get => _receivedTimestamp;
        set
        {
            if (_receivedTimestamp != value)
            {
                _receivedTimestamp = value;
                OnPropertyChanged();
                UpdateStatusMessage();
            }
        }
    }

    public ICommand GoBackCommand { get; }

    public SecondPageViewModel()
    {
        GoBackCommand = new Command(async () => await OnGoBack());
    }

    private void UpdateStatusMessage()
    {
        var message = "SecondPage ViewModel";
        if (!string.IsNullOrEmpty(ReceivedMessage))
        {
            message += $"\nReceived: {ReceivedMessage}";
        }
        if (ReceivedTimestamp.HasValue)
        {
            message += $"\nTimestamp: {ReceivedTimestamp.Value:HH:mm:ss}";
        }
        StatusMessage = message;
    }

    private async Task OnGoBack()
    {
        // This will be wired up through the view
        StatusMessage = "Going back...";
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
