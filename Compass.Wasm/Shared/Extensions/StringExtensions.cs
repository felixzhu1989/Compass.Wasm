using System.Collections.ObjectModel;
using System.Formats.Asn1;
using System.Globalization;
using Compass.Wasm.Shared.Categories;

namespace Compass.Wasm.Shared.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// 获取两个字符串的相似度
    /// </summary>
    public static float SimilarityWith(this string str1, string str2)
    {
        //计算两个字符串的长度。  
        int len1 = str1.Length;
        int len2 = str2.Length;
        //建立上面说的数组，比字符长度大一个空间  
        int[,] dif = new int[len1 + 1, len2 + 1];
        //赋初值，步骤B。  
        for (int a = 0; a <= len1; a++)
        {
            dif[a, 0] = a;
        }
        for (int a = 0; a <= len2; a++)
        {
            dif[0, a] = a;
        }
        //计算两个字符是否一样，计算左上的值  
        int temp;
        for (int i = 1; i <= len1; i++)
        {
            for (int j = 1; j <= len2; j++)
            {
                if (str1[i - 1] == str2[j - 1])
                {
                    temp = 0;
                }
                else
                {
                    temp = 1;
                }
                //取三个值中最小的  
                dif[i, j] = Math.Min(Math.Min(dif[i - 1, j - 1] + temp, dif[i, j - 1] + 1), dif[i - 1, j] + 1);
            }
        }
        //取数组右下角的值，同样不同位置代表不同字符串的比较  
        //Console.WriteLine("差异步骤：" + dif[len1, len2]);
        //计算相似度  
        float similarity = 1 - (float)dif[len1, len2] / Math.Max(str1.Length, str2.Length);
        //Console.WriteLine("相似度：" + similarity);
        return similarity;
    }

    public static string SubStringBetween(this string str,char chr1,char chr2)
    {
        return str.Substring(str.IndexOf(chr1) + 1, str.IndexOf(chr2)-str.IndexOf(chr1)-1);
    }

    
    /// <summary>
    /// 转换成日期
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(this string value)
    {
        if ((!string.IsNullOrEmpty(value))&& value.Contains("→"))
        {
            var strs = value.Split("→");
            value = strs[^1];
        }
        var result = DateTime.TryParse(value, out var date);
        return result ? date : DateTime.Now;
    }

    /// <summary>
    /// 转换成日期，可以null
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime? ToDateTimeWithNull(this string value)
    {
        var result = DateTime.TryParse(value, out var date);
        return result ? date : null;
    }

    /// <summary>
    /// 转换成int 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int ToInt(this string value)
    {
        var result = int.TryParse(value, out var intValue);
        return result ? intValue : 0;
    }

    /// <summary>
    /// 转换成double
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double ToDouble(this string value)
    {
        var result = double.TryParse(value, out var doubleValue);
        return result ? doubleValue : 0;
    }

    /// <summary>
    /// 转换成月份
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 添加Remarks
    /// </summary>
    /// <param name="value"></param>
    /// <param name="remarks"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 返回枚举类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T? ToEnum<T>(this string value)
    {
        if (string.IsNullOrEmpty(value)) return default;
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static bool ToBool(this string value)
    {
        var result = bool.TryParse(value, out var boolValue);
        return result && boolValue;
    }

    //string Acc
    public static ObservableCollection<Accessories> StringToAccs(this string value)
    {
        var accs= new ObservableCollection<Accessories>();
        if (!string.IsNullOrEmpty(value))
        {
            //使用,分割
            var accnums = value.Split(',');
            //使用-号分割
            foreach (var item in accnums)
            {
                var accnum = item.Split('-');
                Enum.TryParse(accnum.First(), true, out AccType_e name);
                var acc=new Accessories()
                {
                    Type = name,
                    Number = int.Parse(accnum.Last())
                };
                accs.Add(acc);
            }
        }
        return accs;
    }

    public static string AccsToString(this ObservableCollection<Accessories> value)
    {
        var str = string.Empty;
        if (value.Count > 0)
        {
            var accnums=new List<string>();
            foreach (var item in value)
            {
              var accnum=$"{item.Type}-{item.Number}";
              accnums.Add(accnum);
            }
            str= string.Join(",", accnums.ToArray());
        }
        return str;
    }
}