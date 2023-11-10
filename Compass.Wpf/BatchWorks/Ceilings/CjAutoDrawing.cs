﻿namespace Compass.Wpf.BatchWorks.Ceilings;
public interface ICjAutoDrawing : IAutoDrawing
{

}
public class CjAutoDrawing : BaseAutoDrawing, ICjAutoDrawing
{
    #region ctor

    private readonly ICjDataService _service;

    public CjAutoDrawing(IContainerProvider provider) : base(provider)
    {
        _service = provider.Resolve<ICjDataService>();
    }

    #endregion

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
                case "CJ_300":
                    CeilingService.Cj300(swModelTop, swAssyTop, suffix, moduleDto.Name, data);
                    break;
                case "CJ_330":
                    CeilingService.Cj330(swModelTop, swAssyTop, suffix, moduleDto.Name, data);
                    break;
                //case "CJ_430":
                //    CeilingService.Cj430(swModelTop, swAssyTop, suffix, moduleDto.Name, data);
                //    break;

                case "CJ_B_300":
                    CeilingService.Bcj300(swModelTop, swAssyTop, suffix, moduleDto.Name, data);
                    break;
                case "CJ_B_330":
                    CeilingService.Bcj330(swModelTop, swAssyTop, suffix, moduleDto.Name, data);
                    break;
                //case "CJ_B_430":
                //    CeilingService.Bcj430(swModelTop, swAssyTop, suffix, moduleDto.Name, data);
                //    break;


                case "CJ_NO_300":
                    CeilingService.Nocj300(swModelTop, swAssyTop, suffix, moduleDto.Name, data);
                    break;
                case "CJ_NO_330":
                    CeilingService.Nocj330(swModelTop, swAssyTop, suffix, moduleDto.Name, data);
                    break;
                case "CJ_NO_340":
                    CeilingService.Nocj340(swModelTop, swAssyTop, suffix, moduleDto.Name, data);
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