using System.Windows.Controls;
using WpfDemo.ViewModels;

namespace WpfDemo.Views;

public partial class DataPage : Page
{
    public DataPage()
    {
        InitializeComponent();
        DataContext = new DataViewModel();
    }
}
