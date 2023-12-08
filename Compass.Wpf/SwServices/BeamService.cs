using Compass.Wasm.Shared.Data;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.SwServices;

public class BeamService : BaseSwService, IBeamService
{
    public readonly IExhaustService ExhaustService;
    public readonly ICeilingService CeilingService;
    public BeamService(IContainerProvider provider) : base(provider)
    {
        ExhaustService = provider.Resolve<IExhaustService>();
        CeilingService=provider.Resolve<ICeilingService>();
    }

    #region 排风腔
    //KCJ
    public void KcjDb800(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, KcjData data)
    {
        //计算过滤器数量
        var filterNumber = (int)((data.Length - data.FilterLeft - data.FilterRight) / 499d) - data.FilterBlindNumber;
        //公共零件
        //重命名排风腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "KCJDB800", module, "FNCE0115-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCE0115(swCompLevel2, data.Length, UvLightType_e.NA, data.LightCable, data.Marvel, data.ExhaustSpigotNumber, data.MiddleToRight, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotDis, data.Ansul, data.AnsulSide, data.AnsulDetectorNumber, data.AnsulDetectorEnd, data.AnsulDetectorDis1, data.AnsulDetectorDis2, data.AnsulDetectorDis3, data.AnsulDetectorDis4, data.AnsulDetectorDis5, data.Japan);
        }

        //过滤器盲板
        FilterBlind(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, data.FilterLeft, "FNCE0107[BP-500]{500}-1", "LocalLPatternBlind", "Dis@DistanceBlind");
        FilterBlind(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, data.FilterLeft, "FNCE0107[BP-500]{500}-5", "LocalLPatternBlind", "Dis@DistanceBlind");
        //过滤器
        KcjFilter(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, filterNumber, data.FilterLeft, data.FilterType, "KcjFcFilter-1", "LocalLPatternFc", "Dis@DistanceFc", "KcjKsaFilter-1", "LocalLPatternKsa", "Dis@DistanceKsa");
        KcjFilter(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, filterNumber, data.FilterLeft, data.FilterType, "KcjFcFilter-7", "LocalLPatternFc", "Dis@DistanceFc", "KcjKsaFilter-7", "LocalLPatternKsa", "Dis@DistanceKsa");

        //过滤器侧板
        KcjFilterSide(swModelTop, swAssyTop, suffix, module, data.FilterSide, data.FilterType, filterNumber, data.FilterLeft, data.FilterRight, "FNCE0108-1", "FNCE0109-1");
        KcjFilterSide(swModelTop, swAssyTop, suffix, module, data.FilterSide, data.FilterType, filterNumber, data.FilterLeft, data.FilterRight, "FNCE0108-2", "FNCE0109-2");

        //SSP灯板支撑条
        SspSupport(swModelTop, swAssyTop, suffix, data.Length, data.DomeSsp, data.Gutter, data.GutterWidth, "FNCE0035-1", "Dis@DistanceDome1", "FNCE0036-1", "Dis@DistanceFlat1");
        SspSupport(swModelTop, swAssyTop, suffix, data.Length, data.DomeSsp, data.Gutter, data.GutterWidth, "FNCE0035-2", "Dis@DistanceDome2", "FNCE0036-2", "Dis@DistanceFlat2");

