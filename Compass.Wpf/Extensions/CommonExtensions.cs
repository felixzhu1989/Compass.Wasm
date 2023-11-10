﻿using CsvHelper;
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

            //法国烟罩
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\KVF_FR_555.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\KVI_FR_555.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\UVF_FR_555.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\UVI_FR_555.SLDASM",


            //其他高度烟罩
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\KVI_450.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\UVI_450.SLDASM",



            //555斜烟罩
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\UVF_555_400.SLDASM",





            //450斜烟罩
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\UVF_450_400.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\UVI_450_400.SLDASM",
            
            


            //KVV
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\KVV_555.SLDASM",


            //CMOD
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\CMODF_555_400.SLDASM",

            



            //Huawei
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\UVI_HW_650.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\UVF_HW_650.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\UWF_HW_650.SLDASM",
            //Ceilings


            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\KCJ_DB_800.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\KCJ_SB_535.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\KCJ_SB_290.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\KCJ_SB_265.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\UCJ_DB_800.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\UCJ_SB_535.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\UCJ_SB_385.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\KCW_DB_800.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\KCW_SB_535.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\KCW_SB_265.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\UCW_DB_800.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\UCW_SB_535.SLDASM",

            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\CJ_300.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\CJ_330.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\CJ_430.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\CJ_B_300.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\CJ_B_330.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\CJ_B_430.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\CJ_NO_300.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\CJ_NO_330.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\CJ_NO_340.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\CJ_NO_SP.SLDASM",

            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\DP_330.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\DP_340.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\DP_CJ_330.SLDASM",


            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\LFU_SA.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\LFU_SC.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\LFU_SS.SLDASM",



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
        //这里设置容错率的一个级别 QRCoder.QRCodeGenerator.ECCLevel.M
        //版本 1 ~ 40
        var codeData = codeGenerator.CreateQrCode(content, QRCoder.QRCodeGenerator.ECCLevel.M, true, true, QRCoder.QRCodeGenerator.EciMode.Utf8, 5);
        
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