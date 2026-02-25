using WinUI3Demo.ViewModels;

namespace WinUI3Demo.Pages;

public sealed partial class DashboardPage : Microsoft.UI.Xaml.Controls.Page
{
    public DashboardViewModel ViewModel { get; } = new();

    public DashboardPage()
    {
        InitializeComponent();
    }
}
