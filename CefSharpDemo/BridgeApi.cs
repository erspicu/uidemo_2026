using System.Text.Json;
namespace CefSharpDemo;

public class BridgeApi
{
    public string GetSystemInfo()
    {
        return JsonSerializer.Serialize(new
        {
            os = Environment.OSVersion.ToString(),
            cpu = Environment.ProcessorCount,
            machine = Environment.MachineName,
            dotnet = Environment.Version.ToString(),
            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        });
    }

    public string GetTableData()
    {
        return JsonSerializer.Serialize(new[]
        {
            new { name="Alice Chen",   role="Engineer",  dept="Engineering", salary=95200,  status="Active" },
            new { name="Bob Martinez", role="Designer",  dept="Design",      salary=78400,  status="Active" },
            new { name="Carol White",  role="Manager",   dept="Product",     salary=112000, status="Active" },
            new { name="David Kim",    role="Analyst",   dept="Finance",     salary=84500,  status="Remote" },
            new { name="Emma Davis",   role="Developer", dept="Engineering", salary=91000,  status="Active" },
            new { name="Frank Lee",    role="DevOps",    dept="IT",          salary=88000,  status="Remote" },
            new { name="Grace Park",   role="QA",        dept="Engineering", salary=72000,  status="Active" },
            new { name="Henry Wong",   role="PM",        dept="Product",     salary=105000, status="Active" },
        });
    }

    public string Ping(string value)
    {
        return $"[C# @ {DateTime.Now:HH:mm:ss}] Pong: \"{value}\" (length: {value.Length})";
    }
}
