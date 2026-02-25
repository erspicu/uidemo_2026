using System.Windows.Controls;
using WpfDemo.ViewModels;

namespace WpfDemo.Views;

public partial class DashboardPage : Page
{
    public DashboardPage()
    {
        InitializeComponent();
        var vm = new DashboardViewModel();
        DataContext = vm;
        ActivityList.ItemsSource = vm.RecentActivity;
    }
}
