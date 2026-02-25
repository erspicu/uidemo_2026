using System.Windows.Controls;
using WpfDemo.ViewModels;

namespace WpfDemo.Views;

public partial class ControlsPage : Page
{
    public ControlsPage()
    {
        InitializeComponent();
        DataContext = new ControlsViewModel();
    }
}
