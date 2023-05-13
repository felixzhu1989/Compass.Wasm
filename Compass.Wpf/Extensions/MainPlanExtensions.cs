using System.Diagnostics.Eventing.Reader;

namespace Compass.Wpf.Extensions;

public static class MainPlanExtensions
{
    public static DateTime ToDateTime(this string value)
    {
        if ((!string.IsNullOrEmpty(value) )&& value.Contains("→"))
        {
           var strs= value.Split("→");
           value = strs[^1];
        }
        var result = DateTime.TryParse(value, out var date);
        return result ? date : DateTime.Now;
    }
    public static int ToInt(this string value)
    {
        var result = int.TryParse(value, out var intValue);
        return result ? intValue : 0;
    }
    public static double ToDouble(this string value)
    {
        var result = double.TryParse(value, out var doubleValue);
        return result ? doubleValue : 0;
    }
    public static DateTime ToMonth(this string value)
    {
        var today = DateTime.Today;
        var month = value.Contains("Jan", StringComparison.OrdinalIgnoreCase) ? 1 :
            value.Contains("Feb", StringComparison.OrdinalIgnoreCase) ? 2 :
            value.Contains("Mar", StringComparison.OrdinalIgnoreCase) ? 3 :
            value.Contains("Apr", StringComparison.OrdinalIgnoreCase) ? 4 :
            value.Contains("May", StringComparison.OrdinalIgnoreCase) ? 5 :
            value.Contains("Jun", StringComparison.OrdinalIgnoreCase) ? 6 :
            value.Contains("Jul", StringComparison.OrdinalIgnoreCase) ? 7 :
            value.Contains("Aug", StringComparison.OrdinalIgnoreCase) ? 8 :
            value.Contains("Sep", StringComparison.OrdinalIgnoreCase) ? 9 :
            value.Contains("Oct", StringComparison.OrdinalIgnoreCase) ? 10 :
            value.Contains("Nov", StringComparison.OrdinalIgnoreCase) ? 11 :
            12;
        return new DateTime(today.Year, month, 1);
    }
    public static string ToRemarks(this string value, string remarks)
    {
        value=value.Replace("/", "-");
        if (string.IsNullOrEmpty(remarks))
        {
            remarks=value;
        }
        else
        {
            if (!remarks.Contains(value)) remarks += $"\n{value}";
        }
        return remarks;
    }
}