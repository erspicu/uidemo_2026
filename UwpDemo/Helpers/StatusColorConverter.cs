using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace UwpDemo.Helpers
{
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
            return new SolidColorBrush(Color.FromArgb(
                255,
                System.Convert.ToByte(hex.Substring(1, 2), 16),
                System.Convert.ToByte(hex.Substring(3, 2), 16),
                System.Convert.ToByte(hex.Substring(5, 2), 16)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException();
    }
}
