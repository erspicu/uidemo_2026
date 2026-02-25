using WinUI3Demo.Pages;

namespace WinUI3Demo;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Title = "WinUI 3 Demo";

        // Ensure dark theme applies to both pane and content
        NavView.RequestedTheme = Microsoft.UI.Xaml.ElementTheme.Dark;

        NavView.SelectedItem = NavView.MenuItems[0];
        ContentFrame.Navigate(typeof(DashboardPage));
    }

    private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is not NavigationViewItem item) return;

        Type? pageType = item.Tag?.ToString() switch
        {
            "dashboard" => typeof(DashboardPage),
            "controls"  => typeof(ControlsPage),
            "animation" => typeof(AnimationPage),
            "data"      => typeof(DataPage),
            _           => null
        };

        if (pageType != null)
            ContentFrame.Navigate(pageType);
    }
}
