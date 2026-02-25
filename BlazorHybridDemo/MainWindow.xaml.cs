using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using BlazorHybridDemo.Services;
namespace BlazorHybridDemo;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        var services = new ServiceCollection();
        services.AddWpfBlazorWebView();
        services.AddSingleton<AppService>();
        Resources.Add("services", services.BuildServiceProvider());
        InitializeComponent();
    }
}
