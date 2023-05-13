using System.Collections.Generic;
using System.Drawing;

namespace Compass.Wpf.Extensions;

public static class CommonExtensions
{
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
            //Ceilings




        };
        var modelPath = modelPathList.FirstOrDefault(x => x.Contains(modelName, StringComparison.OrdinalIgnoreCase));
        if (string.IsNullOrEmpty(modelPath))
        {
            throw new ArgumentNullException(modelName, $@"模型{modelName}在PDM中未找到！");
        }
        return modelPath;
    }

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
}