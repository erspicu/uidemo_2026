using MauiDemo.ViewModels;

namespace MauiDemo.Pages;

public partial class DataPage : ContentPage
{
    public DataPage()
    {
        InitializeComponent();
        BindingContext = new DataViewModel();
    }
}
