using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiDemo.ViewModels;

public class ControlsViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private void Set<T>(ref T field, T value, [CallerMemberName] string? n = null)
    { if (!Equals(field, value)) { field = value; PropertyChanged?.Invoke(this, new(n)); } }

    private double _sliderValue   = 60;
    private double _progressValue = 45;
    private bool   _isToggled     = true;
    private string _inputText     = "Hello, MAUI!";
    private string _statusMessage = "👆 Click a button to see the result";
    private int    _selectedIndex = 1;

    public double SliderValue   { get => _sliderValue;   set { Set(ref _sliderValue, value); } }
    public double ProgressValue { get => _progressValue; set => Set(ref _progressValue, value); }
    public bool   IsToggled     { get => _isToggled;     set => Set(ref _isToggled, value); }
    public string InputText     { get => _inputText;     set => Set(ref _inputText, value); }
    public string StatusMessage { get => _statusMessage; set => Set(ref _statusMessage, value); }
    public int    SelectedIndex { get => _selectedIndex; set => Set(ref _selectedIndex, value); }

    public string[] ComboItems { get; } = ["Option Alpha", "Option Beta", "Option Gamma", "Option Delta"];

    public ICommand PrimaryCommand   => new Command(() => StatusMessage = "✅ Primary button clicked!");
    public ICommand SecondaryCommand => new Command(() => StatusMessage = "🔘 Secondary button clicked!");
    public ICommand DangerCommand    => new Command(() => StatusMessage = "⚠️ Danger button clicked!");
    public ICommand ResetCommand     => new Command(() =>
    {
        SliderValue   = 60;
        ProgressValue = 45;
        IsToggled     = true;
        InputText     = "Hello, MAUI!";
        SelectedIndex = 1;
        StatusMessage = "🔄 All values reset!";
    });
}
