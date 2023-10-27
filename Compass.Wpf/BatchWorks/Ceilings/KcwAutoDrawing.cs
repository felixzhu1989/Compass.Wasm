using System.Threading.Tasks;
using Compass.Wasm.Shared.Data;
using Compass.Wpf.ApiServices.Ceilings;
using Compass.Wpf.SwServices;

namespace Compass.Wpf.BatchWorks.Ceilings;

public interface IKcwAutoDrawing : IAutoDrawing
{

}

public class KcwAutoDrawing:BaseAutoDrawing,IKcwAutoDrawing
{
    #region ctor
    private readonly IKcwDataService _service;

    public KcwAutoDrawing(IContainerProvider provider) : base(provider)
    {
        _service = provider.Resolve<IKcwDataService>();
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

            //过滤掉填错的情况
            if (data.FilterSide is FilterSide_e.右过滤器侧板 or FilterSide_e.无过滤器侧板 or FilterSide_e.NA)
                data.FilterLeft = 0d;
            if (data.FilterSide is FilterSide_e.左过滤器侧板 or FilterSide_e.无过滤器侧板 or FilterSide_e.NA)
                data.FilterRight = 0d;
            //居中尺寸的处理
            data.MiddleToRight = data.MiddleToRight.Equals(0) ? data.Length / 2d : data.MiddleToRight;

            switch (moduleDto.ModelName)
            {
                case "KCW_DB_800":
                    BeamService.KcwDb800(swModelTop, swAssyTop, suffix, moduleDto.Name, data);
                    break;
                case "KCW_SB_535":
                    BeamService.KcwSb535(swModelTop, swAssyTop, suffix, moduleDto.Name, data);
                    break;
                case "KCW_SB_265":
                    BeamService.KcwSb265(swModelTop, swAssyTop, suffix, moduleDto.Name, data);
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