        //日本项目需要压缩零件(吊装垫片和脖颈)
        if (data.Japan)
        {
            swAssyTop.Suppress(suffix, "FNCE0070-1");
            swAssyTop.Suppress("LocalLPatternLifting");
            swAssyTop.Suppress(suffix, "ExhaustSpigot_Fs-1");
            //如果是左侧或者双侧时解压日本灯腔侧板
            if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
            {
                var swAssyPanelJapan = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Japan-1", Aggregator);
                CeilingService.LightPanelSsJapan(swAssyPanelJapan, suffix, module, data.TotalLength, data.LongGlassNumber, data.ShortGlassNumber, data.LeftLength, data.RightLength, data.MiddleLength);
            }
            else
            {
                swAssyTop.Suppress(suffix, "LightPanelSs_Japan-1");
            }
        }
        else
        {
            swAssyTop.UnSuppress(suffix, "FNCE0070-1", Aggregator);
            swAssyTop.UnSuppress("LocalLPatternLifting");

            //排风脖颈
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "ExhaustSpigot_Fs-1", Aggregator);
            ExhaustService.ExhaustSpigotFs(swAssyLevel1, suffix, data.Length, data.MiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.Marvel, data.Marvel, data.Ansul, ExhaustType_e.NA);
            swAssyTop.Suppress(suffix, "LightPanelSs_Japan-1");
        }

        //HCL
        if (data.CeilingLightType is CeilingLightType_e.HCL)
        {
            swAssyTop.Suppress(suffix, "NormalLight_KCJ_DB_800-1");
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(out var swModelLevel1, suffix, "HclLight_KCJ_DB_800-1", Aggregator);

            HclLightKcjDb800(swModelLevel1, swAssyLevel1, suffix, module, data.Length, UvLightType_e.NA, 0, data.FilterLeft, data.FilterRight, data.LightCable, data.CeilingLightType, data.Japan, data.HclSide, data.HclLeft, data.HclRight);

            swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
            swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
        }
        else
        {
            swAssyTop.Suppress(suffix, "HclLight_KCJ_DB_800-1");
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "NormalLight_KCJ_DB_800-1", Aggregator);

            NormalLightKcjDb800(swAssyLevel1, suffix, data.Length, UvLightType_e.NA, 0, data.FilterLeft, data.FilterRight, data.LightCable, data.CeilingLightType, data.Japan);

            //如果是左侧或双侧时，解压灯腔侧板
            if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
            {
                if (data.CeilingLightType is CeilingLightType_e.筒灯)
                {
                    swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
                    var swAssyPanelLed = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Led-1", Aggregator);
                    CeilingService.LightPanelSsLed(swAssyPanelLed, suffix, module, data.TotalLength);
                }
                else
                {
                    swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
                    var swAssyPanelGlass = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Glass-1", Aggregator);
                    CeilingService.LightPanelSsGlass(swAssyPanelGlass, suffix, module, data.TotalLength, data.LongGlassNumber, data.ShortGlassNumber);
                }
            }
            else
            {
                swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
                swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
            }
        }
    }

    public void KcjSb535(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, KcjData data)
    {
        //计算过滤器数量
        var filterNumber = (int)((data.Length - data.FilterLeft - data.FilterRight) / 499d) - data.FilterBlindNumber;
        //左右两侧辅组参考面
        swModelTop.ChangeDim("Dis@BeamLeft", data.Length/2d);
        swModelTop.ChangeDim("Dis@BeamRight", data.Length/2d);

        //过滤器盲板
        FilterBlind(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, data.FilterLeft, "FNCE0107[BP-500]{500}-1", "LocalLPatternBlind", "Dis@DistanceBlind");

        //过滤器
        KcjFilter(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, filterNumber, data.FilterLeft, data.FilterType, "KcjFcFilter-1", "LocalLPatternFc", "Dis@DistanceFc", "KcjKsaFilter-1", "LocalLPatternKsa", "Dis@DistanceKsa");

        //过滤器侧板
        KcjFilterSide(swModelTop, swAssyTop, suffix, module, data.FilterSide, data.FilterType, filterNumber, data.FilterLeft, data.FilterRight, "FNCE0108-1",  "FNCE0109-1");

        //SSP灯板支撑条
        SspSupport(swModelTop, swAssyTop, suffix, data.Length, data.DomeSsp, data.Gutter, data.GutterWidth, "FNCE0035-1", "Dis@DistanceDome", "FNCE0036-1", "Dis@DistanceFlat");

        //日本项目需要压缩零件(吊装垫片和脖颈)
        if (data.Japan)
        {
            swAssyTop.Suppress(suffix, "FNCE0070-1");
            swAssyTop.Suppress("LocalLPatternLifting");
            swAssyTop.Suppress(suffix, "ExhaustSpigot_Fs-1");
            //如果是左侧或者双侧时解压日本灯腔侧板
            if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
            {
                var swAssyPanelJapan = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Japan-1", Aggregator);
                CeilingService.LightPanelSsJapan(swAssyPanelJapan, suffix, module, data.TotalLength, data.LongGlassNumber, data.ShortGlassNumber, data.LeftLength, data.RightLength, data.MiddleLength);
            }
            else
            {
                swAssyTop.Suppress(suffix, "LightPanelSs_Japan-1");
            }
        }
        else
        {
            swAssyTop.UnSuppress(suffix, "FNCE0070-1", Aggregator);
            swAssyTop.UnSuppress("LocalLPatternLifting");

            //排风脖颈
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "ExhaustSpigot_Fs-1", Aggregator);
            ExhaustService.ExhaustSpigotFs(swAssyLevel1, suffix, data.Length, data.MiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.Marvel, data.Marvel, data.Ansul, ExhaustType_e.NA);

            swAssyTop.Suppress(suffix, "LightPanelSs_Japan-1");
        }

        //HCL
        if (data.CeilingLightType is CeilingLightType_e.HCL)
        {
            swAssyTop.Suppress(suffix, "NormalLight_KCJ_SB_535-1");
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(out var swModelLevel1, suffix, "HclLight_KCJ_SB_535-1", Aggregator);

            HclLightKcjSb535(swModelLevel1, swAssyLevel1, suffix, module, "KCJSB535", data.Length, data.Width, UvLightType_e.NA, 0, data.FilterLeft, data.LightCable, data.CeilingLightType, data.HclSide, data.HclLeft, data.HclRight, data.Marvel, data.ExhaustSpigotNumber, data.MiddleToRight, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotDis, data.Ansul, data.AnsulSide, data.AnsulDetector, data.Japan);

            swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
            swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
        }
        else
        {
            swAssyTop.Suppress(suffix, "HclLight_KCJ_SB_535-1");
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(out var swModelLevel1, suffix, "NormalLight_KCJ_SB_535-1", Aggregator);

            NormalLightKcjSb535(swAssyLevel1, suffix, module, "KCJSB535", data.Length, data.Width, UvLightType_e.NA, 0, data.FilterLeft, data.LightCable, data.CeilingLightType, data.Marvel, data.ExhaustSpigotNumber, data.MiddleToRight, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotDis, data.Ansul, data.AnsulSide, data.AnsulDetector, data.Japan);
            //如果是左侧或双侧时，解压灯腔侧板
            if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
            {
                if (data.CeilingLightType is CeilingLightType_e.筒灯)
                {
                    swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
                    var swAssyPanelLed = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Led-1", Aggregator);
                    CeilingService.LightPanelSsLed(swAssyPanelLed, suffix, module, data.TotalLength);
                }
                else
                {
                    swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
                    var swAssyPanelGlass = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Glass-1", Aggregator);
                    CeilingService.LightPanelSsGlass(swAssyPanelGlass, suffix, module, data.TotalLength, data.LongGlassNumber, data.ShortGlassNumber);
                }
            }
            else
            {
                swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
                swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
            }
        }

    }

    public void KcjSb290(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, KcjData data)
    {
        //计算过滤器数量
        var filterNumber = (int)((data.Length - data.FilterLeft - data.FilterRight) / 499d) - data.FilterBlindNumber;

        //重命名排风腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "KCJSB290", module, "FNCE0127-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCE0127(swCompLevel2, data.Length, UvLightType_e.NA, data.Marvel, data.ExhaustSpigotNumber, data.MiddleToRight, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotDis, data.Ansul, data.AnsulSide, data.AnsulDetector, data.Japan);
        }

        //过滤器盲板
        FilterBlind(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, data.FilterLeft, "FNCE0107[BP-500]{500}-1", "LocalLPatternBlind", "Dis@DistanceBlind");

        //过滤器
        KcjFilter(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, filterNumber, data.FilterLeft, data.FilterType, "KcjFcFilter-1", "LocalLPatternFc", "Dis@DistanceFc", "KcjKsaFilter-1", "LocalLPatternKsa", "Dis@DistanceKsa");

        //过滤器侧板
        KcjFilterSide(swModelTop, swAssyTop, suffix, module, data.FilterSide, data.FilterType, filterNumber, data.FilterLeft, data.FilterRight, "FNCE0108-1", "FNCE0109-1");

        //SSP灯板支撑条
        SspSupport(swModelTop, swAssyTop, suffix, data.Length, data.DomeSsp, data.Gutter, data.GutterWidth, "FNCE0035-1", "Dis@DistanceDome", "FNCE0036-1", "Dis@DistanceFlat");

        //日本项目需要压缩零件(吊装垫片和脖颈)
        if (data.Japan)
        {
            swAssyTop.Suppress(suffix, "FNCE0070-1");
            swAssyTop.Suppress("LocalLPatternLifting");
            swAssyTop.Suppress(suffix, "ExhaustSpigot_Fs-1");
        }
        else
        {
            swAssyTop.UnSuppress(suffix, "FNCE0070-1", Aggregator);
            swAssyTop.UnSuppress("LocalLPatternLifting");

            //排风脖颈
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "ExhaustSpigot_Fs-1", Aggregator);
            ExhaustService.ExhaustSpigotFs(swAssyLevel1, suffix, data.Length, data.MiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.Marvel, data.Marvel, data.Ansul, ExhaustType_e.NA);
        }

    }

    public void KcjSb265(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, KcjData data)
    {
        //计算过滤器数量
        var filterNumber = (int)((data.Length - data.FilterLeft - data.FilterRight) / 499d) - data.FilterBlindNumber;

        //重命名排风腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "KCJSB265", module, "FNCE0125-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCE0127(swCompLevel2, data.Length, UvLightType_e.NA, data.Marvel, data.ExhaustSpigotNumber, data.MiddleToRight, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotDis, data.Ansul, data.AnsulSide, data.AnsulDetector, data.Japan);
        }


        //过滤器盲板
        FilterBlind(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, data.FilterLeft, "FNCE0107[BP-500]{500}-1", "LocalLPatternBlind", "Dis@DistanceBlind");

        //过滤器
        KcjFilter(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, filterNumber, data.FilterLeft, data.FilterType, "KcjFcFilter-1", "LocalLPatternFc", "Dis@DistanceFc", "KcjKsaFilter-1", "LocalLPatternKsa", "Dis@DistanceKsa");

        //过滤器侧板
        KcjFilterSide(swModelTop, swAssyTop, suffix, module, data.FilterSide, data.FilterType, filterNumber, data.FilterLeft, data.FilterRight, "FNCE0108-1", "FNCE0109-1");

        //SSP灯板支撑条
        SspSupport(swModelTop, swAssyTop, suffix, data.Length, data.DomeSsp, data.Gutter, data.GutterWidth, "FNCE0035-1", "Dis@DistanceDome", "FNCE0036-1", "Dis@DistanceFlat");

        //日本项目需要压缩零件(吊装垫片和脖颈)
        if (data.Japan)
        {
            swAssyTop.Suppress(suffix, "FNCE0070-1");
            swAssyTop.Suppress("LocalLPatternLifting");
            swAssyTop.Suppress(suffix, "ExhaustSpigot_Fs-1");
        }
        else
        {
            swAssyTop.UnSuppress(suffix, "FNCE0070-1", Aggregator);
            swAssyTop.UnSuppress("LocalLPatternLifting");

            //排风脖颈
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "ExhaustSpigot_Fs-1", Aggregator);
            ExhaustService.ExhaustSpigotFs(swAssyLevel1, suffix, data.Length, data.MiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.Marvel, data.Marvel, data.Ansul, ExhaustType_e.NA);
        }

    }

    //UCJ
    public void UcjDb800(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, UcjData data)
    {
        //计算过滤器数量
        var filterNumber = (int)((data.Length - data.FilterLeft - data.FilterRight) / 499d) - data.FilterBlindNumber;
        //公共零件
        //重命名排风腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "UCJDB800", module, "FNCE0115-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCE0115(swCompLevel2, data.Length, data.UvLightType, data.LightCable, data.Marvel, data.ExhaustSpigotNumber, data.MiddleToRight, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotDis, data.Ansul, data.AnsulSide, data.AnsulDetectorNumber, data.AnsulDetectorEnd, data.AnsulDetectorDis1, data.AnsulDetectorDis2, data.AnsulDetectorDis3, data.AnsulDetectorDis4, data.AnsulDetectorDis5, data.Japan);
        }

        //过滤器盲板
        FilterBlind(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, data.FilterLeft, "FNCE0107[BP-500]{500}-1", "LocalLPatternBlind", "Dis@DistanceBlind");
        FilterBlind(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, data.FilterLeft, "FNCE0107[BP-500]{500}-5", "LocalLPatternBlind", "Dis@DistanceBlind");
        //过滤器
        UcjFilter(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, filterNumber, data.FilterLeft,  "UcjFcCombi-1", "LocalLPatternFc", "Dis@DistanceFc");
        UcjFilter(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, filterNumber, data.FilterLeft,  "UcjFcCombi-7", "LocalLPatternFc", "Dis@DistanceFc");

        //过滤器侧板
        KcjFilterSide(swModelTop, swAssyTop, suffix, module, data.FilterSide, data.FilterType, filterNumber, data.FilterLeft, data.FilterRight, "FNCE0136-1", "FNCE0109-1");
        KcjFilterSide(swModelTop, swAssyTop, suffix, module, data.FilterSide, data.FilterType, filterNumber, data.FilterLeft, data.FilterRight, "FNCE0108-1", "FNCE0162-1");
        //带把手过滤器侧板的磁铁支架
        var leftSide = data.FilterSide is FilterSide_e.两过滤器侧板 or FilterSide_e.左过滤器侧板;
        UcjFilterSideSensor(swAssyTop, suffix, data.CeilingLightType, leftSide, "FNCE0100-1", "FNCE0101-1");
        var rightSide = data.FilterSide is FilterSide_e.两过滤器侧板 or FilterSide_e.右过滤器侧板;
        UcjFilterSideSensor(swAssyTop, suffix, data.CeilingLightType, rightSide, "FNCE0100-2", "FNCE0101-2");


        //SSP灯板支撑条
        SspSupport(swModelTop, swAssyTop, suffix, data.Length, data.DomeSsp, data.Gutter, data.GutterWidth, "FNCE0035-1", "Dis@DistanceDome1", "FNCE0036-1", "Dis@DistanceFlat1");
        SspSupport(swModelTop, swAssyTop, suffix, data.Length, data.DomeSsp, data.Gutter, data.GutterWidth, "FNCE0035-2", "Dis@DistanceDome2", "FNCE0036-2", "Dis@DistanceFlat2");


        //UV灯
        UvLightAsm(swAssyTop, suffix, data.UvLightType, "CeilingUvRackSpecial_4S-1", "CeilingUvRackSpecial_4L-1");


        //日本项目需要压缩零件(吊装垫片和脖颈)
        if (data.Japan)
        {
            swAssyTop.Suppress(suffix, "FNCE0070-1");
            swAssyTop.Suppress("LocalLPatternLifting");
            swAssyTop.Suppress(suffix, "ExhaustSpigot_Fs-1");
            //如果是左侧或者双侧时解压日本灯腔侧板
            if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
            {
                var swAssyPanelJapan = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Japan-1", Aggregator);
                CeilingService.LightPanelSsJapan(swAssyPanelJapan, suffix, module, data.TotalLength, data.LongGlassNumber, data.ShortGlassNumber, data.LeftLength, data.RightLength, data.MiddleLength);
            }
            else
            {
                swAssyTop.Suppress(suffix, "LightPanelSs_Japan-1");
            }
        }
        else
        {
            swAssyTop.UnSuppress(suffix, "FNCE0070-1", Aggregator);
            swAssyTop.UnSuppress("LocalLPatternLifting");

            //排风脖颈
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "ExhaustSpigot_Fs-1", Aggregator);
            //UCJ的排风滑门和轨道在UV灯支架中，因此不需要滑门和导轨,marvelRail取值true
            ExhaustService.ExhaustSpigotFs(swAssyLevel1, suffix, data.Length, data.MiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.Marvel, true, data.Ansul, ExhaustType_e.NA);
            swAssyTop.Suppress(suffix, "LightPanelSs_Japan-1");
        }

        //HCL
        if (data.CeilingLightType is CeilingLightType_e.HCL)
        {
            swAssyTop.Suppress(suffix, "NormalLight_KCJ_DB_800-1");
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(out var swModelLevel1, suffix, "HclLight_KCJ_DB_800-1", Aggregator);

            HclLightKcjDb800(swModelLevel1, swAssyLevel1, suffix, module, data.Length, data.UvLightType, data.FilterBlindNumber+filterNumber, data.FilterLeft, data.FilterRight, data.LightCable, data.CeilingLightType, data.Japan, data.HclSide, data.HclLeft, data.HclRight);
            swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
            swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
        }
        else
        {
            swAssyTop.Suppress(suffix, "HclLight_KCJ_DB_800-1");
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "NormalLight_KCJ_DB_800-1", Aggregator);

            NormalLightKcjDb800(swAssyLevel1, suffix, data.Length, data.UvLightType, data.FilterBlindNumber+filterNumber, data.FilterLeft, data.FilterRight, data.LightCable, data.CeilingLightType, data.Japan);
            //如果是左侧或双侧时，解压灯腔侧板
            if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
            {
                if (data.CeilingLightType is CeilingLightType_e.筒灯)
                {
                    swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
                    var swAssyPanelLed = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Led-1", Aggregator);
                    CeilingService.LightPanelSsLed(swAssyPanelLed, suffix, module, data.TotalLength);
                }
                else
                {
                    swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
                    var swAssyPanelGlass = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Glass-1", Aggregator);
                    CeilingService.LightPanelSsGlass(swAssyPanelGlass, suffix, module, data.TotalLength, data.LongGlassNumber, data.ShortGlassNumber);
                }
            }
            else
            {
                swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
                swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
            }

        }
    }

    public void UcjSb535(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, UcjData data)
    {
        //计算过滤器数量
        var filterNumber = (int)((data.Length - data.FilterLeft - data.FilterRight) / 499d) - data.FilterBlindNumber;
        //左右两侧辅组参考面
        swModelTop.ChangeDim("Dis@BeamLeft", data.Length/2d);
        swModelTop.ChangeDim("Dis@BeamRight", data.Length/2d);

        //过滤器盲板
        FilterBlind(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, data.FilterLeft, "FNCE0107[BP-500]{500}-1", "LocalLPatternBlind", "Dis@DistanceBlind");

        //过滤器
        UcjFilter(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, filterNumber, data.FilterLeft, "UcjFcCombi-1", "LocalLPatternFc", "Dis@DistanceFc");

        //过滤器侧板
        KcjFilterSide(swModelTop, swAssyTop, suffix, module, data.FilterSide, data.FilterType, filterNumber, data.FilterLeft, data.FilterRight, "FNCE0136-1", "FNCE0109-1");
        //带把手过滤器侧板的磁铁支架
        var leftSide = data.FilterSide is FilterSide_e.两过滤器侧板 or FilterSide_e.左过滤器侧板;
        UcjFilterSideSensor(swAssyTop, suffix, data.CeilingLightType, leftSide, "FNCE0100-1", "FNCE0101-1");


        //SSP灯板支撑条
        SspSupport(swModelTop, swAssyTop, suffix, data.Length, data.DomeSsp, data.Gutter, data.GutterWidth, "FNCE0035-1", "Dis@DistanceDome", "FNCE0036-1", "Dis@DistanceFlat");

        //UV灯
        UvLightAsm(swAssyTop, suffix, data.UvLightType, "CeilingUvRackSpecial_4S-1", "CeilingUvRackSpecial_4L-1");

        //日本项目需要压缩零件(吊装垫片和脖颈)
        if (data.Japan)
        {
            swAssyTop.Suppress(suffix, "FNCE0070-1");
            swAssyTop.Suppress("LocalLPatternLifting");
            swAssyTop.Suppress(suffix, "ExhaustSpigot_Fs-1");
            //如果是左侧或者双侧时解压日本灯腔侧板
            if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
            {
                var swAssyPanelJapan = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Japan-1", Aggregator);
                CeilingService.LightPanelSsJapan(swAssyPanelJapan, suffix, module, data.TotalLength, data.LongGlassNumber, data.ShortGlassNumber, data.LeftLength, data.RightLength, data.MiddleLength);
            }
            else
            {
                swAssyTop.Suppress(suffix, "LightPanelSs_Japan-1");
            }
        }
        else
        {
            swAssyTop.UnSuppress(suffix, "FNCE0070-1", Aggregator);
            swAssyTop.UnSuppress("LocalLPatternLifting");

            //排风脖颈
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "ExhaustSpigot_Fs-1", Aggregator);
            //UCJ的排风滑门和轨道在UV灯支架中，因此不需要滑门和导轨,marvelRail取值true
            ExhaustService.ExhaustSpigotFs(swAssyLevel1, suffix, data.Length, data.MiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.Marvel, true, data.Ansul, ExhaustType_e.NA);
            swAssyTop.Suppress(suffix, "LightPanelSs_Japan-1");
        }

        //HCL
        if (data.CeilingLightType is CeilingLightType_e.HCL)
        {
            swAssyTop.Suppress(suffix, "NormalLight_KCJ_SB_535-1");
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(out var swModelLevel1, suffix, "HclLight_KCJ_SB_535-1", Aggregator);

            HclLightKcjSb535(swModelLevel1, swAssyLevel1, suffix, module, "UCJSB535", data.Length, data.Width, data.UvLightType, data.FilterBlindNumber+filterNumber, data.FilterLeft, data.LightCable, data.CeilingLightType, data.HclSide, data.HclLeft, data.HclRight, data.Marvel, data.ExhaustSpigotNumber, data.MiddleToRight, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotDis, data.Ansul, data.AnsulSide, data.AnsulDetector, data.Japan);
            swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
            swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
        }
        else
        {
            swAssyTop.Suppress(suffix, "HclLight_KCJ_SB_535-1");
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(out var swModelLevel1, suffix, "NormalLight_KCJ_SB_535-1", Aggregator);

            NormalLightKcjSb535(swAssyLevel1, suffix, module, "UCJSB535", data.Length, data.Width, data.UvLightType, data.FilterBlindNumber+filterNumber, data.FilterLeft, data.LightCable, data.CeilingLightType, data.Marvel, data.ExhaustSpigotNumber, data.MiddleToRight, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotDis, data.Ansul, data.AnsulSide, data.AnsulDetector, data.Japan);
            //如果是左侧或双侧时，解压灯腔侧板
            if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
            {
                if (data.CeilingLightType is CeilingLightType_e.筒灯)
                {
                    swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
                    var swAssyPanelLed = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Led-1", Aggregator);
                    CeilingService.LightPanelSsLed(swAssyPanelLed, suffix, module, data.TotalLength);
                }
                else
                {
                    swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
                    var swAssyPanelGlass = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Glass-1", Aggregator);
                    CeilingService.LightPanelSsGlass(swAssyPanelGlass, suffix, module, data.TotalLength, data.LongGlassNumber, data.ShortGlassNumber);
                }
            }
            else
            {
                swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
                swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
            }
        }

    }

    public void UcjSb385(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, UcjData data)
    {
        //计算过滤器数量
        var filterNumber = (int)((data.Length - data.FilterLeft - data.FilterRight) / 499d) - data.FilterBlindNumber;

        //重命名排风腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "UCJSB385", module, "FNCE0129-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCE0127(swCompLevel2, data.Length, data.UvLightType, data.Marvel, data.ExhaustSpigotNumber, data.MiddleToRight, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotDis, data.Ansul, data.AnsulSide, data.AnsulDetector, data.Japan);
        }

        //过滤器盲板
        FilterBlind(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, data.FilterLeft, "FNCE0107[BP-500]{500}-1", "LocalLPatternBlind", "Dis@DistanceBlind");

        //过滤器
        UcjFilter(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, filterNumber, data.FilterLeft, "UcjFcCombi-1", "LocalLPatternFc", "Dis@DistanceFc");

        //过滤器侧板
        KcjFilterSide(swModelTop, swAssyTop, suffix, module, data.FilterSide, data.FilterType, filterNumber, data.FilterLeft, data.FilterRight, "FNCE0136-1", "FNCE0109-1");
        //带把手过滤器侧板的磁铁支架
        var leftSide = data.FilterSide is FilterSide_e.两过滤器侧板 or FilterSide_e.左过滤器侧板;
        if (leftSide) swAssyTop.UnSuppress(suffix, "FNCE0100-1", Aggregator);
        else swAssyTop.Suppress(suffix, "FNCE0100-1");

        //SSP灯板支撑条
        SspSupport(swModelTop, swAssyTop, suffix, data.Length, data.DomeSsp, data.Gutter, data.GutterWidth, "FNCE0035-1", "Dis@DistanceDome", "FNCE0036-1", "Dis@DistanceFlat");

        //UV灯
        UvLightAsm(swAssyTop, suffix, data.UvLightType, "CeilingUvRackSpecial_4S-1", "CeilingUvRackSpecial_4L-1");

        //磁棒板
        FNCE0145(swAssyTop, suffix, "FNCE0151-1", data.Length, data.FilterLeft, data.FilterBlindNumber+filterNumber);


        //日本项目需要压缩零件(吊装垫片和脖颈)
        if (data.Japan)
        {
            swAssyTop.Suppress(suffix, "FNCE0070-1");
            swAssyTop.Suppress("LocalLPatternLifting");
            swAssyTop.Suppress(suffix, "ExhaustSpigot_Fs-1");
        }
        else
        {
            swAssyTop.UnSuppress(suffix, "FNCE0070-1", Aggregator);
            swAssyTop.UnSuppress("LocalLPatternLifting");

            //排风脖颈
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "ExhaustSpigot_Fs-1", Aggregator);
            //UCJ的排风滑门和轨道在UV灯支架中，因此不需要滑门和导轨,marvelRail取值true
            ExhaustService.ExhaustSpigotFs(swAssyLevel1, suffix, data.Length, data.MiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.Marvel, true, data.Ansul, ExhaustType_e.NA);
        }

    }

    //KCW
    public void KcwDb800(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, KcwData data)
    {
        //计算过滤器数量
        var filterNumber = (int)((data.Length - data.FilterLeft - data.FilterRight) / 499d) - data.FilterBlindNumber;
        //水洗挡板挂钩数量与间距
        var baffleHookingNumber = data.Length > 1400d ? 3 : 2;
        var baffleHookingDis = (data.Length - 300d) / (baffleHookingNumber - 1);
        //水洗挂管
        BaffleHookingTube(swModelTop, swAssyTop, suffix, module, data.Length-30d, baffleHookingNumber, baffleHookingDis);

        //防水挡板
        WaterproofPanel(swAssyTop, suffix, data.SidePanel, "FNCE0027[WPPSB160]-1");
        WaterproofPanel(swAssyTop, suffix, data.SidePanel, "FNCE0027[WPPSB160]-2");
        

        //公共零件
        //重命名排风腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "KCWDB800", module, "FNCE0158-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCE0158(swCompLevel2, data.SidePanel, data.Length, UvLightType_e.NA, data.Marvel, data.ExhaustSpigotNumber, data.MiddleToRight, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotDis, data.Ansul, data.AnsulSide, 0, 0, 0, data.Japan);
        }
        //灯腔
        FNCE0051(swAssyTop, suffix, "FNCE0051-1", data.Length, baffleHookingNumber, baffleHookingDis, data.CeilingLightType, data.Japan, data.FilterSide, filterNumber+data.FilterBlindNumber, data.FilterLeft, data.FilterRight);



        //其他配件
        FNCE0007(swAssyTop, suffix, "FNCE0007-1", data.Length, CeilingWaterInlet_e.NA);
        FNCE0007(swAssyTop, suffix, "FNCE0008-1", data.Length, CeilingWaterInlet_e.NA);


        FNCE0003(swAssyTop, suffix, "FNCE0003-1", data.Length, data.SidePanel);
        FNCE0003(swAssyTop, suffix, "FNCE0074-1", data.Length, data.SidePanel);


        //过滤器
        KcwFilter(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, filterNumber, data.FilterLeft, "2200600002-1", "LocalLPatternFc", "Dis@DistanceFc");
        KcwFilter(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, filterNumber, data.FilterLeft, "2200600002-7", "LocalLPatternFc", "Dis@DistanceFc");

        //过滤器侧板
        KcwFilterSide(swModelTop, swAssyTop, suffix, module, data.FilterSide, filterNumber, data.FilterLeft, data.FilterRight, "FNCE0058-1", "FNCE0059-1");
        KcwFilterSide(swModelTop, swAssyTop, suffix, module, data.FilterSide, filterNumber, data.FilterLeft, data.FilterRight, "FNCE0058-2", "FNCE0059-2");


        //SSP灯板支撑条
        SspSupport(swModelTop, swAssyTop, suffix, data.Length, data.DomeSsp, data.Gutter, data.GutterWidth, "FNCE0035-1", "Dis@DistanceDome1", "FNCE0036-1", "Dis@DistanceFlat1");
        SspSupport(swModelTop, swAssyTop, suffix, data.Length, data.DomeSsp, data.Gutter, data.GutterWidth, "FNCE0035-2", "Dis@DistanceDome2", "FNCE0036-2", "Dis@DistanceFlat2");

        //UV灯
        //UvLightAsm(swAssyTop, suffix, data.UvLightType, "CeilingUvRackSpecial_4S-1", "CeilingUvRackSpecial_4L-1");

        //侧板
        SidePanelKcwDb(swAssyTop, suffix, "SidePanel_KCW_DB_800-1", data.Length, data.SidePanel, data.DpSide);

        //水洗挡板
        if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
        {
            swAssyTop.UnSuppress(suffix, "Baffle_KCW_300-2", Aggregator);
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Baffle_KCW_300-1", Aggregator);
            BaffleKcw300(swAssyLevel1, suffix, module, data.TotalLength,data.BaffleLeft,data.BaffleRight,data.BaffleW,data.BaffleM,data.BaffleMNumber,UvLightType_e.NA);
        }
        else
        {
            swAssyTop.Suppress(suffix, "Baffle_KCW_300-1");
            swAssyTop.Suppress(suffix, "Baffle_KCW_300-2");
        }

        //日本项目需要压缩零件(吊装垫片和脖颈)
        if (data.Japan)
        {
            swAssyTop.Suppress(suffix, "FNCE0070-1");
            swAssyTop.Suppress("LocalLPatternLifting");
            swAssyTop.Suppress(suffix, "ExhaustSpigot_Fs-1");
            //日本项目FC油网打射钉
            swAssyTop.Suppress(suffix, "FNCE0009-1");
            swAssyTop.Suppress(suffix, "FNCE0009-2");

            //如果是左侧或者双侧时解压日本灯腔侧板
            if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
            {
                var swAssyPanelJapan = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Japan-1", Aggregator);
                CeilingService.LightPanelSsJapan(swAssyPanelJapan, suffix, module, data.TotalLength, data.LongGlassNumber, data.ShortGlassNumber, data.LeftLength, data.RightLength, data.MiddleLength);
            }
            else
            {
                swAssyTop.Suppress(suffix, "LightPanelSs_Japan-1");
            }
        }
        else
        {
            swAssyTop.UnSuppress(suffix, "FNCE0070-1", Aggregator);
            swAssyTop.UnSuppress("LocalLPatternLifting");
            swAssyTop.UnSuppress(suffix, "FNCE0009-2", Aggregator);
            swAssyTop.ChangePartLength(suffix, "FNCE0009-1", "Length@SketchBase", data.Length-5d, Aggregator);
            //排风脖颈
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "ExhaustSpigot_Fs-1", Aggregator);
            //UCJ的排风滑门和轨道在UV灯支架中，因此不需要滑门和导轨,marvelRail取值true
            ExhaustService.ExhaustSpigotFs(swAssyLevel1, suffix, data.Length, data.MiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.Marvel, data.Marvel, data.Ansul, ExhaustType_e.NA);

            swAssyTop.Suppress(suffix, "LightPanelSs_Japan-1");
        }

        //HCL
        if (data.CeilingLightType is CeilingLightType_e.HCL)
        {
            swAssyTop.Suppress(suffix, "NormalLight_KCW_DB_800-1");
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(out var swModelLevel1, suffix, "HclLight_KCW_DB_800-1", Aggregator);

            HclLightKcwDb800(swModelLevel1, swAssyLevel1, suffix, module, data.Length, data.HclSide, data.HclLeft, data.HclRight);

            swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
            swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");

        }
        else
        {
            swAssyTop.Suppress(suffix, "HclLight_KCW_DB_800-1");
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "NormalLight_KCW_DB_800-1", Aggregator);

            NormalLightKcwDb800(swAssyLevel1, suffix, data.Length);

            //如果是左侧或双侧时，解压灯腔侧板
            if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
            {
                if (data.CeilingLightType is CeilingLightType_e.筒灯)
                {
                    swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
                    var swAssyPanelLed = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Led-1", Aggregator);
                    CeilingService.LightPanelSsLed(swAssyPanelLed, suffix, module, data.TotalLength);
                }
                else
                {
                    swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
                    var swAssyPanelGlass = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Glass-1", Aggregator);
                    CeilingService.LightPanelSsGlass(swAssyPanelGlass, suffix, module, data.TotalLength, data.LongGlassNumber, data.ShortGlassNumber);
                }
            }
            else
            {
                swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
                swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
            }
        }
    }

    public void KcwSb535(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, KcwData data)
    {
        //计算过滤器数量
        var filterNumber = (int)((data.Length - data.FilterLeft - data.FilterRight) / 499d) - data.FilterBlindNumber;
        //左右两侧辅组参考面
        swModelTop.ChangeDim("Dis@BeamLeft", data.Length/2d);
        swModelTop.ChangeDim("Dis@BeamRight", data.Length/2d);

        //水洗挡板挂钩数量与间距
        var baffleHookingNumber = data.Length > 1400d ? 3 : 2;
        var baffleHookingDis = (data.Length - 300d) / (baffleHookingNumber - 1) ;
        //水洗挂管
        BaffleHookingTube(swModelTop, swAssyTop, suffix, module, data.Length-30d, baffleHookingNumber, baffleHookingDis);

        //防水挡板
        WaterproofPanel(swAssyTop, suffix, data.SidePanel, "FNCE0027[WPPSB160]-1");

        //公共零件
        //灯腔
        FNCE0031(swAssyTop, suffix, "FNCE0031-1", data.Length, baffleHookingNumber, baffleHookingDis, data.CeilingLightType, data.Japan, data.FilterSide, filterNumber+data.FilterBlindNumber, data.FilterLeft, data.FilterRight);

        //其他配件
        FNCE0007(swAssyTop, suffix, "FNCE0007-1", data.Length, CeilingWaterInlet_e.NA);
        FNCE0007(swAssyTop, suffix, "FNCE0008-1", data.Length, CeilingWaterInlet_e.NA);

        FNCE0003(swAssyTop, suffix, "FNCE0003-1", data.Length, data.SidePanel);

        //过滤器
        KcwFilter(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, filterNumber, data.FilterLeft, "2200600002-1", "LocalLPatternFc", "Dis@DistanceFc");

        KcwFilterSide(swModelTop, swAssyTop, suffix, module, data.FilterSide, filterNumber, data.FilterLeft, data.FilterRight, "FNCE0058-1", "FNCE0059-1");


        //SSP灯板支撑条
        SspSupport(swModelTop, swAssyTop, suffix, data.Length, data.DomeSsp, data.Gutter, data.GutterWidth, "FNCE0035-1", "Dis@DistanceDome", "FNCE0036-1", "Dis@DistanceFlat");

        //UV灯
        //UvLightAsm(swAssyTop, suffix, data.UvLightType, "CeilingUvRackSpecial_4S-1", "CeilingUvRackSpecial_4L-1");

        //侧板
        SidePanelKcwSb(swAssyTop, suffix, "SidePanel_KCW_SB_535-1", data.Length, data.SidePanel, data.DpSide,data.CeilingLightType, "FNCE0044-1", "FNCE0045-1", "FNCO0004[WPSSB535]-1", "FNCO0004[WPSSB535]-2");
        
        //水洗挡板
        if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
        {
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Baffle_KCW_300-1", Aggregator);
            BaffleKcw300(swAssyLevel1, suffix, module, data.TotalLength, data.BaffleLeft, data.BaffleRight, data.BaffleW, data.BaffleM, data.BaffleMNumber, UvLightType_e.NA);
        }
        else
        {
            swAssyTop.Suppress(suffix, "Baffle_KCW_300-1");
        }


        //日本项目需要压缩零件(吊装垫片和脖颈)
        if (data.Japan)
        {
            swAssyTop.Suppress(suffix, "FNCE0070-1");
            swAssyTop.Suppress("LocalLPatternLifting");
            swAssyTop.Suppress(suffix, "ExhaustSpigot_Fs-1");
            //日本项目FC油网打射钉
            swAssyTop.Suppress(suffix, "FNCE0009-1");

            //如果是左侧或者双侧时解压日本灯腔侧板
            if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
            {
                var swAssyPanelJapan = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Japan-1", Aggregator);
                CeilingService.LightPanelSsJapan(swAssyPanelJapan, suffix, module, data.TotalLength, data.LongGlassNumber, data.ShortGlassNumber, data.LeftLength, data.RightLength, data.MiddleLength);
            }
            else
            {
                swAssyTop.Suppress(suffix, "LightPanelSs_Japan-1");
            }
        }
        else
        {
            swAssyTop.UnSuppress(suffix, "FNCE0070-1", Aggregator);
            swAssyTop.UnSuppress("LocalLPatternLifting");
            swAssyTop.ChangePartLength(suffix, "FNCE0009-1", "Length@SketchBase", data.Length-5d, Aggregator);
            //排风脖颈
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "ExhaustSpigot_Fs-1", Aggregator);
            //UCJ的排风滑门和轨道在UV灯支架中，因此不需要滑门和导轨,marvelRail取值true
            ExhaustService.ExhaustSpigotFs(swAssyLevel1, suffix, data.Length, data.MiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.Marvel, data.Marvel, data.Ansul, ExhaustType_e.NA);

            swAssyTop.Suppress(suffix, "LightPanelSs_Japan-1");
        }

        //HCL
        if (data.CeilingLightType is CeilingLightType_e.HCL)
        {
            swAssyTop.Suppress(suffix, "NormalLight_KCW_SB_535-1");
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(out var swModelLevel1, suffix, "HclLight_KCW_SB_535-1", Aggregator);

            HclLightKcwSb535(swModelLevel1, swAssyLevel1, suffix, module, "KCWSB535", data.SidePanel, data.Length, data.Width, UvLightType_e.NA, data.CeilingLightType, data.HclSide, data.HclLeft, data.HclRight, data.Marvel, data.ExhaustSpigotNumber, data.MiddleToRight, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotDis, data.Ansul, data.AnsulSide, 0, 0, 0, data.Japan);
            swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
            swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
        }
        else
        {
            swAssyTop.Suppress(suffix, "HclLight_KCW_SB_535-1");
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "NormalLight_KCW_SB_535-1", Aggregator);

            NormalLightKcwSb535(swAssyLevel1, suffix, module, "KCWSB535", data.SidePanel, data.Length, data.Width, UvLightType_e.NA, data.CeilingLightType, data.Marvel, data.ExhaustSpigotNumber, data.MiddleToRight, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotDis, data.Ansul, data.AnsulSide, 0, 0, 0, data.Japan);

            //如果是左侧或双侧时，解压灯腔侧板
            if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
            {
                if (data.CeilingLightType is CeilingLightType_e.筒灯)
                {
                    swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
                    var swAssyPanelLed = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Led-1", Aggregator);
                    CeilingService.LightPanelSsLed(swAssyPanelLed, suffix, module, data.TotalLength);
                }
                else
                {
                    swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
                    var swAssyPanelGlass = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Glass-1", Aggregator);
                    CeilingService.LightPanelSsGlass(swAssyPanelGlass, suffix, module, data.TotalLength, data.LongGlassNumber, data.ShortGlassNumber);
                }
            }
            else
            {
                swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
                swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
            }
        }
    }

    public void KcwSb265(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, KcwData data)
    {
        //计算过滤器数量
        var filterNumber = (int)((data.Length - data.FilterLeft - data.FilterRight) / 499d) - data.FilterBlindNumber;

        //水洗挡板挂钩数量与间距
        var baffleHookingNumber = data.Length > 1400d ? 3 : 2;
        var baffleHookingDis = (data.Length - 300d) / (baffleHookingNumber - 1);
        //水洗挂管
        BaffleHookingTube(swModelTop, swAssyTop, suffix, module, data.Length-30d, baffleHookingNumber, baffleHookingDis);

        //防水挡板
        WaterproofPanel(swAssyTop, suffix, data.SidePanel, "FNCE0154[WPPSB130]-1");

        //公共零件
        //重命名排风腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "KCWSB265", module, "FNCE0012-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCE0012(swCompLevel2, data.SidePanel, data.Length, data.Marvel, data.ExhaustSpigotNumber, data.MiddleToRight, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotDis, data.Ansul, data.AnsulSide, data.CeilingWaterInlet, data.Japan);
        }

        //过滤器支架
        FNCE0021(swAssyTop, suffix, "FNCE0021-1", data.Length, baffleHookingNumber, baffleHookingDis, data.Japan, data.FilterSide, filterNumber+data.FilterBlindNumber, data.FilterLeft, data.FilterRight);

        //其他配件
        FNCE0007(swAssyTop, suffix, "FNCE0007-1", data.Length, data.CeilingWaterInlet);
        FNCE0007(swAssyTop, suffix, "FNCE0008-1", data.Length, data.CeilingWaterInlet);
        FNCE0003(swAssyTop, suffix, "FNCE0003-1", data.Length, data.SidePanel);

        //过滤器
        KcwFilter(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, filterNumber, data.FilterLeft, "2200600002-1", "LocalLPatternFc", "Dis@DistanceFc");

        KcwFilterSide(swModelTop, swAssyTop, suffix, module, data.FilterSide, filterNumber, data.FilterLeft, data.FilterRight, "FNCE0058-1", "FNCE0059-1");

        //SSP灯板支撑条
        SspSupport(swModelTop, swAssyTop, suffix, data.Length, data.DomeSsp, data.Gutter, data.GutterWidth, "FNCE0035-1", "Dis@DistanceDome", "FNCE0036-1", "Dis@DistanceFlat");

        //侧板
        SidePanelKcwSb(swAssyTop, suffix, "SidePanel_KCW_SB_265-1", data.Length, data.SidePanel, data.DpSide,CeilingLightType_e.NA, "FNCE0010-1", "FNCE0011-1", "FNCO0003[WPSSB265]-1", "FNCO0003[WPSSB265]-2");

        //水洗挡板
        if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
        {
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Baffle_KCW_300-1", Aggregator);
            BaffleKcw300(swAssyLevel1, suffix, module, data.TotalLength, data.BaffleLeft, data.BaffleRight, data.BaffleW, data.BaffleM, data.BaffleMNumber, UvLightType_e.NA);
        }
        else
        {
            swAssyTop.Suppress(suffix, "Baffle_KCW_300-1");
        }

        //日本项目需要压缩零件(吊装垫片和脖颈)
        if (data.Japan)
        {
            swAssyTop.Suppress(suffix, "FNCE0070-1");
            swAssyTop.Suppress("LocalLPatternLifting");
            swAssyTop.Suppress(suffix, "ExhaustSpigot_Fs-1");
            //日本项目FC油网打射钉
            swAssyTop.Suppress(suffix, "FNCE0009-1");
        }
        else
        {
            swAssyTop.UnSuppress(suffix, "FNCE0070-1", Aggregator);
            swAssyTop.UnSuppress("LocalLPatternLifting");
            swAssyTop.ChangePartLength(suffix, "FNCE0009-1", "Length@SketchBase", data.Length-5d, Aggregator);
            //排风脖颈
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "ExhaustSpigot_Fs-1", Aggregator);
            //UCJ的排风滑门和轨道在UV灯支架中，因此不需要滑门和导轨,marvelRail取值true
            ExhaustService.ExhaustSpigotFs(swAssyLevel1, suffix, data.Length, data.MiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.Marvel, data.Marvel, data.Ansul, ExhaustType_e.NA);
        }
    }

    //UCW
    public void UcwDb800(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, UcwData data)
    {
        //计算过滤器数量
        var filterNumber = (int)((data.Length - data.FilterLeft - data.FilterRight) / 499d) - data.FilterBlindNumber;
        //水洗挡板挂钩数量与间距
        var baffleHookingNumber = data.Length > 1400d ? 3 : 2;
        var baffleHookingDis = (data.Length - 300d) / (baffleHookingNumber - 1);
        //水洗挂管
        BaffleHookingTube(swModelTop, swAssyTop, suffix, module, data.Length-30d, baffleHookingNumber, baffleHookingDis);

        //防水挡板
        WaterproofPanel(swAssyTop, suffix, data.SidePanel, "FNCE0027[WPPSB160]-1");
        WaterproofPanel(swAssyTop, suffix, data.SidePanel, "FNCE0027[WPPSB160]-2");

        //公共零件
        //重命名排风腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "UCWDB800", module, "FNCE0158-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCE0158(swCompLevel2, data.SidePanel, data.Length, data.UvLightType, data.Marvel, data.ExhaustSpigotNumber, data.MiddleToRight, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotDis, data.Ansul, data.AnsulSide, data.BaffleSensorNumber, data.BaffleSensorDis1, data.BaffleSensorDis2, data.Japan);
        }
        //灯腔
        FNCE0051(swAssyTop, suffix, "FNCE0051-1", data.Length, baffleHookingNumber, baffleHookingDis, data.CeilingLightType, data.Japan, data.FilterSide, filterNumber+data.FilterBlindNumber, data.FilterLeft, data.FilterRight);



        //其他配件
        FNCE0007(swAssyTop, suffix, "FNCE0007-1", data.Length, CeilingWaterInlet_e.NA);
        FNCE0007(swAssyTop, suffix, "FNCE0008-1", data.Length, CeilingWaterInlet_e.NA);


        FNCE0003(swAssyTop, suffix, "FNCE0003-1", data.Length, data.SidePanel);
        FNCE0003(swAssyTop, suffix, "FNCE0074-1", data.Length, data.SidePanel);


        //过滤器
        KcwFilter(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, filterNumber, data.FilterLeft, "2200600002-1", "LocalLPatternFc", "Dis@DistanceFc");
        KcwFilter(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, filterNumber, data.FilterLeft, "2200600002-7", "LocalLPatternFc", "Dis@DistanceFc");

        //过滤器侧板
        KcwFilterSide(swModelTop, swAssyTop, suffix, module, data.FilterSide, filterNumber, data.FilterLeft, data.FilterRight, "FNCE0058-1", "FNCE0059-1");
        KcwFilterSide(swModelTop, swAssyTop, suffix, module, data.FilterSide, filterNumber, data.FilterLeft, data.FilterRight, "FNCE0058-2", "FNCE0059-2");


        //SSP灯板支撑条
        SspSupport(swModelTop, swAssyTop, suffix, data.Length, data.DomeSsp, data.Gutter, data.GutterWidth, "FNCE0035-1", "Dis@DistanceDome1", "FNCE0036-1", "Dis@DistanceFlat1");
        SspSupport(swModelTop, swAssyTop, suffix, data.Length, data.DomeSsp, data.Gutter, data.GutterWidth, "FNCE0035-2", "Dis@DistanceDome2", "FNCE0036-2", "Dis@DistanceFlat2");

        //UV灯
        UvLightAsm(swAssyTop, suffix, data.UvLightType, "CeilingUvRackSpecial_4S-1", "CeilingUvRackSpecial_4L-1");

        //侧板
        SidePanelKcwDb(swAssyTop, suffix, "SidePanel_KCW_DB_800-1", data.Length, data.SidePanel, data.DpSide);

        //水洗挡板
        if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
        {
            swAssyTop.UnSuppress(suffix, "Baffle_KCW_300-2", Aggregator);
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Baffle_KCW_300-1", Aggregator);
            BaffleKcw300(swAssyLevel1, suffix, module, data.TotalLength, data.BaffleLeft, data.BaffleRight, data.BaffleW, data.BaffleM, data.BaffleMNumber, data.UvLightType);
        }
        else
        {
            swAssyTop.Suppress(suffix, "Baffle_KCW_300-1");
            swAssyTop.Suppress(suffix, "Baffle_KCW_300-2");
        }

        //日本项目需要压缩零件(吊装垫片和脖颈)
        if (data.Japan)
        {
            swAssyTop.Suppress(suffix, "FNCE0070-1");
            swAssyTop.Suppress("LocalLPatternLifting");
            swAssyTop.Suppress(suffix, "ExhaustSpigot_Fs-1");
            //日本项目FC油网打射钉
            swAssyTop.Suppress(suffix, "FNCE0009-1");
            swAssyTop.Suppress(suffix, "FNCE0009-2");

            //如果是左侧或者双侧时解压日本灯腔侧板
            if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
            {
                var swAssyPanelJapan = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Japan-1", Aggregator);
                CeilingService.LightPanelSsJapan(swAssyPanelJapan, suffix, module, data.TotalLength, data.LongGlassNumber, data.ShortGlassNumber, data.LeftLength, data.RightLength, data.MiddleLength);
            }
            else
            {
                swAssyTop.Suppress(suffix, "LightPanelSs_Japan-1");
            }
        }
        else
        {
            swAssyTop.UnSuppress(suffix, "FNCE0070-1", Aggregator);
            swAssyTop.UnSuppress("LocalLPatternLifting");
            swAssyTop.UnSuppress(suffix, "FNCE0009-2", Aggregator);
            swAssyTop.ChangePartLength(suffix, "FNCE0009-1", "Length@SketchBase", data.Length-5d, Aggregator);
            //排风脖颈
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "ExhaustSpigot_Fs-1", Aggregator);
            //UCJ的排风滑门和轨道在UV灯支架中，因此不需要滑门和导轨,marvelRail取值true
            ExhaustService.ExhaustSpigotFs(swAssyLevel1, suffix, data.Length, data.MiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.Marvel, true, data.Ansul, ExhaustType_e.NA);
            swAssyTop.Suppress(suffix, "LightPanelSs_Japan-1");
        }

        //HCL
        if (data.CeilingLightType is CeilingLightType_e.HCL)
        {
            swAssyTop.Suppress(suffix, "NormalLight_KCW_DB_800-1");
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(out var swModelLevel1, suffix, "HclLight_KCW_DB_800-1", Aggregator);

            HclLightKcwDb800(swModelLevel1, swAssyLevel1, suffix, module, data.Length, data.HclSide, data.HclLeft, data.HclRight);
            swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
            swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
        }
        else
        {
            swAssyTop.Suppress(suffix, "HclLight_KCW_DB_800-1");
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "NormalLight_KCW_DB_800-1", Aggregator);

            NormalLightKcwDb800(swAssyLevel1, suffix, data.Length);
            //如果是左侧或双侧时，解压灯腔侧板
            if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
            {
                if (data.CeilingLightType is CeilingLightType_e.筒灯)
                {
                    swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
                    var swAssyPanelLed = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Led-1", Aggregator);
                    CeilingService.LightPanelSsLed(swAssyPanelLed, suffix, module, data.TotalLength);
                }
                else
                {
                    swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
                    var swAssyPanelGlass = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Glass-1", Aggregator);
                    CeilingService.LightPanelSsGlass(swAssyPanelGlass, suffix, module, data.TotalLength, data.LongGlassNumber, data.ShortGlassNumber);
                }
            }
            else
            {
                swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
                swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
            }
        }
    }

    public void UcwSb535(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, UcwData data)
    {
        //计算过滤器数量
        var filterNumber = (int)((data.Length - data.FilterLeft - data.FilterRight) / 499d) - data.FilterBlindNumber;
        //左右两侧辅组参考面
        swModelTop.ChangeDim("Dis@BeamLeft", data.Length/2d);
        swModelTop.ChangeDim("Dis@BeamRight", data.Length/2d);

        //水洗挡板挂钩数量与间距
        var baffleHookingNumber = data.Length > 1400d ? 3 : 2;
        var baffleHookingDis = (data.Length - 300d) / (baffleHookingNumber - 1);
        //水洗挂管
        BaffleHookingTube(swModelTop, swAssyTop, suffix, module, data.Length-30d, baffleHookingNumber, baffleHookingDis);

        //防水挡板
        WaterproofPanel(swAssyTop, suffix, data.SidePanel, "FNCE0027[WPPSB160]-1");

        //公共零件
        //灯腔
        FNCE0031(swAssyTop, suffix, "FNCE0031-1", data.Length, baffleHookingNumber, baffleHookingDis, data.CeilingLightType, data.Japan, data.FilterSide, filterNumber+data.FilterBlindNumber, data.FilterLeft, data.FilterRight);

        //其他配件
        FNCE0007(swAssyTop, suffix, "FNCE0007-1", data.Length, CeilingWaterInlet_e.NA);
        FNCE0007(swAssyTop, suffix, "FNCE0008-1", data.Length, CeilingWaterInlet_e.NA);

        FNCE0003(swAssyTop, suffix, "FNCE0003-1", data.Length, data.SidePanel);

        //过滤器
        KcwFilter(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, filterNumber, data.FilterLeft, "2200600002-1", "LocalLPatternFc", "Dis@DistanceFc");

        KcwFilterSide(swModelTop, swAssyTop, suffix, module, data.FilterSide, filterNumber, data.FilterLeft, data.FilterRight, "FNCE0058-1", "FNCE0059-2");


        //SSP灯板支撑条
        SspSupport(swModelTop, swAssyTop, suffix, data.Length, data.DomeSsp, data.Gutter, data.GutterWidth, "FNCE0035-1", "Dis@DistanceDome", "FNCE0036-1", "Dis@DistanceFlat");

        //UV灯
        UvLightAsm(swAssyTop, suffix, data.UvLightType, "CeilingUvRackSpecial_4S-1", "CeilingUvRackSpecial_4L-1");

        //侧板
        SidePanelKcwSb(swAssyTop, suffix, "SidePanel_KCW_SB_535-1", data.Length, data.SidePanel, data.DpSide,data.CeilingLightType, "FNCE0044-1", "FNCE0045-1", "FNCO0004[WPSSB535]-1", "FNCO0004[WPSSB535]-2");

        //水洗挡板
        if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
        {
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Baffle_KCW_300-1", Aggregator);
            BaffleKcw300(swAssyLevel1, suffix, module, data.TotalLength, data.BaffleLeft, data.BaffleRight, data.BaffleW, data.BaffleM, data.BaffleMNumber, data.UvLightType);
        }
        else
        {
            swAssyTop.Suppress(suffix, "Baffle_KCW_300-1");
        }

        //日本项目需要压缩零件(吊装垫片和脖颈)
        if (data.Japan)
        {
            swAssyTop.Suppress(suffix, "FNCE0070-1");
            swAssyTop.Suppress("LocalLPatternLifting");
            swAssyTop.Suppress(suffix, "ExhaustSpigot_Fs-1");
            //日本项目FC油网打射钉
            swAssyTop.Suppress(suffix, "FNCE0009-1");

            //如果是左侧或者双侧时解压日本灯腔侧板
            if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
            {
                var swAssyPanelJapan = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Japan-1", Aggregator);
                CeilingService.LightPanelSsJapan(swAssyPanelJapan, suffix, module, data.TotalLength, data.LongGlassNumber, data.ShortGlassNumber, data.LeftLength, data.RightLength, data.MiddleLength);
            }
            else
            {
                swAssyTop.Suppress(suffix, "LightPanelSs_Japan-1");
            }
        }
        else
        {
            swAssyTop.UnSuppress(suffix, "FNCE0070-1", Aggregator);
            swAssyTop.UnSuppress("LocalLPatternLifting");
            swAssyTop.ChangePartLength(suffix, "FNCE0009-1", "Length@SketchBase", data.Length-5d, Aggregator);
            //排风脖颈
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "ExhaustSpigot_Fs-1", Aggregator);
            //UCJ的排风滑门和轨道在UV灯支架中，因此不需要滑门和导轨,marvelRail取值true
            ExhaustService.ExhaustSpigotFs(swAssyLevel1, suffix, data.Length, data.MiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.Marvel, true, data.Ansul, ExhaustType_e.NA);

            swAssyTop.Suppress(suffix, "LightPanelSs_Japan-1");
        }

        //HCL
        if (data.CeilingLightType is CeilingLightType_e.HCL)
        {
            swAssyTop.Suppress(suffix, "NormalLight_KCW_SB_535-1");
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(out var swModelLevel1, suffix, "HclLight_KCW_SB_535-1", Aggregator);

            HclLightKcwSb535(swModelLevel1, swAssyLevel1, suffix, module, "UCWSB535", data.SidePanel, data.Length, data.Width, data.UvLightType, data.CeilingLightType, data.HclSide, data.HclLeft, data.HclRight, data.Marvel, data.ExhaustSpigotNumber, data.MiddleToRight, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotDis, data.Ansul, data.AnsulSide, data.BaffleSensorNumber, data.BaffleSensorDis1, data.BaffleSensorDis2, data.Japan);
            swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
            swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
        }
        else
        {
            swAssyTop.Suppress(suffix, "HclLight_KCW_SB_535-1");
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "NormalLight_KCW_SB_535-1", Aggregator);

            NormalLightKcwSb535(swAssyLevel1, suffix, module, "UCWSB535", data.SidePanel, data.Length, data.Width, data.UvLightType, data.CeilingLightType, data.Marvel, data.ExhaustSpigotNumber, data.MiddleToRight, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotDis, data.Ansul, data.AnsulSide, data.BaffleSensorNumber, data.BaffleSensorDis1, data.BaffleSensorDis2, data.Japan);

            //如果是左侧或双侧时，解压灯腔侧板
            if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
            {
                if (data.CeilingLightType is CeilingLightType_e.筒灯)
                {
                    swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
                    var swAssyPanelLed = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Led-1", Aggregator);
                    CeilingService.LightPanelSsLed(swAssyPanelLed, suffix, module, data.TotalLength);
                }
                else
                {
                    swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
                    var swAssyPanelGlass = swAssyTop.GetSubAssemblyDoc(suffix, "LightPanelSs_Glass-1", Aggregator);
                    CeilingService.LightPanelSsGlass(swAssyPanelGlass, suffix, module, data.TotalLength, data.LongGlassNumber, data.ShortGlassNumber);
                }
            }
            else
            {
                swAssyTop.Suppress(suffix, "LightPanelSs_Glass-1");
                swAssyTop.Suppress(suffix, "LightPanelSs_Led-1");
            }
        }
    }
    #endregion

    #region 排风腔通用方法
    /// <summary>
    /// 过滤器盲板
    /// </summary>
    private void FilterBlind(ModelDoc2 swModelLevel1, AssemblyDoc swAssyLevel1, string suffix, int filterBlindNumber, double filterLeft, string filterBlindPart, string filterBlindPattern, string filterBlindDis)
    {
        swAssyLevel1.Suppress(filterBlindPattern);
        swAssyLevel1.Suppress(suffix, filterBlindPart);
        if (filterBlindNumber > 0)
        {
            swAssyLevel1.UnSuppress(suffix, filterBlindPart, Aggregator);
            swModelLevel1.ChangeDim(filterBlindDis, filterLeft);
        }
        if (filterBlindNumber > 1)
        {
            swAssyLevel1.UnSuppress(filterBlindPattern);
            swModelLevel1.ChangeDim($"Number@{filterBlindPattern}", filterBlindNumber);
        }
    }

    /// <summary>
    /// 过滤器
    /// </summary>
    private void KcjFilter(ModelDoc2 swModelLevel1, AssemblyDoc swAssyLevel1, string suffix, int filterBlindNumber, int filterNumber, double filterLeft, FilterType_e filterType, string fcPart, string fcPattern, string fcDis, string ksaPart, string ksaPattern, string ksaDis)
    {
        if (filterType is FilterType_e.KSA)
        {
            swAssyLevel1.Suppress(suffix, fcPart);
            swAssyLevel1.Suppress(fcPattern);
            swAssyLevel1.UnSuppress(suffix, ksaPart, Aggregator);
            swAssyLevel1.UnSuppress(ksaPattern);
            swModelLevel1.ChangeDim(ksaDis, filterLeft+filterBlindNumber*500d);
            swModelLevel1.ChangeDim($"Number@{ksaPattern}", filterNumber);
        }
        else
        {
            swAssyLevel1.Suppress(suffix, ksaPart);
            swAssyLevel1.Suppress(ksaPattern);
            swAssyLevel1.UnSuppress(suffix, fcPart, Aggregator);
            swAssyLevel1.UnSuppress(fcPattern);
            swModelLevel1.ChangeDim(fcDis, filterLeft+filterBlindNumber*500d);
            swModelLevel1.ChangeDim($"Number@{fcPattern}", filterNumber);
        }
    }
    private void UcjFilter(ModelDoc2 swModelLevel1, AssemblyDoc swAssyLevel1, string suffix, int filterBlindNumber, int filterNumber, double filterLeft, string fcPart, string fcPattern, string fcDis)
    {
        swAssyLevel1.UnSuppress(suffix, fcPart, Aggregator);
        swAssyLevel1.UnSuppress(fcPattern);
        swModelLevel1.ChangeDim(fcDis, filterLeft+filterBlindNumber*500d);
        swModelLevel1.ChangeDim($"Number@{fcPattern}", filterNumber);
    }
    private void KcwFilter(ModelDoc2 swModelLevel1, AssemblyDoc swAssyLevel1, string suffix, int filterBlindNumber, int filterNumber, double filterLeft, string fcPart, string fcPattern, string fcDis)
    {
        var disLeft = (filterLeft.Equals(0) ? 1.5d : filterLeft) + filterBlindNumber * 500d;
        swAssyLevel1.UnSuppress(suffix, fcPart, Aggregator);
        swAssyLevel1.UnSuppress(fcPattern);
        swModelLevel1.ChangeDim(fcDis, disLeft);
        swModelLevel1.ChangeDim($"Number@{fcPattern}", filterNumber);
    }
    /// <summary>
    /// 过滤器侧板
    /// </summary>
    private void KcjFilterSide(ModelDoc2 swModelLevel1, AssemblyDoc swAssyLevel1, string suffix, string module, FilterSide_e filterSide, FilterType_e filterType, int filterNumber, double filterLeft, double filterRight, string leftPart, string rightPart)
    {
        switch (filterSide)
        {
            case FilterSide_e.左过滤器侧板:
                {
                    var leftLength = (int)(filterLeft - filterNumber);
                    if (filterType is FilterType_e.KSA)
                        leftLength = (int)(filterLeft+filterNumber*2d);

                    var swCompLeft = swAssyLevel1.RenameComp(suffix, "BP", module, leftPart, leftLength, 250, Aggregator);
                    if (swCompLeft != null) ChangeSideLength(swCompLeft, leftLength);
                    swAssyLevel1.ForceSuppress(suffix, rightPart);
                    break;
                }
            case FilterSide_e.右过滤器侧板:
                {
                    var rightLength = (int)(filterRight - filterNumber);
                    if (filterType is FilterType_e.KSA)
                        rightLength = (int)(filterRight+filterNumber*2d);

                    var swCompRight = swAssyLevel1.RenameComp(suffix, "BP", module, rightPart, rightLength, 250, Aggregator);
                    if (swCompRight != null) ChangeSideLength(swCompRight, rightLength);
                    swAssyLevel1.ForceSuppress(suffix, leftPart);
                    break;
                }
            case FilterSide_e.两过滤器侧板:
                {
                    var leftLength = (int)(filterLeft - filterNumber/2d);
                    if (filterType is FilterType_e.KSA)
                        leftLength = (int)(filterLeft+filterNumber*1.5d);

                    var swCompLeft = swAssyLevel1.RenameComp(suffix, "BP", $"{module}.1", leftPart, leftLength, 250, Aggregator);
                    if (swCompLeft != null) ChangeSideLength(swCompLeft, leftLength);

                    var rightLength = (int)(filterRight - filterNumber/2d);
                    if (filterType is FilterType_e.KSA)
                        rightLength = (int)(filterRight+filterNumber*1.5d);

                    var swCompRight = swAssyLevel1.RenameComp(suffix, "BP", $"{module}.2", rightPart, rightLength, 250, Aggregator);
                    if (swCompRight != null) ChangeSideLength(swCompRight, rightLength);
                    break;
                }
            case FilterSide_e.NA:
            case FilterSide_e.无过滤器侧板:
            default:
                swAssyLevel1.ForceSuppress(suffix, leftPart);
                swAssyLevel1.ForceSuppress(suffix, rightPart);
                break;
        }
        void ChangeSideLength(Component2 swComp, double sideLength)
        {
            var swModel = (ModelDoc2)swComp.GetModelDoc2();
            swModel.ChangeDim("Length@SketchBase", sideLength);
        }
    }

    /// <summary>
    /// 水洗过滤器侧板
    /// </summary>
    private void KcwFilterSide(ModelDoc2 swModelLevel1, AssemblyDoc swAssyLevel1, string suffix, string module, FilterSide_e filterSide, int filterNumber, double filterLeft, double filterRight, string leftPart,string rightPart)
    {
        switch (filterSide)
        {
            case FilterSide_e.左过滤器侧板:
                {
                    var leftLength = (int)(filterLeft - filterNumber-3d);//水洗需要减去三角板
                    var swCompLeft = swAssyLevel1.RenameComp(suffix, "BP", module, leftPart,leftLength, 250, Aggregator);
                    if (swCompLeft != null) ChangeSideLength(swCompLeft, leftLength);
                    swAssyLevel1.ForceSuppress(suffix, rightPart);
                    break;
                }
            case FilterSide_e.右过滤器侧板:
                {
                    var rightLength = (int)(filterRight - filterNumber-3d);
                    var swCompRight = swAssyLevel1.RenameComp(suffix, "BP", module, rightPart, rightLength, 250, Aggregator);
                    //因为没有选中
                    if (swCompRight != null) ChangeSideLength(swCompRight, rightLength);
                    swAssyLevel1.ForceSuppress(suffix, leftPart);
                    break;
                }
            case FilterSide_e.两过滤器侧板:
                {
                    var leftLength = (int)(filterLeft - filterNumber/2d-1.5d);
                    var swCompLeft = swAssyLevel1.RenameComp(suffix, "BP", $"{module}.1", leftPart,leftLength, 250, Aggregator);
                    if (swCompLeft != null) ChangeSideLength(swCompLeft, leftLength);

                    var rightLength = (int)(filterRight - filterNumber/2d-1.5d);
                    var swCompRight = swAssyLevel1.RenameComp(suffix, "BP", $"{module}.2", rightPart,rightLength, 250, Aggregator);
                    if (swCompRight != null) ChangeSideLength(swCompRight, rightLength);
                    break;
                }
            case FilterSide_e.NA:
            case FilterSide_e.无过滤器侧板:
            default:
                swAssyLevel1.ForceSuppress(suffix, leftPart);
                swAssyLevel1.ForceSuppress(suffix, rightPart);
                break;
        }
        void ChangeSideLength(Component2 swComp, double sideLength)
        {
            var swModel = (ModelDoc2)swComp.GetModelDoc2();
            swModel.ChangeDim("Length@SketchBase", sideLength);
            if (sideLength < 100d)
            {
                swComp.Suppress("Edge-FlangeLong");
                swComp.UnSuppress("Edge-FlangeShort");
            }
            else
            {
                swComp.Suppress("Edge-FlangeShort");
                swComp.UnSuppress("Edge-FlangeLong");
            }
        }
    }

    /// <summary>
    /// UCJ带把手过滤器侧板磁感应支架
    /// </summary>
    private void UcjFilterSideSensor(AssemblyDoc swAssyLevel1, string suffix, CeilingLightType_e ceilingLightType, bool sideCheck, string normalPart, string hclPart)
    {
        if (sideCheck)
        {
            if (ceilingLightType is CeilingLightType_e.HCL)
            {
                swAssyLevel1.UnSuppress(suffix, hclPart, Aggregator);
                swAssyLevel1.Suppress(suffix, normalPart);
            }
            else
            {
                swAssyLevel1.UnSuppress(suffix, normalPart, Aggregator);
                swAssyLevel1.Suppress(suffix, hclPart);
            }
        }
        else
        {
            swAssyLevel1.Suppress(suffix, normalPart);
            swAssyLevel1.Suppress(suffix, hclPart);
        }
    }

    /// <summary>
    /// 灯板支架78度，90度
    /// </summary>
    private void SspSupport(ModelDoc2 swModelLevel1, AssemblyDoc swAssyLevel1, string suffix, double length, bool domeSsp, bool gutter, double gutterWidth, string domePart, string domeDis, string flatPart, string flatDis)
    {
        if (!gutter) gutterWidth = 0.5d;
        if (domeSsp)
        {
            swAssyLevel1.Suppress(suffix, flatPart);
            var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, domePart, Aggregator);
            swModelLevel2.ChangeDim("Length@SketchBase", length);
            swModelLevel1.ChangeDim(domeDis, gutterWidth);
        }
        else
        {
            swAssyLevel1.Suppress(suffix, domePart);
            var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, flatPart, Aggregator);
            swModelLevel2.ChangeDim("Length@SketchBase", length);
            swModelLevel1.ChangeDim(flatDis, gutterWidth);
        }
    }

    /// <summary>
    /// UV灯装配体
    /// </summary>
    private void UvLightAsm(AssemblyDoc swAssyLevel1, string suffix, UvLightType_e uvLightType, string shortAsm, string longAsm)
    {
        switch (uvLightType)
        {

            case UvLightType_e.UVR4S:
            case UvLightType_e.UVR6S:
            case UvLightType_e.UVR8S:
                swAssyLevel1.UnSuppress(suffix, shortAsm, Aggregator);
                swAssyLevel1.Suppress(suffix, longAsm);
                break;
            case UvLightType_e.UVR4L:
            case UvLightType_e.UVR6L:
            case UvLightType_e.UVR8L:
                swAssyLevel1.UnSuppress(suffix, longAsm, Aggregator);
                swAssyLevel1.Suppress(suffix, shortAsm);
                break;
            case UvLightType_e.NA:
            case UvLightType_e.Double:
            default:
                swAssyLevel1.Suppress(suffix, shortAsm);
                swAssyLevel1.Suppress(suffix, longAsm);
                break;
        }
    }

    /// <summary>
    /// 水洗挂管
    /// </summary>
    private void BaffleHookingTube(ModelDoc2 swModelLevel1, AssemblyDoc swAssyLevel1, string suffix, string module, double tubeLength,
        int baffleHookingNumber, double baffleHookingDis)
    {
        swModelLevel1.ChangeDim("Number@LocalLPatternBaffleHooking", baffleHookingNumber);
        swModelLevel1.ChangeDim("Dis@LocalLPatternBaffleHooking", baffleHookingDis);
        var swCompLevel2 = swAssyLevel1.RenameComp(suffix, "BFHT", module, "2200600027-1", tubeLength, 14d, Aggregator);
        if (swCompLevel2 != null)
        {
            //Length@Boss-Extrude
            var swModelLevel2 = (ModelDoc2)swCompLevel2.GetModelDoc2();
            swModelLevel2.ChangeDim("Length@Boss-Extrude", tubeLength);
        }
    }

    /// <summary>
    /// 水洗挡板
    /// </summary>
    private void WaterproofPanel(AssemblyDoc swAssyTop, string suffix, SidePanel_e sidePanel, string part)
    {
        switch (sidePanel)
        {
            case SidePanel_e.左:
            case SidePanel_e.双:
                //不需要防水挡板
                swAssyTop.Suppress(suffix, part);
                break;
            case SidePanel_e.中:
            case SidePanel_e.右:
            case SidePanel_e.NA:
            default:
                //需要防水挡板;
                swAssyTop.UnSuppress(suffix, part, Aggregator);
                break;
        }
    }

    /// <summary>
    /// 水洗双排风三角板
    /// </summary>
    private void SidePanelKcwDb(AssemblyDoc swAssyTop, string suffix, string asmName, double length,
        SidePanel_e sidePanel, DpSide_e dpSide)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, asmName, Aggregator);
        swAssyLevel1.ChangeDim("Dis@DistanceLeft", length / 2d);
        swAssyLevel1.ChangeDim("Dis@DistanceRight", length / 2d);
        //防水棉WaterproofSealing
        WaterproofSealing(swAssyLevel1, suffix, sidePanel, "FNCO0005[WPSDB800]-1", "FNCO0005[WPSDB800]-2");

        //连接DP排水腔，焊接3m法兰
        ConnDpFlangle(swAssyLevel1, suffix, sidePanel, dpSide, "FNCE0017-1", "FNCE0017-3");
        ConnDpFlangle(swAssyLevel1, suffix, sidePanel, dpSide, "FNCE0017-2", "FNCE0017-4");

        //水洗挡板侧板
        BaffleSidePart(swAssyLevel1, suffix, sidePanel, "FNCE0016-1", "FNCE0016-3");
        BaffleSidePart(swAssyLevel1, suffix, sidePanel, "FNCE0016-2", "FNCE0016-4");

        //三角板
        FNCE0062(swAssyLevel1, suffix, "FNCE0062-1", sidePanel, dpSide);
        FNCE0063(swAssyLevel1, suffix, "FNCE0063-1", sidePanel, dpSide);
    }

    /// <summary>
    /// 水洗单排风三角板
    /// </summary>
    private void SidePanelKcwSb(AssemblyDoc swAssyTop, string suffix, string asmName, double length, SidePanel_e sidePanel, DpSide_e dpSide,CeilingLightType_e ceilingLightType,string leftPart,string rightPart,string leftWpsPart, string rightWpsPart)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, asmName, Aggregator);
        swAssyLevel1.ChangeDim("Dis@DistanceLeft", length / 2d);
        swAssyLevel1.ChangeDim("Dis@DistanceRight", length / 2d);
        //防水棉WaterproofSealing
        WaterproofSealing(swAssyLevel1, suffix, sidePanel, leftWpsPart, rightWpsPart);

        //连接DP排水腔，焊接3m固定片
        ConnDpFlangle(swAssyLevel1, suffix, sidePanel, dpSide, "FNCE0017-1", "FNCE0017-2");

        //水洗挡板侧板
        BaffleSidePart(swAssyLevel1, suffix, sidePanel, "FNCE0016-1", "FNCE0016-2");

        //三角板
        FNCE0044(swAssyLevel1, suffix, leftPart, sidePanel, dpSide, ceilingLightType);
        FNCE0045(swAssyLevel1, suffix, rightPart, sidePanel, dpSide, ceilingLightType);
    }

    /// <summary>
    /// 水洗挡板
    /// </summary>
    private void BaffleKcw300(AssemblyDoc swAssyLevel1, string suffix, string module, double totalLength,double baffleLeft,double baffleRight, double baffleW, double baffleM, int baffleMNumber,UvLightType_e uvLightType)
    {
        var swModelLevel1 = (ModelDoc2)swAssyLevel1;
        swModelLevel1.ChangeDim("Length@DistanceLength",totalLength-3d);//减去三角板厚度

        #region UL,Baffle_UL_300
        var swAssyBaffleUl = swAssyLevel1.GetSubAssemblyDoc(suffix, "Baffle_UL_300-1", Aggregator);
        BaffleUl300(swAssyBaffleUl, suffix, module, baffleLeft);
        #endregion

        #region UR,Baffle_UR_300
        var swAssyBaffleUr = swAssyLevel1.GetSubAssemblyDoc(suffix, "Baffle_UR_300-1", Aggregator);
        BaffleUr300(swAssyBaffleUr, suffix, module, baffleRight);
        #endregion

        #region W,Baffle_W_300
        var swAssyBaffleW = swAssyLevel1.GetSubAssemblyDoc(suffix, "Baffle_W_300-3", Aggregator);
        BaffleW300(swAssyBaffleW, suffix, module, baffleW,uvLightType);
        #endregion
        
        
        if (baffleMNumber > 0)
        {
            swAssyLevel1.ForceUnSuppress(suffix, "Baffle_W_300-1",Aggregator);
            var swAssyBaffleM = swAssyLevel1.GetSubAssemblyDoc(suffix, "Baffle_M_300-1", Aggregator);
            BaffleM300(swAssyBaffleM, suffix, module, baffleM);
        }
        else
        {
            swAssyLevel1.ForceSuppress(suffix, "Baffle_M_300-1");
            swAssyLevel1.ForceSuppress(suffix, "Baffle_W_300-1");
        }

        if (baffleMNumber > 1)
        {
            swAssyLevel1.UnSuppress("LocalLPatternBaffle");
            swModelLevel1.ChangeDim("Number@LocalLPatternBaffle", baffleMNumber);
            swModelLevel1.ChangeDim("Dis@LocalLPatternBaffle", baffleM+baffleW);
        }
        else
        {
            swAssyLevel1.Suppress("LocalLPatternBaffle");
        }

    }

    private void BaffleUl300(AssemblyDoc swAssyLevel1, string suffix, string module, 
        double baffleLeft)
    {
        var swCompLevel2 = swAssyLevel1.RenameComp(suffix, "BFUL", module, "FNCE0119-1", baffleLeft-2d, 200d, Aggregator);
        if (swCompLevel2 != null)
        {
            var swModelStdPanel = (ModelDoc2)swCompLevel2.GetModelDoc2();
            swModelStdPanel.ChangeDim("Length@SketchBase", baffleLeft-2d);
        }
    }
    private void BaffleUr300(AssemblyDoc swAssyLevel1, string suffix, string module,
        double baffleRight)
    {
        var swCompLevel2 = swAssyLevel1.RenameComp(suffix, "BFUR", module, "FNCE0118-1", baffleRight-2d, 200d, Aggregator);
        if (swCompLevel2 != null)
        {
            var swModelStdPanel = (ModelDoc2)swCompLevel2.GetModelDoc2();
            swModelStdPanel.ChangeDim("Length@SketchBase", baffleRight-2d);
        }
    }
    private void BaffleW300(AssemblyDoc swAssyLevel1, string suffix, string module,
        double baffleW,UvLightType_e uvLightType)
    {
        #region 磁铁
        if (uvLightType is UvLightType_e.NA)
        {
            swAssyLevel1.Suppress(suffix, "FNCE0124-1");
            swAssyLevel1.Suppress(suffix, "2900100016-1");
        }
        else
        {
            swAssyLevel1.UnSuppress(suffix, "FNCE0124-1", Aggregator);
            swAssyLevel1.UnSuppress(suffix, "2900100016-1", Aggregator);
        } 
        #endregion

        var swCompLevel2 = swAssyLevel1.RenameComp(suffix, "BFW", module, "FNCE0121-1", baffleW, 200d, Aggregator);
        if (swCompLevel2 != null)
        {
            var swModelStdPanel = (ModelDoc2)swCompLevel2.GetModelDoc2();
            swModelStdPanel.ChangeDim("Length@SketchBase", baffleW-1d);
            if (uvLightType is UvLightType_e.NA)
            {
                swCompLevel2.Suppress("CutForUv");
            }
            else
            {
                swCompLevel2.UnSuppress("CutForUv");
            }
        }
    }

    private void BaffleM300(AssemblyDoc swAssyLevel1, string suffix, string module,
        double baffleM)
    {
        var swCompLevel2 = swAssyLevel1.RenameComp(suffix, "BFM", module, "FNCE0120-1", baffleM, 200d, Aggregator);
        if (swCompLevel2 != null)
        {
            var swModelStdPanel = (ModelDoc2)swCompLevel2.GetModelDoc2();
            swModelStdPanel.ChangeDim("Length@SketchBase", baffleM-1d);
        }
    }


    /// <summary>
    /// 防水棉
    /// </summary>
    private void WaterproofSealing(AssemblyDoc swAssyLevel1, string suffix, SidePanel_e sidePanel, string leftPart, string rightPart)
    {
        switch (sidePanel)
        {
            case SidePanel_e.左:
            case SidePanel_e.双:
                swAssyLevel1.UnSuppress(suffix, leftPart, Aggregator);
                swAssyLevel1.UnSuppress(suffix, rightPart, Aggregator);
                break;
            case SidePanel_e.右:
            case SidePanel_e.NA:
            case SidePanel_e.中:
            default:
                swAssyLevel1.Suppress(suffix, leftPart);
                swAssyLevel1.UnSuppress(suffix, rightPart, Aggregator);
                break;
        }
    }

    /// <summary>
    /// 连接DP排水腔，焊接3m法兰
    /// </summary>
    private void ConnDpFlangle(AssemblyDoc swAssyLevel1, string suffix, SidePanel_e sidePanel, DpSide_e dpSide, string leftPart, string rightPart)
    {
        var left = dpSide is DpSide_e.左DP腔 or DpSide_e.两DP腔 &&
                   sidePanel is SidePanel_e.左 or SidePanel_e.双;
        swAssyLevel1.SuppressOnCond(suffix, leftPart, left, Aggregator);
        var right = dpSide is DpSide_e.右DP腔 or DpSide_e.两DP腔 &&
                    sidePanel is SidePanel_e.右 or SidePanel_e.双;
        swAssyLevel1.SuppressOnCond(suffix, rightPart, right, Aggregator);
    }

    /// <summary>
    /// 水洗挡板支架
    /// </summary>
    private void BaffleSidePart(AssemblyDoc swAssyLevel1, string suffix, SidePanel_e sidePanel, string leftPart, string rightPart)
    {

        var left = sidePanel is SidePanel_e.左 or SidePanel_e.双;
        swAssyLevel1.SuppressOnCond(suffix, leftPart, left, Aggregator);
        var right = sidePanel is SidePanel_e.右 or SidePanel_e.双;
        swAssyLevel1.SuppressOnCond(suffix, rightPart, right, Aggregator);
    }


    #endregion

    #region NormalLight/HCL
    private void NormalLightKcjDb800(AssemblyDoc swAssyLevel1, string suffix, double length, UvLightType_e uvLightType, int sensorNumber, double filterLeft, double filterRight, LightCable_e lightCable, CeilingLightType_e ceilingLightType, bool japan)
    {
        //灯腔
        FNCE0116(swAssyLevel1, suffix, "FNCE0116-1", length, uvLightType, lightCable, ceilingLightType, japan);
        //玻璃支架
        swAssyLevel1.ChangePartLength(suffix, "FNCE0056-1", "Length@SketchBase", length, Aggregator);

        //磁棒板
        if (uvLightType is UvLightType_e.NA)
        {
            swAssyLevel1.Suppress(suffix, "FNCE0145-1");
            swAssyLevel1.Suppress(suffix, "FNCE0161-1");
        }
        else
        {
            //正面
            FNCE0145(swAssyLevel1, suffix, "FNCE0145-1", length, filterLeft, sensorNumber);
            //背面
            FNCE0145(swAssyLevel1, suffix, "FNCE0161-1", length, filterRight, sensorNumber);
        }
    }

    private void NormalLightKcjSb535(AssemblyDoc swAssyLevel1, string suffix, string module, string type, double length, double width, UvLightType_e uvLightType, int sensorNumber, double filterLeft, LightCable_e lightCable, CeilingLightType_e ceilingLightType, bool marvel, int exhaustSpigotNumber, double middleToRight, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector, bool japan)
    {
        //重命名排风腔体
        var swCompLevel2 = swAssyLevel1.RenameComp(suffix, type, module, "FNCE0111-1", length, width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCE0111(swCompLevel2, length, uvLightType, ceilingLightType, lightCable, HclSide_e.NA, 0, 0, marvel, exhaustSpigotNumber, middleToRight, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotDis, ansul, ansulSide, ansulDetector, japan);
        }

        //灯腔
        FNCE0112(swAssyLevel1, suffix, "FNCE0112-1", length, uvLightType, lightCable, ceilingLightType, japan);

        //玻璃支架
        swAssyLevel1.ChangePartLength(suffix, "FNCE0056-1", "Length@SketchBase", length, Aggregator);

        //磁棒板
        if (uvLightType is UvLightType_e.NA)
        {
            swAssyLevel1.Suppress(suffix, "FNCE0145-1");
        }
        else
        {
            //正面
            FNCE0145(swAssyLevel1, suffix, "FNCE0145-1", length, filterLeft, sensorNumber);
        }
    }

    private void NormalLightKcwDb800(AssemblyDoc swAssyLevel1, string suffix, double length)
    {
        swAssyLevel1.ChangePartLength(suffix, "FNCE0034-1", "Length@SketchBase", length, Aggregator);
        swAssyLevel1.ChangePartLength(suffix, "FNCE0056-1", "Length@SketchBase", length, Aggregator);
    }

    private void NormalLightKcwSb535(AssemblyDoc swAssyLevel1, string suffix, string module, string type, SidePanel_e sidePanel, double length, double width, UvLightType_e uvLightType, CeilingLightType_e ceilingLightType, bool marvel, int exhaustSpigotNumber, double middleToRight, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, bool ansul, AnsulSide_e ansulSide, int baffleSensorNumber, double baffleSensorDis1, double baffleSensorDis2, bool japan)
    {
        //重命名排风腔体
        var swCompLevel2 = swAssyLevel1.RenameComp(suffix, type, module, "FNCE0032-1", length, width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCE0032(swCompLevel2, sidePanel, length, uvLightType, ceilingLightType, HclSide_e.NA, 0, 0, marvel, exhaustSpigotNumber, middleToRight, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotDis, ansul, ansulSide, baffleSensorNumber, baffleSensorDis1, baffleSensorDis2, japan);
        }

        //玻璃支架
        swAssyLevel1.ChangePartLength(suffix, "FNCE0034-1", "Length@SketchBase", length, Aggregator);
    }

    private void HclLightKcjDb800(ModelDoc2 swModelLevel1, AssemblyDoc swAssyLevel1, string suffix, string module, double length, UvLightType_e uvLightType, int sensorNumber, double filterLeft, double filterRight, LightCable_e lightCable, CeilingLightType_e ceilingLightType, bool japan, HclSide_e hclSide, double hclLeft, double hclRight)
    {
        swAssyLevel1.ChangeDim("Dis@DistanceLeft", hclLeft);

        #region 镀锌铁片
        switch (hclSide)
        {
            case HclSide_e.左HCL侧板:
            case HclSide_e.右HCL侧板:
                swAssyLevel1.UnSuppress(suffix, "FNCE0093-1", Aggregator);
                swAssyLevel1.UnSuppress("LocalLPatternMagnet");
                swAssyLevel1.ChangeDim("Number@LocalLPatternMagnet", 4);
                break;
            case HclSide_e.两HCL侧板:
                swAssyLevel1.UnSuppress(suffix, "FNCE0093-1", Aggregator);
                swAssyLevel1.UnSuppress("LocalLPatternMagnet");
                swAssyLevel1.ChangeDim("Number@LocalLPatternMagnet", 8);
                break;
            case HclSide_e.NA:
            case HclSide_e.无HCL侧板:
            default:
                swAssyLevel1.Suppress(suffix, "FNCE0093-1");
                swAssyLevel1.Suppress("LocalLPatternMagnet");
                break;
        }
        #endregion

        //灯腔
        FNCE0116(swAssyLevel1, suffix, "FNCE0087-1", length, uvLightType, lightCable, ceilingLightType, japan);

        //磁棒板
        if (uvLightType is UvLightType_e.NA)
        {
            swAssyLevel1.Suppress(suffix, "FNCE0069-1");
            swAssyLevel1.Suppress(suffix, "FNCE0071-1");
        }
        else
        {
            //正面
            FNCE0145(swAssyLevel1, suffix, "FNCE0069-1", length, filterLeft, sensorNumber);
            //背面
            FNCE0145(swAssyLevel1, suffix, "FNCE0071-1", length, filterRight, sensorNumber);
        }

        //支撑条
        FNCE0099(swAssyLevel1, suffix, "FNCE0099-1", length, hclSide, hclLeft, hclRight);
        FNCE0099(swAssyLevel1, suffix, "FNCE0090-1", length, hclSide, hclLeft, hclRight);
        //支撑条上部
        FNCE0091(swAssyLevel1, suffix, "FNCE0091-1", length, hclSide, hclLeft, hclRight);
        //HCL侧板
        HclSidePanel(swModelLevel1, swAssyLevel1, suffix, module, hclSide, hclLeft, hclRight, "FNCE0092-1", "FNCE0094-1");
    }

    private void HclLightKcjSb535(ModelDoc2 swModelLevel1, AssemblyDoc swAssyLevel1, string suffix, string module, string type, double length, double width, UvLightType_e uvLightType, int sensorNumber, double filterLeft, LightCable_e lightCable, CeilingLightType_e ceilingLightType, HclSide_e hclSide, double hclLeft, double hclRight, bool marvel, int exhaustSpigotNumber, double middleToRight, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector, bool japan)
    {
        swAssyLevel1.ChangeDim("Dis@DistanceLeft", hclLeft);

        #region 镀锌铁片
        switch (hclSide)
        {
            case HclSide_e.左HCL侧板:
            case HclSide_e.右HCL侧板:
                swAssyLevel1.UnSuppress(suffix, "FNCE0093-1", Aggregator);
                swAssyLevel1.UnSuppress("LocalLPatternMagnet");
                swAssyLevel1.ChangeDim("Number@LocalLPatternMagnet", 4);
                break;
            case HclSide_e.两HCL侧板:
                swAssyLevel1.UnSuppress(suffix, "FNCE0093-1", Aggregator);
                swAssyLevel1.UnSuppress("LocalLPatternMagnet");
                swAssyLevel1.ChangeDim("Number@LocalLPatternMagnet", 8);
                break;
            case HclSide_e.NA:
            case HclSide_e.无HCL侧板:
            default:
                swAssyLevel1.Suppress(suffix, "FNCE0093-1");
                swAssyLevel1.Suppress("LocalLPatternMagnet");
                break;
        }
        #endregion

        //重命名排风腔体
        var swCompLevel2 = swAssyLevel1.RenameComp(suffix, type, module, "FNCE0089-1", length, width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCE0111(swCompLevel2, length, uvLightType, ceilingLightType, lightCable, hclSide, hclLeft, hclRight, marvel, exhaustSpigotNumber, middleToRight, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotDis, ansul, ansulSide, ansulDetector, japan);
        }

        //灯腔
        FNCE0112(swAssyLevel1, suffix, "FNCE0085-1", length, uvLightType, lightCable, ceilingLightType, japan);

        //磁棒板
        if (uvLightType is UvLightType_e.NA)
        {
            swAssyLevel1.Suppress(suffix, "FNCE0069-1");
        }
        else
        {
            //正面
            FNCE0145(swAssyLevel1, suffix, "FNCE0069-1", length, filterLeft, sensorNumber);
        }

        //支撑条
        FNCE0099(swAssyLevel1, suffix, "FNCE0090-1", length, hclSide, hclLeft, hclRight);
        //支撑条上部
        FNCE0091(swAssyLevel1, suffix, "FNCE0091-1", length, hclSide, hclLeft, hclRight);
        //HCL侧板
        HclSidePanel(swModelLevel1, swAssyLevel1, suffix, module, hclSide, hclLeft, hclRight, "FNCE0092-1", "FNCE0094-1");
    }

    private void HclLightKcwDb800(ModelDoc2 swModelLevel1, AssemblyDoc swAssyLevel1, string suffix, string module, double length, HclSide_e hclSide, double hclLeft, double hclRight)
    {
        swAssyLevel1.ChangeDim("Dis@DistanceLeft", hclLeft);

        #region 镀锌铁片
        switch (hclSide)
        {
            case HclSide_e.左HCL侧板:
            case HclSide_e.右HCL侧板:
                swAssyLevel1.UnSuppress(suffix, "FNCE0093-1", Aggregator);
                swAssyLevel1.UnSuppress("LocalLPatternMagnet");
                swAssyLevel1.ChangeDim("Number@LocalLPatternMagnet", 4);
                break;
            case HclSide_e.两HCL侧板:
                swAssyLevel1.UnSuppress(suffix, "FNCE0093-1", Aggregator);
                swAssyLevel1.UnSuppress("LocalLPatternMagnet");
                swAssyLevel1.ChangeDim("Number@LocalLPatternMagnet", 8);
                break;
            case HclSide_e.NA:
            case HclSide_e.无HCL侧板:
            default:
                swAssyLevel1.Suppress(suffix, "FNCE0093-1");
                swAssyLevel1.Suppress("LocalLPatternMagnet");
                break;
        }
        #endregion

        //支撑条
        FNCE0099(swAssyLevel1, suffix, "FNCE0099-1", length, hclSide, hclLeft, hclRight);
        FNCE0099(swAssyLevel1, suffix, "FNCE0090-1", length, hclSide, hclLeft, hclRight);
        //支撑条上部
        FNCE0091(swAssyLevel1, suffix, "FNCE0091-1", length, hclSide, hclLeft, hclRight);
        //HCL侧板
        HclSidePanel(swModelLevel1, swAssyLevel1, suffix, module, hclSide, hclLeft, hclRight, "FNCE0092-1", "FNCE0094-1");
    }

    private void HclLightKcwSb535(ModelDoc2 swModelLevel1, AssemblyDoc swAssyLevel1, string suffix, string module, string type, SidePanel_e sidePanel, double length, double width, UvLightType_e uvLightType, CeilingLightType_e ceilingLightType, HclSide_e hclSide, double hclLeft, double hclRight, bool marvel, int exhaustSpigotNumber, double middleToRight, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, bool ansul, AnsulSide_e ansulSide, int baffleSensorNumber, double baffleSensorDis1, double baffleSensorDis2, bool japan)
    {
        swAssyLevel1.ChangeDim("Dis@DistanceLeft", hclLeft);

        #region 镀锌铁片
        switch (hclSide)
        {
            case HclSide_e.左HCL侧板:
            case HclSide_e.右HCL侧板:
                swAssyLevel1.UnSuppress(suffix, "FNCE0093-1", Aggregator);
                swAssyLevel1.UnSuppress("LocalLPatternMagnet");
                swAssyLevel1.ChangeDim("Number@LocalLPatternMagnet", 4);
                break;
            case HclSide_e.两HCL侧板:
                swAssyLevel1.UnSuppress(suffix, "FNCE0093-1", Aggregator);
                swAssyLevel1.UnSuppress("LocalLPatternMagnet");
                swAssyLevel1.ChangeDim("Number@LocalLPatternMagnet", 8);
                break;
            case HclSide_e.NA:
            case HclSide_e.无HCL侧板:
            default:
                swAssyLevel1.Suppress(suffix, "FNCE0093-1");
                swAssyLevel1.Suppress("LocalLPatternMagnet");
                break;
        }
        #endregion

        //重命名排风腔体
        var swCompLevel2 = swAssyLevel1.RenameComp(suffix, type, module, "FNCE0096-1", length, width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCE0032(swCompLevel2, sidePanel, length, uvLightType, ceilingLightType, hclSide, hclLeft, hclRight, marvel, exhaustSpigotNumber, middleToRight, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotDis, ansul, ansulSide, baffleSensorNumber, baffleSensorDis1, baffleSensorDis2, japan);
        }

        //支撑条
        FNCE0099(swAssyLevel1, suffix, "FNCE0090-1", length, hclSide, hclLeft, hclRight);
        //支撑条上部
        FNCE0091(swAssyLevel1, suffix, "FNCE0091-1", length, hclSide, hclLeft, hclRight);
        //HCL侧板
        HclSidePanel(swModelLevel1, swAssyLevel1, suffix, module, hclSide, hclLeft, hclRight, "FNCE0092-1", "FNCE0094-1");
    }

    private void HclSidePanel(ModelDoc2 swModelLevel1, AssemblyDoc swAssyLevel1, string suffix, string module, HclSide_e hclSide, double hclLeft, double hclRight, string leftPart,  string rightPart)
    {
        switch (hclSide)
        {
            case HclSide_e.左HCL侧板:
                {
                    var swCompLeft = swAssyLevel1.RenameComp(suffix, "HCLSP", module, leftPart, hclLeft, 200d, Aggregator);
                    if (swCompLeft != null) ChangeSideLength(swCompLeft, hclLeft);
                    swAssyLevel1.ForceSuppress(suffix, rightPart);
                    break;
                }

            case HclSide_e.右HCL侧板:
                {
                    var swCompRight = swAssyLevel1.RenameComp(suffix, "HCLSP", module, rightPart, hclRight, 200d, Aggregator);
                    if (swCompRight != null) ChangeSideLength(swCompRight, hclRight);
                    swAssyLevel1.ForceSuppress(suffix, leftPart);
                    break;
                }
            case HclSide_e.两HCL侧板:
                {
                    var swCompLeft = swAssyLevel1.RenameComp(suffix, "HCLSP", $"{module}.1", leftPart, hclLeft, 200d, Aggregator);
                    if (swCompLeft != null) ChangeSideLength(swCompLeft, hclLeft);

                    var swCompRight = swAssyLevel1.RenameComp(suffix, "HCLSP", $"{module}.2", rightPart, hclRight, 200d, Aggregator);
                    if (swCompRight != null) ChangeSideLength(swCompRight, hclRight);
                    break;
                }
            default:
            case HclSide_e.NA:
            case HclSide_e.无HCL侧板:
                swAssyLevel1.ForceSuppress(suffix, leftPart);
                swAssyLevel1.ForceSuppress(suffix, rightPart);
                break;
        }

        void ChangeSideLength(Component2 swComp, double sideLength)
        {
            var swModel = (ModelDoc2)swComp.GetModelDoc2();
            swModel.ChangeDim("Length@SketchBase", sideLength);
            swModel.ChangeDim("Dis@SketchMagnet", sideLength-125d);
        }
    }
    #endregion

    #region 双排风
    //KCJ,UCJ
    private void FNCE0115(Component2 swCompLevel2, double length, UvLightType_e uvLightType, LightCable_e lightCable, bool marvel, int exhaustSpigotNumber, double middleToRight, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, bool ansul, AnsulSide_e ansulSide, int ansulDetectorNumber, AnsulDetectorEnd_e ansulDetectorEnd, double ansulDetectorDis1, double ansulDetectorDis2, double ansulDetectorDis3, double ansulDetectorDis4, double ansulDetectorDis5, bool japan)
    {
        var swModelLevel2 = (ModelDoc2)swCompLevel2.GetModelDoc2();
        swModelLevel2.ChangeDim("Length@Base-Flange", length);

        #region 出线孔
        switch (lightCable)
        {
            case LightCable_e.左出线孔:
                swCompLevel2.Suppress("LightCableRight");
                swCompLevel2.UnSuppress("LightCableLeft");
                break;
            case LightCable_e.右出线孔:
                swCompLevel2.UnSuppress("LightCableRight");
                swCompLevel2.Suppress("LightCableLeft");
                break;
            case LightCable_e.两出线孔:
                swCompLevel2.UnSuppress("LightCableRight");
                swCompLevel2.UnSuppress("LightCableLeft");
                break;
            case LightCable_e.NA:
            case LightCable_e.无出线孔:
            default:
                swCompLevel2.Suppress("LightCableRight");
                swCompLevel2.Suppress("LightCableLeft");
                break;
        }

        #endregion

        #region UV灯支架
        switch (uvLightType)
        {
            case UvLightType_e.UVR4L:
            case UvLightType_e.UVR6L:
            case UvLightType_e.UVR8L:
                swCompLevel2.UnSuppress("FcCable");
                swCompLevel2.UnSuppress("UvTab");
                swCompLevel2.UnSuppress("UvRack");
                swModelLevel2.ChangeDim("ToRight@SketchUvRack", middleToRight);
                swModelLevel2.ChangeDim("UvRack@SketchUvRack", 1600d);
                swModelLevel2.ChangeDim("UvCable@SketchUvRack", 1200d);
                break;
            case UvLightType_e.UVR4S:
            case UvLightType_e.UVR6S:
            case UvLightType_e.UVR8S:
                swCompLevel2.UnSuppress("FcCable");
                swCompLevel2.UnSuppress("UvTab");
                swCompLevel2.UnSuppress("UvRack");
                swModelLevel2.ChangeDim("ToRight@SketchUvRack", middleToRight);
                swModelLevel2.ChangeDim("UvRack@SketchUvRack", 893d);
                swModelLevel2.ChangeDim("UvCable@SketchUvRack", 600d);
                break;
            case UvLightType_e.NA:
            default:
                swCompLevel2.Suppress("FcCable");
                swCompLevel2.Suppress("UvTab");
                swCompLevel2.Suppress("UvRack");
                break;
        }

        #endregion

        #region Marvel
        if (marvel)
        {
            swCompLevel2.UnSuppress("MarvelNtc");
            if (exhaustSpigotNumber == 1) swModelLevel2.ChangeDim("ToRight@SketchMarvelNtc", middleToRight + exhaustSpigotLength / 2d + 50d);
            else swModelLevel2.ChangeDim("ToRight@SketchMarvelNtc", middleToRight + exhaustSpigotDis / 2d + exhaustSpigotLength + 50d);

            swCompLevel2.UnSuppress("MarvelTab");
        }
        else
        {
            swCompLevel2.Suppress("MarvelNtc");
            swCompLevel2.Suppress("MarvelTab");
        }
        #endregion

        #region ANSUL

        if (ansul)
        {
            switch (ansulSide)
            {
                //侧喷
                case AnsulSide_e.左侧喷:
                    swCompLevel2.UnSuppress("AnsulSideLeft");
                    swCompLevel2.Suppress("AnsulSideRight");
                    break;
                case AnsulSide_e.右侧喷:
                    swCompLevel2.UnSuppress("AnsulSideRight");
                    swCompLevel2.Suppress("AnsulSideLeft");
                    break;
                case AnsulSide_e.NA:
                case AnsulSide_e.无侧喷:
                default:
                    swCompLevel2.Suppress("AnsulSideRight");
                    swCompLevel2.Suppress("AnsulSideLeft");
                    break;
            }

            #region Ansul探测器，双排风烟罩需要探测器安装在MidRoof上
            //探测器
            if (ansulDetectorNumber> 0)
            {
                swCompLevel2.UnSuppress("AnsulDetector1");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDetector1", ansulDetectorDis1);
                if (ansulDetectorEnd == AnsulDetectorEnd_e.左末端探测器|| (ansulDetectorEnd == AnsulDetectorEnd_e.右末端探测器 && ansulDetectorNumber == 1))
                    swModelLevel2.ChangeDim("Length@SketchAnsulDetector1", 195d);
                else swModelLevel2.ChangeDim("Length@SketchAnsulDetector1", 175d);
            }
            if (ansulDetectorNumber > 1)
            {
                swCompLevel2.UnSuppress("AnsulDetector2");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDetector2", ansulDetectorDis2);
                if (ansulDetectorEnd == AnsulDetectorEnd_e.右末端探测器 && ansulDetectorNumber == 2)
                    swModelLevel2.ChangeDim("Length@SketchAnsulDetector2", 195d);
                else swModelLevel2.ChangeDim("Length@SketchAnsulDetector2", 175d);
            }
            if (ansulDetectorNumber > 2)
            {
                swCompLevel2.UnSuppress("AnsulDetector3");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDetector3", ansulDetectorDis3);
                if (ansulDetectorEnd == AnsulDetectorEnd_e.右末端探测器 && ansulDetectorNumber == 3)
                    swModelLevel2.ChangeDim("Length@SketchAnsulDetector3", 195d);
                else swModelLevel2.ChangeDim("Length@SketchAnsulDetector3", 175d);
            }
            if (ansulDetectorNumber > 3)
            {
                swCompLevel2.UnSuppress("AnsulDetector4");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDetector4", ansulDetectorDis4);
                if (ansulDetectorEnd == AnsulDetectorEnd_e.右末端探测器 && ansulDetectorNumber == 4)
                    swModelLevel2.ChangeDim("Length@SketchAnsulDetector4", 195d);
                else swModelLevel2.ChangeDim("Length@SketchAnsulDetector4", 175d);
            }
            if (ansulDetectorNumber > 4)
            {
                swCompLevel2.UnSuppress("AnsulDetector5");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDetector5", ansulDetectorDis5);
                if (ansulDetectorEnd == AnsulDetectorEnd_e.右末端探测器 && ansulDetectorNumber == 5)
                    swModelLevel2.ChangeDim("Length@SketchAnsulDetector5", 195d);
                else swModelLevel2.ChangeDim("Length@SketchAnsulDetector5", 175d);
            }
            #endregion


        }
        else
        {
            swCompLevel2.Suppress("AnsulSideRight");
            swCompLevel2.Suppress("AnsulSideLeft");

            swCompLevel2.Suppress("AnsulDetector1");
            swCompLevel2.Suppress("AnsulDetector2");
            swCompLevel2.Suppress("AnsulDetector3");
            swCompLevel2.Suppress("AnsulDetector4");
            swCompLevel2.Suppress("AnsulDetector5");
        }

        #endregion

        #region 日本项目
        if (japan)
        {
            swCompLevel2.Suppress("LiftingHoles");
            swCompLevel2.Suppress("OneSpigot");
        }
        else
        {
            swCompLevel2.UnSuppress("LiftingHoles");
            swCompLevel2.UnSuppress("OneSpigot");
            swModelLevel2.ChangeDim("ToRight@SketchOneSpigot", middleToRight);
            swModelLevel2.ChangeDim("Length@SketchOneSpigot", exhaustSpigotLength);
            swModelLevel2.ChangeDim("Width@SketchOneSpigot", exhaustSpigotWidth);
        }
        #endregion
    }

    private void FNCE0116(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, UvLightType_e uvLightType, LightCable_e lightCable, CeilingLightType_e ceilingLightType, bool japan)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", length);

        #region 磁棒板定位孔
        if (uvLightType is UvLightType_e.NA)
        {
            swCompLevel2.Suppress("UvSensorSupport");
            swCompLevel2.Suppress("UvSensorSupportBack");
        }
        else
        {
            swCompLevel2.UnSuppress("UvSensorSupport");
            swCompLevel2.UnSuppress("UvSensorSupportBack");
        }
        #endregion

        #region 灯具出线孔
        switch (lightCable)
        {
            case LightCable_e.左出线孔:
                swCompLevel2.Suppress("LightCableRight");
                swCompLevel2.UnSuppress("LightCableLeft");
                break;
            case LightCable_e.右出线孔:
                swCompLevel2.UnSuppress("LightCableRight");
                swCompLevel2.Suppress("LightCableLeft");
                break;
            case LightCable_e.两出线孔:
                swCompLevel2.UnSuppress("LightCableRight");
                swCompLevel2.UnSuppress("LightCableLeft");
                break;
            case LightCable_e.NA:
            case LightCable_e.无出线孔:
            default:
                swCompLevel2.Suppress("LightCableRight");
                swCompLevel2.Suppress("LightCableLeft");
                break;
        }
        #endregion

        //todo:核实HCL中的这个LightT特征是什么？
        if (ceilingLightType is CeilingLightType_e.日光灯 or CeilingLightType_e.HCL)
            swCompLevel2.UnSuppress("LightT");
        else
            swCompLevel2.Suppress("LightT");

        //日本灯射钉
        if (japan)
            swCompLevel2.UnSuppress("JapanLight");
        else
            swCompLevel2.Suppress("JapanLight");
    }

    private void FNCE0099(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, HclSide_e hclSide, double hclLeft, double hclRight)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length);

        #region HCL侧板固定镀锌铁片铆钉孔
        if (hclSide is HclSide_e.左HCL侧板 or HclSide_e.两HCL侧板)
        {
            swCompLevel2.UnSuppress("HclLeft");
            swModelLevel2.ChangeDim("Dis@SketchHclLeft", hclLeft-150d);
        }
        else
        {
            swCompLevel2.Suppress("HclLeft");
        }
        if (hclSide is HclSide_e.右HCL侧板 or HclSide_e.两HCL侧板)
        {
            swCompLevel2.UnSuppress("HclRight");
            swModelLevel2.ChangeDim("Dis@SketchHclRight", hclRight-150d);
        }
        else
        {
            swCompLevel2.Suppress("HclRight");
        }
        #endregion
    }

    private void FNCE0091(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, HclSide_e hclSide, double hclLeft, double hclRight)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        var netLength = length;
        switch (hclSide)
        {
            case HclSide_e.左HCL侧板:
                netLength = length - hclLeft;
                break;
            case HclSide_e.右HCL侧板:
                netLength = length - hclRight;
                break;
            case HclSide_e.两HCL侧板:
                netLength = length - hclLeft- hclRight;
                break;
            case HclSide_e.NA:
            case HclSide_e.无HCL侧板:
            default:
                break;
        }
        swModelLevel2.ChangeDim("Length@SketchBase", netLength);
    }

    private void FNCE0145(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, double sideLength, int number)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", length-5d);
        //第一个过滤器
        swModelLevel2.ChangeDim("Dis@SketchFilterSensor", sideLength+250d);
        swModelLevel2.ChangeDim("Number@LPatternFilterSensor", number);
        //侧板的磁感应器
        if (sideLength.Equals(0d))
        {
            swCompLevel2.Suppress("SideSensor");
        }
        else
        {
            swCompLevel2.UnSuppress("SideSensor");
            swModelLevel2.ChangeDim("Dis@SketchSideSensor", sideLength/2d-2.5d);

        }
    }

    //KCW,UCW
    private void FNCE0158(Component2 swCompLevel2, SidePanel_e sidePanel, double length, UvLightType_e uvLightType, bool marvel, int exhaustSpigotNumber, double middleToRight, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, bool ansul, AnsulSide_e ansulSide, int baffleSensorNumber, double baffleSensorDis1, double baffleSensorDis2, bool japan)
    {
        var swModelLevel2 = (ModelDoc2)swCompLevel2.GetModelDoc2();
        swModelLevel2.ChangeDim("Length@Base-Flange", length);

        #region 水洗挡板侧板切除
        switch (sidePanel)
        {
            case SidePanel_e.左:
                swCompLevel2.UnSuppress("CutLeftFront");
                swCompLevel2.UnSuppress("CutLeftBack");
                swCompLevel2.Suppress("CutRightFront");
                swCompLevel2.Suppress("CutRightBack");
                break;
            case SidePanel_e.右:
                swCompLevel2.Suppress("CutLeftFront");
                swCompLevel2.Suppress("CutLeftBack");
                swCompLevel2.UnSuppress("CutRightFront");
                swCompLevel2.UnSuppress("CutRightBack");
                break;
            case SidePanel_e.双:
                swCompLevel2.UnSuppress("CutLeftFront");
                swCompLevel2.UnSuppress("CutLeftBack");
                swCompLevel2.UnSuppress("CutRightFront");
                swCompLevel2.UnSuppress("CutRightBack");
                break;
            case SidePanel_e.NA:
            case SidePanel_e.中:
            default:
                swCompLevel2.Suppress("CutLeftFront");
                swCompLevel2.Suppress("CutLeftBack");
                swCompLevel2.Suppress("CutRightFront");
                swCompLevel2.Suppress("CutRightBack");
                break;
        }
        #endregion

        #region UV灯支架
        switch (uvLightType)
        {
            case UvLightType_e.UVR4L:
            case UvLightType_e.UVR6L:
            case UvLightType_e.UVR8L:
                swCompLevel2.UnSuppress("UvTab");
                swCompLevel2.UnSuppress("UvRack");
                swModelLevel2.ChangeDim("ToRight@SketchUvRack", middleToRight);
                swModelLevel2.ChangeDim("UvRack@SketchUvRack", 1600d);
                swModelLevel2.ChangeDim("UvCable@SketchUvRack", 1200);
                break;
            case UvLightType_e.UVR4S:
            case UvLightType_e.UVR6S:
            case UvLightType_e.UVR8S:
                swCompLevel2.UnSuppress("UvTab");
                swCompLevel2.UnSuppress("UvRack");
                swModelLevel2.ChangeDim("ToRight@SketchUvRack", middleToRight);
                swModelLevel2.ChangeDim("UvRack@SketchUvRack", 893d);
                swModelLevel2.ChangeDim("UvCable@SketchUvRack", 600);
                break;
            case UvLightType_e.NA:
            default:
                swCompLevel2.Suppress("UvTab");
                swCompLevel2.Suppress("UvRack");
                break;
        }
        //水洗挡板磁感应孔
        if (uvLightType is UvLightType_e.NA)
        {
            swCompLevel2.Suppress("BaffleSensorFront");
            swCompLevel2.Suppress("BaffleSensorBack");
            swCompLevel2.Suppress("BaffleSensorCable");
            swCompLevel2.Suppress("LPatternBaffleSensor");
        }
        else
        {
            swCompLevel2.UnSuppress("BaffleSensorFront");
            swCompLevel2.UnSuppress("BaffleSensorBack");
            swCompLevel2.UnSuppress("BaffleSensorCable");
            swModelLevel2.ChangeDim("Dis@SketchBaffleSensorFront", baffleSensorDis1);
            swModelLevel2.ChangeDim("Dis@SketchBaffleSensorBack", baffleSensorDis1);
            swModelLevel2.ChangeDim("Dis@SketchBaffleSensorCable", baffleSensorDis1);
            if (baffleSensorNumber > 1)
            {
                swCompLevel2.UnSuppress("LPatternBaffleSensor");
                swModelLevel2.ChangeDim("Number@LPatternBaffleSensor", baffleSensorNumber);
                swModelLevel2.ChangeDim("Dis@LPatternBaffleSensor", baffleSensorDis2);
            }
            else
            {
                swCompLevel2.Suppress("LPatternBaffleSensor");
            }
        }
        #endregion

        #region Marvel
        if (marvel)
        {
            swCompLevel2.UnSuppress("MarvelNtc");
            if (exhaustSpigotNumber == 1) swModelLevel2.ChangeDim("ToRight@SketchMarvelNtc", middleToRight + exhaustSpigotLength / 2d + 50d);
            else swModelLevel2.ChangeDim("ToRight@SketchMarvelNtc", middleToRight + exhaustSpigotDis / 2d + exhaustSpigotLength + 50d);

            swCompLevel2.UnSuppress("MarvelTab");
        }
        else
        {
            swCompLevel2.Suppress("MarvelNtc");
            swCompLevel2.Suppress("MarvelTab");
        }
        #endregion

        #region ANSUL

        if (ansul)
        {
            switch (ansulSide)
            {
                //侧喷
                case AnsulSide_e.左侧喷:
                    swCompLevel2.UnSuppress("AnsulSideLeft");
                    swCompLevel2.Suppress("AnsulSideRight");
                    break;
                case AnsulSide_e.右侧喷:
                    swCompLevel2.UnSuppress("AnsulSideRight");
                    swCompLevel2.Suppress("AnsulSideLeft");
                    break;
                case AnsulSide_e.NA:
                case AnsulSide_e.无侧喷:
                default:
                    swCompLevel2.Suppress("AnsulSideRight");
                    swCompLevel2.Suppress("AnsulSideLeft");
                    break;
            }
        }
        else
        {
            swCompLevel2.Suppress("AnsulSideRight");
            swCompLevel2.Suppress("AnsulSideLeft");
        }

        #endregion

        #region 日本项目
        if (japan)
        {
            swCompLevel2.Suppress("LiftingHoles");
            swCompLevel2.Suppress("OneSpigot");
        }
        else
        {
            swCompLevel2.UnSuppress("LiftingHoles");
            swCompLevel2.UnSuppress("OneSpigot");
            swModelLevel2.ChangeDim("ToRight@SketchOneSpigot", middleToRight);
            swModelLevel2.ChangeDim("Length@SketchOneSpigot", exhaustSpigotLength);
            swModelLevel2.ChangeDim("Width@SketchOneSpigot", exhaustSpigotWidth);
        }
        #endregion

    }

    private void FNCE0051(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, int baffleHookingNumber, double baffleHookingDis, CeilingLightType_e ceilingLightType, bool japan, FilterSide_e filterSide, int filterNumber, double filterLeft, double filterRight)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", length);

        #region 水洗挂管挂钩
        swModelLevel2.ChangeDim("Number@LPatternBaffleHooking", baffleHookingNumber);
        swModelLevel2.ChangeDim("Dis@LPatternBaffleHooking", baffleHookingDis);
        #endregion

        #region 灯具
        //todo:核实HCL中的这个LightT特征是什么？
        if (ceilingLightType is CeilingLightType_e.日光灯)
            swCompLevel2.UnSuppress("LightT");
        else
            swCompLevel2.Suppress("LightT");
        #endregion

        #region 日本项目
        //日本灯射钉，日本项目FC油网不需要导轨，而是打射钉
        if (japan)
        {
            swCompLevel2.Suppress("LightT");
            swCompLevel2.UnSuppress("JapanLight");
            //不要导轨
            swCompLevel2.Suppress("FcRailFront");
            swCompLevel2.Suppress("FcRailBack");
            //要射钉
            switch (filterSide)
            {
                case FilterSide_e.左过滤器侧板:
                    swCompLevel2.UnSuppress("FcLeftFront");
                    swCompLevel2.UnSuppress("FcLeftBack");
                    swModelLevel2.ChangeDim("Dis@SketchFcLeftFront", filterLeft-50d);
                    swModelLevel2.ChangeDim("Dis@SketchFcLeftBack", filterLeft-50d);
                    swCompLevel2.Suppress("FcRightFront");
                    swCompLevel2.Suppress("FcRightBack");
                    break;
                case FilterSide_e.右过滤器侧板:
                    swCompLevel2.Suppress("FcLeftFront");
                    swCompLevel2.Suppress("FcLeftBack");
                    swCompLevel2.UnSuppress("FcRightFront");
                    swCompLevel2.UnSuppress("FcRightBack");
                    swModelLevel2.ChangeDim("Dis@SketchFcRightFront", filterRight-50d);
                    swModelLevel2.ChangeDim("Dis@SketchFcRightBack", filterRight-50d);
                    break;
                case FilterSide_e.两过滤器侧板:
                    swCompLevel2.UnSuppress("FcLeftFront");
                    swCompLevel2.UnSuppress("FcLeftBack");
                    swModelLevel2.ChangeDim("Dis@SketchFcLeftFront", filterLeft-50d);
                    swModelLevel2.ChangeDim("Dis@SketchFcLeftBack", filterLeft-50d);
                    swCompLevel2.UnSuppress("FcRightFront");
                    swCompLevel2.UnSuppress("FcRightBack");
                    swModelLevel2.ChangeDim("Dis@SketchFcRightFront", filterRight-50d);
                    swModelLevel2.ChangeDim("Dis@SketchFcRightBack", filterRight-50d);
                    break;
                case FilterSide_e.无过滤器侧板:
                case FilterSide_e.NA:
                default:
                    swCompLevel2.Suppress("FcLeftFront");
                    swCompLevel2.Suppress("FcLeftBack");
                    swCompLevel2.Suppress("FcRightFront");
                    swCompLevel2.Suppress("FcRightBack");
                    break;
            }
            swCompLevel2.UnSuppress("FcFirstFront");
            swCompLevel2.UnSuppress("FcFirstBack");
            swModelLevel2.ChangeDim("Dis@SketchFcFirstFront", filterLeft+25d);
            swModelLevel2.ChangeDim("Dis@SketchFcFirstBack", filterLeft+25d);
            if (filterNumber > 1)
            {
                swCompLevel2.Suppress("LPatternFc");
                swModelLevel2.ChangeDim("Number@LPatternFc", filterNumber);
            }
            else
            {
                swCompLevel2.Suppress("LPatternFc");
            }
        }
        else
        {
            swCompLevel2.Suppress("JapanLight");
            //需要导轨
            swCompLevel2.UnSuppress("FcRailFront");
            swCompLevel2.UnSuppress("FcRailBack");
            //不需要射钉
            swCompLevel2.Suppress("FcLeftFront");
            swCompLevel2.Suppress("FcLeftBack");
            swCompLevel2.Suppress("FcRightFront");
            swCompLevel2.Suppress("FcRightBack");
            swCompLevel2.Suppress("FcFirstFront");
            swCompLevel2.Suppress("FcFirstBack");
            swCompLevel2.Suppress("LPatternFc");
        }
        #endregion

    }

    private void FNCE0007(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, CeilingWaterInlet_e ceilingWaterInlet)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length-5d);

        #region 入水口开孔
        if (ceilingWaterInlet is CeilingWaterInlet_e.上入水管)
        {
            swCompLevel2.UnSuppress("PipeUp");
        }
        else
        {
            swCompLevel2.Suppress("PipeUp");
        }
        #endregion
    }


    private void FNCE0003(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, SidePanel_e sidePanel)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", length);

        switch (sidePanel)
        {

            case SidePanel_e.左:
                swCompLevel2.UnSuppress("CutLeft");
                swCompLevel2.Suppress("CutRight");
                break;
            case SidePanel_e.右:
                swCompLevel2.Suppress("CutLeft");
                swCompLevel2.UnSuppress("CutRight");
                break;
            case SidePanel_e.双:
                swCompLevel2.UnSuppress("CutLeft");
                swCompLevel2.UnSuppress("CutRight");
                break;
            case SidePanel_e.中:
            case SidePanel_e.NA:
            default:
                swCompLevel2.Suppress("CutLeft");
                swCompLevel2.Suppress("CutRight");
                break;
        }

    }

    private void FNCE0062(AssemblyDoc swAssyLevel1, string suffix, string partName, SidePanel_e sidePanel, DpSide_e dpSide)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        var baffle = sidePanel is SidePanel_e.左 or SidePanel_e.双;
        swCompLevel2.SuppressOnCond("BaffleFront1", baffle);
        swCompLevel2.SuppressOnCond("BaffleFront2", baffle);
        swCompLevel2.SuppressOnCond("BaffleFront3", baffle);
        swCompLevel2.SuppressOnCond("BaffleFront4", baffle);
        swCompLevel2.SuppressOnCond("BaffleBack1", baffle);
        swCompLevel2.SuppressOnCond("BaffleBack2", baffle);
        swCompLevel2.SuppressOnCond("BaffleBack3", baffle);
        swCompLevel2.SuppressOnCond("BaffleBack4", baffle);
        var notCut = sidePanel is SidePanel_e.左 or SidePanel_e.双 && (dpSide is not DpSide_e.左DP腔 or DpSide_e.两DP腔);
        swCompLevel2.SuppressOnCond("CutDrainFront", !notCut);
        swCompLevel2.SuppressOnCond("CutDrainBack", !notCut);
    }
    private void FNCE0063(AssemblyDoc swAssyLevel1, string suffix, string partName, SidePanel_e sidePanel, DpSide_e dpSide)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        var baffle = sidePanel is SidePanel_e.右 or SidePanel_e.双;
        swCompLevel2.SuppressOnCond("BaffleFront1", baffle);
        swCompLevel2.SuppressOnCond("BaffleFront2", baffle);
        swCompLevel2.SuppressOnCond("BaffleFront3", baffle);
        swCompLevel2.SuppressOnCond("BaffleFront4", baffle);
        swCompLevel2.SuppressOnCond("BaffleBack1", baffle);
        swCompLevel2.SuppressOnCond("BaffleBack2", baffle);
        swCompLevel2.SuppressOnCond("BaffleBack3", baffle);
        swCompLevel2.SuppressOnCond("BaffleBack4", baffle);
        var notCut = sidePanel is SidePanel_e.右 or SidePanel_e.双 && (dpSide is not DpSide_e.右DP腔 or DpSide_e.两DP腔);
        swCompLevel2.SuppressOnCond("CutDrainFront", !notCut);
        swCompLevel2.SuppressOnCond("CutDrainBack", !notCut);
    }

    private void FNCE0044(AssemblyDoc swAssyLevel1, string suffix, string partName, SidePanel_e sidePanel, DpSide_e dpSide, CeilingLightType_e ceilingLightType)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        var baffle = sidePanel is SidePanel_e.左 or SidePanel_e.双;
        swCompLevel2.SuppressOnCond("Baffle1", baffle);
        swCompLevel2.SuppressOnCond("Baffle2", baffle);
        swCompLevel2.SuppressOnCond("Baffle3", baffle);
        swCompLevel2.SuppressOnCond("Baffle4", baffle);
        var notCut = sidePanel is SidePanel_e.左 or SidePanel_e.双 && (dpSide is not DpSide_e.左DP腔 or DpSide_e.两DP腔);
        swCompLevel2.SuppressOnCond("CutDrain", !notCut);
        if (ceilingLightType is not CeilingLightType_e.NA)
        {
            swCompLevel2.SuppressOnCond("Hcl", ceilingLightType is CeilingLightType_e.HCL);
        }
        
    }
    private void FNCE0045(AssemblyDoc swAssyLevel1, string suffix, string partName, SidePanel_e sidePanel, DpSide_e dpSide, CeilingLightType_e ceilingLightType)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        var baffle = sidePanel is SidePanel_e.右 or SidePanel_e.双;
        swCompLevel2.SuppressOnCond("Baffle1", baffle);
        swCompLevel2.SuppressOnCond("Baffle2", baffle);
        swCompLevel2.SuppressOnCond("Baffle3", baffle);
        swCompLevel2.SuppressOnCond("Baffle4", baffle);
        var notCut = sidePanel is SidePanel_e.右 or SidePanel_e.双 && (dpSide is not DpSide_e.右DP腔 or DpSide_e.两DP腔);
        swCompLevel2.SuppressOnCond("CutDrain", !notCut);
        if (ceilingLightType is not CeilingLightType_e.NA)
        {
            swCompLevel2.SuppressOnCond("Hcl", ceilingLightType is CeilingLightType_e.HCL);
        }
    }

    #endregion

    #region 单排风
    private void FNCE0111(Component2 swCompLevel2, double length, UvLightType_e uvLightType, CeilingLightType_e ceilingLightType, LightCable_e lightCable, HclSide_e hclSide, double hclLeft, double hclRight, bool marvel, int exhaustSpigotNumber, double middleToRight, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector, bool japan)
    {
        var swModelLevel2 = (ModelDoc2)swCompLevel2.GetModelDoc2();
        swModelLevel2.ChangeDim("Length@Base-Flange", length);

        #region 出线孔
        switch (lightCable)
        {
            case LightCable_e.左出线孔:
                swCompLevel2.Suppress("LightCableRight");
                swCompLevel2.UnSuppress("LightCableLeft");
                break;
            case LightCable_e.右出线孔:
                swCompLevel2.UnSuppress("LightCableRight");
                swCompLevel2.Suppress("LightCableLeft");
                break;
            case LightCable_e.两出线孔:
                swCompLevel2.UnSuppress("LightCableRight");
                swCompLevel2.UnSuppress("LightCableLeft");
                break;
            case LightCable_e.NA:
            case LightCable_e.无出线孔:
            default:
                swCompLevel2.Suppress("LightCableRight");
                swCompLevel2.Suppress("LightCableLeft");
                break;
        }

        #endregion

        #region UV灯支架

        switch (uvLightType)
        {
            case UvLightType_e.UVR4L:
            case UvLightType_e.UVR6L:
            case UvLightType_e.UVR8L:
                swCompLevel2.UnSuppress("FcCable");
                swCompLevel2.UnSuppress("UvTab");
                swCompLevel2.UnSuppress("UvRack");
                swModelLevel2.ChangeDim("ToRight@SketchUvRack", middleToRight);
                swModelLevel2.ChangeDim("UvRack@SketchUvRack", 1600d);
                swModelLevel2.ChangeDim("UvCable@SketchUvRack", 1200d);
                break;
            case UvLightType_e.UVR4S:
            case UvLightType_e.UVR6S:
            case UvLightType_e.UVR8S:
                swCompLevel2.UnSuppress("FcCable");
                swCompLevel2.UnSuppress("UvTab");
                swCompLevel2.UnSuppress("UvRack");
                swModelLevel2.ChangeDim("ToRight@SketchUvRack", middleToRight);
                swModelLevel2.ChangeDim("UvRack@SketchUvRack", 893d);
                swModelLevel2.ChangeDim("UvCable@SketchUvRack", 600d);
                break;
            case UvLightType_e.NA:
            default:
                swCompLevel2.Suppress("FcCable");
                swCompLevel2.Suppress("UvTab");
                swCompLevel2.Suppress("UvRack");
                break;
        }

        #endregion

        #region Marvel
        if (marvel)
        {
            swCompLevel2.UnSuppress("MarvelNtc");
            if (exhaustSpigotNumber == 1) swModelLevel2.ChangeDim("ToRight@SketchMarvelNtc", middleToRight + exhaustSpigotLength / 2d + 50d);
            else swModelLevel2.ChangeDim("ToRight@SketchMarvelNtc", middleToRight + exhaustSpigotDis / 2d + exhaustSpigotLength + 50d);

            swCompLevel2.UnSuppress("MarvelTab");
        }
        else
        {
            swCompLevel2.Suppress("MarvelNtc");
            swCompLevel2.Suppress("MarvelTab");
        }
        #endregion

        #region ANSUL
        if (ansul)
        {
            switch (ansulSide)
            {
                //侧喷
                case AnsulSide_e.左侧喷:
                    swCompLevel2.UnSuppress("AnsulSideLeft");
                    swCompLevel2.Suppress("AnsulSideRight");
                    break;
                case AnsulSide_e.右侧喷:
                    swCompLevel2.UnSuppress("AnsulSideRight");
                    swCompLevel2.Suppress("AnsulSideLeft");
                    break;
                case AnsulSide_e.NA:
                case AnsulSide_e.无侧喷:
                default:
                    swCompLevel2.Suppress("AnsulSideRight");
                    swCompLevel2.Suppress("AnsulSideLeft");
                    break;
            }

            #region Ansul探测器入口
            //探测器
            switch (ansulDetector)
            {
                case AnsulDetector_e.左探测器口:
                    swCompLevel2.Suppress("AnsulDetectorRight");
                    swCompLevel2.UnSuppress("AnsulDetectorLeft");
                    break;
                case AnsulDetector_e.右探测器口:
                    swCompLevel2.UnSuppress("AnsulDetectorRight");
                    swCompLevel2.Suppress("AnsulDetectorLeft");
                    break;
                case AnsulDetector_e.两探测器口:
                    swCompLevel2.UnSuppress("AnsulDetectorRight");
                    swCompLevel2.UnSuppress("AnsulDetectorLeft");
                    break;
                case AnsulDetector_e.NA:
                case AnsulDetector_e.无探测器口:
                default:
                    swCompLevel2.Suppress("AnsulDetectorRight");
                    swCompLevel2.Suppress("AnsulDetectorLeft");
                    break;
            }
            #endregion
        }
        else
        {
            swCompLevel2.Suppress("AnsulSideRight");
            swCompLevel2.Suppress("AnsulSideLeft");
            swCompLevel2.Suppress("AnsulDetectorRight");
            swCompLevel2.Suppress("AnsulDetectorLeft");
        }

        #endregion

        #region 日本项目
        if (japan)
        {
            swCompLevel2.Suppress("LiftingHoles");
            swCompLevel2.Suppress("OneSpigot");
        }
        else
        {
            swCompLevel2.UnSuppress("LiftingHoles");
            swCompLevel2.UnSuppress("OneSpigot");
            swModelLevel2.ChangeDim("ToRight@SketchOneSpigot", middleToRight);
            swModelLevel2.ChangeDim("Length@SketchOneSpigot", exhaustSpigotLength);
            swModelLevel2.ChangeDim("Width@SketchOneSpigot", exhaustSpigotWidth);
        }
        #endregion

        #region HCL侧板固定镀锌铁片铆钉孔
        if (ceilingLightType is CeilingLightType_e.HCL)
        {
            if (hclSide is HclSide_e.左HCL侧板 or HclSide_e.两HCL侧板)
            {
                swCompLevel2.UnSuppress("HclLeft");
                swModelLevel2.ChangeDim("Dis@SketchHclLeft", hclLeft-150d);
            }
            else
            {
                swCompLevel2.Suppress("HclLeft");
            }
            if (hclSide is HclSide_e.右HCL侧板 or HclSide_e.两HCL侧板)
            {
                swCompLevel2.UnSuppress("HclRight");
                swModelLevel2.ChangeDim("Dis@SketchHclRight", hclRight-150d);
            }
            else
            {
                swCompLevel2.Suppress("HclRight");
            }
        }
        #endregion
    }

    private void FNCE0112(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, UvLightType_e uvLightType, LightCable_e lightCable, CeilingLightType_e ceilingLightType, bool japan)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", length);

        #region 磁棒板定位孔
        if (uvLightType is UvLightType_e.NA)
        {
            swCompLevel2.Suppress("UvSensorSupport");
        }
        else
        {
            swCompLevel2.UnSuppress("UvSensorSupport");
        }
        #endregion

        #region 灯具出线孔
        switch (lightCable)
        {
            case LightCable_e.左出线孔:
                swCompLevel2.Suppress("LightCableRight");
                swCompLevel2.UnSuppress("LightCableLeft");
                break;
            case LightCable_e.右出线孔:
                swCompLevel2.UnSuppress("LightCableRight");
                swCompLevel2.Suppress("LightCableLeft");
                break;
            case LightCable_e.两出线孔:
                swCompLevel2.UnSuppress("LightCableRight");
                swCompLevel2.UnSuppress("LightCableLeft");
                break;
            case LightCable_e.NA:
            case LightCable_e.无出线孔:
            default:
                swCompLevel2.Suppress("LightCableRight");
                swCompLevel2.Suppress("LightCableLeft");
                break;
        }
        #endregion

        #region 灯具
        //todo:核实HCL中的这个LightT特征是什么？
        if (ceilingLightType is CeilingLightType_e.日光灯 or CeilingLightType_e.HCL)
            swCompLevel2.UnSuppress("LightT");
        else
            swCompLevel2.Suppress("LightT");

        //日本灯射钉
        if (japan)
            swCompLevel2.UnSuppress("JapanLight");
        else
            swCompLevel2.Suppress("JapanLight");
        #endregion
    }


    private void FNCE0127(Component2 swCompLevel2, double length, UvLightType_e uvLightType, bool marvel, int exhaustSpigotNumber, double middleToRight, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector, bool japan)
    {
        var swModelLevel2 = (ModelDoc2)swCompLevel2.GetModelDoc2();
        swModelLevel2.ChangeDim("Length@Base-Flange", length);

        #region Marvel
        if (marvel)
        {
            swCompLevel2.UnSuppress("MarvelNtc");
            if (exhaustSpigotNumber == 1) swModelLevel2.ChangeDim("ToRight@SketchMarvelNtc", middleToRight + exhaustSpigotLength / 2d + 50d);
            else swModelLevel2.ChangeDim("ToRight@SketchMarvelNtc", middleToRight + exhaustSpigotDis / 2d + exhaustSpigotLength + 50d);

            swCompLevel2.UnSuppress("MarvelTab");
        }
        else
        {
            swCompLevel2.Suppress("MarvelNtc");
            swCompLevel2.Suppress("MarvelTab");
        }
        #endregion

        #region UV灯支架
        if (uvLightType is not UvLightType_e.NA)
        {
            switch (uvLightType)
            {
                case UvLightType_e.UVR4L:
                case UvLightType_e.UVR6L:
                case UvLightType_e.UVR8L:
                    swCompLevel2.UnSuppress("FcCable");
                    swCompLevel2.UnSuppress("UvSensorSupport");
                    swCompLevel2.UnSuppress("UvRack");
                    swModelLevel2.ChangeDim("ToRight@SketchUvRack", middleToRight);
                    swModelLevel2.ChangeDim("UvRack@SketchUvRack", 1600d);
                    swCompLevel2.UnSuppress("UvCable");
                    swModelLevel2.ChangeDim("ToRight@SketchUvCable", middleToRight);
                    swModelLevel2.ChangeDim("UvCable@SketchUvCable", 1200d);
                    break;
                case UvLightType_e.UVR4S:
                case UvLightType_e.UVR6S:
                case UvLightType_e.UVR8S:
                    swCompLevel2.UnSuppress("FcCable");
                    swCompLevel2.UnSuppress("UvSensorSupport");
                    swCompLevel2.UnSuppress("UvRack");
                    swModelLevel2.ChangeDim("ToRight@SketchUvRack", middleToRight);
                    swModelLevel2.ChangeDim("UvRack@SketchUvRack", 893d);
                    swCompLevel2.UnSuppress("UvCable");
                    swModelLevel2.ChangeDim("ToRight@SketchUvCable", middleToRight);
                    swModelLevel2.ChangeDim("UvCable@SketchUvCable", 600d);
                    break;
                default:
                    swCompLevel2.Suppress("FcCable");
                    swCompLevel2.Suppress("UvSensorSupport");
                    swCompLevel2.Suppress("UvRack");
                    break;
            }
        }

        #endregion

        #region ANSUL
        if (ansul)
        {
            switch (ansulSide)
            {
                //侧喷
                case AnsulSide_e.左侧喷:
                    swCompLevel2.UnSuppress("AnsulSideLeft");
                    swCompLevel2.Suppress("AnsulSideRight");
                    break;
                case AnsulSide_e.右侧喷:
                    swCompLevel2.UnSuppress("AnsulSideRight");
                    swCompLevel2.Suppress("AnsulSideLeft");
                    break;
                case AnsulSide_e.NA:
                case AnsulSide_e.无侧喷:
                default:
                    swCompLevel2.Suppress("AnsulSideRight");
                    swCompLevel2.Suppress("AnsulSideLeft");
                    break;
            }

            #region Ansul探测器入口
            //探测器
            switch (ansulDetector)
            {
                case AnsulDetector_e.左探测器口:
                    swCompLevel2.Suppress("AnsulDetectorRight");
                    swCompLevel2.UnSuppress("AnsulDetectorLeft");
                    break;
                case AnsulDetector_e.右探测器口:
                    swCompLevel2.UnSuppress("AnsulDetectorRight");
                    swCompLevel2.Suppress("AnsulDetectorLeft");
                    break;
                case AnsulDetector_e.两探测器口:
                    swCompLevel2.UnSuppress("AnsulDetectorRight");
                    swCompLevel2.UnSuppress("AnsulDetectorLeft");
                    break;
                case AnsulDetector_e.NA:
                case AnsulDetector_e.无探测器口:
                default:
                    swCompLevel2.Suppress("AnsulDetectorRight");
                    swCompLevel2.Suppress("AnsulDetectorLeft");
                    break;
            }
            #endregion
        }
        else
        {
            swCompLevel2.Suppress("AnsulSideRight");
            swCompLevel2.Suppress("AnsulSideLeft");
            swCompLevel2.Suppress("AnsulDetectorRight");
            swCompLevel2.Suppress("AnsulDetectorLeft");
        }

        #endregion

        #region 日本项目
        if (japan)
        {
            swCompLevel2.Suppress("LiftingHoles");
            swCompLevel2.Suppress("OneSpigot");
        }
        else
        {
            swCompLevel2.UnSuppress("LiftingHoles");
            swCompLevel2.UnSuppress("OneSpigot");
            swModelLevel2.ChangeDim("ToRight@SketchOneSpigot", middleToRight);
            swModelLevel2.ChangeDim("Length@SketchOneSpigot", exhaustSpigotLength);
            swModelLevel2.ChangeDim("Width@SketchOneSpigot", exhaustSpigotWidth);
        }
        #endregion
    }

    private void FNCE0031(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, int baffleHookingNumber, double baffleHookingDis, CeilingLightType_e ceilingLightType, bool japan, FilterSide_e filterSide, int filterNumber, double filterLeft, double filterRight)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", length);

        #region 水洗挂管挂钩
        swModelLevel2.ChangeDim("Number@LPatternBaffleHooking", baffleHookingNumber);
        swModelLevel2.ChangeDim("Dis@LPatternBaffleHooking", baffleHookingDis);
        #endregion

        #region 灯具
        //todo:核实HCL中的这个LightT特征是什么？
        if (ceilingLightType is CeilingLightType_e.日光灯)
            swCompLevel2.UnSuppress("LightT");
        else
            swCompLevel2.Suppress("LightT");
        #endregion

        #region 日本项目
        //日本灯射钉，日本项目FC油网不需要导轨，而是打射钉
        if (japan)
        {
            swCompLevel2.Suppress("LightT");
            swCompLevel2.UnSuppress("JapanLight");
            //不要导轨
            swCompLevel2.Suppress("FcRail");
            //要射钉
            switch (filterSide)
            {
                case FilterSide_e.左过滤器侧板:
                    swCompLevel2.UnSuppress("FcLeft");
                    swModelLevel2.ChangeDim("Dis@SketchFcLeft", filterLeft-50d);
                    swCompLevel2.Suppress("FcRight");
                    break;
                case FilterSide_e.右过滤器侧板:
                    swCompLevel2.Suppress("FcLeft");
                    swCompLevel2.UnSuppress("FcRight");
                    swModelLevel2.ChangeDim("Dis@SketchFcRight", filterRight-50d);
                    break;
                case FilterSide_e.两过滤器侧板:
                    swCompLevel2.UnSuppress("FcLeft");
                    swModelLevel2.ChangeDim("Dis@SketchFcLeft", filterLeft-50d);
                    swCompLevel2.UnSuppress("FcRight");
                    swModelLevel2.ChangeDim("Dis@SketchFcRight", filterRight-50d);
                    break;
                case FilterSide_e.无过滤器侧板:
                case FilterSide_e.NA:
                default:
                    swCompLevel2.Suppress("FcLeft");
                    swCompLevel2.Suppress("FcRight");
                    break;
            }
            swCompLevel2.UnSuppress("FcFirst");
            swModelLevel2.ChangeDim("Dis@SketchFcFirst", filterLeft+25d);
            if (filterNumber > 1)
            {
                swCompLevel2.Suppress("LPatternFc");
                swModelLevel2.ChangeDim("Number@LPatternFc", filterNumber);
            }
            else
            {
                swCompLevel2.Suppress("LPatternFc");
            }
        }
        else
        {
            swCompLevel2.Suppress("JapanLight");
            //需要导轨
            swCompLevel2.UnSuppress("FcRail");
            //不需要射钉
            swCompLevel2.Suppress("FcLeft");
            swCompLevel2.Suppress("FcRight");
            swCompLevel2.Suppress("FcFirst");
            swCompLevel2.Suppress("LPatternFc");
        }
        #endregion

    }

    private void FNCE0032(Component2 swCompLevel2, SidePanel_e sidePanel, double length, UvLightType_e uvLightType, CeilingLightType_e ceilingLightType, HclSide_e hclSide, double hclLeft, double hclRight, bool marvel, int exhaustSpigotNumber, double middleToRight, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, bool ansul, AnsulSide_e ansulSide, int baffleSensorNumber, double baffleSensorDis1, double baffleSensorDis2, bool japan)
    {
        var swModelLevel2 = (ModelDoc2)swCompLevel2.GetModelDoc2();
        swModelLevel2.ChangeDim("Length@Base-Flange", length);
        #region 水洗挡板侧板切除
        switch (sidePanel)
        {
            case SidePanel_e.左:
                swCompLevel2.UnSuppress("CutLeft");
                swCompLevel2.Suppress("CutRight");
                break;
            case SidePanel_e.右:
                swCompLevel2.Suppress("CutLeft");
                swCompLevel2.UnSuppress("CutRight");
                break;
            case SidePanel_e.双:
                swCompLevel2.UnSuppress("CutLeft");
                swCompLevel2.UnSuppress("CutRight");
                break;
            case SidePanel_e.NA:
            case SidePanel_e.中:
            default:
                swCompLevel2.Suppress("CutLeft");
                swCompLevel2.Suppress("CutRight");
                break;
        }
        #endregion

        #region UV灯支架
        switch (uvLightType)
        {
            case UvLightType_e.UVR4L:
            case UvLightType_e.UVR6L:
            case UvLightType_e.UVR8L:
                swCompLevel2.UnSuppress("UvTab");
                swCompLevel2.UnSuppress("UvRack");
                swModelLevel2.ChangeDim("ToRight@SketchUvRack", middleToRight);
                swModelLevel2.ChangeDim("UvRack@SketchUvRack", 1600d);
                swModelLevel2.ChangeDim("UvCable@SketchUvRack", 1800d);
                break;
            case UvLightType_e.UVR4S:
            case UvLightType_e.UVR6S:
            case UvLightType_e.UVR8S:
                swCompLevel2.UnSuppress("UvTab");
                swCompLevel2.UnSuppress("UvRack");
                swModelLevel2.ChangeDim("ToRight@SketchUvRack", middleToRight);
                swModelLevel2.ChangeDim("UvRack@SketchUvRack", 893d);
                swModelLevel2.ChangeDim("UvCable@SketchUvRack", 1200d);
                break;
            case UvLightType_e.NA:
            default:
                swCompLevel2.Suppress("UvTab");
                swCompLevel2.Suppress("UvRack");
                break;
        }
        //水洗挡板磁感应孔
        if (uvLightType is UvLightType_e.NA)
        {
            swCompLevel2.Suppress("BaffleSensor");
            swCompLevel2.Suppress("BaffleSensorCable");
            swCompLevel2.Suppress("LPatternBaffleSensor");
        }
        else
        {
            swCompLevel2.UnSuppress("BaffleSensor");
            swCompLevel2.UnSuppress("BaffleSensorCable");
            swModelLevel2.ChangeDim("Dis@SketchBaffleSensor", baffleSensorDis1);
            swModelLevel2.ChangeDim("Dis@SketchBaffleSensorCable", baffleSensorDis1);
            if (baffleSensorNumber > 1)
            {
                swCompLevel2.UnSuppress("LPatternBaffleSensor");
                swModelLevel2.ChangeDim("Number@LPatternBaffleSensor", baffleSensorNumber);
                swModelLevel2.ChangeDim("Dis@LPatternBaffleSensor", baffleSensorDis2);
            }
            else
            {
                swCompLevel2.Suppress("LPatternBaffleSensor");
            }
        }
        #endregion

        #region Marvel
        if (marvel)
        {
            swCompLevel2.UnSuppress("MarvelNtc");
            if (exhaustSpigotNumber == 1) swModelLevel2.ChangeDim("ToRight@SketchMarvelNtc", middleToRight + exhaustSpigotLength / 2d + 50d);
            else swModelLevel2.ChangeDim("ToRight@SketchMarvelNtc", middleToRight + exhaustSpigotDis / 2d + exhaustSpigotLength + 50d);

            swCompLevel2.UnSuppress("MarvelTab");
        }
        else
        {
            swCompLevel2.Suppress("MarvelNtc");
            swCompLevel2.Suppress("MarvelTab");
        }
        #endregion

        #region ANSUL
        if (ansul)
        {
            switch (ansulSide)
            {
                //侧喷
                case AnsulSide_e.左侧喷:
                    swCompLevel2.UnSuppress("AnsulSideLeft");
                    swCompLevel2.Suppress("AnsulSideRight");
                    break;
                case AnsulSide_e.右侧喷:
                    swCompLevel2.UnSuppress("AnsulSideRight");
                    swCompLevel2.Suppress("AnsulSideLeft");
                    break;
                case AnsulSide_e.NA:
                case AnsulSide_e.无侧喷:
                default:
                    swCompLevel2.Suppress("AnsulSideRight");
                    swCompLevel2.Suppress("AnsulSideLeft");
                    break;
            }
        }
        else
        {
            swCompLevel2.Suppress("AnsulSideRight");
            swCompLevel2.Suppress("AnsulSideLeft");
        }

        #endregion

        #region 日本项目
        if (japan)
        {
            swCompLevel2.Suppress("LiftingHoles");
            swCompLevel2.Suppress("OneSpigot");
        }
        else
        {
            swCompLevel2.UnSuppress("LiftingHoles");
            swCompLevel2.UnSuppress("OneSpigot");
            swModelLevel2.ChangeDim("ToRight@SketchOneSpigot", middleToRight);
            swModelLevel2.ChangeDim("Length@SketchOneSpigot", exhaustSpigotLength);
            swModelLevel2.ChangeDim("Width@SketchOneSpigot", exhaustSpigotWidth);
        }
        #endregion

        #region HCL侧板固定镀锌铁片铆钉孔
        if (ceilingLightType is CeilingLightType_e.HCL)
        {
            if (hclSide is HclSide_e.左HCL侧板 or HclSide_e.两HCL侧板)
            {
                swCompLevel2.UnSuppress("HclLeft");
                swModelLevel2.ChangeDim("Dis@SketchHclLeft", hclLeft-150d);
            }
            else
            {
                swCompLevel2.Suppress("HclLeft");
            }
            if (hclSide is HclSide_e.右HCL侧板 or HclSide_e.两HCL侧板)
            {
                swCompLevel2.UnSuppress("HclRight");
                swModelLevel2.ChangeDim("Dis@SketchHclRight", hclRight-150d);
            }
            else
            {
                swCompLevel2.Suppress("HclRight");
            }
        }
        #endregion
    }

    private void FNCE0012(Component2 swCompLevel2, SidePanel_e sidePanel, double length, bool marvel, int exhaustSpigotNumber, double middleToRight, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, bool ansul, AnsulSide_e ansulSide, CeilingWaterInlet_e ceilingWaterInlet, bool japan)
    {
        var swModelLevel2 = (ModelDoc2)swCompLevel2.GetModelDoc2();
        swModelLevel2.ChangeDim("Length@Base-Flange", length);

        #region 水洗挡板侧板切除
        switch (sidePanel)
        {
            case SidePanel_e.左:
                swCompLevel2.UnSuppress("CutLeft");
                swCompLevel2.Suppress("CutRight");
                break;
            case SidePanel_e.右:
                swCompLevel2.Suppress("CutLeft");
                swCompLevel2.UnSuppress("CutRight");
                break;
            case SidePanel_e.双:
                swCompLevel2.UnSuppress("CutLeft");
                swCompLevel2.UnSuppress("CutRight");
                break;
            case SidePanel_e.NA:
            case SidePanel_e.中:
            default:
                swCompLevel2.Suppress("CutLeft");
                swCompLevel2.Suppress("CutRight");
                break;
        }
        #endregion

        #region 入水口开孔
        if (ceilingWaterInlet is CeilingWaterInlet_e.上入水管)
        {
            swCompLevel2.Suppress("PipeFront");
            swCompLevel2.UnSuppress("PipeUp");
        }
        else
        {
            swCompLevel2.Suppress("PipeUp");
            swCompLevel2.UnSuppress("PipeFront");
        }
        #endregion

        #region Marvel
        if (marvel)
        {
            swCompLevel2.UnSuppress("MarvelNtc");
            if (exhaustSpigotNumber == 1) swModelLevel2.ChangeDim("ToRight@SketchMarvelNtc", middleToRight + exhaustSpigotLength / 2d + 50d);
            else swModelLevel2.ChangeDim("ToRight@SketchMarvelNtc", middleToRight + exhaustSpigotDis / 2d + exhaustSpigotLength + 50d);

            swCompLevel2.UnSuppress("MarvelTab");
        }
        else
        {
            swCompLevel2.Suppress("MarvelNtc");
            swCompLevel2.Suppress("MarvelTab");
        }
        #endregion

        #region ANSUL
        if (ansul)
        {
            switch (ansulSide)
            {
                //侧喷
                case AnsulSide_e.左侧喷:
                    swCompLevel2.UnSuppress("AnsulSideLeft");
                    swCompLevel2.Suppress("AnsulSideRight");
                    break;
                case AnsulSide_e.右侧喷:
                    swCompLevel2.UnSuppress("AnsulSideRight");
                    swCompLevel2.Suppress("AnsulSideLeft");
                    break;
                case AnsulSide_e.NA:
                case AnsulSide_e.无侧喷:
                default:
                    swCompLevel2.Suppress("AnsulSideRight");
                    swCompLevel2.Suppress("AnsulSideLeft");
                    break;
            }
        }
        else
        {
            swCompLevel2.Suppress("AnsulSideRight");
            swCompLevel2.Suppress("AnsulSideLeft");
        }

        #endregion

        #region 日本项目
        if (japan)
        {
            swCompLevel2.Suppress("LiftingHoles");
            swCompLevel2.Suppress("OneSpigot");
        }
        else
        {
            swCompLevel2.UnSuppress("LiftingHoles");
            swCompLevel2.UnSuppress("OneSpigot");
            swModelLevel2.ChangeDim("ToRight@SketchOneSpigot", middleToRight);
            swModelLevel2.ChangeDim("Length@SketchOneSpigot", exhaustSpigotLength);
            swModelLevel2.ChangeDim("Width@SketchOneSpigot", exhaustSpigotWidth);
        }
        #endregion
    }

    private void FNCE0021(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, int baffleHookingNumber, double baffleHookingDis, bool japan, FilterSide_e filterSide, int filterNumber, double filterLeft, double filterRight)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", length-5d);

        #region 水洗挂管挂钩
        swModelLevel2.ChangeDim("Number@LPatternBaffleHooking", baffleHookingNumber);
        swModelLevel2.ChangeDim("Dis@LPatternBaffleHooking", baffleHookingDis);
        #endregion
        
        #region 日本项目
        //日本灯射钉，日本项目FC油网不需要导轨，而是打射钉
        if (japan)
        {
            //不要导轨
            swCompLevel2.Suppress("FcRail");
            //要射钉
            switch (filterSide)
            {
                case FilterSide_e.左过滤器侧板:
                    swCompLevel2.UnSuppress("FcLeft");
                    swModelLevel2.ChangeDim("Dis@SketchFcLeft", filterLeft-50d);
                    swCompLevel2.Suppress("FcRight");
                    break;
                case FilterSide_e.右过滤器侧板:
                    swCompLevel2.Suppress("FcLeft");
                    swCompLevel2.UnSuppress("FcRight");
                    swModelLevel2.ChangeDim("Dis@SketchFcRight", filterRight-50d);
                    break;
                case FilterSide_e.两过滤器侧板:
                    swCompLevel2.UnSuppress("FcLeft");
                    swModelLevel2.ChangeDim("Dis@SketchFcLeft", filterLeft-50d);
                    swCompLevel2.UnSuppress("FcRight");
                    swModelLevel2.ChangeDim("Dis@SketchFcRight", filterRight-50d);
                    break;
                case FilterSide_e.无过滤器侧板:
                case FilterSide_e.NA:
                default:
                    swCompLevel2.Suppress("FcLeft");
                    swCompLevel2.Suppress("FcRight");
                    break;
            }
            swCompLevel2.UnSuppress("FcFirst");
            swModelLevel2.ChangeDim("Dis@SketchFcFirst", filterLeft+25d);
            if (filterNumber > 1)
            {
                swCompLevel2.Suppress("LPatternFc");
                swModelLevel2.ChangeDim("Number@LPatternFc", filterNumber);
            }
            else
            {
                swCompLevel2.Suppress("LPatternFc");
            }
        }
        else
        {
            //需要导轨
            swCompLevel2.UnSuppress("FcRail");
            //不需要射钉
            swCompLevel2.Suppress("FcLeft");
            swCompLevel2.Suppress("FcRight");
            swCompLevel2.Suppress("FcFirst");
            swCompLevel2.Suppress("LPatternFc");
        }
        #endregion
    }

    #endregion


}