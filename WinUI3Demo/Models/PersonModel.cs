namespace WinUI3Demo.Models;

public class PersonModel
{
    public string Name       { get; set; } = "";
    public int    Age        { get; set; }
    public string Department { get; set; } = "";
    public string Status     { get; set; } = "";
    public double Score      { get; set; }

    public string ScoreText  => $"{Score:F1}";
    public string StatusColor => Status switch
    {
        "Active"   => "#15803D",
        "On Leave" => "#92400E",
        "Inactive" => "#991B1B",
        _          => "#374151"
    };
}
