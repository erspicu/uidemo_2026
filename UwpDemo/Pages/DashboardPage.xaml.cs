using UwpDemo.ViewModels;
using Windows.UI.Xaml.Controls;

namespace UwpDemo.Pages
{
    public sealed partial class DashboardPage : Page
    {
        public DashboardViewModel ViewModel { get; } = new DashboardViewModel();

        public DashboardPage()
        {
            InitializeComponent();
        }
    }
}
