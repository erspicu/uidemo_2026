namespace MauiDemo;

/// <summary>Divides a double value by 100 (for ProgressBar which expects 0-1 range).</summary>
public class DivideBy100Converter : IValueConverter
{
    public static readonly DivideBy100Converter Instance = new();
    public object? Convert(object? value, Type t, object? p, System.Globalization.CultureInfo c)
        => value is double d ? d / 100.0 : 0.0;
    public object? ConvertBack(object? value, Type t, object? p, System.Globalization.CultureInfo c)
        => value is double d ? d * 100.0 : 0.0;
}
