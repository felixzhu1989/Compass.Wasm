using System.Threading.Tasks;
using Compass.Wasm.Shared.Data;
using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wpf.ApiServices.Hoods;
using Compass.Wpf.SwServices;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.BatchWorks.Hoods;

public interface IUviAutoDrawing : IAutoDrawing
{
}

public class UviAutoDrawing : BaseAutoDrawing, IUviAutoDrawing
{
    #region ctor
    private readonly IUviDataService _service;
    public UviAutoDrawing(IContainerProvider provider) : base(provider)
    {
        _service = provider.Resolve<IUviDataService>();
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
                case "UVI_555":
                    Uvi555(data, swModelTop, swAssyTop, suffix);
                    break;
                case "UVI_FR_555":
                    UviFr555(data, swModelTop, swAssyTop, suffix);
                    break;
                case "UVI_450":
                    Uvi450(data, swModelTop, swAssyTop, suffix);
                    break;
                case "UVI_450_400":
                    Uvi450400(data, swModelTop, swAssyTop, suffix);
                    break;

                case "UVI_HW_650":
                    UviHw650(data, swModelTop, swAssyTop, suffix);
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

    private void Uvi555(UviData data,ModelDoc2 swModelTop,AssemblyDoc swAssyTop,string suffix)
    {
        #region 计算中间值与顶层操作
        //计算烟罩净长度，计算烟罩净深度
        var netLength = data.SidePanel==SidePanel_e.左||data.SidePanel==SidePanel_e.右 ? data.Length-50d : data.SidePanel==SidePanel_e.双 ? data.Length-100d : data.Length;
        //赋值为0时为均分一半，否则需要赋值
        var netMiddleToRight = data.MiddleToRight.Equals(0) ? netLength/2d : data.MiddleToRight;
        var netWidth = data.BackCj ? data.Width - 90d : data.Width;

        //烟罩宽度，考虑是否有BackCj
        swModelTop.ChangeDim("Width@DistanceWidth", netWidth);
        #endregion

        #region  Exhaust_UV_555，UV555排风装配
        ExhaustService.Uv555(swAssyTop, suffix, netLength, data.Height, data.SidePanel, data.UvLightType, netMiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.DrainType, data.WaterCollection, data.BackToBack, data.Marvel, data.Ansul, data.AnsulSide, data.AnsulDetector);
        #endregion

        #region SidePanel_Fs,大侧板装配
        SidePanelService.SidePanelFs(swAssyTop, suffix, data.SidePanel, netLength, data.Width, data.Height, data.BackCj, ExhaustType_e.UV);
        #endregion

        #region MidRoof_Fs,MidRoof装配
        MidRoofService.MidRoofFs(swAssyTop, suffix, netLength, netWidth, ExhaustType_e.UV, data.UvLightType, data.Bluetooth, netMiddleToRight, data.LightType, data.SpotLightNumber, data.SpotLightDistance, data.Marvel, data.Ansul, data.AnsulDropNumber, data.AnsulDropToFront, data.AnsulDropDis1, data.AnsulDropDis2, data.AnsulDropDis3, data.AnsulDropDis4, data.AnsulDropDis5, 0, AnsulDetectorEnd_e.无末端探测器, 0, 0, 0, 0, 0);
        #endregion

        #region Supply_I_555,I555新风装配
        SupplyService.I555(swAssyTop, suffix, netLength, netWidth, data.Height, ExhaustType_e.UV, data.SidePanel, data.UvLightType, data.Bluetooth, data.Marvel, data.LedLogo, data.WaterCollection);
        #endregion

        #region BackCj_Fs,BackCj装配
        SupplyService.BackCj(swAssyTop, suffix, data.BackCj, netLength, data.Height, data.CjSpigotToRight);
        #endregion
    }
    private void UviFr555(UviData data, ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix)
    {
        #region 计算中间值与顶层操作
        //计算烟罩净长度，计算烟罩净深度
        var netLength = data.SidePanel==SidePanel_e.左||data.SidePanel==SidePanel_e.右 ? data.Length-50d : data.SidePanel==SidePanel_e.双 ? data.Length-100d : data.Length;
        //赋值为0时为均分一半，否则需要赋值
        var netMiddleToRight = data.MiddleToRight.Equals(0) ? netLength/2d : data.MiddleToRight;
        var netWidth = data.BackCj ? data.Width - 90d : data.Width;

        //烟罩宽度，考虑是否有BackCj
        swModelTop.ChangeDim("Width@DistanceWidth", netWidth);
        #endregion

        #region  Exhaust_UV_FR_555，UVFR555排风装配
        ExhaustService.UvFr555(swAssyTop, suffix, netLength, netWidth, data.Height, data.SidePanel, data.UvLightType, netMiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.DrainType, data.WaterCollection, data.BackToBack, data.Marvel, data.Ansul, data.AnsulSide, data.AnsulDetector);
        #endregion

        #region SidePanel_Fr,大侧板装配
        SidePanelService.SidePanelFr(swAssyTop, suffix, data.SidePanel, netLength, data.Width, data.Height, data.BackCj, ExhaustType_e.UV);
        #endregion

        #region MidRoof_Fs,MidRoof装配
        MidRoofService.MidRoofFr(swAssyTop, suffix, netLength, netWidth, ExhaustType_e.UV, data.UvLightType, data.Bluetooth, netMiddleToRight, data.LightType, data.SpotLightNumber, data.SpotLightDistance, data.Marvel, data.Ansul, data.AnsulDropNumber, data.AnsulDropToFront, data.AnsulDropDis1, data.AnsulDropDis2, data.AnsulDropDis3, data.AnsulDropDis4, data.AnsulDropDis5, 0, AnsulDetectorEnd_e.无末端探测器, 0, 0, 0, 0, 0);
        #endregion

        #region Supply_I_FR_555,IFR555新风装配
        SupplyService.IFr555(swAssyTop, suffix, netLength, netWidth, data.Height, ExhaustType_e.UV, data.SidePanel, data.UvLightType, data.Bluetooth, data.Marvel, data.LedLogo, data.WaterCollection);
        #endregion

        #region BackCj_Fr,BackCj装配
        SupplyService.BackCjFr(swAssyTop, suffix, data.BackCj, netLength, data.Height, data.CjSpigotToRight);
        #endregion
    }

    private void Uvi450(UviData data, ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix)
    {
        #region 计算中间值与顶层操作
        //计算烟罩净长度，计算烟罩净深度
        var netLength = data.SidePanel==SidePanel_e.左||data.SidePanel==SidePanel_e.右 ? data.Length-50d : data.SidePanel==SidePanel_e.双 ? data.Length-100 : data.Length;
        //赋值为0时为均分一半，否则需要赋值
        var netMiddleToRight = data.MiddleToRight.Equals(0) ? netLength/2d : data.MiddleToRight;
        var netWidth = data.BackCj ? data.Width - 90 : data.Width;

        //烟罩宽度，考虑是否右BackCj
        swModelTop.ChangeDim("Width@DistanceWidth", netWidth);
        #endregion

        #region  Exhaust_UV_450，UV450排风装配
        ExhaustService.Uv450(swAssyTop, suffix, netLength, data.Height, data.SidePanel, data.UvLightType, netMiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.DrainType, data.WaterCollection, data.BackToBack, data.Marvel, data.Ansul, data.AnsulSide, data.AnsulDetector);
        #endregion

        #region SidePanel_Fs,大侧板装配
        SidePanelService.SidePanelFs(swAssyTop, suffix, data.SidePanel, netLength, data.Width, data.Height, data.BackCj, ExhaustType_e.UV);
        #endregion

        #region MidRoof_Fs,MidRoof装配
        MidRoofService.MidRoofFs(swAssyTop, suffix, netLength, netWidth, ExhaustType_e.UV, data.UvLightType, data.Bluetooth, netMiddleToRight, data.LightType, data.SpotLightNumber, data.SpotLightDistance, data.Marvel, data.Ansul, data.AnsulDropNumber, data.AnsulDropToFront, data.AnsulDropDis1, data.AnsulDropDis2, data.AnsulDropDis3, data.AnsulDropDis4, data.AnsulDropDis5, 0, AnsulDetectorEnd_e.无末端探测器, 0, 0, 0, 0, 0);
        #endregion

        #region Supply_I_450,I450新风装配
        SupplyService.I450(swAssyTop, suffix, netLength, netWidth, data.Height, ExhaustType_e.UV, data.SidePanel, data.UvLightType, data.Bluetooth, data.Marvel, data.LedLogo, data.WaterCollection);
        #endregion

        #region BackCj_Fs,BackCj装配
        SupplyService.BackCj(swAssyTop, suffix, data.BackCj, netLength, data.Height, data.CjSpigotToRight);
        #endregion
    }

    private void Uvi450400(UviData data, ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix)
    {
        #region 计算中间值与顶层操作
        //计算烟罩净长度，计算烟罩净深度
        var netLength = data.SidePanel==SidePanel_e.左||data.SidePanel==SidePanel_e.右 ? data.Length-50d : data.SidePanel==SidePanel_e.双 ? data.Length-100d : data.Length;
        //赋值为0时为均分一半，否则需要赋值
        var netMiddleToRight = data.MiddleToRight.Equals(0) ? netLength/2d : data.MiddleToRight;
        var netWidth = data.BackCj ? data.Width - 90d : data.Width;

        //烟罩宽度，考虑是否有BackCj
        swModelTop.ChangeDim("Width@DistanceWidth", netWidth);
        #endregion

        #region  Exhaust_UV_450，UV450排风装配
        ExhaustService.Uv450(swAssyTop, suffix, netLength,data.Height, data.SidePanel, data.UvLightType, netMiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.DrainType, data.WaterCollection, data.BackToBack, data.Marvel, data.Ansul, data.AnsulSide, data.AnsulDetector);
        #endregion

        #region SidePanel_Neq,大侧板装配
        SidePanelService.SidePanelNeq(swAssyTop, suffix, data.SidePanel, netLength, data.Width, data.Height,400d, data.BackCj, ExhaustType_e.UV);
        #endregion

        #region MidRoof_Fs,MidRoof装配
        MidRoofService.MidRoofFs(swAssyTop, suffix, netLength, netWidth, ExhaustType_e.UV, data.UvLightType, data.Bluetooth, netMiddleToRight, data.LightType, data.SpotLightNumber, data.SpotLightDistance, data.Marvel, data.Ansul, data.AnsulDropNumber, data.AnsulDropToFront, data.AnsulDropDis1, data.AnsulDropDis2, data.AnsulDropDis3, data.AnsulDropDis4, data.AnsulDropDis5, 0, AnsulDetectorEnd_e.无末端探测器, 0, 0, 0, 0, 0);
        #endregion

        #region Supply_I_400,I400新风装配
        SupplyService.I400(swAssyTop, suffix, netLength, netWidth, data.Height, ExhaustType_e.UV, data.SidePanel, data.UvLightType, data.Bluetooth, data.Marvel, data.LedLogo, data.WaterCollection);
        #endregion

        #region BackCj_Fs,BackCj装配
        //SupplyService.BackCj(swAssyTop, suffix, data.BackCj, netLength, data.Height, data.CjSpigotToRight);
        #endregion
    }


    private void UviHw650(UviData data, ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix)
    {
        #region 计算中间值与顶层操作
        //计算烟罩净长度，计算烟罩净深度
        var netLength = data.SidePanel==SidePanel_e.左||data.SidePanel==SidePanel_e.右 ? data.Length-50d : data.SidePanel==SidePanel_e.双 ? data.Length-100d : data.Length;
        //赋值为0时为均分一半，否则需要赋值
        var netMiddleToRight = data.MiddleToRight.Equals(0) ? netLength/2d : data.MiddleToRight;
        var netWidth = data.BackCj ? data.Width - 90d : data.Width;

        //烟罩宽度，考虑是否有BackCj
        swModelTop.ChangeDim("Width@DistanceWidth", netWidth);
        #endregion

        #region  Exhaust_UV_HW_650，排风装配
        ExhaustService.UvHw650(swAssyTop, suffix, netLength, data.Height, data.SidePanel, data.UvLightType, netMiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.DrainType, data.WaterCollection, data.BackToBack, data.Marvel, data.Ansul, data.AnsulSide, data.AnsulDetector);
        #endregion

        #region SidePanel_Hw,大侧板装配
        SidePanelService.SidePanelHw(swAssyTop, suffix, data.SidePanel, netLength, data.Width, data.Height, false, ExhaustType_e.UV);
        #endregion

        #region MidRoof_Hw,MidRoof装配
        MidRoofService.MidRoofHw(swAssyTop, suffix, netLength, netWidth,data.Height, ExhaustType_e.UV, data.UvLightType, data.Bluetooth, netMiddleToRight, data.LightType, data.LightToFront, data.SpotLightNumber, data.SpotLightDistance, data.Marvel, data.Ansul, data.AnsulDropNumber, data.AnsulDropToFront, data.AnsulDropDis1, data.AnsulDropDis2, data.AnsulDropDis3, data.AnsulDropDis4, data.AnsulDropDis5, 0, AnsulDetectorEnd_e.无末端探测器, 0, 0, 0, 0, 0);

        #endregion

        #region Supply_I_HW_650,新风装配
        SupplyService.IHw650(swAssyTop, suffix, netLength, netWidth, data.Height, ExhaustType_e.UV, data.SidePanel, data.UvLightType, data.Bluetooth, data.Marvel, data.LedLogo, data.WaterCollection);
        #endregion
    }



}