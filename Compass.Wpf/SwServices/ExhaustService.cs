﻿using Compass.Wasm.Shared.Data;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Packaging;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.SwServices;

public class ExhaustService : BaseSwService, IExhaustService
{
    public ExhaustService(IContainerProvider provider) : base(provider)
    {
    }


    #region 标准555烟罩
    public void Kv555(AssemblyDoc swAssyTop, string suffix, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Exhaust_KV_555-1", Aggregator);
        ExhaustKv555(swAssyLevel1, suffix, length, sidePanel, uvLightType, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotHeight, exhaustSpigotDis, drainType, waterCollection, backToBack, marvel, ansul, ansulSide, ansulDetector, ExhaustType_e.KV);
    }

    public void Uv555(AssemblyDoc swAssyTop, string suffix, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Exhaust_UV_555-1", Aggregator);
        ExhaustKv555(swAssyLevel1, suffix, length, sidePanel, uvLightType, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotHeight, exhaustSpigotDis, drainType, waterCollection, backToBack, marvel, ansul, ansulSide, ansulDetector, ExhaustType_e.UV);
        ExhaustUv555(swAssyLevel1, suffix, length, uvLightType, middleToRight, ansul, ansulSide, ansulDetector);
    }


    public void Kw555(AssemblyDoc swAssyTop, string suffix, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, WaterInlet_e waterInlet)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Exhaust_KW_555-1", Aggregator);
        ExhaustKw555(swAssyLevel1, suffix, length, sidePanel, uvLightType, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotHeight, exhaustSpigotDis, drainType, waterCollection, backToBack, marvel, ansul, ansulSide, waterInlet, ExhaustType_e.KW);
    }

    public void Uw555(AssemblyDoc swAssyTop, string suffix, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, WaterInlet_e waterInlet)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Exhaust_UW_555-1", Aggregator);
        ExhaustKw555(swAssyLevel1, suffix, length, sidePanel, uvLightType, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotHeight, exhaustSpigotDis, drainType, waterCollection, backToBack, marvel, ansul, ansulSide, waterInlet, ExhaustType_e.UW);
        //UwExhaust555
        ExhaustUw555(swAssyLevel1, suffix, length, uvLightType, middleToRight, ansul, ansulSide, waterInlet);
    }
    #endregion

    #region 华为烟罩
    public void UvHw650(AssemblyDoc swAssyTop, string suffix, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Exhaust_UV_HW_650-1", Aggregator);
        ExhaustUvHw650(swAssyLevel1, suffix, length, sidePanel, uvLightType, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotHeight, exhaustSpigotDis, drainType, waterCollection, backToBack, marvel, ansul, ansulSide, ansulDetector, ExhaustType_e.UV);
    }

    public void UwHw650(AssemblyDoc swAssyTop, string suffix, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, WaterInlet_e waterInlet)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Exhaust_UW_HW_650-1", Aggregator);
        ExhaustUwHw650(swAssyLevel1, suffix, length, sidePanel, uvLightType, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotHeight, exhaustSpigotDis, drainType, waterCollection, backToBack, marvel, ansul, ansulSide, waterInlet, ExhaustType_e.UW);
    }

    #endregion

    //内部实现-----------------------------------------

    #region 排风脖颈

    private void ExhaustSpigotFs(AssemblyDoc swAssyLevel1, string suffix, double length, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, bool marvel, bool ansul, ExhaustType_e exhaustType)
    {
        ExhaustSpigot(swAssyLevel1,suffix,length,middleToRight,exhaustSpigotNumber,exhaustSpigotLength,exhaustSpigotWidth,exhaustSpigotHeight,exhaustSpigotDis,marvel,ansul,exhaustType, "FNHE0006-1", "FNHE0007-1", "FNHE0008-1", "FNHE0009-1", "FNCE0013-1", "FNCE0013-2");
    }
    private void ExhaustSpigotHw(AssemblyDoc swAssyLevel1, string suffix, double length, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, bool marvel, bool ansul, ExhaustType_e exhaustType)
    {
        //ExhaustSpigot(swAssyLevel1, suffix, length, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotHeight, exhaustSpigotDis, marvel, ansul, exhaustType, "FNHE0166-1", "FNHE0167-1", "FNHE0168-1", "FNHE0169-1", "FNHE0164-1", "FNHE0164-2");

        //华为烟罩全部需要下料
        const string frontPart = "FNHE0166-1";
        const string backPart = "FNHE0167-1";
        const string leftPart = "FNHE0168-1";
        const string rightPart = "FNHE0169-1";
        const string doorPart1 = "FNHE0164-1";
        const string doorPart2 = "FNHE0164-4";//他妈的怎么会变实例号呢

        //前
        FNHE0006(swAssyLevel1, suffix, frontPart, exhaustSpigotLength, exhaustSpigotHeight, ansul);
        //后
        FNHE0007(swAssyLevel1, suffix, backPart, exhaustSpigotLength, exhaustSpigotHeight);
        //左
        FNHE0008(swAssyLevel1, suffix, leftPart, exhaustSpigotWidth, exhaustSpigotHeight, ansul, exhaustType);
        //右,
        FNHE0009(swAssyLevel1, suffix, rightPart, exhaustSpigotWidth, exhaustSpigotHeight, ansul, exhaustType);

        swAssyLevel1.UnSuppress(suffix, doorPart2, Aggregator);
        FNCE0013(swAssyLevel1, suffix, doorPart1, exhaustSpigotLength, exhaustSpigotWidth);

        //两个排风口的情况
        if (exhaustSpigotNumber == 2)
        {
            //计算右边脖颈右侧距离基准中心距离
            var toMiddle = exhaustSpigotDis / 2d + exhaustSpigotLength + length / 2d - middleToRight;
            swAssyLevel1.ChangeDim("ToMiddle@DistanceExhaustSpigot", toMiddle);
            swAssyLevel1.UnSuppress("LocalLPatternExhaustSpigot");
            swAssyLevel1.ChangeDim("Dis@LocalLPatternExhaustSpigot", exhaustSpigotLength+exhaustSpigotDis);
        }
        else
        {
            var toMiddle = exhaustSpigotLength/2d+ length / 2d - middleToRight;
            swAssyLevel1.ChangeDim("ToMiddle@DistanceExhaustSpigot", toMiddle);
            swAssyLevel1.Suppress("LocalLPatternExhaustSpigot");
        }
    }


    private void ExhaustSpigot(AssemblyDoc swAssyLevel1, string suffix, double length, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, bool marvel, bool ansul, ExhaustType_e exhaustType,string frontPart,string backPart,string leftPart,string rightPart,string doorPart1,string doorPart2)
    {
        //进入ExhaustSpigot_Fs，子装配
        //当有marvel或者脖颈高度100，且没有ansul时，脖颈宽度为300,无需脖颈
        if ((marvel|| exhaustSpigotHeight.Equals(100d))&& !ansul&&exhaustSpigotWidth.Equals(300d))
        {
            //压缩脖颈零件，压缩脖颈阵列
            swAssyLevel1.Suppress(suffix, frontPart);
            swAssyLevel1.Suppress(suffix, backPart);
            swAssyLevel1.Suppress(suffix, leftPart);
            swAssyLevel1.Suppress(suffix, rightPart);
            swAssyLevel1.Suppress(suffix, doorPart1);
            swAssyLevel1.Suppress(suffix, doorPart2);
            swAssyLevel1.Suppress("LocalLPatternExhaustSpigot");
        }
        else
        {
            //todo:有待细化逻辑
            //前
            FNHE0006(swAssyLevel1, suffix, frontPart, exhaustSpigotLength, exhaustSpigotHeight, ansul);
            //后
            FNHE0007(swAssyLevel1, suffix, backPart, exhaustSpigotLength, exhaustSpigotHeight);
            //左
            FNHE0008(swAssyLevel1, suffix, leftPart, exhaustSpigotWidth, exhaustSpigotHeight, ansul, exhaustType);
            //右,
            FNHE0009(swAssyLevel1, suffix, rightPart, exhaustSpigotWidth, exhaustSpigotHeight, ansul, exhaustType);

            //如果是标准宽度，则压缩滑门
            if (exhaustSpigotWidth.Equals(300d))
            {
                swAssyLevel1.Suppress(suffix, doorPart1);
                swAssyLevel1.Suppress(suffix, doorPart2);
            }
            else
            {
                swAssyLevel1.UnSuppress(suffix, doorPart2, Aggregator);
                FNCE0013(swAssyLevel1, suffix, doorPart1, exhaustSpigotLength, exhaustSpigotWidth);
            }
            //两个排风口的情况
            if (exhaustSpigotNumber == 2)
            {
                //计算右边脖颈右侧距离基准中心距离
                var toMiddle = exhaustSpigotDis / 2d + exhaustSpigotLength + length / 2d - middleToRight;
                swAssyLevel1.ChangeDim("ToMiddle@DistanceExhaustSpigot", toMiddle);
                swAssyLevel1.UnSuppress("LocalLPatternExhaustSpigot");
                swAssyLevel1.ChangeDim("Dis@LocalLPatternExhaustSpigot", exhaustSpigotLength+exhaustSpigotDis);
            }
            else
            {
                var toMiddle = exhaustSpigotLength/2d+ length / 2d - middleToRight;
                swAssyLevel1.ChangeDim("ToMiddle@DistanceExhaustSpigot", toMiddle);
                swAssyLevel1.Suppress("LocalLPatternExhaustSpigot");
            }
        }

    }

    private void FNHE0006(AssemblyDoc swAssyLevel1, string suffix, string partName, double exhaustSpigotLength, double exhaustSpigotHeight,
        bool ansul)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", exhaustSpigotLength + 50d);
        swModelLevel2.ChangeDim("Height@SketchBase", exhaustSpigotHeight);
        if (ansul) swCompLevel2.UnSuppress("Ansul");
        else swCompLevel2.Suppress("Ansul");
    }
    private void FNHE0007(AssemblyDoc swAssyLevel1, string suffix, string partName, double exhaustSpigotLength, double exhaustSpigotHeight)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", exhaustSpigotLength + 50d);
        swModelLevel2.ChangeDim("Height@SketchBase", exhaustSpigotHeight);
    }
    private void FNHE0008(AssemblyDoc swAssyLevel1, string suffix, string partName, double exhaustSpigotWidth, double exhaustSpigotHeight, bool ansul, ExhaustType_e exhaustType)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", exhaustSpigotWidth);
        swModelLevel2.ChangeDim("Height@SketchBase", exhaustSpigotHeight);
        //todo: 有待细化Ansul探测器逻辑，水洗烟罩需要AnsulDetector
        if (ansul && (exhaustType is ExhaustType_e.KW or ExhaustType_e.UW or ExhaustType_e.CMOD))
        {
            swCompLevel2.UnSuppress("AnsulDetector");
        }
        else
        {
            swCompLevel2.Suppress("AnsulDetector");
        }
    }
    private void FNHE0009(AssemblyDoc swAssyLevel1, string suffix, string partName, double exhaustSpigotWidth, double exhaustSpigotHeight, bool ansul, ExhaustType_e exhaustType)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", exhaustSpigotWidth);
        swModelLevel2.ChangeDim("Height@SketchBase", exhaustSpigotHeight);
        //todo: 有待细化Ansul探测器逻辑，水洗烟罩需要AnsulDetector
        if (ansul && (exhaustType is ExhaustType_e.KW or ExhaustType_e.UW or ExhaustType_e.CMOD))
        {
            swCompLevel2.UnSuppress("AnsulDetector");
        }
        else
        {
            swCompLevel2.Suppress("AnsulDetector");
        }
    }
    private void FNCE0013(AssemblyDoc swAssyLevel1, string suffix, string partName, double exhaustSpigotLength, double exhaustSpigotWidth)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        //标准的滑门宽度加40，长度/2
        swModelLevel2.ChangeDim("Length@SketchBase", exhaustSpigotLength / 2d);
        swModelLevel2.ChangeDim("Width@SketchBase", exhaustSpigotWidth + 40d);
    }
    #endregion

    #region Kv555
    private void ExhaustKv555(AssemblyDoc swAssyLevel1, string suffix, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector, ExhaustType_e exhaustType)
    {
        //排风主体
        FNHE0001(swAssyLevel1, suffix, "FNHE0001-1", length, sidePanel, uvLightType, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotDis, drainType, waterCollection, backToBack, marvel, ansul, ansulSide, ansulDetector);
        //排风腔前面板
        FNHE0002(swAssyLevel1, suffix, "FNHE0002-1", length, uvLightType, middleToRight);
        //排风脖颈
        var swAssyLevel2 = swAssyLevel1.GetSubAssemblyDoc(suffix, "ExhaustSpigot_Fs-1", Aggregator);
        ExhaustSpigotFs(swAssyLevel2, suffix, length, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotHeight, exhaustSpigotDis, marvel, ansul, exhaustType);

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
            swAssyLevel1.ChangeDim("Width@DistanceSpigot", exhaustSpigotWidth+45d);
            //根据脖颈数量计算导轨长度，两个排风口时只能总长减去200
            var railLength = exhaustSpigotNumber == 1
                ? exhaustSpigotLength * 2d + 100d : length - 200d;
            swModelLevel2.ChangeDim("Length@Base-Flange", railLength > length - 200d ? length - 200d : railLength);
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
    private void ExhaustUv555(AssemblyDoc swAssyLevel1, string suffix, double length, UvLightType_e uvLightType, double middleToRight, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector)
    {
        FNHE0014(swAssyLevel1, suffix, "FNHE0014-1", length, ansul, ansulDetector);
        FNHE0015(swAssyLevel1, suffix, "FNHE0015-1", length);
        FNHE0016(swAssyLevel1, suffix, "FNHE0016-1", length, uvLightType, middleToRight);
        MeshFilter(swAssyLevel1, suffix, length-2d, ansul, ansulSide, "FNHE0012-1", "FNHE0013-1");
    }

    private void FNHE0014(AssemblyDoc swAssyLevel1, string suffix, string partName, double length,
        bool ansul, AnsulDetector_e ansulDetector)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length-8d);

        #region 借用逻辑-MidRoof铆螺母孔
        //2023/3/10 修改MidRoof螺丝孔逻辑，以最低450间距计算间距即可
        const double sideDis = 150d;
        const double minMidRoofNutDis = 450d;
        var midRoofNutNumber = Math.Ceiling((length - 2*sideDis) / minMidRoofNutDis);
        midRoofNutNumber = midRoofNutNumber < 3 ? 3 : midRoofNutNumber;
        var midRoofNutDis = (length - 2*sideDis)/(midRoofNutNumber-1);

        swModelLevel2.ChangeDim("Dis@LPatternMidRoofNut", midRoofNutDis);
        #endregion

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

        if (ansul)
        {
            if (meshSideLength * 2d < minMeshSideLengthAnsul) meshSideLength += meshLength/2d;
            if ((meshSideLength + offsetDis) > minMeshSideLengthAnsul)
            {
                switch (ansulSide)
                {
                    case AnsulSide_e.左侧喷:
                        FNHE0012(meshSideLength + offsetDis);
                        FNHE0013(meshSideLength - offsetDis);
                        break;
                    case AnsulSide_e.右侧喷:
                    case AnsulSide_e.无侧喷:
                    case AnsulSide_e.NA:
                    default:
                        FNHE0012(meshSideLength - offsetDis);
                        FNHE0013(meshSideLength + offsetDis);
                        break;
                }
            }
            else
            {
                switch (ansulSide)
                {
                    case AnsulSide_e.左侧喷:
                        FNHE0012(meshSideLength *2d);
                        swAssyLevel1.Suppress(suffix, rightPart);
                        break;
                    case AnsulSide_e.右侧喷:
                    case AnsulSide_e.无侧喷:
                    case AnsulSide_e.NA:
                    default:
                        swAssyLevel1.Suppress(suffix, leftPart);
                        FNHE0013(meshSideLength *2d);
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
                    FNHE0013(meshSideLength *2d);
                    break;
                case > minMeshSideLength*2d:
                    FNHE0013(meshSideLength -offsetDis);
                    FNHE0013(meshSideLength +offsetDis);
                    break;
                default:
                    swAssyLevel1.Suppress(suffix, leftPart);
                    swAssyLevel1.Suppress(suffix, rightPart);
                    break;
            }
        }
        void FNHE0012(double sideLength)
        {
            var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, leftPart, Aggregator);
            swModelLevel2.ChangeDim("Length@SketchBase", sideLength);
            if (ansulSide==AnsulSide_e.左侧喷) swCompLevel2.UnSuppress("AnsulSideLeft");
            else swCompLevel2.Suppress("AnsulSideLeft");
        }
        void FNHE0013(double sideLength)
        {
            var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, rightPart, Aggregator);
            swModelLevel2.ChangeDim("Length@SketchBase", sideLength);
            if (ansulSide==AnsulSide_e.右侧喷) swCompLevel2.UnSuppress("AnsulSideRight");
            else swCompLevel2.Suppress("AnsulSideRight");
        }
    }
    #endregion

    #region Kw555
    private void ExhaustKw555(AssemblyDoc swAssyLevel1, string suffix, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, WaterInlet_e waterInlet, ExhaustType_e exhaustType)
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

        //排风脖颈
        var swAssyLevel2 = swAssyLevel1.GetSubAssemblyDoc(suffix, "ExhaustSpigot_Fs-1", Aggregator);
        ExhaustSpigotFs(swAssyLevel2, suffix, length, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotHeight, exhaustSpigotDis, marvel, ansul, exhaustType);

        //KSA侧板，水洗烟罩在三角板内侧，因此长度减去3，三角板的厚度
        KsaFilter(swAssyLevel1, suffix, length-3d, "FNHE0003-1", "FNHE0004-1", "FNHE0005-1");

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

    #region Uw555
    private void ExhaustUw555(AssemblyDoc swAssyLevel1, string suffix, double length, UvLightType_e uvLightType, double middleToRight, bool ansul, AnsulSide_e ansulSide, WaterInlet_e waterInlet)
    {

        FNHE0040(swAssyLevel1, suffix, "FNHE0040-1", length);
        FNHE0015(swAssyLevel1, suffix, "FNHE0015-1", length);
        FNHE0041(swAssyLevel1, suffix, "FNHE0041-1", length, uvLightType, middleToRight);
        UwMeshFilter(swAssyLevel1, suffix, length, ansul, ansulSide, waterInlet, "FNHE0038-1", "FNHE0039-1");
    }

    private void FNHE0040(AssemblyDoc swAssyLevel1, string suffix, string partName, double length)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length-8d);

        #region 借用逻辑-MidRoof铆螺母孔
        //2023/3/10 修改MidRoof螺丝孔逻辑，以最低450间距计算间距即可
        const double sideDis = 150d;
        const double minMidRoofNutDis = 450d;
        var midRoofNutNumber = Math.Ceiling((length - 2*sideDis) / minMidRoofNutDis);
        midRoofNutNumber = midRoofNutNumber < 3 ? 3 : midRoofNutNumber;
        var midRoofNutDis = (length - 2*sideDis)/(midRoofNutNumber-1);

        swModelLevel2.ChangeDim("Dis@LPatternMidRoofNut", midRoofNutDis);
        #endregion
    }

    private void FNHE0041(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, UvLightType_e uvLightType, double middleToRight)
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

    private void UwMeshFilter(AssemblyDoc swAssyLevel1, string suffix, double length, bool ansul, AnsulSide_e ansulSide, WaterInlet_e waterInlet, string leftPart, string rightPart)
    {
        //MESH侧板长度(除去排风三角板3dm计算)1500
        const double meshLength = 498d;
        const double minMeshSideLengthAnsul = 55d;
        const double offsetDis = 20d;//Mesh与KSA偏差的距离
        //MESH侧板长度(除去排风三角板3dm计算,20220812-增加了考虑水管孔避让，最小55，再加上20错开KSA)
        double meshSideLength = (length - 3d - meshLength*(int)((length - 2d-minMeshSideLengthAnsul+offsetDis) / meshLength)) / 2d;

        //有Ansul时，Ansul和进水管不同一侧
        if (ansul&&((waterInlet == WaterInlet_e.左入水 && ansulSide == AnsulSide_e.右侧喷) ||
            (waterInlet == WaterInlet_e.右入水 && ansulSide == AnsulSide_e.左侧喷)))
        {
            //再减少一个MESH(498/2)
            if ((meshSideLength - offsetDis) < minMeshSideLengthAnsul) meshSideLength += meshLength / 2d;
            FNHE0038(meshSideLength - offsetDis);
            FNHE0039(meshSideLength + offsetDis);
        }
        //Ansul和进水管同一侧，或者没有ansul时
        else
        {
            //如果只有一个MESH侧板，再减少一个MESH(498/2)
            if (meshSideLength * 2d < minMeshSideLengthAnsul) meshSideLength += meshLength / 2d;
            //最大侧板>55，才能穿水管
            if ((meshSideLength + offsetDis) > minMeshSideLengthAnsul)
            {
                if (waterInlet == WaterInlet_e.左入水)
                {
                    FNHE0038(meshSideLength + offsetDis);
                    FNHE0039(meshSideLength - offsetDis);
                }
                else
                {
                    FNHE0038(meshSideLength - offsetDis);
                    FNHE0039(meshSideLength + offsetDis);
                }
            }
            else
            {
                if (waterInlet == WaterInlet_e.左入水)
                {
                    FNHE0038(meshSideLength *2d);
                    swAssyLevel1.Suppress(suffix, rightPart);
                }
                else
                {
                    swAssyLevel1.Suppress(suffix, leftPart);
                    FNHE0039(meshSideLength *2d);
                }
            }
        }

        void FNHE0038(double sideLength)
        {
            var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, leftPart, Aggregator);
            swModelLevel2.ChangeDim("Length@SketchBase", sideLength);
            if (waterInlet == WaterInlet_e.左入水) swCompLevel2.UnSuppress("WaterPipeLeft");
            else swCompLevel2.Suppress("WaterPipeLeft");
            if (ansulSide==AnsulSide_e.左侧喷) swCompLevel2.UnSuppress("AnsulSideLeft");
            else swCompLevel2.Suppress("AnsulSideLeft");
        }
        void FNHE0039(double sideLength)
        {
            var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, rightPart, Aggregator);
            swModelLevel2.ChangeDim("Length@SketchBase", sideLength);
            if (waterInlet == WaterInlet_e.右入水) swCompLevel2.UnSuppress("WaterPipeRight");
            else swCompLevel2.Suppress("WaterPipeRight");
            if (ansulSide==AnsulSide_e.右侧喷) swCompLevel2.UnSuppress("AnsulSideRight");
            else swCompLevel2.Suppress("AnsulSideRight");
        }
    }


    #endregion
    
    #region UvHw650
    private void ExhaustUvHw650(AssemblyDoc swAssyLevel1, string suffix, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector, ExhaustType_e exhaustType)
    {
        //排风主体
        FNHE0186(swAssyLevel1, suffix, "FNHE0186-1", length, sidePanel, uvLightType, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotDis, drainType, waterCollection, backToBack, marvel, ansul, ansulSide, ansulDetector);
        //排风腔前面板
        FNHE0187(swAssyLevel1, suffix, "FNHE0187-1", length, uvLightType, middleToRight);

        //排风脖颈
        var swAssyLevel2 = swAssyLevel1.GetSubAssemblyDoc(suffix, "ExhaustSpigot_Hw-1", Aggregator);

        ExhaustSpigotHw(swAssyLevel2, suffix, length, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotHeight, exhaustSpigotDis, marvel, ansul, exhaustType);

        //KSA侧边
        KsaFilter(swAssyLevel1, suffix, length, "FNHE0160-1", "FNHE0161-1", "FNHE0170-1");

        //排风三角板
        //ExhaustSide(swAssyLevel1, suffix, ansul, sidePanel, "FNHE0185-1", "FNHE0185-2");

        //排风滑门导轨
        ExhaustRail(swAssyLevel1, suffix, marvel, length, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotDis, "FNHE0165-1", "FNHE0165-2");

        //Ksa阵列
        Ksa(swAssyLevel1, length);

        //UV零件
        FNHE0014(swAssyLevel1, suffix, "FNHE0188-1", length-4d, ansul, ansulDetector);

        FNHE0015(swAssyLevel1, suffix, "FNHE0189-1", length-2d);

        FNHE0190(swAssyLevel1, suffix, "FNHE0190-1", length, uvLightType, middleToRight);

        MeshFilterHw(swAssyLevel1, suffix, length, ansul, ansulSide, "FNHE0162-1", "FNHE0163-1");

        swAssyLevel2 = swAssyLevel1.GetSubAssemblyDoc(suffix, "UvSupport_Hw650-1", Aggregator);
        UvSupportHw650(swAssyLevel2,length, middleToRight, uvLightType);
    }
    private void FNHE0186(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector)
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
                swModelLevel2.ChangeDim("UvRack@SketchUvRack", 1640d);//1622/912?
                swCompLevel2.UnSuppress("UvCable");
                swModelLevel2.ChangeDim("ToRight@SketchUvCable", middleToRight);
                swModelLevel2.ChangeDim("UvCable@SketchUvCable", 1500d);
                swCompLevel2.Suppress("UvDouble");
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
                swCompLevel2.Suppress("UvDouble");
                break;
            case UvLightType_e.Double:
                swCompLevel2.Suppress("UvRack");
                swCompLevel2.Suppress("UvCable");
                swCompLevel2.UnSuppress("UvDouble");
                break;
            case UvLightType_e.NA:
            default:
                swCompLevel2.Suppress("UvRack");
                swCompLevel2.Suppress("UvCable");
                swCompLevel2.Suppress("UvDouble");
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
        //if (marvel)
        //{
        //    swCompLevel2.UnSuppress("MarvelNtc");
        //    if (exhaustSpigotNumber == 1) swModelLevel2.ChangeDim("ToRight@SketchMarvelNtc", middleToRight + exhaustSpigotLength / 2d + 50d);
        //    else swModelLevel2.ChangeDim("ToRight@SketchMarvelNtc", middleToRight + exhaustSpigotDis / 2d + exhaustSpigotLength + 50d);
        //}
        //else swCompLevel2.Suppress("MarvelNtc");
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

    private void FNHE0187(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, UvLightType_e uvLightType,
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
                swCompLevel2.Suppress("UvDoorDouble");
                swCompLevel2.Suppress("UvCableDouble");
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
                swCompLevel2.Suppress("UvDoorDouble");
                swCompLevel2.Suppress("UvCableDouble");
                break;
            case UvLightType_e.Double:
                swCompLevel2.UnSuppress("TabUp");
                swCompLevel2.UnSuppress("SensorCable");
                swCompLevel2.Suppress("UvDoorShort");
                swCompLevel2.Suppress("UvDoorLong");
                swCompLevel2.Suppress("UvCable");
                swCompLevel2.UnSuppress("UvDoorDouble");
                swCompLevel2.UnSuppress("UvCableDouble");
                break;
            case UvLightType_e.NA:
            default:
                swCompLevel2.Suppress("TabUp");
                swCompLevel2.Suppress("SensorCable");
                swCompLevel2.Suppress("UvDoorShort");
                swCompLevel2.Suppress("UvDoorLong");
                swCompLevel2.Suppress("UvCable");
                swCompLevel2.Suppress("UvDoorDouble");
                swCompLevel2.Suppress("UvCableDouble");
                break;
        }
        #endregion
    }

    private void FNHE0190(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, UvLightType_e uvLightType, double middleToRight)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length-7d);
        #region UVHood
        switch (uvLightType)
        {
            case UvLightType_e.UVR4L:
            case UvLightType_e.UVR6L:
            case UvLightType_e.UVR8L:
                swCompLevel2.Suppress("UvDoorShort");
                swCompLevel2.UnSuppress("UvDoorLong");
                swModelLevel2.ChangeDim("ToRight@SketchUvDoorLong", middleToRight-3.5d);
                swCompLevel2.Suppress("UvDoorDouble");
                break;
            case UvLightType_e.UVR4S:
            case UvLightType_e.UVR6S:
            case UvLightType_e.UVR8S:
                swCompLevel2.UnSuppress("UvDoorShort");
                swCompLevel2.Suppress("UvDoorLong");
                swModelLevel2.ChangeDim("ToRight@SketchUvDoorShort", middleToRight-3.5d);
                swCompLevel2.Suppress("UvDoorDouble");
                break;
            case UvLightType_e.Double:
                swCompLevel2.Suppress("UvDoorShort");
                swCompLevel2.Suppress("UvDoorLong");
                swCompLevel2.UnSuppress("UvDoorDouble");
                break;
            case UvLightType_e.NA:
            default:
                swCompLevel2.Suppress("UvDoorShort");
                swCompLevel2.Suppress("UvDoorLong");
                swCompLevel2.Suppress("UvDoorDouble");
                break;
        }
        #endregion
    }

    private void MeshFilterHw(AssemblyDoc swAssyLevel1, string suffix, double length, bool ansul, AnsulSide_e ansulSide, string leftPart, string rightPart)
    {
        //肖启才说，手划伤了，华为不锈钢mesh用498.5去计算,包边再减去4
        //MESH侧板长度(除去排风三角板3dm计算)1500
        const double meshLength = 498.5d;
        const double minMeshSideLengthAnsul = 55d;
        const double ngMeshSideLength = 1.5d;
        const double minMeshSideLength = 15d;
        const double offsetDis = 20d;//Mesh与KSA偏差的距离

        double meshSideLength = (length - 3d-4d - meshLength*(int)((length - 2d) / meshLength)) / 2d;

        if (ansul)
        {
            if (meshSideLength * 2d < minMeshSideLengthAnsul) meshSideLength += meshLength/2d;
            if ((meshSideLength + offsetDis) > minMeshSideLengthAnsul)
            {
                switch (ansulSide)
                {
                    case AnsulSide_e.左侧喷:
                        FNHE0162(meshSideLength + offsetDis);
                        FNHE0163(meshSideLength - offsetDis);
                        break;
                    case AnsulSide_e.右侧喷:
                    case AnsulSide_e.无侧喷:
                    case AnsulSide_e.NA:
                    default:
                        FNHE0162(meshSideLength - offsetDis);
                        FNHE0163(meshSideLength + offsetDis);
                        break;
                }
            }
            else
            {
                switch (ansulSide)
                {
                    case AnsulSide_e.左侧喷:
                        FNHE0162(meshSideLength *2d);
                        swAssyLevel1.Suppress(suffix, rightPart);
                        break;
                    case AnsulSide_e.右侧喷:
                    case AnsulSide_e.无侧喷:
                    case AnsulSide_e.NA:
                    default:
                        swAssyLevel1.Suppress(suffix, leftPart);
                        FNHE0163(meshSideLength *2d);
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
                    FNHE0163(meshSideLength *2d);
                    break;
                case > minMeshSideLength*2d:
                    FNHE0163(meshSideLength -offsetDis);
                    FNHE0163(meshSideLength +offsetDis);
                    break;
                default:
                    swAssyLevel1.Suppress(suffix, leftPart);
                    swAssyLevel1.Suppress(suffix, rightPart);
                    break;
            }
        }
        void FNHE0162(double sideLength)
        {
            var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, leftPart, Aggregator);
            swModelLevel2.ChangeDim("Length@SketchBase", sideLength);
            if (ansulSide==AnsulSide_e.左侧喷) swCompLevel2.UnSuppress("AnsulSideLeft");
            else swCompLevel2.Suppress("AnsulSideLeft");
            swCompLevel2.Suppress("WaterPipeLeft");
        }
        void FNHE0163(double sideLength)
        {
            var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, rightPart, Aggregator);
            swModelLevel2.ChangeDim("Length@SketchBase", sideLength);
            if (ansulSide==AnsulSide_e.右侧喷) swCompLevel2.UnSuppress("AnsulSideRight");
            else swCompLevel2.Suppress("AnsulSideRight");
            swCompLevel2.Suppress("WaterPipeRight");
        }
    }


    private void UvSupportHw650(AssemblyDoc swAssyLevel1,double length, double middleToRight, UvLightType_e uvLightType)
    {
        var leftDis = 1922d/2d; //双灯时一直居中
        var rightDis = 1922d/2d;
        switch (uvLightType)
        {
            case UvLightType_e.UVR4S:
            case UvLightType_e.UVR6S:
            case UvLightType_e.UVR8S:
                //930
                leftDis = 930d / 2d + length / 2d - middleToRight;
                rightDis=930d / 2d - length / 2d + middleToRight;
                swAssyLevel1.Suppress("LocalLPatternDoubleLeft");
                swAssyLevel1.Suppress("LocalLPatternDoubleRight");
                break;
            case UvLightType_e.UVR4L:
            case UvLightType_e.UVR6L:
            case UvLightType_e.UVR8L:
                //1640
                leftDis = 1640d / 2d + length / 2d - middleToRight;
                rightDis=1640d / 2d - length / 2d + middleToRight;
                swAssyLevel1.Suppress("LocalLPatternDoubleLeft");
                swAssyLevel1.Suppress("LocalLPatternDoubleRight");
                break;
            case UvLightType_e.Double:
            case UvLightType_e.NA:
            default:
                swAssyLevel1.UnSuppress("LocalLPatternDoubleLeft");
                swAssyLevel1.UnSuppress("LocalLPatternDoubleRight");
                break;
        }
        swAssyLevel1.ChangeDim("UvRack@DistanceUvRackLeft", leftDis);
        swAssyLevel1.ChangeDim("UvRack@DistanceUvRackRight", rightDis);
    }

    #endregion

    #region UwHw650
    private void ExhaustUwHw650(AssemblyDoc swAssyLevel1, string suffix, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, WaterInlet_e waterInlet, ExhaustType_e exhaustType)
    {
        //排风主体(背板)
        FNHE0031(swAssyLevel1, suffix, "FNHE0179-1", length, sidePanel, drainType, waterCollection, backToBack);

        //排风主体(顶板)
        FNHE0148(swAssyLevel1, suffix, "FNHE0148-1", length, uvLightType, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotDis, marvel, ansul, ansulSide, waterInlet);

        //排风腔前面板
        FNHE0180(swAssyLevel1, suffix, "FNHE0180-1", length, uvLightType, middleToRight, waterInlet);

        //三角板
        FNHE0034(swAssyLevel1, suffix, "FNHE0182-1", sidePanel, drainType, uvLightType);
        FNHE0035(swAssyLevel1, suffix, "FNHE0181-1", sidePanel, drainType);

        //水洗挡板
        FNHE0036(swAssyLevel1, suffix, "FNHE0150-1", length, uvLightType);

        //KSA下轨道支架
        FNHE0037(swAssyLevel1, suffix, "FNHE0151-1", length);

        //排风脖颈
        var swAssyLevel2 = swAssyLevel1.GetSubAssemblyDoc(suffix, "ExhaustSpigot_Hw-1", Aggregator);
        ExhaustSpigotHw(swAssyLevel2, suffix, length, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotHeight, exhaustSpigotDis, marvel, ansul, exhaustType);

        //KSA侧板，水洗烟罩在三角板内侧，因此长度减去3，三角板的厚度
        KsaFilter(swAssyLevel1, suffix, length-3d, "FNHE0160-1", "FNHE0161-1", "FNHE0170-1");

        //排风滑门导轨
        ExhaustRail(swAssyLevel1, suffix, marvel, length, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotDis, "FNHE0165-1", "FNHE0165-2");

        //Ksa阵列，水洗烟罩在三角板内侧，因此长度减去3，三角板的厚度
        Ksa(swAssyLevel1, length-3d);

        FNHE0040(swAssyLevel1, suffix, "FNHE0154-1", length);

        FNHE0015(swAssyLevel1, suffix, "FNHE0189-1", length-2d);

        FNHE0190(swAssyLevel1, suffix, "FNHE0152-1", length, uvLightType, middleToRight);


        UwMeshFilter(swAssyLevel1, suffix, length, ansul, ansulSide, waterInlet, "FNHE0162-1", "FNHE0163-1");

        swAssyLevel2 = swAssyLevel1.GetSubAssemblyDoc(suffix, "UwSupport_Hw650-1", Aggregator);
        UvSupportHw650(swAssyLevel2, length, middleToRight, uvLightType);
    }
    private void FNHE0148(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, bool marvel, bool ansul, AnsulSide_e ansulSide, WaterInlet_e waterInlet)
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
                swCompLevel2.Suppress("UvDouble");
                break;
            case UvLightType_e.UVR4S:
            case UvLightType_e.UVR6S:
            case UvLightType_e.UVR8S:
                swCompLevel2.UnSuppress("UvRack");
                swModelLevel2.ChangeDim("ToRight@SketchUvRack", middleToRight);
                swModelLevel2.ChangeDim("UvRack@SketchUvRack", 930d);
                swCompLevel2.Suppress("UvDouble");
                break;
            case UvLightType_e.Double:
                swCompLevel2.Suppress("UvRack");
                swCompLevel2.UnSuppress("UvDouble");
                break;
            case UvLightType_e.NA:
            default:
                swCompLevel2.Suppress("UvRack");
                swCompLevel2.Suppress("UvDouble");
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
    private void FNHE0180(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, UvLightType_e uvLightType, double middleToRight, WaterInlet_e waterInlet)
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
                swCompLevel2.Suppress("UvDoorDouble");
                swCompLevel2.Suppress("UvCableDouble");
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
                swCompLevel2.Suppress("UvDoorDouble");
                swCompLevel2.Suppress("UvCableDouble");
                break;
            case UvLightType_e.Double:
                swCompLevel2.UnSuppress("TabUp");
                swCompLevel2.UnSuppress("SensorCable");
                swCompLevel2.UnSuppress("BaffleCable");
                swCompLevel2.Suppress("UvDoorShort");
                swCompLevel2.Suppress("UvDoorLong");
                swCompLevel2.Suppress("UvCable");
                swCompLevel2.UnSuppress("UvDoorDouble");
                swCompLevel2.UnSuppress("UvCableDouble");
                break;
            case UvLightType_e.NA:
            default:
                swCompLevel2.Suppress("TabUp");
                swCompLevel2.Suppress("SensorCable");
                swCompLevel2.Suppress("BaffleCable");
                swCompLevel2.Suppress("UvDoorShort");
                swCompLevel2.Suppress("UvDoorLong");
                swCompLevel2.Suppress("UvCable");
                swCompLevel2.Suppress("UvDoorDouble");
                swCompLevel2.Suppress("UvCableDouble");
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
    #endregion
}