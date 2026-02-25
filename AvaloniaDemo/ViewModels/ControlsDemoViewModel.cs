using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaDemo.ViewModels;

public partial class ControlsDemoViewModel : ViewModelBase
{
    [ObservableProperty] private double _sliderValue    = 60;
    [ObservableProperty] private double _progressValue  = 45;
    [ObservableProperty] private bool   _isToggled      = true;
    [ObservableProperty] private bool   _check1         = true;
    [ObservableProperty] private bool   _check2         = false;
    [ObservableProperty] private bool   _check3         = true;
    [ObservableProperty] private int    _selectedIndex  = 1;
    [ObservableProperty] private int    _selectedRadio  = 1;
    [ObservableProperty] private string _inputText      = "Hello, Avalonia!";
    [ObservableProperty] private string _statusMessage  = "👆 Click a button to see the result";

    public string[] ComboItems  { get; } = ["Option Alpha", "Option Beta", "Option Gamma", "Option Delta"];
    public string[] RadioLabels { get; } = ["Option A", "Option B", "Option C"];

    [RelayCommand] private void PrimaryAction()   => StatusMessage = "✅ Primary button clicked!";
    [RelayCommand] private void SecondaryAction() => StatusMessage = "🔘 Secondary button clicked!";
    [RelayCommand] private void DangerAction()    => StatusMessage = "⚠️ Danger button clicked!";

    [RelayCommand]
    private void ResetAll()
    {
        SliderValue    = 60;
        ProgressValue  = 45;
        IsToggled      = true;
        Check1         = true;
        Check2         = false;
        Check3         = true;
        InputText      = "Hello, Avalonia!";
        SelectedIndex  = 1;
        SelectedRadio  = 1;
        StatusMessage  = "🔄 All values reset!";
    }
}
