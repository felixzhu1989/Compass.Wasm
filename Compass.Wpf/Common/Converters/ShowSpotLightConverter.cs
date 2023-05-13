using System.Globalization;
using System.Windows.Data;

namespace Compass.Wpf.Common.Converters;

public class ShowSpotLightConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value !=null && value.ToString().Contains("筒灯"))
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