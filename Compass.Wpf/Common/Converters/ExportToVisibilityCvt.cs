using System.Globalization;
using System.Windows.Data;

namespace Compass.Wpf.Common.Converters;

public class AssyToVisibilityCvt : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value.ToString().Contains("装配体模式")) return Visibility.Visible;
        return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class CopyToVisibilityCvt : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value.ToString().Contains("拷贝文件模式")) return Visibility.Visible;
        return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}