using System.Collections.Generic;
using System.Collections.ObjectModel;
using WinUI3Demo.Models;

namespace WinUI3Demo.ViewModels;

public class DataViewModel : ViewModelBase
{
    private string _search = "";
    public string Search
    {
        get => _search;
        set { SetProperty(ref _search, value); FilterData(); }
    }

    public ObservableCollection<PersonModel> FilteredPeople { get; } = new();

    private static readonly List<PersonModel> _source = new()
    {
        new() { Name="Alice Chen",   Age=28, Department="Engineering", Status="Active",   Score=92.5 },
        new() { Name="Bob Smith",    Age=35, Department="Marketing",   Status="Active",   Score=87.0 },
        new() { Name="Carol White",  Age=31, Department="Design",      Status="On Leave", Score=95.0 },
        new() { Name="David Lee",    Age=42, Department="Engineering", Status="Active",   Score=78.5 },
        new() { Name="Eva Martinez", Age=26, Department="Sales",       Status="Active",   Score=88.0 },
        new() { Name="Frank Wilson", Age=38, Department="HR",          Status="Inactive", Score=71.0 },
        new() { Name="Grace Kim",    Age=29, Department="Engineering", Status="Active",   Score=96.5 },
        new() { Name="Henry Brown",  Age=45, Department="Management",  Status="Active",   Score=82.0 },
    };

    public DataViewModel()
    {
        foreach (var p in _source) FilteredPeople.Add(p);
    }

    private void FilterData()
    {
        FilteredPeople.Clear();
        var q = Search.ToLower();
        foreach (var p in _source)
            if (string.IsNullOrEmpty(q) ||
                p.Name.ToLower().Contains(q) ||
                p.Department.ToLower().Contains(q) ||
                p.Status.ToLower().Contains(q))
                FilteredPeople.Add(p);
    }
}
