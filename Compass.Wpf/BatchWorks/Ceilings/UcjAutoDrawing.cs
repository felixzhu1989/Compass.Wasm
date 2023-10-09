using System.Threading.Tasks;
using Compass.Wpf.ApiServices.Ceilings;
using Compass.Wpf.SwServices;

namespace Compass.Wpf.BatchWorks.Ceilings;

public interface IUcjAutoDrawing : IAutoDrawing
{

}
public class UcjAutoDrawing:BaseAutoDrawing,IUcjAutoDrawing
{

    #region ctor

    private readonly IUcjDataService _service;
    public UcjAutoDrawing(IContainerProvider provider) : base(provider)
    {
        _service = provider.Resolve<IUcjDataService>();
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
                case "UCJ_DB_800":

                    break;
                case "UCJ_SB_535":

                    break;

                case "UCJ_SB_385":

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