using System.IO;
using System.Text.Json;
using System.Windows;
using Microsoft.Web.WebView2.Core;

namespace WebView2Demo;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += async (_, _) =>
        {
            await webView.EnsureCoreWebView2Async();
            webView.CoreWebView2.WebMessageReceived += OnMessage;
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "index.html");
            webView.CoreWebView2.Navigate(new Uri(path).AbsoluteUri);
        };
    }

    private void OnMessage(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        JsonElement doc;
        try
        {
            // JS sends postMessage(JSON.stringify({...})) — TryGetWebMessageAsString()
            // returns the raw string; if it throws (non-string msg), fall back to WebMessageAsJson
            string raw;
            try { raw = e.TryGetWebMessageAsString(); }
            catch { raw = e.WebMessageAsJson; }
            doc = JsonSerializer.Deserialize<JsonElement>(raw);
        }
        catch { return; }

        var cmd = doc.GetProperty("cmd").GetString();

        string resp = cmd switch
        {
            "getSystemInfo" => JsonSerializer.Serialize(new
            {
                cmd = "systemInfo",
                os = Environment.OSVersion.ToString(),
                cpu = Environment.ProcessorCount,
                machine = Environment.MachineName,
                dotnet = Environment.Version.ToString(),
                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            }),
            "getData" => JsonSerializer.Serialize(new
            {
                cmd = "tableData",
                rows = new[]
                {
                    new { name="Alice Chen",   role="Engineer",  dept="Engineering", salary=95200,  status="Active" },
                    new { name="Bob Martinez", role="Designer",  dept="Design",      salary=78400,  status="Active" },
                    new { name="Carol White",  role="Manager",   dept="Product",     salary=112000, status="Active" },
                    new { name="David Kim",    role="Analyst",   dept="Finance",     salary=84500,  status="Remote" },
                    new { name="Emma Davis",   role="Developer", dept="Engineering", salary=91000,  status="Active" },
                    new { name="Frank Lee",    role="DevOps",    dept="IT",          salary=88000,  status="Remote" },
                    new { name="Grace Park",   role="QA",        dept="Engineering", salary=72000,  status="Active" },
                    new { name="Henry Wong",   role="PM",        dept="Product",     salary=105000, status="Active" },
                }
            }),
            "ping" => JsonSerializer.Serialize(new
            {
                cmd = "pong",
                value = doc.TryGetProperty("value", out var v) ? v.GetString() : "",
                time = DateTime.Now.ToString("HH:mm:ss")
            }),
            _ => JsonSerializer.Serialize(new { cmd = "error", msg = "unknown" })
        };

        webView.CoreWebView2.PostWebMessageAsString(resp);
    }
}
