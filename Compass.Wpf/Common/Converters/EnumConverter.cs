using System.Globalization;
using System.Windows.Data;
namespace Compass.Wpf.Common.Converters;

public class EnumConverter<T> : IValueConverter where T : Enum
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (T)Enum.Parse(typeof(T), value.ToString());
    }
}