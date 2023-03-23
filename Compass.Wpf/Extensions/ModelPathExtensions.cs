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
            //Hoods
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\KVI_555.SLDASM",
            @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\UVI_555.SLDASM",
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