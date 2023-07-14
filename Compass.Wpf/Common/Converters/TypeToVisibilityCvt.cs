using System.Globalization;
using System.Windows.Data;

namespace Compass.Wpf.Common.Converters;

public class TypeToVisibilityCvt : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.Equals("托盘") ? Visibility.Hidden : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}