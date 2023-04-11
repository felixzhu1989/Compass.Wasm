﻿using Compass.Wasm.Shared.DataService;
using Compass.Wpf.Extensions;
using Prism.Ioc;
using SolidWorks.Interop.sldworks;
using System;

namespace Compass.Wpf.DrawingServices;

public class SupplyService : BaseDrawingService, ISupplyService
{
    public SupplyService(IContainerProvider provider) : base(provider)
    {
    }
    public void I555(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Supply_I_555-1", Aggregator);

        //新风面板螺丝孔数量及间距,最小间距650，距离边缘150 2023/3/10
        const double sideDis=150d;
        const double minFrontPanelNutDis = 650d;
        var frontPanelNutNumber = Math.Ceiling((length - 2*sideDis) / minFrontPanelNutDis);
        frontPanelNutNumber = frontPanelNutNumber < 2d ? 2d : frontPanelNutNumber;
        var frontPanelNutDis = (length -  2*sideDis) / (frontPanelNutNumber - 1);

        //MidRoof铆螺母孔 2023/3/10
        //修改MidRoof螺丝孔逻辑，以最低450间距计算间距即可
        const double minMidRoofNutDis = 450d;
        var midRoofNutNumber = Math.Ceiling((length - 2*sideDis) / minMidRoofNutDis);
        midRoofNutNumber = midRoofNutNumber < 3d ? 3d : midRoofNutNumber;
        var midRoofNutDis = (length -  2*sideDis)/(midRoofNutNumber-1);

        //新风主体
        FNHA0001(swAssyLevel1, suffix, "FNHA0001-1", length, width, sidePanel, uvLightType, bluetooth, marvel, midRoofNutDis);
        //I新风底部CJ孔板
        FNHA0002(swAssyLevel1, suffix, "FNHA0002-1", length, sidePanel, bluetooth, ledLogo, waterCollection, frontPanelNutDis);
        //I新风前面板
        FNHA0003(swAssyLevel1, suffix, "FNHA0003-1", length, midRoofNutDis, frontPanelNutDis);

        //集水翻边
        if (waterCollection)
        {
            if (sidePanel == SidePanel_e.双)
            {
                FNHS0005(swAssyLevel1, suffix, "FNHS0005-1", width, height, exhaustType, 555d);
                FNHS0005(swAssyLevel1, suffix, "FNHS0006-1", width, height, exhaustType, 555d);
            }
            else if (sidePanel == SidePanel_e.左)
            {
                FNHS0005(swAssyLevel1, suffix, "FNHS0005-1", width, height, exhaustType, 555d);
                swAssyLevel1.Suppress(suffix, "FNHS0006-1");
            }
            else if (sidePanel == SidePanel_e.右)
            {
                FNHS0005(swAssyLevel1, suffix, "FNHS0006-1", width, height, exhaustType, 555d);
                swAssyLevel1.Suppress(suffix, "FNHS0005-1");
            }
            else
            {
                swAssyLevel1.Suppress(suffix, "FNHS0005-1");
                swAssyLevel1.Suppress(suffix, "FNHS0006-1");
            }
        }
        else
        {
            swAssyLevel1.Suppress(suffix, "FNHS0005-1");
            swAssyLevel1.Suppress(suffix, "FNHS0006-1");
        }
    }

    public void F555(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection, int supplySpigotNumber, double supplySpigotDis)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Supply_F_555-1", Aggregator);

        //新风面板螺丝孔数量及间距,最小间距650，距离边缘150 2023/3/10
        const double sideDis = 150d;
        const double minFrontPanelNutDis = 650d;
        var frontPanelNutNumber = Math.Ceiling((length - 2*sideDis) / minFrontPanelNutDis);
        frontPanelNutNumber = frontPanelNutNumber < 2d ? 2d : frontPanelNutNumber;
        var frontPanelNutDis = (length -  2*sideDis) / (frontPanelNutNumber - 1);

        //MidRoof铆螺母孔 2023/3/10
        //修改MidRoof螺丝孔逻辑，以最低450间距计算间距即可
        const double minMidRoofNutDis = 450d;
        var midRoofNutNumber = Math.Ceiling((length - 2*sideDis) / minMidRoofNutDis);
        midRoofNutNumber = midRoofNutNumber < 3d ? 3d : midRoofNutNumber;
        var midRoofNutDis = (length -  2*sideDis)/(midRoofNutNumber-1);

