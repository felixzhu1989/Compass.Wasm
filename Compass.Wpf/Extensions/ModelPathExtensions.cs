using System;
using System.Collections.Generic;
using System.Linq;

namespace Compass.Wpf.Extensions;

public static class ModelPathExtensions
{
    public static string GetModelPath(this string modelName)
    {
        List<string> modelPathList=new List<string>
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
}