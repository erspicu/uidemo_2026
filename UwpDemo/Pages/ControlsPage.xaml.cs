using UwpDemo.ViewModels;
using Windows.UI.Xaml.Controls;

namespace UwpDemo.Pages
{
    public sealed partial class ControlsPage : Page
    {
        public ControlsViewModel ViewModel { get; } = new ControlsViewModel();

        public ControlsPage()
        {
            InitializeComponent();
        }
    }
}
