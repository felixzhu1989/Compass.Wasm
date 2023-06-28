﻿using System.Threading.Tasks;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Data;
using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wpf.ApiServices.Hoods;
using Compass.Wpf.SwServices;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.BatchWorks.Hoods;

public interface IUvfAutoDrawing : IAutoDrawing
{
}

public class UvfAutoDrawing : BaseAutoDrawing, IUvfAutoDrawing
{
    #region ctor
    private readonly IUvfDataService _service;
    public UvfAutoDrawing(IContainerProvider provider) : base(provider)
    {
        _service = provider.Resolve<IUvfDataService>();

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
            //打包,后续需要使用到的变量：suffix，packPath
            var packPath = SwApp.PackToProject(out var suffix, modelPath, moduleDto, Aggregator);
            //顶级Model,顶级Assy,打开Pack后的模型packPath
            var swAssyTop = SwApp.OpenAssemblyDoc(out var swModelTop, packPath, Aggregator);
            #endregion

            switch (moduleDto.ModelName)
            {
                case "UVF_555":
                    Uvf555(data, swModelTop, swAssyTop, suffix);
                    break;
                case "UVF_HW_650":
                    UvfHw650(data, swModelTop, swAssyTop, suffix);
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

    private void Uvf555(UvfData data, ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix)
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

        #region  Exhaust_UV_555，KV555排风装配
        ExhaustService.Uv555(swAssyTop, suffix, netLength, data.Height, data.SidePanel, data.UvLightType, netMiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.DrainType, data.WaterCollection, data.BackToBack, data.Marvel, data.Ansul, data.AnsulSide, data.AnsulDetector);

        #endregion

        #region SidePanel_Fs,大侧板装配
        SidePanelService.SidePanelFs(swAssyTop, suffix, data.SidePanel, netLength, data.Width, data.Height, data.BackCj, ExhaustType_e.UV);
        #endregion

        #region MidRoof_Fs,MidRoof装配
        MidRoofService.MidRoofFs(swAssyTop, suffix, netLength, netWidth, ExhaustType_e.UV, data.UvLightType, data.Bluetooth, netMiddleToRight, data.LightType, data.SpotLightNumber, data.SpotLightDistance, data.Marvel, data.Ansul, data.AnsulDropNumber, data.AnsulDropToFront, data.AnsulDropDis1, data.AnsulDropDis2, data.AnsulDropDis3, data.AnsulDropDis4, data.AnsulDropDis5, 0, AnsulDetectorEnd_e.无末端探测器, 0, 0, 0, 0, 0);
        #endregion

        #region Supply_F_555,F555新风装配
        SupplyService.F555(swAssyTop, suffix, netLength, netWidth, data.Height, ExhaustType_e.KV, data.SidePanel, data.UvLightType, data.Bluetooth, data.Marvel, data.LedLogo, data.WaterCollection, data.SupplySpigotNumber, data.SupplySpigotDis);
        #endregion

        #region BackCj_Fs,BackCj装配
        SupplyService.BackCj(swAssyTop, suffix, data.BackCj, netLength, data.Height, data.CjSpigotToRight);
        #endregion
    }
    private void UvfHw650(UvfData data, ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix)
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

        #region  Exhaust_UV_HW_650，排风装配
        ExhaustService.UvHw650(swAssyTop, suffix, netLength, data.Height, data.SidePanel, data.UvLightType, netMiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.DrainType, data.WaterCollection, data.BackToBack, data.Marvel, data.Ansul, data.AnsulSide, data.AnsulDetector);

        #endregion

        #region SidePanel_Hw,大侧板装配
        SidePanelService.SidePanelHw(swAssyTop, suffix, data.SidePanel, netLength, data.Width, data.Height, false, ExhaustType_e.UV);
        #endregion

        #region MidRoof_Hw,MidRoof装配
        MidRoofService.MidRoofHw(swAssyTop, suffix, netLength, netWidth, data.Height,ExhaustType_e.UV, data.UvLightType, data.Bluetooth, netMiddleToRight, data.LightType,data.LightToFront, data.SpotLightNumber, data.SpotLightDistance, data.Marvel, data.Ansul, data.AnsulDropNumber, data.AnsulDropToFront, data.AnsulDropDis1, data.AnsulDropDis2, data.AnsulDropDis3, data.AnsulDropDis4, data.AnsulDropDis5, 0, AnsulDetectorEnd_e.无末端探测器, 0, 0, 0, 0, 0);
        #endregion

        #region Supply_F_HW_650,F555新风装配
        SupplyService.FHw650(swAssyTop, suffix, netLength, netWidth, data.Height, ExhaustType_e.KV, data.SidePanel, data.UvLightType, data.Bluetooth, data.Marvel, data.LedLogo, data.WaterCollection, data.SupplySpigotNumber, data.SupplySpigotDis);
        #endregion

    }
}