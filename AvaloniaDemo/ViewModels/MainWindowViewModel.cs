using System.Collections.ObjectModel;
using AvaloniaDemo.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaDemo.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase? _currentPage;

    [ObservableProperty]
    private NavItem? _selectedNavItem;

    public ObservableCollection<NavItem> NavItems { get; }

    public MainWindowViewModel()
    {
        var dashboard = new DashboardViewModel();
        var controls  = new ControlsDemoViewModel();
        var animation = new AnimationDemoViewModel();
        var data      = new DataDemoViewModel();

        NavItems = new ObservableCollection<NavItem>
        {
            new() { Title = "Dashboard",  Icon = "🏠", ViewModel = dashboard },
            new() { Title = "Controls",   Icon = "🎛️",  ViewModel = controls  },
            new() { Title = "Animation",  Icon = "✨",  ViewModel = animation },
            new() { Title = "Data Grid",  Icon = "📊",  ViewModel = data      },
        };

        SelectedNavItem = NavItems[0];
        CurrentPage     = dashboard;
    }

    partial void OnSelectedNavItemChanged(NavItem? value)
    {
        if (value != null)
            CurrentPage = value.ViewModel;
    }
}
