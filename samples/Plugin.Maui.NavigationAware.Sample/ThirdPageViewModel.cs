using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Plugin.Maui.NavigationAware.Sample;

/// <summary>
/// ViewModel for ThirdPage demonstrating automatic ViewModel binding via ViewModelLocator
/// </summary>
public class ThirdPageViewModel : INotifyPropertyChanged
{
    private string _statusMessage = "ThirdPage ViewModel Auto-Wired!";
    private string _inputMessage = string.Empty;
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

    public string InputMessage
    {
        get => _inputMessage;
        set
        {
            if (_inputMessage != value)
            {
                _inputMessage = value;
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

    public ThirdPageViewModel()
    {
        // Constructor - ViewModel is created automatically by ViewModelLocator
    }

    private void UpdateStatusMessage()
    {
        var message = "ThirdPage ViewModel Auto-Wired!";
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

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
