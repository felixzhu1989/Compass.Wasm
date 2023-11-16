using System.Globalization;
using System.Windows.Data;

namespace Compass.Wpf.Common.Converters;

public class ShowLeftSideCvt : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value !=null && (value.ToString().Contains("左")||value.ToString().Contains("双")))
        {
            return Visibility.Visible;
        }
        return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}