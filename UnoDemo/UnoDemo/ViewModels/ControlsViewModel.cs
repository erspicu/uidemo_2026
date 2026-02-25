namespace UnoDemo.ViewModels;

public class ControlsViewModel : ViewModelBase
{
    private bool _toggleA = true;
    public bool ToggleA { get => _toggleA; set => SetProperty(ref _toggleA, value); }

    private bool _toggleB = false;
    public bool ToggleB { get => _toggleB; set => SetProperty(ref _toggleB, value); }

    private double _sliderValue = 65;
    public double SliderValue
    {
        get => _sliderValue;
        set { SetProperty(ref _sliderValue, value); OnPropertyChanged(nameof(SliderLabel)); }
    }
    public string SliderLabel => $"Value: {(int)SliderValue}";

    private int _selectedColorIndex = 1;
    public int SelectedColorIndex
    {
        get => _selectedColorIndex;
        set => SetProperty(ref _selectedColorIndex, value);
    }
}
