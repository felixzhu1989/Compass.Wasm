using System.Threading.Tasks;
using Compass.Wasm.Shared.DataService;
using Compass.Wasm.Shared.ProjectService;
using Compass.Wpf.DrawingServices;
using Compass.Wpf.Extensions;
using Compass.Wpf.Service.Hoods;
using Prism.Ioc;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.BatchWorks.Hoods;

public class KviAutoDrawing : IKviAutoDrawing
{
    private readonly IKviDataService _service;
    private readonly ISldWorks _swApp;
    private readonly IExhaustService _exhaustService;
    private readonly ISidePanelService _sidePanelService;
    private readonly IMidRoofService _midRoofService;
    private readonly ISupplyService _supplyService;
    

    public KviAutoDrawing(IKviDataService service, ISldWorksService sldWorksService, IContainerProvider containerProvider)
    {
        _service = service;
        _swApp = sldWorksService.SwApp;
        _exhaustService = containerProvider.Resolve<IExhaustService>();
        _sidePanelService=containerProvider.Resolve<ISidePanelService>();
        _midRoofService = containerProvider.Resolve<IMidRoofService>();
        _supplyService=containerProvider.Resolve<ISupplyService>();

    }
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
            var packPath = _swApp.PackToProject(out var suffix, modelPath, moduleDto);
            //顶级Model,顶级Assy,打开Pack后的模型packPath
            var swAssyTop = _swApp.OpenAssemblyDoc(out var swModelTop, packPath);
            #endregion


            #region 计算中间值与顶层操作
            //计算烟罩净长度，计算烟罩净深度
            var netLength = data.SidePanel==SidePanel_e.左||data.SidePanel==SidePanel_e.右 ? data.Length-50d : data.SidePanel==SidePanel_e.双 ? data.Length-100 : data.Length;
            var netMiddleToRight = data.MiddleToRight.Equals(data.Length/2d) ? netLength/2d : data.MiddleToRight;
            var netWidth = data.BackCj ? data.Width - 90 : data.Width;

            //烟罩宽度，考虑是否右BackCj
            swModelTop.ChangeDim("Width@DistanceWidth", netWidth);
            #endregion

            #region  Exhaust_KV_555，KV555排风装配
            _exhaustService.Kv555(swAssyTop, suffix, netLength, data.SidePanel, UvLightType_e.No, netMiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotDis, data.DrainType, data.WaterCollection, data.BackToBack, data.Marvel, data.Ansul, data.AnsulSide, data.AnsulDetector);
            #endregion

            #region ExhaustSpigot_Fs，排风脖颈装配
            _exhaustService.ExhaustSpigotFs(swAssyTop, suffix, netMiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.Marvel, data.Ansul);
            #endregion

            #region SidePanel_Fs,大侧板装配
            _sidePanelService.SidePanelFs(swAssyTop, suffix, data.SidePanel, netLength, data.Width, data.Height, data.BackCj, ExhaustType_e.KV);
            #endregion

            #region MidRoof_Fs,MidRoof装配
            _midRoofService.MidRoofFs(swAssyTop, suffix, netLength, netWidth, ExhaustType_e.KV, UvLightType_e.No, false, netMiddleToRight, data.LightType, data.SpotLightNumber, data.SpotLightDistance, data.Marvel, data.Ansul, data.AnsulDropNumber, data.AnsulDropToFront, data.AnsulDropDis1, data.AnsulDropDis2, data.AnsulDropDis3, data.AnsulDropDis4, data.AnsulDropDis5, 0, AnsulDetectorEnd_e.无末端探测器, 0, 0, 0, 0, 0);
            #endregion

            #region Supply_I_555,I555新风装配
            _supplyService.I555(swAssyTop, suffix, netLength, netWidth,data.Height, ExhaustType_e.KV,data.SidePanel, UvLightType_e.No, false, data.Marvel, data.LedLogo, data.WaterCollection);
            #endregion

            #region BackCj_Fs,BackCj装配
            _supplyService.BackCj(swAssyTop,suffix,data.BackCj,netLength,data.Height,data.CjSpigotToRight);
            #endregion

            
            

            #region 保存操作
            //设置成true，直接更新顶层，速度很快，设置成false，每个零件都会更新，很慢
            swModelTop.ForceRebuild3(true); 
            swModelTop.Save(); //保存，很耗时间
            _swApp.CloseDoc(packPath); //关闭，很快
            #endregion
        }
        catch
        {
            _swApp.CommandInProgress = false;
            await Task.Delay(500);
            throw;
        }
    }
}