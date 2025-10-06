using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Plugin.Maui.NavigationAware.Sample;

/// <summary>
/// ViewModel for MainPage demonstrating ViewModel Locator
/// </summary>
public class MainPageViewModel : INotifyPropertyChanged
{
    private string _statusMessage = "MainPage ViewModel Loaded";
    private string _inputMessage = string.Empty;

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

    public ICommand NavigateCommand { get; }

    public MainPageViewModel()
    {
        NavigateCommand = new Command(async () => await OnNavigate());
    }

    private async Task OnNavigate()
    {
        // This will be wired up through the view
        StatusMessage = "Navigating...";
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
