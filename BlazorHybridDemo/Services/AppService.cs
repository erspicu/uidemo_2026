namespace BlazorHybridDemo.Services;
public class AppService
{
    public string GetOsVersion() => Environment.OSVersion.ToString();
    public int GetCpuCount() => Environment.ProcessorCount;
    public string GetMachineName() => Environment.MachineName;
    public string GetDotNetVersion() => Environment.Version.ToString();
    public string GetCurrentTime() => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    public List<DataRow> GetTableData() => new()
    {
        new("Alice Chen",   "Engineer",  "Engineering", 95200, "Active"),
        new("Bob Martinez", "Designer",  "Design",      78400, "Active"),
        new("Carol White",  "Manager",   "Product",    112000, "Active"),
        new("David Kim",    "Analyst",   "Finance",     84500, "Remote"),
        new("Emma Davis",   "Developer", "Engineering",  91000, "Active"),
        new("Frank Lee",    "DevOps",    "IT",          88000, "Remote"),
        new("Grace Park",   "QA",        "Engineering",  72000, "Active"),
        new("Henry Wong",   "PM",        "Product",    105000, "Active"),
    };
    public record DataRow(string Name, string Role, string Dept, int Salary, string Status);
}
