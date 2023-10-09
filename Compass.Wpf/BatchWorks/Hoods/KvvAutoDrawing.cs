using System.Threading.Tasks;
using Compass.Wasm.Shared.Data;
using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wpf.ApiServices.Hoods;
using Compass.Wpf.SwServices;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.BatchWorks.Hoods;

public interface IKvvAutoDrawing : IAutoDrawing
{
}

public class KvvAutoDrawing : BaseAutoDrawing, IKvvAutoDrawing
{
    #region ctor
    private readonly IKvvDataService _service;
    public KvvAutoDrawing(IContainerProvider provider) : base(provider)
    {
        _service = provider.Resolve<IKvvDataService>();

    }
    #endregion

    public async Task AutoDrawingAsync(ModuleDto moduleDto)
    {
        try
        {
            #region 文件夹准备与打包，打开顶级装配体
            var dataResult = await _service.GetFirstOrDefault(moduleDto.Id.Value);
            var data = dataResult.Result;//获取制图数据
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
                case "KVV_555":
                    Kvv555(data, swModelTop, swAssyTop, suffix);
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

    private void Kvv555(KvvData data, ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix)
    {

        #region 计算中间值与顶层操作
        //赋值为0时为均分一半，否则需要赋值
        var netMiddleToRight = data.MiddleToRight.Equals(0) ? data.Length/2d : data.MiddleToRight;
        //烟罩宽度，考虑是否右BackCj
        swModelTop.ChangeDim("Width@DistanceWidth", data.Width);
        //内部灯板宽度
        //当烟罩宽度为100的整数倍时（如1000,1500），middleroof 取值155.2，冷凝板参考上表
        //当烟罩宽度为100的整数倍时+50（如1150,1550），middleroof 取值255.2，冷凝板参考整数档（如1550参考1500档）
        var insidePanelWidth = data.Width%100d>45d ? 255.2d : 155.2d;
        
        //冷凝板参数
        double panelAngle;
        double panelHeight;
        //KVV555的数据
        switch ((int)(data.Width / 100d))//取百位以上整数
        {
            case 6:
                panelAngle = 160d * Math.PI / 180d;
                panelHeight = 357d;
                break;
            case 7:
                panelAngle = 160d * Math.PI / 180d;
                panelHeight = 357d;
                break;
            case 8:
                panelAngle = 147d * Math.PI / 180d;
                panelHeight = 403d;
                break;
            case 9:
                panelAngle = 147d * Math.PI / 180d;
                panelHeight = 403d;
                break;
            case 10://OK
                panelAngle = 136d * Math.PI / 180d;
                panelHeight = 465d;
                break;
            case 11://OK
                panelAngle = 132d * Math.PI / 180d;//132d
                panelHeight = 495d;//495d
                break;
            case 12://OK
                panelAngle = 129d * Math.PI / 180d;//129d
                panelHeight = 535d;//535d
                break;
            case 13://OK
                panelAngle = 125d * Math.PI / 180d;//125
                panelHeight = 575d;//575
                break;
            case 14://OK
                panelAngle = 123d * Math.PI / 180d;//123
                panelHeight = 616d;//616
                break;
            case 15://OK
                panelAngle = 120d * Math.PI / 180d;//120
                panelHeight = 658d;//658
                break;
            case 16:
                panelAngle = 119d * Math.PI / 180d;
                panelHeight = 707d;
                break;
            case 17:
                panelAngle = 119d * Math.PI / 180d;
                panelHeight = 707d;
                break;
            default://默认是标准模版中的值，防止模型报错
                panelAngle = 125d * Math.PI / 180d;//125
                panelHeight = 575d;//575
                break;
        }
        #endregion
        ExhaustService.Kvv555(swAssyTop, suffix, data.Length, data.Height, panelAngle, panelHeight);
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "ExhaustSpigot_Fs-1", Aggregator);
        //脖颈
        ExhaustService.ExhaustSpigotFs(swAssyLevel1,suffix,data.Length, netMiddleToRight,data.ExhaustSpigotNumber,data.ExhaustSpigotLength,data.ExhaustSpigotWidth,data.ExhaustSpigotHeight,data.ExhaustSpigotDis,false,false,ExhaustType_e.NA);


        SidePanelService.SidePanelKvv(swAssyTop, suffix, data.Length,data.Width, data.Height, panelAngle, panelHeight, insidePanelWidth);

        MidRoofService.MidRoofKvv(swAssyTop,suffix,data.Length,data.Width,insidePanelWidth,data.ExhaustSpigotLength,data.ExhaustSpigotWidth,data.LightType);

    }
}