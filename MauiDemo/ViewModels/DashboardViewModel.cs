using System.Collections.ObjectModel;
using MauiDemo.Models;

namespace MauiDemo.ViewModels;

public class DashboardViewModel : BindableObject
{
    public int    TotalUsers     { get; } = 1_248;
    public int    ActiveSessions { get; } = 342;
    public string Revenue        { get; } = "$89,420";
    public int    OpenIssues     { get; } = 7;

    public ObservableCollection<ActivityItem> RecentActivity { get; } = new()
    {
        new() { Time = "09:15", Action = "New user registered",           Type = "Info"    },
        new() { Time = "09:32", Action = "Payment processed: $299",       Type = "Success" },
        new() { Time = "10:01", Action = "Server response slow (> 2s)",   Type = "Warning" },
        new() { Time = "10:45", Action = "Build v2.4.1 deployed",         Type = "Success" },
        new() { Time = "11:20", Action = "Critical error in auth module", Type = "Error"   },
        new() { Time = "12:00", Action = "Backup completed (4.2 GB)",     Type = "Info"    },
        new() { Time = "13:30", Action = "Memory usage threshold 85%",    Type = "Warning" },
    };
}
