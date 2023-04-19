using Compass.Wpf.Extensions;
using Prism.Ioc;
using SolidWorks.Interop.sldworks;
using System;
using Compass.Wasm.Shared.Data;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Compass.Wpf.DrawingServices;

public class ExhaustService : BaseDrawingService, IExhaustService
{
    public ExhaustService(IContainerProvider provider) : base(provider)
    {
    }

    public void Kv555(AssemblyDoc swAssyTop, string suffix, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Exhaust_KV_555-1", Aggregator);
        Exhaust555(swAssyLevel1, suffix, length, sidePanel, uvLightType, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotDis, drainType, waterCollection, backToBack, marvel, ansul, ansulSide, ansulDetector);
    }

    public void Uv555(AssemblyDoc swAssyTop, string suffix, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Exhaust_UV_555-1", Aggregator);
        Exhaust555(swAssyLevel1, suffix, length, sidePanel, uvLightType, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotDis, drainType, waterCollection, backToBack, marvel, ansul, ansulSide, ansulDetector);
        UvExhaust555(swAssyLevel1, suffix, length, uvLightType, middleToRight, ansul, ansulSide, ansulDetector);
    }


    public void Kw555(AssemblyDoc swAssyTop, string suffix, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, WaterInlet_e waterInlet)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Exhaust_KW_555-1", Aggregator);
        KwExhaust555(swAssyLevel1, suffix, length, sidePanel, uvLightType, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotDis, drainType, waterCollection, backToBack, marvel, ansul, ansulSide, waterInlet);
    }


    public void ExhaustSpigotFs(AssemblyDoc swAssyTop, string suffix, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, bool marvel, bool ansul,ExhaustType_e exhaustType)
    {
        //当有marvel或者脖颈高度100，且没有ansul时，脖颈宽度为300,无需脖颈
        if ((marvel|| exhaustSpigotHeight.Equals(100d))&& !ansul&&exhaustSpigotWidth.Equals(300d))
        {
            //压缩脖颈子装配，压缩脖颈阵列
            swAssyTop.Suppress(suffix, "ExhaustSpigot_Fs-1");
            swAssyTop.Suppress("LocalLPatternExhaustSpigot");
            return;
        }
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "ExhaustSpigot_Fs-1", Aggregator);
        //进入ExhaustSpigot_Fs，子装配
        //todo:有待细化逻辑
        //前
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, "FNHE0006-1", Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", exhaustSpigotLength + 50d);
        swModelLevel2.ChangeDim("Height@SketchBase", exhaustSpigotHeight);
        if (ansul) swCompLevel2.UnSuppress("Ansul");
        else swCompLevel2.Suppress("Ansul");

        //后
        swAssyLevel1.UnSuppress(out swModelLevel2, suffix, "FNHE0007-1", Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", exhaustSpigotLength + 50d);
        swModelLevel2.ChangeDim("Height@SketchBase", exhaustSpigotHeight);

        //左，todo:有待细化Ansul探测器逻辑，水洗烟罩需要AnsulDetector
        swCompLevel2=swAssyLevel1.UnSuppress(out swModelLevel2, suffix, "FNHE0008-1", Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", exhaustSpigotWidth);
        swModelLevel2.ChangeDim("Height@SketchBase", exhaustSpigotHeight);
        if (ansul && (exhaustType is ExhaustType_e.KW or ExhaustType_e.UW or ExhaustType_e.CMOD))
        {
            swCompLevel2.UnSuppress("AnsulDetector");
        }
        else
        {
            swCompLevel2.Suppress("AnsulDetector");
        }

        //右,
        swCompLevel2=swAssyLevel1.UnSuppress(out swModelLevel2, suffix, "FNHE0009-1", Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", exhaustSpigotWidth);
        swModelLevel2.ChangeDim("Height@SketchBase", exhaustSpigotHeight);
        if (ansul && (exhaustType is ExhaustType_e.KW or ExhaustType_e.UW or ExhaustType_e.CMOD))
        {
            swCompLevel2.UnSuppress("AnsulDetector");
        }
        else
        {
            swCompLevel2.Suppress("AnsulDetector");
        }

        //如果是标准宽度，则压缩滑门
        if (exhaustSpigotWidth.Equals(300d))
        {
            swAssyLevel1.Suppress(suffix, "FNCE0013-1");
            swAssyLevel1.Suppress(suffix, "FNCE0013-2");
        }
        else
        {
            swAssyLevel1.UnSuppress(out swModelLevel2, suffix, "FNCE0013-1", Aggregator);
            swModelLevel2.ChangeDim("Length@SketchBase", exhaustSpigotLength / 2d + 10d);
            swModelLevel2.ChangeDim("Width@SketchBase", exhaustSpigotWidth + 20d);
        }
        //两个排风口的情况
        if (exhaustSpigotNumber == 2)
        {
            swAssyTop.ChangeDim("ToRight@DistanceExhaustSpigot", middleToRight-(exhaustSpigotLength+exhaustSpigotDis)/2d);
            swAssyTop.UnSuppress("LocalLPatternExhaustSpigot");
            swAssyTop.ChangeDim("Dis@LocalLPatternExhaustSpigot", exhaustSpigotLength+exhaustSpigotDis);
        }
        else
        {
            swAssyTop.ChangeDim("ToRight@DistanceExhaustSpigot", middleToRight);
            swAssyTop.Suppress("LocalLPatternExhaustSpigot");
        }
    }



    #region Kv555
    private void Exhaust555(AssemblyDoc swAssyLevel1, string suffix, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector)
    {
        //排风主体
        FNHE0001(swAssyLevel1, suffix, "FNHE0001-1", length, sidePanel, uvLightType, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotDis, drainType, waterCollection, backToBack, marvel, ansul, ansulSide, ansulDetector);
        //排风腔前面板
        FNHE0002(swAssyLevel1, suffix, "FNHE0002-1", length, uvLightType, middleToRight);
        //KSA侧边
        KsaFilter(swAssyLevel1, suffix, length, "FNHE0003-1", "FNHE0004-1", "FNHE0005-1");
        //排风三角板
        ExhaustSide(swAssyLevel1, suffix, ansul, sidePanel, "5201030401-1", "5201030401-2");
        //排风滑门导轨
        ExhaustRail(swAssyLevel1, suffix, marvel, length, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotDis, "FNCE0018-1", "FNCE0018-2");
        //Ksa阵列
        Ksa(swAssyLevel1, length);
    }
    private void FNHE0001(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length);

        #region MidRoof铆螺母孔
        //2023/3/10 修改MidRoof螺丝孔逻辑，以最低450间距计算间距即可
        const double sideDis = 150d;
        const double minMidRoofNutDis = 450d;
        var midRoofNutNumber = Math.Ceiling((length - 2*sideDis) / minMidRoofNutDis);
        midRoofNutNumber = midRoofNutNumber < 3 ? 3 : midRoofNutNumber;
        var midRoofNutDis = (length - 2*sideDis)/(midRoofNutNumber-1);

        swModelLevel2.ChangeDim("Dis@LPatternMidRoofNut", midRoofNutDis);
        #endregion

        #region UVHood
        switch (uvLightType)
        {
            case UvLightType_e.UVR4L:
            case UvLightType_e.UVR6L:
            case UvLightType_e.UVR8L:
                swCompLevel2.UnSuppress("UvRack");
                swModelLevel2.ChangeDim("ToRight@SketchUvRack", middleToRight);
                swModelLevel2.ChangeDim("UvRack@SketchUvRack", 1640d);
                swCompLevel2.UnSuppress("UvCable");
                swModelLevel2.ChangeDim("ToRight@SketchUvCable", middleToRight);
                swModelLevel2.ChangeDim("UvCable@SketchUvCable", 1500d);
                break;
            case UvLightType_e.UVR4S:
            case UvLightType_e.UVR6S:
            case UvLightType_e.UVR8S:
                swCompLevel2.UnSuppress("UvRack");
                swModelLevel2.ChangeDim("ToRight@SketchUvRack", middleToRight);
                swModelLevel2.ChangeDim("UvRack@SketchUvRack", 930d);
                swCompLevel2.UnSuppress("UvCable");
                swModelLevel2.ChangeDim("ToRight@SketchUvCable", middleToRight);
                swModelLevel2.ChangeDim("UvCable@SketchUvCable", 790d);
                break;
            case UvLightType_e.NA:
            default:
                swCompLevel2.Suppress("UvRack");
                swCompLevel2.Suppress("UvCable");
                break;
        }
        #endregion

        #region 排风口
        if (exhaustSpigotNumber == 1)
        {
            swCompLevel2.UnSuppress("OneSpigot");
            swCompLevel2.Suppress("TwoSpigots");
            swModelLevel2.ChangeDim("ToRight@SketchOneSpigot", middleToRight);
            swModelLevel2.ChangeDim("Length@SketchOneSpigot", exhaustSpigotLength);
            swModelLevel2.ChangeDim("Width@SketchOneSpigot", exhaustSpigotWidth);
        }
        else
        {
            swCompLevel2.Suppress("OneSpigot");
            swCompLevel2.UnSuppress("TwoSpigots");
            swModelLevel2.ChangeDim("ToRight@SketchTwoSpigots", middleToRight);
            swModelLevel2.ChangeDim("Dis@SketchTwoSpigots", exhaustSpigotDis);
            swModelLevel2.ChangeDim("Length@SketchTwoSpigots", exhaustSpigotLength);
            swModelLevel2.ChangeDim("Width@SketchTwoSpigots", exhaustSpigotWidth);
        }

        #endregion

        #region 集水翻边
        if (waterCollection)
        {
            switch (sidePanel)
            {
                case SidePanel_e.左:
                    swCompLevel2.Suppress("DrainChannelRight");
                    swCompLevel2.UnSuppress("DrainChannelLeft");
                    break;
                case SidePanel_e.右:
                    swCompLevel2.UnSuppress("DrainChannelRight");
                    swCompLevel2.Suppress("DrainChannelLeft");
                    break;
                case SidePanel_e.双:
                    swCompLevel2.UnSuppress("DrainChannelRight");
                    swCompLevel2.UnSuppress("DrainChannelLeft");
                    break;
                case SidePanel_e.NA:
                case SidePanel_e.中:
                default:
                    swCompLevel2.Suppress("DrainChannelRight");
                    swCompLevel2.Suppress("DrainChannelLeft");
                    break;
            }
        }
        else
        {
            swCompLevel2.Suppress("DrainChannelRight");
            swCompLevel2.Suppress("DrainChannelLeft");
        }
        #endregion

        #region 油塞
        switch (drainType)
        {
            case DrainType_e.左油塞:
                swCompLevel2.UnSuppress("DrainTapLeft");
                swCompLevel2.Suppress("DrainTapRight");
                break;
            case DrainType_e.右油塞:
                swCompLevel2.Suppress("DrainTapLeft");
                swCompLevel2.UnSuppress("DrainTapRight");
                break;
            case DrainType_e.NA:
            case DrainType_e.右排水管:
            case DrainType_e.左排水管:
            case DrainType_e.上排水:
            case DrainType_e.集油槽:
            default:
                swCompLevel2.Suppress("DrainTapLeft");
                swCompLevel2.Suppress("DrainTapRight");
                break;
        }

        #endregion

        #region 背靠背
        if (backToBack) swCompLevel2.UnSuppress("BackToBack");
        else swCompLevel2.Suppress("BackToBack");

        #endregion

        #region MARVEL
        if (marvel)
        {
            swCompLevel2.UnSuppress("MarvelNtc");
            if (exhaustSpigotNumber == 1) swModelLevel2.ChangeDim("ToRight@SketchMarvelNtc", middleToRight + exhaustSpigotLength / 2d + 50d);
            else swModelLevel2.ChangeDim("ToRight@SketchMarvelNtc", middleToRight + exhaustSpigotDis / 2d + exhaustSpigotLength + 50d);
        }
        else swCompLevel2.Suppress("MarvelNtc");
        #endregion

        #region ANSUL
        switch (ansulSide)
        {
            //侧喷
            case AnsulSide_e.左侧喷:
                swCompLevel2.UnSuppress("ChannelLeft");
                swCompLevel2.UnSuppress("AnsulSideLeft");
                break;
            case AnsulSide_e.右侧喷:
                swCompLevel2.UnSuppress("ChannelRight");
                swCompLevel2.UnSuppress("AnsulSideRight");
                break;
            case AnsulSide_e.NA:
            case AnsulSide_e.无侧喷:
            default:
                swCompLevel2.Suppress("ChannelRight");
                swCompLevel2.Suppress("AnsulSideRight");
                swCompLevel2.Suppress("ChannelLeft");
                swCompLevel2.Suppress("AnsulSideLeft");
                break;
        }
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
            case AnsulDetector_e.双侧探测器口:
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

    private void FNHE0002(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, UvLightType_e uvLightType,
        double middleToRight)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length);
        #region UVHood
        switch (uvLightType)
        {
            case UvLightType_e.UVR4L:
            case UvLightType_e.UVR6L:
            case UvLightType_e.UVR8L:
                swCompLevel2.UnSuppress("TabUp");
                swCompLevel2.UnSuppress("SensorCable");
                swCompLevel2.Suppress("UvDoorShort");
                swCompLevel2.UnSuppress("UvDoorLong");
                swModelLevel2.ChangeDim("ToRight@SketchUvDoorLong", middleToRight);
                swCompLevel2.UnSuppress("UvCable");
                swModelLevel2.ChangeDim("ToRight@SketchUvCable", middleToRight);
                swModelLevel2.ChangeDim("UvCable@SketchUvCable", 1500d);
                break;
            case UvLightType_e.UVR4S:
            case UvLightType_e.UVR6S:
            case UvLightType_e.UVR8S:
                swCompLevel2.UnSuppress("TabUp");
                swCompLevel2.UnSuppress("SensorCable");
                swCompLevel2.UnSuppress("UvDoorShort");
                swCompLevel2.Suppress("UvDoorLong");
                swModelLevel2.ChangeDim("ToRight@SketchUvDoorShort", middleToRight);
                swCompLevel2.UnSuppress("UvCable");
                swModelLevel2.ChangeDim("ToRight@SketchUvCable", middleToRight);
                swModelLevel2.ChangeDim("UvCable@SketchUvCable", 790d);
                break;
            case UvLightType_e.NA:
            default:
                swCompLevel2.Suppress("TabUp");
                swCompLevel2.Suppress("SensorCable");
                swCompLevel2.Suppress("UvDoorShort");
                swCompLevel2.Suppress("UvDoorLong");
                swCompLevel2.Suppress("UvCable");
                break;
        }
        #endregion
    }

    private void KsaFilter(AssemblyDoc swAssyLevel1, string suffix, double length, string leftPart, string rightPart, string specialPart)
    {
        //KSA数量，KSA侧板长度(以全长计算)
        const double ksaLength = 498d;
        const double ngKsaSideLength = 2d;
        const double minKsaSideLength = 12d;
        const double goodKsaSideLength = 25d;
        int ksaNo = (int)((length + 0.5d) / ksaLength);
        double ksaSideLength = (length - ksaNo * ksaLength) / 2d;

        ModelDoc2 swModelLevel2;
        switch (ksaSideLength)
        {
            case <= ngKsaSideLength:
                swAssyLevel1.Suppress(suffix, leftPart);
                swAssyLevel1.Suppress(suffix, rightPart);
                swAssyLevel1.Suppress(suffix, specialPart);
                break;
            case < minKsaSideLength and > ngKsaSideLength:
                swAssyLevel1.Suppress(suffix, leftPart);
                swAssyLevel1.Suppress(suffix, rightPart);
                swAssyLevel1.UnSuppress(out swModelLevel2, suffix, specialPart, Aggregator);
                swModelLevel2.ChangeDim("Length@SketchBase", ksaSideLength * 2d);
                break;
            case < goodKsaSideLength and >= minKsaSideLength:
                swAssyLevel1.UnSuppress(out swModelLevel2, suffix, leftPart, Aggregator);
                swModelLevel2.ChangeDim("Length@SketchBase", ksaSideLength * 2);
                swAssyLevel1.Suppress(suffix, rightPart);
                swAssyLevel1.Suppress(suffix, specialPart);
                break;
            default:
                swAssyLevel1.UnSuppress(out swModelLevel2, suffix, leftPart, Aggregator);
                swModelLevel2.ChangeDim("Length@SketchBase", ksaSideLength);
                swAssyLevel1.UnSuppress(out swModelLevel2, suffix, rightPart, Aggregator);
                swModelLevel2.ChangeDim("Length@SketchBase", ksaSideLength);
                swAssyLevel1.Suppress(suffix, specialPart);
                break;
        }
    }

    private void ExhaustSide(AssemblyDoc swAssyLevel1, string suffix, bool ansul, SidePanel_e sidePanel, string leftPart, string rightPart)
    {
        if (ansul)
        {
            Component2 swCompLevel2;
            switch (sidePanel)
            {
                case SidePanel_e.中:
                    swAssyLevel1.UnSuppress(suffix, leftPart, Aggregator);
                    swCompLevel2 = swAssyLevel1.UnSuppress(suffix, rightPart, Aggregator);
                    swCompLevel2.UnSuppress("AnsulDetector");
                    break;
                case SidePanel_e.左:
                    swAssyLevel1.Suppress(suffix, leftPart);
                    swCompLevel2 = swAssyLevel1.UnSuppress(suffix, rightPart, Aggregator);
                    swCompLevel2.UnSuppress("AnsulDetector");
                    break;
                case SidePanel_e.右:
                    swAssyLevel1.Suppress(suffix, rightPart);
                    swCompLevel2 = swAssyLevel1.UnSuppress(suffix, leftPart, Aggregator);
                    swCompLevel2.FeatureByName("AnsulDetector");
                    break;
                case SidePanel_e.双:
                case SidePanel_e.NA:
                default:
                    swAssyLevel1.Suppress(suffix, leftPart);
                    swAssyLevel1.Suppress(suffix, rightPart);
                    break;
            }
        }
        else
        {
            swAssyLevel1.Suppress(suffix, leftPart);
            swAssyLevel1.Suppress(suffix, rightPart);
        }
    }

    private void ExhaustRail(AssemblyDoc swAssyLevel1, string suffix, bool marvel, double length, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, string railPart1, string railPart2)
    {
        if (marvel)
        {
            //有Marvel时不需要导轨
            swAssyLevel1.Suppress(suffix, railPart2);
            swAssyLevel1.Suppress(suffix, railPart1);
        }
        else
        {
            swAssyLevel1.UnSuppress(suffix, railPart2, Aggregator);
            swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, railPart1, Aggregator);
            swAssyLevel1.ChangeDim("Width@DistanceSpigot", exhaustSpigotWidth+25d);
            //根据脖颈数量计算导轨长度，两个排风口时只能总长减去100
            var railLength = exhaustSpigotNumber == 1
                ? exhaustSpigotLength * 2d + 100d : length - 100d;
            swModelLevel2.ChangeDim("Length@Base-Flange", railLength > length - 100d ? length - 100d : railLength);
        }
    }

    private void Ksa(AssemblyDoc swAssyLevel1, double length)
    {
        //KSA数量，KSA侧板长度(以全长计算)
        const double ksaLength = 498d;
        const double minKsaSideLength = 12d;
        int ksaNo = (int)((length + 0.5d) / ksaLength);
        double ksaSideLength = (length - ksaNo * ksaLength) / 2d;
        //KSA距离左边缘
        swAssyLevel1.ChangeDim("Ksa@DistanceKsa", ksaSideLength < minKsaSideLength ? 0.5d : ksaSideLength);
        //判断KSA数量，KSA侧板长度，如果太小，则使用特殊小侧板侧边
        if (ksaNo == 1)
        {
            swAssyLevel1.Suppress("LocalLPatternKsa");
        }
        else
        {
            swAssyLevel1.UnSuppress("LocalLPatternKsa");
        }
    }
    #endregion

    #region Uv555
    private void UvExhaust555(AssemblyDoc swAssyLevel1, string suffix, double length, UvLightType_e uvLightType, double middleToRight, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector)
    {
        FNHE0014(swAssyLevel1, suffix, "FNHE0014-1", length, ansul, ansulDetector);
        FNHE0015(swAssyLevel1, suffix, "FNHE0015-1", length);
        FNHE0016(swAssyLevel1, suffix, "FNHE0016-1", length, uvLightType, middleToRight);
        MeshFilter(swAssyLevel1, suffix, length, ansul, ansulSide, "FNHE0012-1", "FNHE0013-1");
    }

    private void FNHE0014(AssemblyDoc swAssyLevel1, string suffix, string partName, double length,
        bool ansul, AnsulDetector_e ansulDetector)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length-8d);
        if (ansul)
        {
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
                case AnsulDetector_e.双侧探测器口:
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
        }
        else
        {
            swCompLevel2.Suppress("AnsulDetectorRight");
            swCompLevel2.Suppress("AnsulDetectorLeft");
        }
    }

    private void FNHE0015(AssemblyDoc swAssyLevel1, string suffix, string partName, double length)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length-5d);
    }

    private void FNHE0016(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, UvLightType_e uvLightType, double middleToRight)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length-5d);
        #region UVHood
        switch (uvLightType)
        {
            case UvLightType_e.UVR4L:
            case UvLightType_e.UVR6L:
            case UvLightType_e.UVR8L:
                swCompLevel2.Suppress("UvDoorShort");
                swCompLevel2.UnSuppress("UvDoorLong");
                swModelLevel2.ChangeDim("ToRight@SketchUvDoorLong", middleToRight-2.5d);
                break;
            case UvLightType_e.UVR4S:
            case UvLightType_e.UVR6S:
            case UvLightType_e.UVR8S:
                swCompLevel2.UnSuppress("UvDoorShort");
                swCompLevel2.Suppress("UvDoorLong");
                swModelLevel2.ChangeDim("ToRight@SketchUvDoorShort", middleToRight-2.5d);
                break;
            case UvLightType_e.NA:
            default:
                swCompLevel2.Suppress("UvDoorShort");
                swCompLevel2.Suppress("UvDoorLong");
                break;
        }
        #endregion
    }

    private void MeshFilter(AssemblyDoc swAssyLevel1, string suffix, double length, bool ansul, AnsulSide_e ansulSide, string leftPart, string rightPart)
    {
        //MESH侧板长度(除去排风三角板3dm计算)1500
        const double meshLength = 498d;
        const double minMeshSideLengthAnsul = 55d;
        const double ngMeshSideLength = 1.5d;
        const double minMeshSideLength = 15d;
        const double offsetDis = 20d;//Mesh与KSA偏差的距离

        double meshSideLength = (length - 3d - meshLength*(int)((length - 2d) / meshLength)) / 2d;
        Component2 swCompLevel2;
        ModelDoc2 swModelLevel2;
        if (ansul)
        {
            if (meshSideLength * 2d < minMeshSideLengthAnsul) meshSideLength += meshLength/2d;
            if ((meshSideLength + offsetDis) > minMeshSideLengthAnsul)
            {
                switch (ansulSide)
                {
                    case AnsulSide_e.左侧喷:
                        swCompLevel2= swAssyLevel1.UnSuppress(out swModelLevel2, suffix, leftPart, Aggregator);
                        swModelLevel2.ChangeDim("Length@SketchBase", meshSideLength + offsetDis);
                        swCompLevel2.UnSuppress("AnsulSideLeft");

                        swCompLevel2 = swAssyLevel1.UnSuppress(out swModelLevel2, suffix, rightPart, Aggregator);
                        swModelLevel2.ChangeDim("Length@SketchBase", meshSideLength - offsetDis);
                        swCompLevel2.Suppress("AnsulDetectorRight");
                        break;
                    case AnsulSide_e.右侧喷:
                        swCompLevel2 = swAssyLevel1.UnSuppress(out swModelLevel2, suffix, leftPart, Aggregator);
                        swModelLevel2.ChangeDim("Length@SketchBase", meshSideLength - offsetDis);
                        swCompLevel2.Suppress("AnsulSideLeft");

                        swCompLevel2 = swAssyLevel1.UnSuppress(out swModelLevel2, suffix, rightPart, Aggregator);
                        swModelLevel2.ChangeDim("Length@SketchBase", meshSideLength + offsetDis);
                        swCompLevel2.UnSuppress("AnsulDetectorRight");
                        break;
                    case AnsulSide_e.NA:
                    case AnsulSide_e.无侧喷:
                    default:
                        swCompLevel2 = swAssyLevel1.UnSuppress(out swModelLevel2, suffix, leftPart, Aggregator);
                        swModelLevel2.ChangeDim("Length@SketchBase", meshSideLength - offsetDis);
                        swCompLevel2.Suppress("AnsulSideLeft");

                        swCompLevel2 = swAssyLevel1.UnSuppress(out swModelLevel2, suffix, rightPart, Aggregator);
                        swModelLevel2.ChangeDim("Length@SketchBase", meshSideLength + offsetDis);
                        swCompLevel2.Suppress("AnsulDetectorRight");
                        break;
                }
            }
            else
            {
                switch (ansulSide)
                {
                    case AnsulSide_e.左侧喷:
                        swCompLevel2= swAssyLevel1.UnSuppress(out swModelLevel2, suffix, leftPart, Aggregator);
                        swModelLevel2.ChangeDim("Length@SketchBase", meshSideLength *2);
                        swCompLevel2.UnSuppress("AnsulSideLeft");

                        swAssyLevel1.Suppress(suffix, rightPart);
                        break;
                    case AnsulSide_e.右侧喷:
                        swAssyLevel1.Suppress(suffix, leftPart);

                        swCompLevel2 = swAssyLevel1.UnSuppress(out swModelLevel2, suffix, rightPart, Aggregator);
                        swModelLevel2.ChangeDim("Length@SketchBase", meshSideLength *2);
                        swCompLevel2.UnSuppress("AnsulDetectorRight");
                        break;
                    case AnsulSide_e.NA:
                    case AnsulSide_e.无侧喷:
                    default:
                        swAssyLevel1.Suppress(suffix, leftPart);

                        swCompLevel2 = swAssyLevel1.UnSuppress(out swModelLevel2, suffix, rightPart, Aggregator);
                        swModelLevel2.ChangeDim("Length@SketchBase", meshSideLength *2);
                        swCompLevel2.Suppress("AnsulDetectorRight");
                        break;
                }
            }
        }
        else
        {
            //应该是（2 * meshSideLength）总长剩余1.5就没有小侧板
            if (meshSideLength*2d < minMeshSideLength && meshSideLength*2d > ngMeshSideLength) meshSideLength += meshLength/2;
            switch (meshSideLength)
            {
                case <= minMeshSideLength*2d when meshSideLength*2d > ngMeshSideLength:
                    swAssyLevel1.Suppress(suffix, leftPart);

                    swCompLevel2 = swAssyLevel1.UnSuppress(out swModelLevel2, suffix, rightPart, Aggregator);
                    swModelLevel2.ChangeDim("Length@SketchBase", meshSideLength * 2d);
                    swCompLevel2.Suppress("AnsulDetectorRight");
                    break;
                case > minMeshSideLength*2d:
                    swCompLevel2 = swAssyLevel1.UnSuppress(out swModelLevel2, suffix, leftPart, Aggregator);
                    swModelLevel2.ChangeDim("Length@SketchBase", meshSideLength - offsetDis);
                    swCompLevel2.Suppress("AnsulSideLeft");

                    swCompLevel2 = swAssyLevel1.UnSuppress(out swModelLevel2, suffix, rightPart, Aggregator);
                    swModelLevel2.ChangeDim("Length@SketchBase", meshSideLength + offsetDis);
                    swCompLevel2.Suppress("AnsulDetectorRight");
                    break;
                default:
                    swAssyLevel1.Suppress(suffix, leftPart);
                    swAssyLevel1.Suppress(suffix, rightPart);
                    break;
            }
        }
    }
    #endregion

    #region Kw555
    private void KwExhaust555(AssemblyDoc swAssyLevel1, string suffix, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, WaterInlet_e waterInlet)
    {
        //排风主体(背板)
        FNHE0031(swAssyLevel1, suffix, "FNHE0031-1", length, sidePanel, drainType, waterCollection, backToBack);

        //排风主体(顶板)
        FNHE0032(swAssyLevel1, suffix, "FNHE0032-1", length, uvLightType, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotDis, marvel, ansul, ansulSide, waterInlet);

        //排风腔前面板
        FNHE0033(swAssyLevel1, suffix, "FNHE0033-1", length, uvLightType, middleToRight, waterInlet);

        //三角板
        FNHE0034(swAssyLevel1, suffix, "FNHE0034-1", sidePanel, drainType, uvLightType);
        FNHE0035(swAssyLevel1, suffix, "FNHE0035-1", sidePanel, drainType);

        //水洗挡板
        FNHE0036(swAssyLevel1, suffix, "FNHE0036-1", length, uvLightType);

        //KSA下轨道支架
        FNHE0037(swAssyLevel1, suffix, "FNHE0037-1", length);

        //KSA侧板，水洗烟罩在三角板内侧，因此长度减去3，三角板的厚度
        KsaFilter(swAssyLevel1,suffix,length-3d, "FNHE0003-1", "FNHE0004-1", "FNHE0005-1");


        //排风滑门导轨
        ExhaustRail(swAssyLevel1, suffix, marvel, length, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotDis, "FNCE0018-1", "FNCE0018-2");

        //Ksa阵列，水洗烟罩在三角板内侧，因此长度减去3，三角板的厚度
        Ksa(swAssyLevel1, length-3d);
    }


    private void FNHE0031(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, SidePanel_e sidePanel, DrainType_e drainType, bool waterCollection, bool backToBack)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length);

        #region 集水翻边
        if (waterCollection)
        {
            switch (sidePanel)
            {
                case SidePanel_e.左:
                    swCompLevel2.Suppress("DrainChannelRight");
                    swCompLevel2.UnSuppress("DrainChannelLeft");
                    break;
                case SidePanel_e.右:
                    swCompLevel2.UnSuppress("DrainChannelRight");
                    swCompLevel2.Suppress("DrainChannelLeft");
                    break;
                case SidePanel_e.双:
                    swCompLevel2.UnSuppress("DrainChannelRight");
                    swCompLevel2.UnSuppress("DrainChannelLeft");
                    break;
                case SidePanel_e.NA:
                case SidePanel_e.中:
                default:
                    swCompLevel2.Suppress("DrainChannelRight");
                    swCompLevel2.Suppress("DrainChannelLeft");
                    break;
            }
        }
        else
        {
            swCompLevel2.Suppress("DrainChannelRight");
            swCompLevel2.Suppress("DrainChannelLeft");
        }
        #endregion

        #region 排水管
        switch (drainType)
        {

            case DrainType_e.右排水管:
                swCompLevel2.Suppress("DrainPipeLeft");
                swCompLevel2.UnSuppress("DrainPipeRight");
                break;
            case DrainType_e.左排水管:
                swCompLevel2.UnSuppress("DrainPipeLeft");
                swCompLevel2.Suppress("DrainPipeRight");
                break;
            case DrainType_e.左油塞:
            case DrainType_e.右油塞:
            case DrainType_e.上排水:
            case DrainType_e.集油槽:
            case DrainType_e.NA:
            default:
                swCompLevel2.Suppress("DrainPipeLeft");
                swCompLevel2.Suppress("DrainPipeRight");
                break;
        }
        #endregion

        #region 背靠背
        if (backToBack) swCompLevel2.UnSuppress("BackToBack");
        else swCompLevel2.Suppress("BackToBack");
        #endregion
    }

    private void FNHE0032(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, bool marvel, bool ansul, AnsulSide_e ansulSide, WaterInlet_e waterInlet)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length);

        #region MidRoof铆螺母孔
        //2023/3/10 修改MidRoof螺丝孔逻辑，以最低450间距计算间距即可
        const double sideDis = 150d;
        const double minMidRoofNutDis = 450d;
        var midRoofNutNumber = Math.Ceiling((length - 2*sideDis) / minMidRoofNutDis);
        midRoofNutNumber = midRoofNutNumber < 3 ? 3 : midRoofNutNumber;
        var midRoofNutDis = (length - 2*sideDis)/(midRoofNutNumber-1);

        swModelLevel2.ChangeDim("Dis@LPatternMidRoofNut", midRoofNutDis);
        #endregion

        #region UVHood，水洗烟罩
        //解压检修门，水洗烟罩，且带UV，短灯>=1600,长灯>=2400
        switch (uvLightType)
        {
            case UvLightType_e.UVR4L:
            case UvLightType_e.UVR6L:
            case UvLightType_e.UVR8L:
                swCompLevel2.UnSuppress("UvRack");
                swModelLevel2.ChangeDim("ToRight@SketchUvRack", middleToRight);
                swModelLevel2.ChangeDim("UvRack@SketchUvRack", 1640d);
                swCompLevel2.UnSuppress("UvCable");
                swModelLevel2.ChangeDim("ToRight@SketchUvCable", middleToRight);
                swModelLevel2.ChangeDim("UvCable@SketchUvCable", 1500d);
                WaterPipeInDoor(length >= 2400 ? waterInlet : WaterInlet_e.NA);
                break;
            case UvLightType_e.UVR4S:
            case UvLightType_e.UVR6S:
            case UvLightType_e.UVR8S:
                swCompLevel2.UnSuppress("UvRack");
                swModelLevel2.ChangeDim("ToRight@SketchUvRack", middleToRight);
                swModelLevel2.ChangeDim("UvRack@SketchUvRack", 930d);
                swCompLevel2.UnSuppress("UvCable");
                swModelLevel2.ChangeDim("ToRight@SketchUvCable", middleToRight);
                swModelLevel2.ChangeDim("UvCable@SketchUvCable", 790d);
                WaterPipeInDoor(length >= 1600 ? waterInlet : WaterInlet_e.NA);
                break;
            case UvLightType_e.NA:
            default:
                swCompLevel2.Suppress("UvRack");
                swCompLevel2.Suppress("UvCable");
                WaterPipeInDoor(WaterInlet_e.NA);
                break;
        }
        //内部方法，用于入水口检修门
        void WaterPipeInDoor(WaterInlet_e door)
        {
            switch (door)
            {
                case WaterInlet_e.右入水:
                    swCompLevel2.Suppress("WaterPipeInDoorLeft");
                    swCompLevel2.UnSuppress("WaterPipeInDoorRight");
                    break;
                case WaterInlet_e.左入水:
                    swCompLevel2.UnSuppress("WaterPipeInDoorLeft");
                    swCompLevel2.Suppress("WaterPipeInDoorRight");
                    break;
                case WaterInlet_e.NA:
                default:
                    swCompLevel2.Suppress("WaterPipeInDoorLeft");
                    swCompLevel2.Suppress("WaterPipeInDoorRight");
                    break;
            }
        }

        #endregion

        #region 入水口
        switch (waterInlet)
        {
            case WaterInlet_e.右入水:
                swCompLevel2.Suppress("WaterPipeInLeft");
                swCompLevel2.UnSuppress("WaterPipeInRight");
                break;
            case WaterInlet_e.左入水:
                swCompLevel2.UnSuppress("WaterPipeInLeft");
                swCompLevel2.Suppress("WaterPipeInRight");
                break;
            case WaterInlet_e.NA:
            default:
                swCompLevel2.Suppress("WaterPipeInLeft");
                swCompLevel2.Suppress("WaterPipeInRight");
                break;
        }
        #endregion

        #region 排风口
        if (exhaustSpigotNumber == 1)
        {
            swCompLevel2.UnSuppress("OneSpigot");
            swCompLevel2.Suppress("TwoSpigots");
            swModelLevel2.ChangeDim("ToRight@SketchOneSpigot", middleToRight);
            swModelLevel2.ChangeDim("Length@SketchOneSpigot", exhaustSpigotLength);
            swModelLevel2.ChangeDim("Width@SketchOneSpigot", exhaustSpigotWidth);
        }
        else
        {
            swCompLevel2.Suppress("OneSpigot");
            swCompLevel2.UnSuppress("TwoSpigots");
            swModelLevel2.ChangeDim("ToRight@SketchTwoSpigots", middleToRight);
            swModelLevel2.ChangeDim("Dis@SketchTwoSpigots", exhaustSpigotDis);
            swModelLevel2.ChangeDim("Length@SketchTwoSpigots", exhaustSpigotLength);
            swModelLevel2.ChangeDim("Width@SketchTwoSpigots", exhaustSpigotWidth);
        }

        #endregion

        #region MARVEL
        if (marvel)
        {
            swCompLevel2.UnSuppress("MarvelNtc");
            if (exhaustSpigotNumber == 1) swModelLevel2.ChangeDim("ToRight@SketchMarvelNtc", middleToRight + exhaustSpigotLength / 2d + 50d);
            else swModelLevel2.ChangeDim("ToRight@SketchMarvelNtc", middleToRight + exhaustSpigotDis / 2d + exhaustSpigotLength + 50d);
        }
        else swCompLevel2.Suppress("MarvelNtc");
        #endregion

        #region ANSUL
        switch (ansulSide)
        {
            //侧喷
            case AnsulSide_e.左侧喷:
                swCompLevel2.UnSuppress("ChannelLeft");
                swCompLevel2.UnSuppress("AnsulSideLeft");
                break;
            case AnsulSide_e.右侧喷:
                swCompLevel2.UnSuppress("ChannelRight");
                swCompLevel2.UnSuppress("AnsulSideRight");
                break;
            case AnsulSide_e.NA:
            case AnsulSide_e.无侧喷:
            default:
                swCompLevel2.Suppress("ChannelRight");
                swCompLevel2.Suppress("AnsulSideRight");
                swCompLevel2.Suppress("ChannelLeft");
                swCompLevel2.Suppress("AnsulSideLeft");
                break;
        }
        #endregion
    }

    private void FNHE0033(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, UvLightType_e uvLightType, double middleToRight, WaterInlet_e waterInlet)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length);
        #region UVHood
        switch (uvLightType)
        {
            case UvLightType_e.UVR4L:
            case UvLightType_e.UVR6L:
            case UvLightType_e.UVR8L:
                swCompLevel2.UnSuppress("TabUp");
                swCompLevel2.UnSuppress("SensorCable");
                swCompLevel2.UnSuppress("BaffleCable");
                swCompLevel2.Suppress("UvDoorShort");
                swCompLevel2.UnSuppress("UvDoorLong");
                swModelLevel2.ChangeDim("ToRight@SketchUvDoorLong", middleToRight);
                swCompLevel2.UnSuppress("UvCable");
                swModelLevel2.ChangeDim("ToRight@SketchUvCable", middleToRight);
                swModelLevel2.ChangeDim("UvCable@SketchUvCable", 1500d);
                break;
            case UvLightType_e.UVR4S:
            case UvLightType_e.UVR6S:
            case UvLightType_e.UVR8S:
                swCompLevel2.UnSuppress("TabUp");
                swCompLevel2.UnSuppress("SensorCable");
                swCompLevel2.UnSuppress("BaffleCable");
                swCompLevel2.UnSuppress("UvDoorShort");
                swCompLevel2.Suppress("UvDoorLong");
                swModelLevel2.ChangeDim("ToRight@SketchUvDoorShort", middleToRight);
                swCompLevel2.UnSuppress("UvCable");
                swModelLevel2.ChangeDim("ToRight@SketchUvCable", middleToRight);
                swModelLevel2.ChangeDim("UvCable@SketchUvCable", 790d);
                break;
            case UvLightType_e.NA:
            default:
                swCompLevel2.Suppress("TabUp");
                swCompLevel2.Suppress("SensorCable");
                swCompLevel2.Suppress("BaffleCable");
                swCompLevel2.Suppress("UvDoorShort");
                swCompLevel2.Suppress("UvDoorLong");
                swCompLevel2.Suppress("UvCable");
                break;
        }
        #endregion

        #region 入水口
        switch (waterInlet)
        {
            case WaterInlet_e.右入水:
                swCompLevel2.Suppress("WaterPipeInLeft");
                swCompLevel2.UnSuppress("WaterPipeInRight");
                break;
            case WaterInlet_e.左入水:
                swCompLevel2.UnSuppress("WaterPipeInLeft");
                swCompLevel2.Suppress("WaterPipeInRight");
                break;
            case WaterInlet_e.NA:
            default:
                swCompLevel2.Suppress("WaterPipeInLeft");
                swCompLevel2.Suppress("WaterPipeInRight");
                break;
        }
        #endregion
    }

    private void FNHE0034(AssemblyDoc swAssyLevel1, string suffix, string partName, SidePanel_e sidePanel, DrainType_e drainType, UvLightType_e uvLightType)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        if (drainType == DrainType_e.上排水&&(sidePanel==SidePanel_e.中||sidePanel==SidePanel_e.右))
        {
            swCompLevel2.UnSuppress("NoDrainPipe");
        }
        else
        {
            swCompLevel2.Suppress("NoDrainPipe");
        }

        if (uvLightType == UvLightType_e.NA)
        {
            swCompLevel2.Suppress("BaffleCable");
        }
        else
        {
            swCompLevel2.UnSuppress("BaffleCable");
        }
    }
    private void FNHE0035(AssemblyDoc swAssyLevel1, string suffix, string partName, SidePanel_e sidePanel, DrainType_e drainType)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        if (drainType == DrainType_e.上排水&&(sidePanel==SidePanel_e.中||sidePanel==SidePanel_e.左))
        {
            swCompLevel2.UnSuppress("NoDrainPipe");
        }
        else
        {
            swCompLevel2.Suppress("NoDrainPipe");
        }
    }

    private void FNHE0036(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, UvLightType_e uvLightType)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        const double sideLength = 50d+2.5d;//因为水洗挡板向外折弯，因此再扣除2.5
        //2021.06.08 july更改模型，磁铁拉铆钉避让1.5dm
        swModelLevel2.ChangeDim("Length@SketchBase", length -sideLength*2d - 1.5d);
        if (uvLightType == UvLightType_e.NA)
        {
            swCompLevel2.Suppress("BaffleCable");
        }
        else
        {
            swCompLevel2.UnSuppress("BaffleCable");
        }
    }

    private void FNHE0037(AssemblyDoc swAssyLevel1, string suffix, string partName, double length)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length -5d);
        
    }

    #endregion

}