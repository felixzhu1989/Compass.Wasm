using Compass.Wasm.Shared.Data;
using Compass.Wasm.Shared.Data.Ceilings;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.SwServices;

public class CeilingService : BaseSwService, ICeilingService
{
    public readonly IExhaustService ExhaustService;
    public CeilingService(IContainerProvider provider) : base(provider)
    {
        ExhaustService = provider.Resolve<IExhaustService>();
    }
    #region 公共方法
    //重命名公共方法
    private Component2? RenameComp(ModelDoc2 swModel, AssemblyDoc swAssy, string suffix, string type, string module, string compName, int num, double length, double width)
    {
        var assyName = swModel.GetTitle().Substring(0, swModel.GetTitle().Length - 7);
        var originPath = $"{$"{compName}-{num}".AddSuffix(suffix)}@{assyName}";
        var strRename = $"{compName}[{type}-{module}]{{{(int)length}}}({(int)width})";
        var status = swModel.Extension.SelectByID2(originPath, "COMPONENT", 0, 0, 0, false, 0, null, 0);
        if (status)
        {
            swAssy.UnSuppress(suffix, $"{compName}-{num}", Aggregator);
            swModel.Extension.SelectByID2(originPath, "COMPONENT", 0, 0, 0, false, 0, null, 0);
            swModel.Extension.RenameDocument(strRename);//执行重命名
        }
        swModel.ClearSelection2(true);
        status = swModel.Extension.SelectByID2($"{strRename}-{num}@{assyName}", "COMPONENT", 0, 0, 0, false,
            0, null, 0);
        swModel.ClearSelection2(true);
        return status ? swAssy.GetComponentByName($"{strRename}-{num}") : null;
    }
    //检查是否存在，再压缩
    private void SuppressIfExist(ModelDoc2 swModel, AssemblyDoc swAssy, string suffix, string compName, int num)
    {
        var assyName = swModel.GetTitle().Substring(0, swModel.GetTitle().Length - 7);
        var originPath = $"{$"{compName}-{num}".AddSuffix(suffix)}@{assyName}";
        var status = swModel.Extension.SelectByID2(originPath, "COMPONENT", 0, 0, 0, false, 0, null, 0);
        if (status) { swAssy.Suppress(suffix, $"{compName}-{num}"); }
    } 
    #endregion



    public void KcjDb800(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, KcjData data)
    {
        //计算过滤器数量
        var filterNumber = (int)((data.Length - data.FilterLeft - data.FilterRight) / 499d) - data.FilterBlindNumber;
        //公共零件
        //重命名排风腔体
        var swCompLevel2 = RenameComp(swModelTop, swAssyTop, suffix, "KCJDB800", module, "FNCE0115", 1, data.Length, data.Width);
        if (swCompLevel2 != null)
        {
            FNCE0115(swCompLevel2, data.Length, data.CeilingLightType, data.LightCable, data.Marvel, data.ExhaustSpigotNumber, data.MiddleToRight, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotDis, data.Ansul, data.AnsulSide, data.AnsulDetectorNumber, data.AnsulDetectorEnd, data.AnsulDetectorDis1, data.AnsulDetectorDis2, data.AnsulDetectorDis3, data.AnsulDetectorDis4, data.AnsulDetectorDis5, data.Japan);
        }
        //过滤器盲板
        FilterBlind(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, data.FilterLeft, "FNCE0107[BP-500]{500}-1", "LocalLPatternBlind", "Dis@DistanceBlind");
        FilterBlind(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, data.FilterLeft, "FNCE0107[BP-500]{500}-5", "LocalLPatternBlind", "Dis@DistanceBlind");
        //过滤器
        Filter(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, filterNumber, data.FilterLeft, data.FilterType, "KCJ FC FILTER-1", "LocalLPatternFc", "Dis@DistanceFc");
        Filter(swModelTop, swAssyTop, suffix, data.FilterBlindNumber, filterNumber, data.FilterLeft, data.FilterType, "KCJ FC FILTER-7", "LocalLPatternFc", "Dis@DistanceFc");

        //过滤器侧板
        FilterSide(swModelTop, swAssyTop, suffix, module, data.FilterSide, data.FilterType, filterNumber, data.FilterLeft, data.FilterRight, "FNCE0108", 1, "FNCE0109", 1);
        FilterSide(swModelTop, swAssyTop, suffix, module, data.FilterSide, data.FilterType, filterNumber, data.FilterLeft, data.FilterRight, "FNCE0108", 2, "FNCE0109", 2);

        //SSP灯板支撑条
        SspSupport(swModelTop, swAssyTop, suffix, data.Length, data.DomeSsp, data.Gutter, data.GutterWidth, "FNCE0035-1", "Dis@DistanceDome1", "FNCE0036-1", "Dis@DistanceFlat1");
        SspSupport(swModelTop, swAssyTop, suffix, data.Length, data.DomeSsp, data.Gutter, data.GutterWidth, "FNCE0035-2", "Dis@DistanceDome2", "FNCE0036-2", "Dis@DistanceFlat2");

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
            ExhaustService.ExhaustSpigotFs(swAssyLevel1, suffix, data.Length, data.MiddleToRight, data.ExhaustSpigotNumber, data.ExhaustSpigotLength, data.ExhaustSpigotWidth, data.ExhaustSpigotHeight, data.ExhaustSpigotDis, data.Marvel, data.Ansul, ExhaustType_e.KV);
        }

