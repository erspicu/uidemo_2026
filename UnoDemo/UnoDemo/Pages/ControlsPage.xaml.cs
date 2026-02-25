using UnoDemo.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace UnoDemo.Pages;

public sealed partial class ControlsPage : Page
{
    public ControlsPage()
    {
        InitializeComponent();
        DataContext = new ControlsViewModel();
    }
}
