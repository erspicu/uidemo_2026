using UwpDemo.ViewModels;
using Windows.UI.Xaml.Controls;

namespace UwpDemo.Pages
{
    public sealed partial class DataPage : Page
    {
        public DataViewModel ViewModel { get; } = new DataViewModel();

        public DataPage()
        {
            InitializeComponent();
            UpdateCountLabel();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.Search = SearchBox.Text;
            UpdateCountLabel();
        }

        private void UpdateCountLabel()
        {
            CountLabel.Text = $"{ViewModel.FilteredPeople.Count} of 8 records";
        }
    }
}
