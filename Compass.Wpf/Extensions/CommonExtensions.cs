using CsvHelper;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

namespace Compass.Wpf.Extensions;

public static class CommonExtensions
{
    /// <summary>
    /// 获取模型PDM地址
    /// </summary>
    /// <param name="modelName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string GetModelPath(this string modelName)
    {
        var modelPathList=new List<string>
        {
            //todo:给总目录，自动搜索？
            //Hoods
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\KVF_555.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\KVI_555.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\UVF_555.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\UVI_555.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\KWF_555.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\KWI_555.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\UWF_555.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\UWI_555.SLDASM",

            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\KVV_555.SLDASM",


            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\UVI_450_400.SLDASM",
            
            //Huawei
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\UVI_HW_650.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\UVF_HW_650.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\UWF_HW_650.SLDASM",
            //Ceilings




        };
        var modelPath = modelPathList.FirstOrDefault(x => x.Contains(modelName, StringComparison.OrdinalIgnoreCase));
        if (string.IsNullOrEmpty(modelPath))
        {
            throw new ArgumentNullException(modelName, $@"PDM中暂时没有标准化模型{modelName}，请联系管理员处理！");
        }
        return modelPath;
    }

    /// <summary>
    /// 生成二维码
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static Bitmap GetQrCodeBitmap(this string content)
    {
        //QRCodeGenerator ：用来通过指定的方式生成二维码存储的数据对象，
        //也就是 QRCodeData 二维码中间的 Matrix，
        //之后 QRCode 得到 QRCodeData 并生成二维码
        var codeGenerator = new QRCoder.QRCodeGenerator();
        var codeData = codeGenerator.CreateQrCode(content, QRCoder.QRCodeGenerator.ECCLevel.M/* 这里设置容错率的一个级别 */, true, true, QRCoder.QRCodeGenerator.EciMode.Utf8, 5);//版本 1 ~ 40
        var code = new QRCoder.QRCode(codeData);
        return code.GetGraphic(4);
    }

    /// <summary>
    /// 从csv文件读取数据
    /// </summary>
    public static List<T> GetRecords<T>(this string path)
    {
        //数据存储在SliderParam.csv文件中，方便excel编辑
        //https://joshclose.github.io/CsvHelper/getting-started/
        //Install-Package CsvHelper

        //Excel默认以ANSI格式保存csv文件，读取的时候中文会乱码，应当指定Encoding为GB2312
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using var reader = new StreamReader(path, Encoding.GetEncoding("GB2312"));
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<T>().ToList();
    }
}