        //HCL
        if (data.CeilingLightType is CeilingLightType_e.HCL)
        {
            swAssyTop.Suppress(suffix, "NormalLight_KCJ_DB_800-1");
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(out var swModelLevel1, suffix, "HclLight_KCJ_DB_800-1", Aggregator);

            HclLight(swModelLevel1, swAssyLevel1, suffix, module, data.Length, UvLightType_e.NA, data.LightCable, data.CeilingLightType, data.Japan, data.HclSide, data.HclLeft, data.HclRight);


        }
        else
        {
            swAssyTop.Suppress(suffix, "HclLight_KCJ_DB_800-1");
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "NormalLight_KCJ_DB_800-1", Aggregator);

            NormalLight(swAssyLevel1, suffix, data.Length, UvLightType_e.NA, data.LightCable, data.CeilingLightType, data.Japan);
        }
    }





    #region KCJDB800
    private void FNCE0115(Component2 swCompLevel2, double length, CeilingLightType_e ceilingLightType, LightCable_e lightCable, bool marvel, int exhaustSpigotNumber, double middleToRight, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, bool ansul, AnsulSide_e ansulSide, int ansulDetectorNumber, AnsulDetectorEnd_e ansulDetectorEnd, double ansulDetectorDis1, double ansulDetectorDis2, double ansulDetectorDis3, double ansulDetectorDis4, double ansulDetectorDis5, bool japan)
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

            #region Ansul探测器，水洗烟罩需要探测器安装在MidRoof上
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


    private void Filter(ModelDoc2 swModelLevel1, AssemblyDoc swAssyLevel1, string suffix, int filterBlindNumber, int filterNumber, double filterLeft, FilterType_e filterType, string fcPart, string fcPattern, string fcDis)
    {
        if (filterType is FilterType_e.KSA)
        {
            swAssyLevel1.Suppress(suffix, fcPart);
            swAssyLevel1.Suppress(fcPattern);
        }
        else
        {
            swAssyLevel1.UnSuppress(suffix, fcPart, Aggregator);
            swAssyLevel1.UnSuppress(fcPattern);
            swModelLevel1.ChangeDim(fcDis, filterLeft+filterBlindNumber*500);
            swModelLevel1.ChangeDim($"Number@{fcPattern}", filterNumber);
        }
    }

    private void FilterSide(ModelDoc2 swModelLevel1, AssemblyDoc swAssyLevel1, string suffix, string module, FilterSide_e filterSide, FilterType_e filterType, int filterNumber, double filterLeft, double filterRight, string leftPart, int leftNum, string rightPart, int rightNum)
    {
        switch (filterSide)
        {
            case FilterSide_e.左油网:
                {
                    var leftLength = (int)(filterLeft - filterNumber);
                    if (filterType is FilterType_e.KSA)
                        leftLength = (int)(filterLeft+filterNumber*2d);

                    var swCompLeft = RenameComp(swModelLevel1, swAssyLevel1, suffix, "BP", module, leftPart, leftNum, leftLength, 250);
                    if (swCompLeft != null) ChangeSideLength(swCompLeft, leftLength);
                    SuppressIfExist(swModelLevel1, swAssyLevel1, suffix, rightPart, rightNum);
                    break;
                }
            case FilterSide_e.右油网:
                {
                    var rightLength = (int)(filterRight - filterNumber);
                    if (filterType is FilterType_e.KSA)
                        rightLength = (int)(filterRight+filterNumber*2d);

                    var swCompRight = RenameComp(swModelLevel1, swAssyLevel1, suffix, "BP", module, rightPart, rightNum, rightLength, 250);
                    if (swCompRight != null) ChangeSideLength(swCompRight, rightLength);
                    SuppressIfExist(swModelLevel1, swAssyLevel1, suffix, leftPart, leftNum);

                    break;
                }
            case FilterSide_e.两油网:
                {
                    var leftLength = (int)(filterLeft - filterNumber/2d);
                    if (filterType is FilterType_e.KSA)
                        leftLength = (int)(filterLeft+filterNumber*1.5d);

                    var swCompLeft = RenameComp(swModelLevel1, swAssyLevel1, suffix, "BP", $"{module}.1", leftPart, leftNum, leftLength, 250);
                    if (swCompLeft != null) ChangeSideLength(swCompLeft, leftLength);

                    var rightLength = (int)(filterRight - filterNumber/2d);
                    if (filterType is FilterType_e.KSA)
                        rightLength = (int)(filterRight+filterNumber*1.5d);

                    var swCompRight = RenameComp(swModelLevel1, swAssyLevel1, suffix, "BP", $"{module}.2", rightPart, rightNum, rightLength, 250);
                    if (swCompRight != null) ChangeSideLength(swCompRight, rightLength);
                    break;
                }
            case FilterSide_e.NA:
            case FilterSide_e.无油网:
            default:
                SuppressIfExist(swModelLevel1, swAssyLevel1, suffix, leftPart, leftNum);
                SuppressIfExist(swModelLevel1, swAssyLevel1, suffix, rightPart, rightNum);
                break;
        }
        void ChangeSideLength(Component2 swComp, double sideLength)
        {
            var swModel = (ModelDoc2)swComp.GetModelDoc2();
            swModel.ChangeDim("Length@SketchBase", sideLength);
        }
    }


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


    private void NormalLight(AssemblyDoc swAssyLevel1, string suffix, double length, UvLightType_e uvLightType, LightCable_e lightCable, CeilingLightType_e ceilingLightType, bool japan)
    {
        //灯腔
        FNCE0116(swAssyLevel1, suffix, "FNCE0116-1", length, uvLightType, lightCable, ceilingLightType, japan);
        //玻璃支架
        FNCE0056(swAssyLevel1, suffix, "FNCE0056-1", length);
    }


    private void HclLight(ModelDoc2 swModelLevel1, AssemblyDoc swAssyLevel1, string suffix, string module, double length, UvLightType_e uvLightType, LightCable_e lightCable, CeilingLightType_e ceilingLightType, bool japan, HclSide_e hclSide, double hclLeft, double hclRight)
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

        //支撑条
        FNCE0099(swAssyLevel1, suffix, "FNCE0099-1", length, hclSide, hclLeft, hclRight);
        FNCE0099(swAssyLevel1, suffix, "FNCE0090-1", length, hclSide, hclLeft, hclRight);
        //支撑条上部
        FNCE0091(swAssyLevel1, suffix, "FNCE0091-1", length, hclSide, hclLeft, hclRight);

        HclSidePanel(swModelLevel1, swAssyLevel1, suffix, module, hclSide, hclLeft, hclRight, "FNCE0092", 1, "FNCE0094", 1);
    }

    private void HclSidePanel(ModelDoc2 swModelLevel1, AssemblyDoc swAssyLevel1, string suffix, string module, HclSide_e hclSide, double hclLeft, double hclRight, string leftPart, int leftNum, string rightPart, int rightNum)
    {
        switch (hclSide)
        {
            case HclSide_e.左HCL侧板:
                {
                    var swCompLeft = RenameComp(swModelLevel1, swAssyLevel1, suffix, "HCLSP", module, leftPart, leftNum,
                        hclLeft, 200d);
                    if (swCompLeft != null) ChangeSideLength(swCompLeft, hclLeft);
                    SuppressIfExist(swModelLevel1, swAssyLevel1, suffix, rightPart, rightNum);
                    break;
                }

            case HclSide_e.右HCL侧板:
                {
                    var swCompRight = RenameComp(swModelLevel1, swAssyLevel1, suffix, "HCLSP", module, rightPart, rightNum,
                        hclRight, 200d);
                    if (swCompRight != null) ChangeSideLength(swCompRight, hclRight);
                    SuppressIfExist(swModelLevel1, swAssyLevel1, suffix, leftPart, leftNum);

                    break;
                }
            case HclSide_e.两HCL侧板:
                {
                    var swCompLeft = RenameComp(swModelLevel1, swAssyLevel1, suffix, "HCLSP", $"{module}.1", leftPart, leftNum,
                        hclLeft, 200d);
                    if (swCompLeft != null) ChangeSideLength(swCompLeft, hclLeft);

                    var swCompRight = RenameComp(swModelLevel1, swAssyLevel1, suffix, "HCLSP", $"{module}.2", rightPart, rightNum,
                        hclRight, 200d);
                    if (swCompRight != null) ChangeSideLength(swCompRight, hclRight);
                    break;
                }
            default:
            case HclSide_e.NA:
            case HclSide_e.无HCL侧板:
                SuppressIfExist(swModelLevel1, swAssyLevel1, suffix, leftPart, leftNum);
                SuppressIfExist(swModelLevel1, swAssyLevel1, suffix, rightPart, rightNum);
                break;
        }

        void ChangeSideLength(Component2 swComp, double sideLength)
        {
            var swModel = (ModelDoc2)swComp.GetModelDoc2();
            swModel.ChangeDim("Length@SketchBase", sideLength);
            swModel.ChangeDim("Dis@SketchMagnet", sideLength-125d);
        }
    }

    private void FNCE0116(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, UvLightType_e uvLightType, LightCable_e lightCable, CeilingLightType_e ceilingLightType, bool japan)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", length);

        #region 过滤器定位孔
        if (uvLightType is UvLightType_e.NA)
        {
            swCompLevel2.Suppress("FcSupport");
            swCompLevel2.Suppress("FcSupportBack");
        }
        else
        {
            swCompLevel2.UnSuppress("FcSupport");
            swCompLevel2.UnSuppress("FcSupportBack");
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

        if (ceilingLightType is CeilingLightType_e.日光灯)
            swCompLevel2.UnSuppress("LightT");
        else
            swCompLevel2.Suppress("LightT");

        if (japan)
            swCompLevel2.UnSuppress("JapanLight");
        else
            swCompLevel2.Suppress("JapanLight");
    }

    private void FNCE0056(AssemblyDoc swAssyLevel1, string suffix, string partName, double length)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length);
    }

    private void FNCE0099(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, HclSide_e hclSide, double hclLeft, double hclRight)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length);

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
    #endregion
}