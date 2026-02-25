using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using WpfDemo.Models;

namespace WpfDemo.ViewModels;

public class DataViewModel : ViewModelBase
{
    private readonly List<PersonModel> _all;
    private string      _searchText     = "";
    private PersonModel? _selectedPerson;

    public string SearchText
    {
        get => _searchText;
        set { Set(ref _searchText, value); ApplyFilter(); }
    }
    public PersonModel? SelectedPerson
    {
        get => _selectedPerson;
        set => Set(ref _selectedPerson, value);
    }

    public ObservableCollection<PersonModel> FilteredPeople { get; } = new();

    public DataViewModel()
    {
        _all = new List<PersonModel>
        {
            new() { Name = "Alice Chen",   Age = 28, Department = "Engineering", Status = "Active",   Score = 92.5 },
            new() { Name = "Bob Martinez", Age = 35, Department = "Marketing",   Status = "Active",   Score = 85.0 },
            new() { Name = "Carol White",  Age = 42, Department = "HR",          Status = "On Leave", Score = 78.3 },
            new() { Name = "David Kim",    Age = 31, Department = "Engineering", Status = "Active",   Score = 95.1 },
            new() { Name = "Eva Thompson", Age = 26, Department = "Design",      Status = "Active",   Score = 88.7 },
            new() { Name = "Frank Garcia", Age = 39, Department = "Finance",     Status = "Inactive", Score = 71.2 },
            new() { Name = "Grace Liu",    Age = 33, Department = "Engineering", Status = "Active",   Score = 90.4 },
            new() { Name = "Henry Brown",  Age = 45, Department = "Management",  Status = "Active",   Score = 83.9 },
            new() { Name = "Ivy Davis",    Age = 29, Department = "Marketing",   Status = "Active",   Score = 87.6 },
            new() { Name = "Jack Wilson",  Age = 37, Department = "Finance",     Status = "Active",   Score = 76.8 },
            new() { Name = "Karen Moore",  Age = 24, Department = "Design",      Status = "On Leave", Score = 82.1 },
            new() { Name = "Leo Anderson", Age = 32, Department = "Engineering", Status = "Active",   Score = 93.3 },
        };
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        FilteredPeople.Clear();
        var q = _searchText.Trim();
        foreach (var p in _all.Where(p =>
            string.IsNullOrEmpty(q) ||
            p.Name.Contains(q, System.StringComparison.OrdinalIgnoreCase) ||
            p.Department.Contains(q, System.StringComparison.OrdinalIgnoreCase) ||
            p.Status.Contains(q, System.StringComparison.OrdinalIgnoreCase)))
            FilteredPeople.Add(p);
    }
}
