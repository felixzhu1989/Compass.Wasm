﻿namespace Compass.Wpf.BatchWorks.Ceilings;

public interface IDpAutoDrawing : IAutoDrawing
{

}
public class DpAutoDrawing:BaseAutoDrawing,IDpAutoDrawing
{
    private readonly IDpDataService _service;
    public DpAutoDrawing(IContainerProvider provider) : base(provider)
    {
        _service = provider.Resolve<IDpDataService>();
    }

    public async Task AutoDrawingAsync(ModuleDto moduleDto)
    {
        try
        {
            #region 文件夹准备与打包，打开顶级装配体

            var dataResult = await _service.GetFirstOrDefault(moduleDto.Id.Value);
            var data = dataResult.Result; //获取制图数据
            //todo:检查模型moduleDto.ModelName，看是那种子类
            var modelPath = moduleDto.ModelName.GetModelPath();
            //优化进程外调用命令变缓慢的问题
            SwApp.CommandInProgress = true;
            //打包,后续需要使用到的变量：suffix，packPath
            var packPath = SwApp.PackToProject(out var suffix, modelPath, moduleDto, Aggregator);
            //顶级Model,顶级Assy,打开Pack后的模型packPath
            var swAssyTop = SwApp.OpenAssemblyDoc(out var swModelTop, packPath, Aggregator);

            #endregion

            switch (moduleDto.ModelName)
            {
                case "DP_330":
                    CeilingService.Dp330(swModelTop, swAssyTop, suffix, moduleDto.Name, data);
                    break;
                case "DP_340":
                    CeilingService.Dp340(swModelTop, swAssyTop, suffix, moduleDto.Name, data);
                    break;
                case "DP_CJ_330":
                    CeilingService.DpCj330(swModelTop, swAssyTop, suffix, moduleDto.Name, data);
                    break;
            }

            #region 保存操作

            //设置成true，直接更新顶层，速度很快，设置成false，每个零件都会更新，很慢
            swModelTop.ForceRebuild3(true);
            swModelTop.Save(); //保存，很耗时间
            SwApp.CloseDoc(packPath); //关闭，很快

            #endregion
        }
        catch
        {
            SwApp.CommandInProgress = false;
            await Task.Delay(500);
            throw;
        }
        finally
        {
            SwApp.CommandInProgress = false;
        }
    }
}