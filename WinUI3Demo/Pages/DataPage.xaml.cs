using WinUI3Demo.ViewModels;

namespace WinUI3Demo.Pages;

public sealed partial class DataPage : Microsoft.UI.Xaml.Controls.Page
{
    public DataViewModel ViewModel { get; } = new();

    public DataPage()
    {
        InitializeComponent();
        UpdateCountLabel();
    }

    private void SearchBox_TextChanged(object sender, Microsoft.UI.Xaml.Controls.TextChangedEventArgs e)
    {
        ViewModel.Search = SearchBox.Text;
        UpdateCountLabel();
    }

    private void UpdateCountLabel()
    {
        CountLabel.Text = $"{ViewModel.FilteredPeople.Count} of 8 records";
    }
}
