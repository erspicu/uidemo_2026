namespace UwpDemo.Models
{
    public class ActivityItem
    {
        public string Time   { get; set; } = "";
        public string Action { get; set; } = "";
        public string Type   { get; set; } = "Info";
        public string TypeIcon => Type switch
        {
            "Success" => "✅",
            "Warning" => "⚠️",
            "Error"   => "❌",
            _         => "ℹ️"
        };
    }
}
