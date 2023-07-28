using System.Globalization;
using System.Windows.Data;

namespace Compass.Wpf.Common.Converters;

public class BoolToHeightCvt : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || !bool.TryParse(value.ToString(), out var result)) return "0";
        return result ? "Auto" : "0";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class BoolToHeightInverseCvt : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || !bool.TryParse(value.ToString(), out var result)) return "Auto";
        return result ? "0" : "Auto";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}