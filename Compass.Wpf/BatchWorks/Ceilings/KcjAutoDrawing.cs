using Compass.Wasm.Shared.Data;
using Compass.Wasm.Shared.Data.Ceilings;
using Compass.Wpf.ApiServices.Ceilings;
using Compass.Wpf.SwServices;
using SolidWorks.Interop.sldworks;
using System.Threading.Tasks;

namespace Compass.Wpf.BatchWorks.Ceilings;
public interface IKcjAutoDrawing : IAutoDrawing
{

}
public class KcjAutoDrawing : BaseAutoDrawing, IKcjAutoDrawing
{
    #region ctor

    private readonly IKcjDataService _service;

    public KcjAutoDrawing(IContainerProvider provider) : base(provider)
    {
        _service = provider.Resolve<IKcjDataService>();
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
            //打包,后续需要使用到的变量：suffix，packPath
            var packPath = SwApp.PackToProject(out var suffix, modelPath, moduleDto, Aggregator);
            //顶级Model,顶级Assy,打开Pack后的模型packPath
            var swAssyTop = SwApp.OpenAssemblyDoc(out var swModelTop, packPath, Aggregator);

            #endregion

            switch (moduleDto.ModelName)
            {
                case "KCJ_DB_800":
                    KcjDb800(data, swModelTop, swAssyTop, suffix,moduleDto.Name);
                    break;
                case "KCJ_SB_535":

                    break;

                case "KCJ_SB_290":

                    break;

                case "KCJ_SB_265":

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
    }

    private void KcjDb800(KcjData data, ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix,string module)
    {
        //过滤掉填错的情况
        if (data.FilterSide is FilterSide_e.右油网 or FilterSide_e.无油网 or FilterSide_e.NA) data.FilterLeft = 0.5d;
        //居中尺寸的处理
        data.MiddleToRight = data.MiddleToRight.Equals(0) ? data.Length/2d : data.MiddleToRight;


        CeilingService.KcjDb800(swModelTop,swAssyTop,suffix,module,data);
    }
}