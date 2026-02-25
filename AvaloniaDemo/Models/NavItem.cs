using AvaloniaDemo.ViewModels;

namespace AvaloniaDemo.Models;

public class NavItem
{
    public string Title { get; set; } = "";
    public string Icon  { get; set; } = "";
    public ViewModelBase ViewModel { get; set; } = null!;
}
