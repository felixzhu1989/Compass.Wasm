using System.Globalization;
using System.Windows.Data;

namespace Compass.Wpf.Common.Converters;

public class BoolToVisibilityCvt: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || !bool.TryParse(value.ToString(), out var result)) return Visibility.Hidden;
        return result ? Visibility.Visible : Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}