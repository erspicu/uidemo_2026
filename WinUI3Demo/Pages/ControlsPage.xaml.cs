using WinUI3Demo.ViewModels;

namespace WinUI3Demo.Pages;

public sealed partial class ControlsPage : Microsoft.UI.Xaml.Controls.Page
{
    public ControlsViewModel ViewModel { get; } = new();

    public ControlsPage()
    {
        InitializeComponent();
    }
}
