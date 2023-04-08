using Compass.Wasm.Shared.DataService;
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
        var frontPanelNutNumber = Math.Ceiling((length - 300d) / 650d);
        frontPanelNutNumber = frontPanelNutNumber < 2d ? 2d : frontPanelNutNumber;
        var frontPanelNutDis = (length - 300d) / (frontPanelNutNumber - 1);

        //MidRoof铆螺母孔 2023/3/10
        //修改MidRoof螺丝孔逻辑，以最低450间距计算间距即可
        var midRoofNutNumber = Math.Ceiling((length - 300d) / 450d);
        midRoofNutNumber = midRoofNutNumber < 3d ? 3d : midRoofNutNumber;
        var midRoofNutDis = (length - 300d)/(midRoofNutNumber-1);

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

    public void F555(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType,
        SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection,
        int supplySpigotNumber, double supplySpigotDis)
    {
        



    }


    public void BackCj(AssemblyDoc swAssyTop, string suffix, bool backCj, double length, double height, double cjSpigotToRight)
    {
        if (!backCj)
        {
            swAssyTop.Suppress(suffix, "BackCj_Fs-1");
            return;
        }
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "BackCj_Fs-1", Aggregator);
        int cjNumber = (int)((length - 40d) / 30d);//天花烟罩马蹄形CJ孔阵列距离为30
        double firstCjDis = (length - 30d * cjNumber) / 2d;
        if (firstCjDis < 20d) firstCjDis += 20d;
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
        swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);//todo:?


        swModelLevel2.ChangeDim("Width@SketchHood", width);

        swModelLevel2.ChangeDim("SuHeight@SketchHood", suHeight);//新风
        swModelLevel2.ChangeDim("ExHeight@SketchHood", height);//排风

        // KVUV555排风尺寸，ExHeitht555，ExButton76.5，ExFront85,ExAngle135
        // KWUW排风尺寸，ExHeitht555，ExButton150，ExFront101,ExAngle145
        // 450高度排风没有水洗，ExHeitht450，ExButton105，ExFront50,ExAngle135
        // 角度特殊，不能除以1000,应当乘回去
        var exButton = height.Equals(450d) ? 105d : exhaustType == ExhaustType_e.KW||exhaustType == ExhaustType_e.UW ? 150d : 76.5d;
        var exFront = height.Equals(450d) ? 50d : exhaustType == ExhaustType_e.KW||exhaustType == ExhaustType_e.UW ? 101d : 85d;
        var exAngle = (height.Equals(450d) ? 135d : exhaustType == ExhaustType_e.KW||exhaustType == ExhaustType_e.UW ? 145d : 135d)*1000d* Math.PI/ 180d;

        swModelLevel2.ChangeDim("ExButton@SketchHood", exButton);
        swModelLevel2.ChangeDim("ExFront@SketchHood", exFront);
        swModelLevel2.ChangeDim("ExAngle@SketchHood", exAngle);
    }
    #endregion


    private void FNHA0001(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, double width, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, double midRoofNutDis)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", length);
        //吊装孔
        //因为后方一点距离前端固定90，这里计算前端一点移动的距离
        var midRoofTopHoleDis = width - 535d - 360d - 90d - (int)((width - 535d - 360d - 90d - 100d) / 50d) * 50d;
        swModelLevel2.ChangeDim("Dis@SketchTopHole", 200d - midRoofTopHoleDis);

        #region MidRoof铆螺母孔
        swModelLevel2.ChangeDim("Dis@LPatternMidRoofNut", midRoofNutDis);
        #endregion

        #region 新风前面板卡口，距离与铆螺母数量相同，无需重复计算
        swModelLevel2.ChangeDim("Dis@LPatternPlug", midRoofNutDis);
        #endregion

        #region IR
        if (marvel) swCompLevel2.UnSuppress("IrLhc2");
        else swCompLevel2.Suppress("IrLhc2");
        #endregion

        #region UV HOOD
        if (uvLightType!=UvLightType_e.No)
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

    }

    private void FNHA0002(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, SidePanel_e sidePanel, bool bluetooth, bool ledLogo, bool waterCollection, double frontPanelNutDis)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", length);
        //新风CJ孔数量和新风CJ孔第一个CJ距离边缘距离
        int frontCjNo = (int)((length - 30d) / 32d) + 1;
        double frontCjFirstDis = (length - (frontCjNo - 1) * 32d) / 2d;

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



}