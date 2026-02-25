using System.Collections.ObjectModel;
using WinUI3Demo.Models;

namespace WinUI3Demo.ViewModels;

public class DashboardViewModel : ViewModelBase
{
    public int    TotalUsers     { get; } = 1_284;
    public int    ActiveProjects { get; } = 47;
    public double Revenue        { get; } = 89_430.00;
    public int    OpenIssues     { get; } = 23;
    public string RevenueText    => $"${Revenue:N0}";

    public ObservableCollection<ActivityItem> RecentActivities { get; } = new()
    {
        new() { Time="09:42", Action="User Alice logged in",  Type="Success" },
        new() { Time="09:38", Action="Build #472 completed",  Type="Success" },
        new() { Time="09:31", Action="Deploy to staging",     Type="Info"    },
        new() { Time="09:15", Action="Memory usage at 85%",   Type="Warning" },
        new() { Time="08:57", Action="Payment failed: #9921", Type="Error"   },
        new() { Time="08:41", Action="New user registered",   Type="Info"    },
    };
}
