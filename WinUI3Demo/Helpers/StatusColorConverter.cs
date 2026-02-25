using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace WinUI3Demo.Helpers;

public class StatusColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var status = value as string ?? "";
        var hex = status switch
        {
            "Active"   => "#15803D",
            "On Leave" => "#92400E",
            "Inactive" => "#991B1B",
            _          => "#374151"
        };
        return new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(
            255,
            System.Convert.ToByte(hex[1..3], 16),
            System.Convert.ToByte(hex[3..5], 16),
            System.Convert.ToByte(hex[5..7], 16)));
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => throw new NotImplementedException();
}
