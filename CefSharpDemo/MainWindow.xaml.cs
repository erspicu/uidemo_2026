using System.IO;
using System.Windows;
using CefSharp;
using CefSharp.Wpf.HwndHost;

namespace CefSharpDemo;

public partial class MainWindow : Window
{
    private ChromiumWebBrowser? _browser;

    public MainWindow()
    {
        var settings = new CefSettings();
        if (!Cef.IsInitialized)
            Cef.Initialize(settings);
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _browser = new ChromiumWebBrowser();
        _browser.JavascriptObjectRepository.Register("csharpBridge", new BridgeApi());
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "index.html");
        _browser.Address = new Uri(path).AbsoluteUri;
        RootGrid.Children.Add(_browser);
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        Cef.Shutdown();
    }
}
