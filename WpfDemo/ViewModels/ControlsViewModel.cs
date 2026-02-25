using System.Windows.Input;
using WpfDemo.Helpers;

namespace WpfDemo.ViewModels;

public class ControlsViewModel : ViewModelBase
{
    private double _sliderValue   = 60;
    private double _progressValue = 45;
    private bool   _isToggled     = true;
    private string _inputText     = "Hello, WPF!";
    private string _statusMessage = "👆 Click a button to see the result";
    private int    _selectedIndex = 1;

    public double SliderValue   { get => _sliderValue;   set => Set(ref _sliderValue, value); }
    public double ProgressValue { get => _progressValue; set => Set(ref _progressValue, value); }
    public bool   IsToggled     { get => _isToggled;     set => Set(ref _isToggled, value); }
    public string InputText     { get => _inputText;     set => Set(ref _inputText, value); }
    public string StatusMessage { get => _statusMessage; set => Set(ref _statusMessage, value); }
    public int    SelectedIndex { get => _selectedIndex; set => Set(ref _selectedIndex, value); }

    public string[] ComboItems { get; } = ["Option Alpha", "Option Beta", "Option Gamma", "Option Delta"];

    public ICommand PrimaryCommand   => new RelayCommand(() => StatusMessage = "✅ Primary button clicked!");
    public ICommand SecondaryCommand => new RelayCommand(() => StatusMessage = "🔘 Secondary button clicked!");
    public ICommand DangerCommand    => new RelayCommand(() => StatusMessage = "⚠️ Danger button clicked!");
    public ICommand ResetCommand     => new RelayCommand(() =>
    {
        SliderValue   = 60;
        ProgressValue = 45;
        IsToggled     = true;
        InputText     = "Hello, WPF!";
        SelectedIndex = 1;
        StatusMessage = "🔄 All values reset!";
    });
}
