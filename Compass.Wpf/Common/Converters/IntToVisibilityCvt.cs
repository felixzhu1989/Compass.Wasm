﻿using System.Globalization;
using System.Windows.Data;

namespace Compass.Wpf.Common.Converters;

public class IntToVisibilityCvt: IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value !=null && int.TryParse(value.ToString(),out int result))
        {
            if (result == 0) return Visibility.Visible;
        }
        return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}