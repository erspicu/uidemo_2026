using System.IO;
using System.Windows;
using CefSharp;
using CefSharp.Wpf;

namespace CefSharpDemo;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        var settings = new CefSettings();
        if (!Cef.IsInitialized)
            Cef.Initialize(settings);
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        browser.JavascriptObjectRepository.Register("csharpBridge", new BridgeApi(), isAsync: false);
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "index.html");
        browser.Address = new Uri(path).AbsoluteUri;
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        Cef.Shutdown();
    }
}
