using UnoDemo.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace UnoDemo.Pages;

public sealed partial class DashboardPage : Page
{
    public DashboardPage()
    {
        InitializeComponent();
        DataContext = new DashboardViewModel();
    }
}
