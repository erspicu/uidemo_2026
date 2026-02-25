using System;
using UnoDemo.Pages;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace UnoDemo
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
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
            if (pageType != null) ContentFrame.Navigate(pageType);
        }
    }
}
