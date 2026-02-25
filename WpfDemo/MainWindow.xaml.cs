using System.Windows;
using System.Windows.Controls;
using WpfDemo.Views;

namespace WpfDemo;

public partial class MainWindow : Window
{
    private readonly Button[] _navButtons;
    private readonly DashboardPage  _dashboard  = new();
    private readonly ControlsPage   _controls   = new();
    private readonly AnimationPage  _animation  = new();
    private readonly DataPage       _data       = new();

    public MainWindow()
    {
        InitializeComponent();
        _navButtons = [BtnDashboard, BtnControls, BtnAnimation, BtnData];
        MainFrame.Navigate(_dashboard);
    }

    private void Nav_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;

        foreach (var b in _navButtons)
            b.Style = (Style)Resources["NavBtn"];
        btn.Style = (Style)Resources["NavBtnSelected"];

        MainFrame.Navigate(btn.Tag switch
        {
            "Controls"  => (object)_controls,
            "Animation" => _animation,
            "Data"      => _data,
            _           => _dashboard
        });
    }
}