using Compass.Wasm.Shared.Data;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.SwServices;

public class MidRoofService : BaseSwService, IMidRoofService
{
    public MidRoofService(IContainerProvider provider) : base(provider)
    {
    }
    public void MidRoofFs(AssemblyDoc swAssyTop, string suffix, double length, double width, ExhaustType_e exhaustType, UvLightType_e uvLightType, bool bluetooth, double middleToRight, LightType_e lightType, int spotLightNumber, double spotLightDistance, bool marvel, bool ansul, int ansulDropNumber, double ansulDropToFront, double ansulDropDis1, double ansulDropDis2, double ansulDropDis3, double ansulDropDis4, double ansulDropDis5, int ansulDetectorNumber, AnsulDetectorEnd_e ansulDetectorEnd, double ansulDetectorDis1, double ansulDetectorDis2, double ansulDetectorDis3, double ansulDetectorDis4, double ansulDetectorDis5)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "MidRoof_Fs-1", Aggregator);
        //计算净宽度，总宽度减去排风，减去新风再减1
        var exWidth = exhaustType==ExhaustType_e.CMOD ? 290d : 535d;//排风宽度,CMOD为290
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
        //2023.08.29,有筒灯时,UV时或者Marvel时需要安装UVC的支架，长度为宽度+40
        if (lightType is LightType_e.筒灯)
        {
            if (uvLightType is UvLightType_e.NA && !marvel)
            {
                swAssyLevel1.Suppress(suffix, "FNHM0045-2");
                swAssyLevel1.Suppress(suffix, "FNHM0045-1");
            }
            else
            {
                swAssyLevel1.UnSuppress(suffix, "FNHM0045-2", Aggregator);
                FNHM0045(swAssyLevel1, suffix, "FNHM0045-1", netWidth);
            }
        }
        else
        {
            swAssyLevel1.Suppress(suffix, "FNHM0045-2");
            swAssyLevel1.Suppress(suffix, "FNHM0045-1");
        }
        

        //槽钢长度
        swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, "2900100001-1", Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", width-100d);

        //IR安装支架
        if (marvel) swAssyLevel1.UnSuppress(suffix, "IR_LHC_2-1", Aggregator);
        else swAssyLevel1.Suppress(suffix, "IR_LHC_2-1");
    }

    public void MidRoofFr(AssemblyDoc swAssyTop, string suffix, double length, double width, ExhaustType_e exhaustType, UvLightType_e uvLightType, bool bluetooth, double middleToRight, LightType_e lightType, int spotLightNumber, double spotLightDistance, bool marvel, bool ansul, int ansulDropNumber, double ansulDropToFront, double ansulDropDis1, double ansulDropDis2, double ansulDropDis3, double ansulDropDis4, double ansulDropDis5, int ansulDetectorNumber, AnsulDetectorEnd_e ansulDetectorEnd, double ansulDetectorDis1, double ansulDetectorDis2, double ansulDetectorDis3, double ansulDetectorDis4, double ansulDetectorDis5)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "MidRoof_Fr-1", Aggregator);
        swAssyLevel1.ChangeDim("Length@DistanceLength", length);

        FNHM0041(swAssyLevel1, suffix, "FNHM0041-1", length, lightType);

        switch (lightType)
        {
            case LightType_e.长灯:
                swAssyLevel1.UnSuppress(suffix, "5201020405-1",Aggregator);
                swAssyLevel1.Suppress(suffix, "5201020404-1");
                swAssyLevel1.Suppress(suffix, "FNHM0044-1");
                break;
            case LightType_e.短灯:
                swAssyLevel1.UnSuppress(suffix, "5201020404-1", Aggregator);
                swAssyLevel1.Suppress(suffix, "5201020405-1");
                swAssyLevel1.Suppress(suffix, "FNHM0044-1");
                break;
            case LightType_e.筒灯:
                swAssyLevel1.Suppress(suffix, "5201020404-1");
                swAssyLevel1.Suppress(suffix, "5201020405-1");
                swAssyLevel1.UnSuppress(suffix, "FNHM0044-1", Aggregator);
                break;
            case LightType_e.NA:
            case LightType_e.飞利浦三防灯:
            default:
                swAssyLevel1.Suppress(suffix, "5201020404-1");
                swAssyLevel1.Suppress(suffix, "5201020405-1");
                swAssyLevel1.Suppress(suffix, "FNHM0044-1");
                break;
        }
    }

    public void MidRoofHw(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, UvLightType_e uvLightType, bool bluetooth, double middleToRight, LightType_e lightType, double lightToFront, int spotLightNumber, double spotLightDistance, bool marvel, bool ansul, int ansulDropNumber, double ansulDropToFront, double ansulDropDis1, double ansulDropDis2, double ansulDropDis3, double ansulDropDis4, double ansulDropDis5, int ansulDetectorNumber, AnsulDetectorEnd_e ansulDetectorEnd, double ansulDetectorDis1, double ansulDetectorDis2, double ansulDetectorDis3, double ansulDetectorDis4, double ansulDetectorDis5)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "MidRoof_Hw-1", Aggregator);
        //计算净宽度，总宽度减去排风，减去新风再减1
        const double exWidth = 535d;//排风宽度
        const double suWidth = 360d;//新风宽度
        var netWidth = width - exWidth - suWidth;

        FNHM0031(swAssyLevel1, suffix, "FNHM0031-1", length, netWidth, height, exhaustType, uvLightType, bluetooth, middleToRight, lightType, lightToFront, spotLightNumber, spotLightDistance, marvel, ansul, ansulDropNumber, ansulDropToFront, ansulDropDis1, ansulDropDis2, ansulDropDis3, ansulDropDis4, ansulDropDis5, ansulDetectorNumber, ansulDetectorEnd, ansulDetectorDis1, ansulDetectorDis2, ansulDetectorDis3, ansulDetectorDis4, ansulDetectorDis5);



        //槽钢长度
        swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, "2900100001-1", Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", width-100d);

        //IR安装支架
        //if (marvel) swAssyLevel1.UnSuppress(suffix, "IR_LHC_2-1", Aggregator);
        //else swAssyLevel1.Suppress(suffix, "IR_LHC_2-1");

        //华为侧板
        FNHM0032(swAssyLevel1, suffix, "FNHM0032-1", netWidth, height, ExhaustType_e.UV);
    }

    public void MidRoofKvv(AssemblyDoc swAssyTop, string suffix, double length, double width, double insidePanelWidth, double exhaustSpigotLength, double exhaustSpigotWidth, LightType_e lightType)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "MidRoof_Kvv-1", Aggregator);
        FNHM0008(swAssyLevel1, suffix, "FNHM0008-1", length, insidePanelWidth, lightType);

        var sideDis = width > 1400 ? 200d : 150d;
        var insHole = 50d * Convert.ToInt32((width / 2d - sideDis) / 50d);
        if (width > 1400)
        {
            swAssyLevel1.Suppress(suffix, "FNHM0007-1");
            swAssyLevel1.UnSuppress(out _, suffix, "FNHM0017-2", Aggregator);
            FNHM0007(swAssyLevel1, suffix, "FNHM0017-1", length, width, insHole, exhaustSpigotLength,
                exhaustSpigotWidth, lightType);
        }
        else
        {
            swAssyLevel1.Suppress(suffix, "FNHM0017-1");
            swAssyLevel1.Suppress(suffix, "FNHM0017-2");
            FNHM0007(swAssyLevel1, suffix, "FNHM0007-1", length, width, insHole, exhaustSpigotLength,
                exhaustSpigotWidth, lightType);
        }
    }




    #region 普通烟罩
    private void FNHM0001(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, double netWidth, ExhaustType_e exhaustType, UvLightType_e uvLightType, bool bluetooth, double middleToRight, LightType_e lightType, int spotLightNumber, double spotLightDistance, bool marvel, bool ansul, int ansulDropNumber, double ansulDropToFront, double ansulDropDis1, double ansulDropDis2, double ansulDropDis3, double ansulDropDis4, double ansulDropDis5, int ansulDetectorNumber, AnsulDetectorEnd_e ansulDetectorEnd, double ansulDetectorDis1, double ansulDetectorDis2, double ansulDetectorDis3, double ansulDetectorDis4, double ansulDetectorDis5)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);

        swModelLevel2.ChangeDim("Length@SketchBase", length - 4d);
        swModelLevel2.ChangeDim("Width@SketchBase", netWidth + 221d+4d);
        swModelLevel2.ChangeDim("Width@SketchWidth", netWidth - 2d);

        //因为后方一点距离前端固定90，这里计算前端一点移动的距离
        const double holeDis = 50d;//两孔间距，槽钢开孔50
        const double fixDis = 90d;

        var midRoofTopHoleDis = netWidth - fixDis - (int)((netWidth- fixDis - 2* holeDis) / holeDis) * holeDis;

        swModelLevel2.ChangeDim("Dis@SketchTopHole", midRoofTopHoleDis);
        //侧向连接孔
        swModelLevel2.ChangeDim("Dis@SketchSideHole", netWidth*2d / 3d);

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
                swModelLevel2.ChangeDim("UvCable@SketchUvCable", 790d);
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
        swCompLevel2.Suppress("SpotLight");
        swCompLevel2.Suppress("LPatternSpotLight");
        swCompLevel2.Suppress("FsLong");
        swCompLevel2.Suppress("FsShort");
        var toMiddle = spotLightDistance * (spotLightNumber / 2d - 1d) + spotLightDistance / 2d;
        switch (lightType)
        {
            case LightType_e.筒灯:
                {
                    swCompLevel2.UnSuppress("SpotLight");
                    if (spotLightNumber == 1) swModelLevel2.ChangeDim("ToMiddle@SketchSpotLight", 0d);
                    else
                    {
                        swModelLevel2.ChangeDim("ToMiddle@SketchSpotLight", toMiddle);
                        swCompLevel2.UnSuppress("LPatternSpotLight");
                        swModelLevel2.ChangeDim("Number@LPatternSpotLight", spotLightNumber);
                        swModelLevel2.ChangeDim("Dis@LPatternSpotLight", spotLightDistance);
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

    private void FNHM0006(AssemblyDoc swAssyLevel1, string suffix, string partName, double netWidth)
    {
        swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", netWidth-4d);
    }


    private void FNHM0045(AssemblyDoc swAssyLevel1, string suffix, string partName, double netWidth)
    {
        swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", netWidth+40d);
    }
    #endregion

    #region 法国烟罩
    private void FNHM0041(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, LightType_e lightType)
    {
        //随着烟罩长度的变化，MidRoof侧板根据灯具的不同发生变化，长灯1329，短灯755,多减去1dm
        var panelLength = (length - 1330d) / 2d;//长
        if (lightType is LightType_e.短灯) panelLength = (length - 756d) / 2d;//短

        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", panelLength);
    }
    #endregion


    #region 华为烟罩
    private void FNHM0031(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, double netWidth, double height, ExhaustType_e exhaustType, UvLightType_e uvLightType, bool bluetooth, double middleToRight, LightType_e lightType, double lightToFront, int spotLightNumber, double spotLightDistance, bool marvel, bool ansul, int ansulDropNumber, double ansulDropToFront, double ansulDropDis1, double ansulDropDis2, double ansulDropDis3, double ansulDropDis4, double ansulDropDis5, int ansulDetectorNumber, AnsulDetectorEnd_e ansulDetectorEnd, double ansulDetectorDis1, double ansulDetectorDis2, double ansulDetectorDis3, double ansulDetectorDis4, double ansulDetectorDis5)
    {
        var diff = height-555d;
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);

        swModelLevel2.ChangeDim("Length@SketchBase", length);
        swModelLevel2.ChangeDim("Width@SketchBase", netWidth + 221d+2d*diff);
        swModelLevel2.ChangeDim("Width@SketchWidth", netWidth - 3d);
        swModelLevel2.ChangeDim("Height@SketchHeight1", 85d+diff);//180,85
        swModelLevel2.ChangeDim("Height@SketchHeight2", 85d+diff);

        //侧向连接孔
        swModelLevel2.ChangeDim("Dis@SketchSide", netWidth*2d / 3d);

        #region MidRoof铆螺母孔
        //2023/3/10 修改MidRoof螺丝孔逻辑，以最低450间距计算间距即可
        const double sideDis = 150d;
        const double minMidRoofNutDis = 450d;
        var midRoofNutNumber = Math.Ceiling((length - 2*sideDis) / minMidRoofNutDis);
        midRoofNutNumber = midRoofNutNumber < 3 ? 3 : midRoofNutNumber;
        var midRoofNutDis = (length - 2d*sideDis)/(midRoofNutNumber-1);

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
                swCompLevel2.Suppress("UvCableDouble");
                break;
            case UvLightType_e.UVR4S:
            case UvLightType_e.UVR6S:
            case UvLightType_e.UVR8S:
                swCompLevel2.UnSuppress("UvCable");
                swModelLevel2.ChangeDim("ToRight@SketchUvCable", middleToRight);
                swModelLevel2.ChangeDim("UvCable@SketchUvCable", 790d);
                swCompLevel2.Suppress("UvCableDouble");
                break;
            case UvLightType_e.Double:
                swCompLevel2.Suppress("UvCable");
                swCompLevel2.UnSuppress("UvCableDouble");
                break;
            case UvLightType_e.NA:
            default:
                swCompLevel2.Suppress("UvCable");
                swCompLevel2.Suppress("UvCableDouble");
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

        var toFront = lightToFront==0 ? netWidth/2d : lightToFront - 360d;

        swCompLevel2.Suppress("SpotLight");
        swCompLevel2.Suppress("LPatternSpotLight");
        swCompLevel2.Suppress("FsLong");
        swCompLevel2.Suppress("FsShort");
        var toMiddle = spotLightDistance * (spotLightNumber / 2d - 1d) + spotLightDistance / 2d;
        switch (lightType)
        {
            case LightType_e.筒灯:
                {
                    swCompLevel2.UnSuppress("SpotLight");
                    swModelLevel2.ChangeDim("ToFront@SketchSpotLight", toFront);
                    if (spotLightNumber == 1) swModelLevel2.ChangeDim("ToMiddle@SketchSpotLight", 0d);
                    else
                    {
                        swModelLevel2.ChangeDim("ToMiddle@SketchSpotLight", toMiddle);
                        swCompLevel2.UnSuppress("LPatternSpotLight");
                        swModelLevel2.ChangeDim("Number@LPatternSpotLight", spotLightNumber);
                        swModelLevel2.ChangeDim("Dis@LPatternSpotLight", spotLightDistance);
                    }
                    break;
                }
            case LightType_e.长灯:
                swCompLevel2.UnSuppress("FsLong");
                swModelLevel2.ChangeDim("ToFront@SketchFsLong", toFront);
                break;
            case LightType_e.短灯:
                swCompLevel2.UnSuppress("FsShort");
                swModelLevel2.ChangeDim("ToFront@SketchFsShort", toFront);
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
                if (ansulDetectorNumber> 0)
                {
                    swCompLevel2.UnSuppress("AnsulDetector1");
                    swModelLevel2.ChangeDim("Dis@SketchAnsulDetector1", ansulDetectorDis1);
                    if (ansulDetectorEnd == AnsulDetectorEnd_e.左末端探测器|| (ansulDetectorEnd == AnsulDetectorEnd_e.右末端探测器 && ansulDetectorNumber == 1))
                        swModelLevel2.ChangeDim("c@SketchAnsulDetector1", 195d);
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
        //if (marvel)
        //{
        //    swCompLevel2.UnSuppress("IrLhc2");
        //    swCompLevel2.UnSuppress("Ir2");
        //}
        //else
        //{
        //    swCompLevel2.Suppress("IrLhc2");
        //    swCompLevel2.Suppress("Ir2");
        //}
        #endregion

        #region CMOD NTC Sensor
        //if (exhaustType==ExhaustType_e.CMOD) swCompLevel2.UnSuppress("NtcSensor");
        //else swCompLevel2.Suppress("NtcSensor");
        #endregion
    }

    private void FNHM0032(AssemblyDoc swAssyLevel1, string suffix, string partName, double netWidth, double height,
        ExhaustType_e exhaustType)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        //2023.6.2以前减3.5，因为MidRoof宽度方向塞不进去，多减1
        swModelLevel2.ChangeDim("Length@SketchBase", netWidth - 4.5d);
        swModelLevel2.ChangeDim("Height@SketchBase", height - 478d);

        //因为后方一点距离前端固定90，这里计算前端一点移动的距离
        const double holeDis = 50d; //两孔间距，槽钢开孔50
        const double fixDis = 90d;

        var midRoofTopHoleDis = netWidth - fixDis - (int)((netWidth - fixDis - 2 * holeDis) / holeDis) * holeDis;

        swModelLevel2.ChangeDim("Dis@SketchTopHole", midRoofTopHoleDis);
        //侧向连接孔
        swModelLevel2.ChangeDim("Dis@SketchSideHole", netWidth * 2d / 3d);
        swModelLevel2.ChangeDim("Dis@SketchSide", netWidth * 2d / 3d);

        if (exhaustType is ExhaustType_e.KW or ExhaustType_e.UW or ExhaustType_e.CMOD)
        {
            //探测器
            swCompLevel2.UnSuppress("AnsulDetectorAcross");
        }
        else
        {
            swCompLevel2.Suppress("AnsulDetectorAcross");
        }
    }
    #endregion

    #region KVV

    private void FNHM0007(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, double width, double insHole, double exhaustSpigotLength, double exhaustSpigotWidth, LightType_e lightType)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);

        swModelLevel2.ChangeDim("Length@SketchBase", length - 6d);
        swModelLevel2.ChangeDim("Width@SketchBase", width-4d);
        swModelLevel2.ChangeDim("Width@SketchMidRpoof", insHole);
        swModelLevel2.ChangeDim("Length@SketchSpigot", exhaustSpigotLength);
        swModelLevel2.ChangeDim("Width@SketchSpigot", exhaustSpigotWidth);
        //PhilipsLamp
        if (lightType==LightType_e.飞利浦三防灯) swCompLevel2.UnSuppress("PhilipsLamp");
        else swCompLevel2.Suppress("PhilipsLamp");
    }

    private void FNHM0008(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, double insidePanelWidth, LightType_e lightType)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);

        swModelLevel2.ChangeDim("Length@SketchBase", length - 6d);
        swModelLevel2.ChangeDim("Width@SketchBase", insidePanelWidth);
        //PhilipsLamp
        if (lightType==LightType_e.飞利浦三防灯) swCompLevel2.UnSuppress("PhilipsLamp");
        else swCompLevel2.Suppress("PhilipsLamp");
    }


    #endregion
}