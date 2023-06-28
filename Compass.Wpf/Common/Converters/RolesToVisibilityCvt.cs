using System.Globalization;
using System.Windows.Data;

namespace Compass.Wpf.Common.Converters;

public class RolesToVisibilityCvt:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value.ToString().Contains(AppSession.Roles)) return Visibility.Visible;
        return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}