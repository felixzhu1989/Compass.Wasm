﻿using System.Threading.Tasks;
using Compass.Wasm.Shared.Data;
using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wpf.ApiServices.Hoods;
using Compass.Wpf.SwServices;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.BatchWorks.Hoods;

public interface ICmodfAutoDrawing : IAutoDrawing
{
}

public class CmodfAutoDrawing : BaseAutoDrawing, ICmodfAutoDrawing
{
    #region ctor
    private readonly ICmodfDataService _service;
    public CmodfAutoDrawing(IContainerProvider provider) : base(provider)
    {
        _service = provider.Resolve<ICmodfDataService>();
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
                case "CMODF_555_400":
                    Cmodf555400(data, swModelTop, swAssyTop, suffix);
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

    private void Cmodf555400(CmodfData data, ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix)
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

        #region  Exhaust_CMOD_555，CMOD_555排风装配

        ExhaustService.Cmod555(swAssyTop, suffix, netLength, data.Height, data.SidePanel, netMiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.DrainType, data.WaterCollection, data.BackToBack, data.Marvel, data.Ansul, data.AnsulSide, data.WaterInlet);


        #endregion

        #region SidePanel_Neq,大侧板装配
        SidePanelService.SidePanelNeq(swAssyTop, suffix, data.SidePanel, netLength, data.Width, data.Height, 400d, data.BackCj, ExhaustType_e.CMOD);
        #endregion

        #region MidRoof_Fs,MidRoof装配
        MidRoofService.MidRoofFs(swAssyTop, suffix, netLength, netWidth, ExhaustType_e.CMOD, UvLightType_e.NA, false, netMiddleToRight, data.LightType, data.SpotLightNumber, data.SpotLightDistance, data.Marvel, data.Ansul, data.AnsulDropNumber, data.AnsulDropToFront, data.AnsulDropDis1, data.AnsulDropDis2, data.AnsulDropDis3, data.AnsulDropDis4, data.AnsulDropDis5, data.AnsulDetectorNumber, data.AnsulDetectorEnd, data.AnsulDetectorDis1, data.AnsulDetectorDis2, data.AnsulDetectorDis3, data.AnsulDetectorDis4, data.AnsulDetectorDis5);
        #endregion

        #region Supply_F_400,F400新风装配
        SupplyService.F400(swAssyTop, suffix, netLength, netWidth, data.Height, ExhaustType_e.CMOD, data.SidePanel, UvLightType_e.NA, false, data.Marvel, data.LedLogo, data.WaterCollection, data.SupplySpigotNumber, data.SupplySpigotDis);
        #endregion

        #region BackCj_Fs,BackCj装配
        //SupplyService.BackCj(swAssyTop, suffix, data.BackCj, netLength, data.Height, data.CjSpigotToRight);
        #endregion
    }
}