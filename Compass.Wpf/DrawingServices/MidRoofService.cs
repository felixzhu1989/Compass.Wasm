using Compass.Wasm.Shared.DataService;
using Compass.Wpf.Extensions;
using Prism.Ioc;
using SolidWorks.Interop.sldworks;
using System;

namespace Compass.Wpf.DrawingServices;

public class MidRoofService : BaseDrawingService, IMidRoofService
{

    public MidRoofService(IContainerProvider provider) : base(provider)
    {

    }


    public void MidRoofFs(AssemblyDoc swAssyTop, string suffix, double length, double width, ExhaustType_e exhaustType, UvLightType_e uvLightType, bool bluetooth, double middleToRight, LightType_e lightType, int spotLightNumber, double spotLightDistance, bool marvel, bool ansul, int ansulDropNumber, double ansulDropToFront, double ansulDropDis1, double ansulDropDis2, double ansulDropDis3, double ansulDropDis4, double ansulDropDis5, int ansulDetectorNumber, AnsulDetectorEnd_e ansulDetectorEnd, double ansulDetectorDis1, double ansulDetectorDis2, double ansulDetectorDis3, double ansulDetectorDis4, double ansulDetectorDis5)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "MidRoof_Fs-1", Aggregator);
        //计算净宽度，总宽度减去排风，减去新风再减1
        const double exWidth = 535d;//排风宽度
        const double suWidth = 360d;//新风宽度
        var netWidth = width - exWidth - suWidth;

        FNHM0001(swAssyLevel1, suffix, "FNHM0001-1", length, netWidth, exhaustType, uvLightType, bluetooth, middleToRight, lightType, spotLightNumber, spotLightDistance, marvel, ansul, ansulDropNumber, ansulDropToFront, ansulDropDis1, ansulDropDis2, ansulDropDis3, ansulDropDis4, ansulDropDis5, ansulDetectorNumber, ansulDetectorEnd, ansulDetectorDis1, ansulDetectorDis2, ansulDetectorDis3, ansulDetectorDis4, ansulDetectorDis5);

        //需要加强筋的条件
        if (width > 1649d && (length > 2000d||(lightType == LightType_e.短灯 && length > 1600d)))
        {
            swAssyLevel1.UnSuppress(suffix, "FNHM0006-2", Aggregator);
            FNHM0006(swAssyLevel1, suffix, "FNHM0006-1", netWidth);
        }
        else
        {
            swAssyLevel1.Suppress(suffix, "FNHM0006-2");
            swAssyLevel1.Suppress(suffix, "FNHM0006-1");
        }

        //槽钢长度
        swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, "2900100001-1", Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", width-100d);

        //IR安装支架
        if (marvel) swAssyLevel1.UnSuppress(suffix, "IR_LHC_2-1", Aggregator);
        else swAssyLevel1.Suppress(suffix, "IR_LHC_2-1");
    }

    public void FNHM0001(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, double netWidth, ExhaustType_e exhaustType, UvLightType_e uvLightType, bool bluetooth, double middleToRight, LightType_e lightType, int spotLightNumber, double spotLightDistance, bool marvel, bool ansul, int ansulDropNumber, double ansulDropToFront, double ansulDropDis1, double ansulDropDis2, double ansulDropDis3, double ansulDropDis4, double ansulDropDis5, int ansulDetectorNumber, AnsulDetectorEnd_e ansulDetectorEnd, double ansulDetectorDis1, double ansulDetectorDis2, double ansulDetectorDis3, double ansulDetectorDis4, double ansulDetectorDis5)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);

        swModelLevel2.ChangeDim("Length@SketchBase", length - 4d);
        swModelLevel2.ChangeDim("Width@SketchBase", netWidth + 226d);
        swModelLevel2.ChangeDim("Width@SketchWidth", netWidth - 1d);

        //因为后方一点距离前端固定90，这里计算前端一点移动的距离
        const double holeDis = 50d;//两孔间距，槽钢开孔50
        const double fixDis = 90d;

        var midRoofTopHoleDis = netWidth - fixDis - (int)((netWidth- fixDis - 2* holeDis) / holeDis) * holeDis;

        swModelLevel2.ChangeDim("Dis@SketchTopHole", midRoofTopHoleDis);
        //侧向连接孔
        swModelLevel2.ChangeDim("Dis@SketchSideHole", netWidth / 3d);

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
                swCompLevel2.UnSuppress("UvCable");
                swModelLevel2.ChangeDim("ToRight@SketchUvCable", middleToRight);
                swModelLevel2.ChangeDim("UvCable@SketchUvCable", 1500d);
                break;
            case UvLightType_e.UVR4S:
            case UvLightType_e.UVR6S:
            case UvLightType_e.UVR8S:
                swCompLevel2.UnSuppress("UvCable");
                swModelLevel2.ChangeDim("ToRight@SketchUvCable", middleToRight);
                swModelLevel2.ChangeDim("UvCable@SketchUvCable", 790);
                break;
            case UvLightType_e.NA:
            default:
                swCompLevel2.Suppress("UvCable");
                break;
        }
        #endregion

        #region 开方孔,UV或带MARVEL时解压
        //KsaTabCable，KSA感应线和测风压口
        //UvDoorCable，UV门感应线
        //BluetoothCable，蓝牙线出口（Logo走风机线，不需要）
        //CutFrontRight，风机线，都需要
        if (uvLightType!=UvLightType_e.NA)
        {
            //UV
            swCompLevel2.UnSuppress("KsaTabCable");
            swCompLevel2.UnSuppress("UvDoorCable");
        }
        else
        {
            //非UV
            if (marvel) swCompLevel2.UnSuppress("KsaTabCable");
            else swCompLevel2.Suppress("KsaTabCable");
            swCompLevel2.Suppress("UvDoorCable");
        }
        swCompLevel2.UnSuppress("CjFanCable");
        if (bluetooth) swCompLevel2.UnSuppress("BluetoothCable");
        else swCompLevel2.Suppress("BluetoothCable");
        #endregion

        #region 灯具选项
        swCompLevel2.Suppress("Led60");
        swCompLevel2.Suppress("LPatternLed60");
        swCompLevel2.Suppress("Led140");
        swCompLevel2.Suppress("LPatternLed140");
        swCompLevel2.Suppress("FsLong");
        swCompLevel2.Suppress("FsShort");
        var toMiddle = spotLightDistance * (spotLightNumber / 2d - 1d) + spotLightDistance / 2d;
        switch (lightType)
        {
            case LightType_e.筒灯60:
            {
                swCompLevel2.UnSuppress("Led60");
                if (spotLightNumber == 1) swModelLevel2.ChangeDim("ToMiddle@SketchLed60", 0d);
                else
                {
                    swModelLevel2.ChangeDim("ToMiddle@SketchLed60", toMiddle);
                    swCompLevel2.UnSuppress("LPatternLed60");
                    swModelLevel2.ChangeDim("Number@LPatternLed60", spotLightNumber);
                    swModelLevel2.ChangeDim("Dis@LPatternLed60", spotLightDistance);
                }
                break;
            }
            case LightType_e.筒灯140:
            {
                swCompLevel2.UnSuppress("Led140");
                if (spotLightNumber == 1) swModelLevel2.ChangeDim("ToMiddle@SketchLed140", 0d);
                else
                {
                    swModelLevel2.ChangeDim("ToMiddle@SketchLed140", toMiddle);
                    swCompLevel2.UnSuppress("LPatternLed140");
                    swModelLevel2.ChangeDim("Number@LPatternLed140", spotLightNumber);
                    swModelLevel2.ChangeDim("Dis@LPatternLed140", spotLightDistance);
                }
                break;
            }
            case LightType_e.长灯:
                swCompLevel2.UnSuppress("FsLong");
                break;
            case LightType_e.短灯:
                swCompLevel2.UnSuppress("FsShort");
                break;
            case LightType_e.NA:
            default:
                break;
        }
        #endregion

        #region ANSUL选项
        swCompLevel2.Suppress("AnsulDrop1");
        swCompLevel2.Suppress("AnsulDrop2");
        swCompLevel2.Suppress("AnsulDrop3");
        swCompLevel2.Suppress("AnsulDrop4");
        swCompLevel2.Suppress("AnsulDrop5");
        //UW/KW HOOD 水洗烟罩探测器在midRoof上，非水洗烟罩压缩
        swCompLevel2.Suppress("AnsulDetector1");
        swCompLevel2.Suppress("AnsulDetector2");
        swCompLevel2.Suppress("AnsulDetector3");
        swCompLevel2.Suppress("AnsulDetector4");
        swCompLevel2.Suppress("AnsulDetector5");
        swCompLevel2.Suppress("AnsulDetectorAcross");
        if (ansul)
        {
            #region Ansul下喷
            if (ansulDropNumber > 0)
            {
                swCompLevel2.UnSuppress("AnsulDrop1");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDrop1", ansulDropDis1);
                swModelLevel2.ChangeDim("DisY@SketchAnsulDrop1", ansulDropToFront - 360d);
            }
            if (ansulDropNumber > 1)
            {
                swCompLevel2.UnSuppress("AnsulDrop2");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDrop2", ansulDropDis2);
            }
            if (ansulDropNumber > 2)
            {
                swCompLevel2.UnSuppress("AnsulDrop3");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDrop3", ansulDropDis3);
            }
            if (ansulDropNumber > 3)
            {
                swCompLevel2.UnSuppress("AnsulDrop4");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDrop4", ansulDropDis4);
            }
            if (ansulDropNumber > 4)
            {
                swCompLevel2.UnSuppress("AnsulDrop5");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDrop5", ansulDropDis5);
            }
            #endregion

            #region Ansul探测器，水洗烟罩需要探测器安装在MidRoof上
            if (exhaustType is ExhaustType_e.KW or ExhaustType_e.UW or ExhaustType_e.CMOD)
            {
                //探测器
                swCompLevel2.UnSuppress("AnsulDetectorAcross");
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
            }
            #endregion
        }
        #endregion

        #region IR
        if (marvel)
        {
            swCompLevel2.UnSuppress("IrLhc2");
            swCompLevel2.UnSuppress("Ir2");
        }
        else
        {
            swCompLevel2.Suppress("IrLhc2");
            swCompLevel2.Suppress("Ir2");
        }
        #endregion

        #region CMOD NTC Sensor
        if (exhaustType==ExhaustType_e.CMOD) swCompLevel2.UnSuppress("NtcSensor");
        else swCompLevel2.Suppress("NtcSensor");
        #endregion
    }

    public void FNHM0006(AssemblyDoc swAssyLevel1, string suffix, string partName, double netWidth)
    {
        swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", netWidth-4d);
    }
}