using Compass.Wasm.Shared.DataService;
using Compass.Wpf.Extensions;
using Prism.Ioc;
using SolidWorks.Interop.sldworks;
using System;

namespace Compass.Wpf.DrawingServices;

public class ExhaustService : BaseDrawingService, IExhaustService
{
    public ExhaustService(IContainerProvider provider) : base(provider)
    {
    }

    public void Kv555(AssemblyDoc swAssyTop, string suffix, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Exhaust_KV_555-1", Aggregator);
        //排风主体
        FNHE0001(swAssyLevel1, suffix, "FNHE0001-1", length, sidePanel, uvLightType, middleToRight, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotDis, drainType, waterCollection, backToBack, marvel, ansul, ansulSide, ansulDetector);
        //排风腔前面板
        FNHE0002(swAssyLevel1, suffix, "FNHE0002-1", length, UvLightType_e.NA, middleToRight);
        //KSA侧边
        KsaFilter(swAssyLevel1, suffix, length, "FNHE0003-1", "FNHE0004-1", "FNHE0005-1");
        //排风三角板
        ExhaustSide(swAssyLevel1, suffix, ansul, sidePanel, "5201030401-1", "5201030401-2");
        //排风滑门导轨
        ExhaustRail(swAssyLevel1, suffix, marvel, length, exhaustSpigotNumber, exhaustSpigotLength, exhaustSpigotWidth, exhaustSpigotDis, "FNCE0018-1", "FNCE0018-2");
        //Ksa阵列
        Ksa(swAssyLevel1, suffix, length);
    }

