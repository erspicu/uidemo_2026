using UnoDemo.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace UnoDemo.Pages;

public sealed partial class DataPage : Page
{
    private readonly DataViewModel _vm = new();

    public DataPage()
    {
        InitializeComponent();
        DataContext = _vm;
        CountLabel.Text = $"{_vm.FilteredPeople.Count} of 8 records";
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        _vm.Search = SearchBox.Text;
        CountLabel.Text = $"{_vm.FilteredPeople.Count} of 8 records";
    }
}
