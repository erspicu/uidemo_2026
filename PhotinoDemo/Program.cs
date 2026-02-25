using System.Text.Json;
using PhotinoNET;

var window = new PhotinoWindow()
    .SetTitle("🟢 Photino Demo")
    .SetSize(1200, 720)
    .Center()
    .SetResizable(true)
    .SetDevToolsEnabled(true)
    .RegisterWebMessageReceivedHandler((sender, msg) =>
    {
        JsonElement doc;
        try { doc = JsonSerializer.Deserialize<JsonElement>(msg); }
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

        (sender as PhotinoWindow)?.SendWebMessage(resp);
    });

window.Load("wwwroot/index.html");
window.WaitForClose();