    public void ExhaustSpigotFs(AssemblyDoc swAssyTop, string suffix, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, bool marvel, bool ansul)
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
        //左，todo:有待细化Ansul探测器逻辑
        swAssyLevel1.UnSuppress(out swModelLevel2, suffix, "FNHE0008-1", Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", exhaustSpigotWidth);
        swModelLevel2.ChangeDim("Height@SketchBase", exhaustSpigotHeight);

        //右,
        swAssyLevel1.UnSuppress(out swModelLevel2, suffix, "FNHE0009-1", Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", exhaustSpigotWidth);
        swModelLevel2.ChangeDim("Height@SketchBase", exhaustSpigotHeight);

        //滑门
        swAssyLevel1.UnSuppress(out swModelLevel2, suffix, "FNCE0013-1", Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", exhaustSpigotLength / 2d + 10d);
        swModelLevel2.ChangeDim("Width@SketchBase", exhaustSpigotWidth + 20d);

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


    private void FNHE0001(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length);

        #region MidRoof铆螺母孔
        //2023/3/10 修改MidRoof螺丝孔逻辑，以最低450间距计算间距即可
        var midRoofNutNumber = Math.Ceiling((length - 300d) / 450d);
        midRoofNutNumber = midRoofNutNumber < 3 ? 3 : midRoofNutNumber;
        var midRoofNutDis = (length - 300d)/(midRoofNutNumber-1);
        swModelLevel2.ChangeDim("Dis@LPatternMidRoofNut", midRoofNutDis);
        #endregion

        #region UVHood
        if (uvLightType == UvLightType_e.UVR4L||uvLightType == UvLightType_e.UVR6L||uvLightType == UvLightType_e.UVR8L)
        {
            swCompLevel2.UnSuppress("UvRack");
            swModelLevel2.ChangeDim("ToRight@SketchUvRack", middleToRight);
            swModelLevel2.ChangeDim("UvRack@SketchUvRack", 1640d);
            swCompLevel2.UnSuppress("UvCable");
            swModelLevel2.ChangeDim("ToRight@SketchUvCable", middleToRight);
            swModelLevel2.ChangeDim("UvCable@SketchUvCable", 1500d);
        }
        else if (uvLightType == UvLightType_e.UVR4S||uvLightType == UvLightType_e.UVR6S||uvLightType == UvLightType_e.UVR8S)
        {
            swCompLevel2.UnSuppress("UvRack");
            swModelLevel2.ChangeDim("ToRight@SketchUvRack", middleToRight);
            swModelLevel2.ChangeDim("UvRack@SketchUvRack", 930d);
            swCompLevel2.UnSuppress("UvCable");
            swModelLevel2.ChangeDim("ToRight@SketchUvCable", middleToRight);
            swModelLevel2.ChangeDim("UvCable@SketchUvCable", 790d);
        }
        else
        {
            swCompLevel2.Suppress("UvRack");
            swCompLevel2.Suppress("UvCable");
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
        if (waterCollection && (sidePanel == SidePanel_e.双 || sidePanel == SidePanel_e.右)) swCompLevel2.UnSuppress("DrainChannelRight");
        else swCompLevel2.Suppress("DrainChannelRight");
        if (waterCollection  && (sidePanel == SidePanel_e.双 || sidePanel == SidePanel_e.左)) swCompLevel2.UnSuppress("DrainChannelLeft");
        else swCompLevel2.Suppress("DrainChannelLeft");
        #endregion

        #region 油塞

        if (drainType== DrainType_e.左油塞)
        {
            swCompLevel2.UnSuppress("DrainTapLeft");
            swCompLevel2.Suppress("DrainTapRight");
        }
        else if (drainType== DrainType_e.右油塞)
        {
            swCompLevel2.Suppress("DrainTapLeft");
            swCompLevel2.UnSuppress("DrainTapRight");
        }
        else
        {
            swCompLevel2.Suppress("DrainTapLeft");
            swCompLevel2.Suppress("DrainTapRight");
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
        swCompLevel2.Suppress("ChannelLeft");
        swCompLevel2.Suppress("ChannelRight");
        swCompLevel2.Suppress("AnsulSideLeft");
        swCompLevel2.Suppress("AnsulSideRight");
        swCompLevel2.Suppress("AnsulDetectorRight");
        swCompLevel2.Suppress("AnsulDetectorLeft");
        if (ansul)
        {
            //侧喷
            if (ansulSide==AnsulSide_e.左侧喷)
            {
                swCompLevel2.UnSuppress("ChannelLeft");
                swCompLevel2.UnSuppress("AnsulSideLeft");
            }
            else if (ansulSide==AnsulSide_e.右侧喷)
            {
                swCompLevel2.UnSuppress("ChannelRight");
                swCompLevel2.UnSuppress("AnsulSideRight");
            }


            //探测器
            if (ansulDetector == AnsulDetector_e.右探测器口|| ansulDetector == AnsulDetector_e.双侧探测器口)
            {
                swCompLevel2.UnSuppress("AnsulDetectorRight");
            }
            if (ansulDetector == AnsulDetector_e.左探测器口 || ansulDetector == AnsulDetector_e.双侧探测器口)
            {
                swCompLevel2.UnSuppress("AnsulDetectorLeft");
            }
        }
        #endregion

    }

    private void FNHE0002(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, UvLightType_e uvLightType,
        double middleToRight)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length);
        #region UVHood
        if (uvLightType==UvLightType_e.UVR4L||uvLightType==UvLightType_e.UVR6L||uvLightType==UvLightType_e.UVR8L)
        {
            swCompLevel2.UnSuppress("TabUp");
            swCompLevel2.UnSuppress("SensorCable");
            swCompLevel2.Suppress("UvDoorShort");
            swCompLevel2.UnSuppress("UvDoorLong");
            swModelLevel2.ChangeDim("ToRight@SketchUvDoorShort", middleToRight);
            swCompLevel2.UnSuppress("UvCable");
            swModelLevel2.ChangeDim("ToRight@SketchUvCable", middleToRight);
            swModelLevel2.ChangeDim("UvCable@SketchUvCable", 1500d);
        }
        else if (uvLightType==UvLightType_e.UVR4S||uvLightType==UvLightType_e.UVR6S||uvLightType==UvLightType_e.UVR8S)
        {
            swCompLevel2.UnSuppress("TabUp");
            swCompLevel2.UnSuppress("SensorCable");
            swCompLevel2.UnSuppress("UvDoorShort");
            swCompLevel2.Suppress("UvDoorLong");
            swModelLevel2.ChangeDim("ToRight@SketchUvDoorLong", middleToRight);
            swCompLevel2.UnSuppress("UvCable");
            swModelLevel2.ChangeDim("ToRight@SketchUvCable", middleToRight);
            swModelLevel2.ChangeDim("UvCable@SketchUvCable", 790d);
        }
        else
        {
            swCompLevel2.Suppress("TabUp");
            swCompLevel2.Suppress("SensorCable");
            swCompLevel2.Suppress("UvDoorShort");
            swCompLevel2.Suppress("UvDoorLong");
            swCompLevel2.Suppress("UvCable");
        }
        #endregion

    }

    private void KsaFilter(AssemblyDoc swAssyLevel1, string suffix, double length, string leftPart, string rightPart, string specialPart)
    {
        //KSA数量，KSA侧板长度(以全长计算)
        int ksaNo = (int)((length + 0.5d) / 498d);
        double ksaSideLength = (length - ksaNo * 498d) / 2d;

        ModelDoc2 swModelLevel2;
        if (ksaSideLength <= 2)
        {
            swAssyLevel1.Suppress(suffix, leftPart);
            swAssyLevel1.Suppress(suffix, rightPart);
            swAssyLevel1.Suppress(suffix, specialPart);
        }
        else if (ksaSideLength < 12d && ksaSideLength >2d)
        {
            swAssyLevel1.Suppress(suffix, leftPart);
            swAssyLevel1.Suppress(suffix, rightPart);
            swAssyLevel1.UnSuppress(out swModelLevel2, suffix, specialPart, Aggregator);
            swModelLevel2.ChangeDim("Length@SketchBase", ksaSideLength * 2d);
        }
        else if (ksaSideLength < 25d && ksaSideLength >= 12d)
        {
            swAssyLevel1.UnSuppress(out swModelLevel2, suffix, leftPart, Aggregator);
            swModelLevel2.ChangeDim("Length@SketchBase", ksaSideLength * 2);
            swAssyLevel1.Suppress(suffix, rightPart);
            swAssyLevel1.Suppress(suffix, specialPart);
        }
        else
        {
            swAssyLevel1.UnSuppress(out swModelLevel2, suffix, leftPart, Aggregator);
            swModelLevel2.ChangeDim("Length@SketchBase", ksaSideLength);
            swAssyLevel1.UnSuppress(out swModelLevel2, suffix, rightPart, Aggregator);
            swModelLevel2.ChangeDim("Length@SketchBase", ksaSideLength);
            swAssyLevel1.Suppress(suffix, specialPart);
        }
    }

    private void ExhaustSide(AssemblyDoc swAssyLevel1, string suffix, bool ansul, SidePanel_e sidePanel, string leftPart, string rightPart)
    {
        Component2 swCompLevel2;
        if (ansul && sidePanel == SidePanel_e.中)
        {
            swAssyLevel1.UnSuppress(suffix, leftPart, Aggregator);
            swCompLevel2 = swAssyLevel1.UnSuppress(suffix, rightPart, Aggregator);
            swCompLevel2.UnSuppress("AnsulDetector");
        }
        else if (ansul && sidePanel==SidePanel_e.左)
        {
            swAssyLevel1.Suppress(suffix, leftPart);
            swCompLevel2 = swAssyLevel1.UnSuppress(suffix, rightPart, Aggregator);
            swCompLevel2.UnSuppress("AnsulDetector");
        }
        else if (ansul&& sidePanel == SidePanel_e.右)
        {
            swAssyLevel1.Suppress(suffix, rightPart);
            swCompLevel2 = swAssyLevel1.UnSuppress(suffix, leftPart, Aggregator);
            swCompLevel2.FeatureByName("AnsulDetector");
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
            //根据脖颈数量计算导轨长度
            var railLength = exhaustSpigotNumber == 1
                ? exhaustSpigotLength * 2d + 100d
                : exhaustSpigotLength * 3d + exhaustSpigotDis + 100d;
            //导轨太长，那么只能总长减去100
            swModelLevel2.ChangeDim("Length@Base-Flange", railLength > length - 100d ? length - 100d : railLength);
        }
    }

    private void Ksa(AssemblyDoc swAssyLevel1, string suffix, double length)
    {
        //KSA数量，KSA侧板长度(以全长计算)
        int ksaNo = (int)((length + 1d) / 498d);
        double ksaSideLength = (length - ksaNo * 498d) / 2d;
        //KSA距离左边缘
        swAssyLevel1.ChangeDim("Ksa@DistanceKsa", ksaSideLength < 12d ? 0.5d : ksaSideLength);
        //判断KSA数量，KSA侧板长度，如果太小，则使用特殊小侧板侧边
        if (ksaNo == 1)
        {
            swAssyLevel1.Suppress("LocalLPatternKsa");
        }
        else
        {
            swAssyLevel1.UnSuppress("LocalLPatternKsa");
            swAssyLevel1.ChangeDim("KsaNumber@LocalLPatternKsa", ksaNo);
        }
    }

}