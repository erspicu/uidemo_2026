using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AvaloniaDemo.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaDemo.ViewModels;

public partial class DataDemoViewModel : ViewModelBase
{
    private readonly ObservableCollection<PersonModel> _allPeople;

    [ObservableProperty] private string       _searchText     = "";
    [ObservableProperty] private PersonModel? _selectedPerson;
    [ObservableProperty] private IEnumerable<PersonModel> _filteredPeople;

    public DataDemoViewModel()
    {
        _allPeople = new ObservableCollection<PersonModel>
        {
            new() { Name = "Alice Chen",    Age = 28, Department = "Engineering", Status = "Active",   Score = 92.5 },
            new() { Name = "Bob Martinez",  Age = 35, Department = "Marketing",   Status = "Active",   Score = 85.0 },
            new() { Name = "Carol White",   Age = 42, Department = "HR",          Status = "On Leave", Score = 78.3 },
            new() { Name = "David Kim",     Age = 31, Department = "Engineering", Status = "Active",   Score = 95.1 },
            new() { Name = "Eva Thompson",  Age = 26, Department = "Design",      Status = "Active",   Score = 88.7 },
            new() { Name = "Frank Garcia",  Age = 39, Department = "Finance",     Status = "Inactive", Score = 71.2 },
            new() { Name = "Grace Liu",     Age = 33, Department = "Engineering", Status = "Active",   Score = 90.4 },
            new() { Name = "Henry Brown",   Age = 45, Department = "Management",  Status = "Active",   Score = 83.9 },
            new() { Name = "Ivy Davis",     Age = 29, Department = "Marketing",   Status = "Active",   Score = 87.6 },
            new() { Name = "Jack Wilson",   Age = 37, Department = "Finance",     Status = "Active",   Score = 76.8 },
            new() { Name = "Karen Moore",   Age = 24, Department = "Design",      Status = "On Leave", Score = 82.1 },
            new() { Name = "Leo Anderson",  Age = 32, Department = "Engineering", Status = "Active",   Score = 93.3 },
        };
        _filteredPeople = _allPeople;
    }

    partial void OnSearchTextChanged(string value)
    {
        FilteredPeople = string.IsNullOrWhiteSpace(value)
            ? _allPeople
            : _allPeople.Where(p =>
                p.Name.Contains(value, StringComparison.OrdinalIgnoreCase)       ||
                p.Department.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                p.Status.Contains(value, StringComparison.OrdinalIgnoreCase));
    }
}
