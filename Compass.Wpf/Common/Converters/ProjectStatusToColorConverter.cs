using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wpf.Common.Converters;

/// <summary>
/// 项目状态转换成颜色
/// </summary>
[ValueConversion(typeof(ProjectStatus_e), typeof(Brush))]
public class ProjectStatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        switch (value.ToString())
        {
            case "计划":
                return new SolidColorBrush(Colors.RoyalBlue);
            case "制图":
                return new SolidColorBrush(Colors.DeepSkyBlue);
            case "生产":
                return new SolidColorBrush(Colors.Orange);
            case "入库":
                return new SolidColorBrush(Colors.Lime);
            case "发货":
                return new SolidColorBrush(Colors.Silver);
            case "结束":
                return new SolidColorBrush(Colors.Gray);
            default:
                return new SolidColorBrush(Colors.Red);
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}