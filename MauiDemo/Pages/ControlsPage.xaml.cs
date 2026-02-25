using MauiDemo.ViewModels;

namespace MauiDemo.Pages;

public partial class ControlsPage : ContentPage
{
    public ControlsPage()
    {
        InitializeComponent();
        BindingContext = new ControlsViewModel();
    }
}
