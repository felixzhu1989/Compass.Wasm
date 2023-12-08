using System.Collections.Generic;
using System.IO;

namespace Compass.Wpf.Extensions;

public static class ModelExtensions
{
    /// <summary>
    /// 获取模型PDM地址
    /// </summary>
    public static string GetModelPath(this string modelName)
    {
        //todo:给总目录，自动搜索？
        const string rootPath = @"D:\halton\01 Tech Dept\05 Products Library";
        var autoFindModelList = Directory.GetFiles(rootPath, "*.SLDASM", SearchOption.AllDirectories);

        //第一步，筛查包含字段的
        var containList = autoFindModelList.Where(x => x.Contains(modelName, StringComparison.OrdinalIgnoreCase)).ToList();
        //第二步，精确匹配相等
        var modelPath = string.Empty;
        foreach (var item in containList)
        {
           var itemName = Path.GetFileNameWithoutExtension(item);
           if (itemName.Equals(modelName,StringComparison.OrdinalIgnoreCase)  ) modelPath=item;
        }

        if (string.IsNullOrEmpty(modelPath))
        {
            throw new ArgumentNullException(modelName, $@"PDM中暂时没有标准化模型{modelName}，请联系管理员处理！");
        }
        return modelPath;
    }
}