        //新风网孔板加强经
        if (length > 1599d) swAssyLevel1.UnSuppress(suffix, "FNHA0011-1",  Aggregator);
        else swAssyLevel1.Suppress(suffix,"FNHA0011-1" );


        //新风主体
        FNHA0004(swAssyLevel1, suffix, "FNHA0004-1", length, width, sidePanel, uvLightType, bluetooth, marvel, midRoofNutDis,supplySpigotNumber,supplySpigotDis);

        //F新风底部CJ孔板,FNHA0005重用FNHA0002方法
        FNHA0002(swAssyLevel1, suffix, "FNHA0005-1", length, sidePanel, bluetooth, ledLogo, waterCollection, frontPanelNutDis);

        //F新风前面板，FNHA0007，重用FNHA0003方法
        FNHA0003(swAssyLevel1, suffix, "FNHA0007-1", length, midRoofNutDis, frontPanelNutDis);

        //镀锌板
        FNHA0006(swAssyLevel1, suffix, "FNHA0006-1", length);

        //新风滑门导轨
        FNHA0010(swAssyLevel1, suffix, "FNHA0010-1", length);
        FNHA0010(swAssyLevel1, suffix, "FNHE0010-1", length);

        //集水翻边
        if (waterCollection)
        {
            if (sidePanel == SidePanel_e.双)
            {
                FNHS0005(swAssyLevel1, suffix, "FNHS0005-1", width, height, exhaustType, 555d);
                FNHS0005(swAssyLevel1, suffix, "FNHS0006-1", width, height, exhaustType, 555d);
            }
            else if (sidePanel == SidePanel_e.左)
            {
                FNHS0005(swAssyLevel1, suffix, "FNHS0005-1", width, height, exhaustType, 555d);
                swAssyLevel1.Suppress(suffix, "FNHS0006-1");
            }
            else if (sidePanel == SidePanel_e.右)
            {
                FNHS0005(swAssyLevel1, suffix, "FNHS0006-1", width, height, exhaustType, 555d);
                swAssyLevel1.Suppress(suffix, "FNHS0005-1");
            }
            else
            {
                swAssyLevel1.Suppress(suffix, "FNHS0005-1");
                swAssyLevel1.Suppress(suffix, "FNHS0006-1");
            }
        }
        else
        {
            swAssyLevel1.Suppress(suffix, "FNHS0005-1");
            swAssyLevel1.Suppress(suffix, "FNHS0006-1");
        }
    }


    public void BackCj(AssemblyDoc swAssyTop, string suffix, bool backCj, double length, double height, double cjSpigotToRight)
    {
        if (!backCj)
        {
            swAssyTop.Suppress(suffix, "BackCj_Fs-1");
            return;
        }
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "BackCj_Fs-1", Aggregator);
        
        const double cjHoleDis = 30d;//天花烟罩马蹄形CJ孔阵列距离为30
        const double minCjDis = 20d;//距边最小距离
        int cjNumber = (int)((length - 2* minCjDis) / cjHoleDis);
        double firstCjDis = (length - cjHoleDis * cjNumber) / 2d;
        if (firstCjDis < minCjDis) firstCjDis += minCjDis;

        swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, "FNHA0084-1", Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length);
        swModelLevel2.ChangeDim("Height@SketchBase", height+1d);
        swModelLevel2.ChangeDim("Dis@SketchCj", firstCjDis);
        swModelLevel2.ChangeDim("ToRight@SketchSpigot", cjSpigotToRight);

        swAssyLevel1.UnSuppress(out swModelLevel2, suffix, "FNCJ0016-1", Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length-10d);

        swAssyLevel1.UnSuppress(out swModelLevel2, suffix, "FNHE0102-1", Aggregator);
        swModelLevel2.ChangeDim("Height@SketchBase", height-1d);
    }





    #region 集水翻边

    private void FNHS0005(AssemblyDoc swAssyLevel1, string suffix, string partName, double width, double height, ExhaustType_e exhaustType, double suHeight)
    {
        // KVUV555排风尺寸，     ExHeitht555，ExButton76.5，ExFront85,ExAngle135
        // KWUW排风尺寸，        ExHeitht555，ExButton150，ExFront101,ExAngle145
        // 450高度排风没有水洗，  ExHeitht450，ExButton105，ExFront50,ExAngle135
        var exButton = height.Equals(450d) ? 105d : exhaustType == ExhaustType_e.KW||exhaustType == ExhaustType_e.UW ? 150d : 76.5d;
        var exFront = height.Equals(450d) ? 50d : exhaustType == ExhaustType_e.KW||exhaustType == ExhaustType_e.UW ? 101d : 85d;
        // 角度特殊，不能除以1000,应当乘回去
        var exAngle = (height.Equals(450d) ? 135d : exhaustType == ExhaustType_e.KW||exhaustType == ExhaustType_e.UW ? 145d : 135d)*1000d* Math.PI/ 180d;


        swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);//todo:?
        swModelLevel2.ChangeDim("Width@SketchHood", width);
        swModelLevel2.ChangeDim("SuHeight@SketchHood", suHeight);//新风
        swModelLevel2.ChangeDim("ExHeight@SketchHood", height);//排风

        swModelLevel2.ChangeDim("ExButton@SketchHood", exButton);
        swModelLevel2.ChangeDim("ExFront@SketchHood", exFront);
        swModelLevel2.ChangeDim("ExAngle@SketchHood", exAngle);
    }
    #endregion


    #region I555
    private void FNHA0001(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, double width, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, double midRoofNutDis)
    {
        //因为后方一点距离前端固定90，这里计算前端一点移动的距离
        const double exWidth = 535d;//排风宽度
        const double suWidth = 360d;//新风宽度
        const double holeDis = 50d;//两孔间距，槽钢开孔50
        const double fixDis = exWidth+suWidth+ 90d;//减去的固定间距为孔距边90和新风排风宽度

        var midRoofTopHoleDis = width - fixDis - (int)((width - fixDis -2* holeDis) / holeDis) * holeDis;

        var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", length);
        //吊装孔
        swModelLevel2.ChangeDim("Dis@SketchTopHole", 4*holeDis - midRoofTopHoleDis);

        #region MidRoof铆螺母孔
        swModelLevel2.ChangeDim("Dis@LPatternMidRoofNut", midRoofNutDis);
        #endregion

        #region 新风前面板卡口，距离与铆螺母数量相同，无需重复计算
        swModelLevel2.ChangeDim("Dis@LPatternPlug", midRoofNutDis);
        #endregion

        

        #region UV HOOD
        if (uvLightType!=UvLightType_e.NA)
        {
            if (bluetooth) swCompLevel2.UnSuppress("BluetoothCable");
            else swCompLevel2.Suppress("BluetoothCable");
            if (sidePanel == SidePanel_e.左 || sidePanel== SidePanel_e.双) swCompLevel2.UnSuppress("JunctionBoxUv");
            else swCompLevel2.Suppress("JunctionBoxUv");
        }
        else
        {
            swCompLevel2.Suppress("BluetoothCable");
            if (marvel) swCompLevel2.UnSuppress("JunctionBoxUv");
            else swCompLevel2.Suppress("JunctionBoxUv");
        }
        #endregion

        #region IR
        if (marvel) swCompLevel2.UnSuppress("IrLhc2");
        else swCompLevel2.Suppress("IrLhc2");
        #endregion
    }

    private void FNHA0002(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, SidePanel_e sidePanel, bool bluetooth, bool ledLogo, bool waterCollection, double frontPanelNutDis)
    {
        //新风CJ孔数量和新风CJ孔第一个CJ距离边缘距离
        const double cjHoleDis = 32d;//CJ孔间距
        int frontCjNo = (int)((length - cjHoleDis) / cjHoleDis) + 1;
        double frontCjFirstDis = (length - (frontCjNo - 1) * cjHoleDis) / 2d;

        var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", length);


        //第一个CJ孔距离边缘
        swModelLevel2.ChangeDim("Dis@SketchCjSide", frontCjFirstDis);

        #region 前面板螺丝孔
        swModelLevel2.ChangeDim("Dis@LPatternFrontPanelNut", frontPanelNutDis);
        #endregion

        #region Logo与蓝牙
        if (bluetooth) swCompLevel2.UnSuppress("Bluetooth");
        else swCompLevel2.Suppress("Bluetooth");
        if (ledLogo) swCompLevel2.UnSuppress("Logo");
        else swCompLevel2.Suppress("Logo");
        #endregion

        #region 集水翻边
        if (waterCollection && (sidePanel == SidePanel_e.右 || sidePanel == SidePanel_e.双)) swCompLevel2.UnSuppress("DrainChannelRight");
        else swCompLevel2.Suppress("DrainChannelRight");

        if (waterCollection && (sidePanel == SidePanel_e.左|| sidePanel == SidePanel_e.双)) swCompLevel2.UnSuppress("DrainChannelLeft");
        else swCompLevel2.Suppress("DrainChannelLeft");
        #endregion
    }


    private void FNHA0003(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, double midRoofNutDis, double frontPanelNutDis)
    {
        swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length - 2d);

        #region 新风前面板卡口，距离与铆螺母数量相同，无需重复计算
        swModelLevel2.ChangeDim("Dis@LPatternPlug", midRoofNutDis);
        #endregion

        #region 前面板螺丝孔
        swModelLevel2.ChangeDim("Dis@LPatternFrontPanelNut", frontPanelNutDis);
        #endregion
    }
    #endregion

    #region F555
    private void FNHA0004(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, double width, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, double midRoofNutDis, int supplySpigotNumber, double supplySpigotDis)
    {
        //因为后方一点距离前端固定90，这里计算前端一点移动的距离
        const double exWidth = 535d;//排风宽度
        const double suWidth = 360d;//新风宽度
        const double holeDis = 50d;//两孔间距，槽钢开孔50
        const double fixDis = exWidth+suWidth+ 90d;//减去的固定间距为孔距边90和新风排风宽度

        var midRoofTopHoleDis = width - fixDis - (int)((width - fixDis -2* holeDis) / holeDis) * holeDis;
        var suToMiddle = supplySpigotDis * (supplySpigotNumber/2-1)+supplySpigotDis/2d;//新风脖颈口距离中间位置

        var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", length);
        #region 新风脖颈
        if (supplySpigotNumber == 1)
        {
            swCompLevel2.Suppress("LPatternSu250");
            swModelLevel2.ChangeDim("ToMiddle@SketchSu250",0);
        }
        else
        {
            swCompLevel2.UnSuppress("LPatternSu250");
            swModelLevel2.ChangeDim("ToMiddle@SketchSu250", suToMiddle);
            swModelLevel2.ChangeDim("Number@LPatternSu250", supplySpigotNumber);
            swModelLevel2.ChangeDim("Dis@LPatternSu250",supplySpigotDis);

        }
        #endregion

        //吊装孔
        swModelLevel2.ChangeDim("Dis@SketchTopHole", 4*holeDis - midRoofTopHoleDis);

        #region MidRoof铆螺母孔
        swModelLevel2.ChangeDim("Dis@LPatternMidRoofNut", midRoofNutDis);
        #endregion

        #region 新风前面板卡口，距离与铆螺母数量相同，无需重复计算
        swModelLevel2.ChangeDim("Dis@LPatternPlug", midRoofNutDis);
        #endregion
        
        #region UV HOOD
        if (uvLightType!=UvLightType_e.NA)
        {
            if (bluetooth) swCompLevel2.UnSuppress("BluetoothCable");
            else swCompLevel2.Suppress("BluetoothCable");
            if (sidePanel == SidePanel_e.左 || sidePanel== SidePanel_e.双) swCompLevel2.UnSuppress("JunctionBoxUv");
            else swCompLevel2.Suppress("JunctionBoxUv");
        }
        else
        {
            swCompLevel2.Suppress("BluetoothCable");
            if (marvel) swCompLevel2.UnSuppress("JunctionBoxUv");
            else swCompLevel2.Suppress("JunctionBoxUv");
        }
        #endregion

        #region IR
        if (marvel) swCompLevel2.UnSuppress("IrLhc2");
        else swCompLevel2.Suppress("IrLhc2");
        #endregion
    }

    //镀锌板
    private void FNHA0006(AssemblyDoc swAssyLevel1, string suffix, string partName, double length)
    {
        swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length - 6d);
    }

    //新风滑门导轨
    private void FNHA0010(AssemblyDoc swAssyLevel1, string suffix, string partName, double length)
    {
        swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length - 200d);
    }


    #endregion



}