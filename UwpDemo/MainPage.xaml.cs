using System;
using UwpDemo.Pages;
using Windows.UI.Xaml.Controls;

namespace UwpDemo
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
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
}
