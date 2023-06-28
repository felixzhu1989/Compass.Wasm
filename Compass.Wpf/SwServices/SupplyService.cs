using Compass.Wasm.Shared.Data;
using DocumentFormat.OpenXml.Drawing.Charts;
using SolidWorks.Interop.sldworks;
namespace Compass.Wpf.SwServices;

public class SupplyService : BaseSwService, ISupplyService
{
    public SupplyService(IContainerProvider provider) : base(provider)
    {
    }
    #region 标准烟罩
    public void I555(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Supply_I_555-1", Aggregator);

        //新风面板螺丝孔数量及间距,最小间距580，距离边缘150 2023/6/20
        const double sideDis = 150d;
        const double minFrontPanelNutDis = 580d;
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
        FNHA0003(swAssyLevel1, suffix, "FNHA0003-1", length,555d, midRoofNutDis, frontPanelNutDis);

        //集水翻边
        const double suHeight = 555d;
        const string leftPart = "FNHS0005-1";
        const string rightPart = "FNHS0006-1";
        WaterCollection(swAssyLevel1, suffix, waterCollection, sidePanel, exhaustType, width, height, suHeight, leftPart, rightPart);
    }

    public void F555(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection, int supplySpigotNumber, double supplySpigotDis)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Supply_F_555-1", Aggregator);

        //新风面板螺丝孔数量及间距,最小间距580，距离边缘150 2023/6/20
        const double sideDis = 150d;
        const double minFrontPanelNutDis = 580d;
        var frontPanelNutNumber = Math.Ceiling((length - 2*sideDis) / minFrontPanelNutDis);
        frontPanelNutNumber = frontPanelNutNumber < 2d ? 2d : frontPanelNutNumber;
        var frontPanelNutDis = (length -  2*sideDis) / (frontPanelNutNumber - 1);

        //MidRoof铆螺母孔 2023/3/10
        //修改MidRoof螺丝孔逻辑，以最低450间距计算间距即可
        const double minMidRoofNutDis = 450d;
        var midRoofNutNumber = Math.Ceiling((length - 2*sideDis) / minMidRoofNutDis);
        midRoofNutNumber = midRoofNutNumber < 3d ? 3d : midRoofNutNumber;
        var midRoofNutDis = (length -  2*sideDis)/(midRoofNutNumber-1);

        //新风网孔板加强筋
        if (length > 1599d) swAssyLevel1.UnSuppress(suffix, "FNHA0011-1", Aggregator);
        else swAssyLevel1.Suppress(suffix, "FNHA0011-1");


        //新风主体
        FNHA0004(swAssyLevel1, suffix, "FNHA0004-1", length, width, sidePanel, uvLightType, bluetooth, marvel, midRoofNutDis, supplySpigotNumber, supplySpigotDis);

        //F新风底部CJ孔板,FNHA0005重用FNHA0002方法
        FNHA0002(swAssyLevel1, suffix, "FNHA0005-1", length, sidePanel, bluetooth, ledLogo, waterCollection, frontPanelNutDis);

        //F新风前面板，FNHA0007
        FNHA0007(swAssyLevel1, suffix, "FNHA0007-1", length,555d, midRoofNutDis, frontPanelNutDis);

        //镀锌板
        FNHA0006(swAssyLevel1, suffix, "FNHA0006-1", length);

        //新风滑门导轨
        FNHA0010(swAssyLevel1, suffix, "FNHA0010-1", length);
        FNHA0010(swAssyLevel1, suffix, "FNHE0010-1", length);

        //集水翻边
        const double suHeight = 555d;
        const string leftPart = "FNHS0005-1";
        const string rightPart = "FNHS0006-1";
        WaterCollection(swAssyLevel1, suffix, waterCollection, sidePanel, exhaustType, width, height, suHeight, leftPart, rightPart);
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

    public void I400(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Supply_I_400-1", Aggregator);

        //新风面板螺丝孔数量及间距,最小间距580，距离边缘150 2023/6/20
        const double sideDis = 150d;
        const double minFrontPanelNutDis = 580d;
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
        FNHA0001(swAssyLevel1, suffix, "FNHA0040-1", length, width, sidePanel, uvLightType, bluetooth, marvel, midRoofNutDis);
        //I新风底部CJ孔板
        FNHA0002(swAssyLevel1, suffix, "FNHA0002-1", length, sidePanel, bluetooth, ledLogo, waterCollection, frontPanelNutDis);
        //I新风前面板
        FNHA0003(swAssyLevel1, suffix, "FNHA0003-1", length,400d, midRoofNutDis, frontPanelNutDis);

        //集水翻边
        const double suHeight = 400d;
        const string leftPart = "FNHS0005-1";
        const string rightPart = "FNHS0006-1";
        WaterCollection(swAssyLevel1, suffix, waterCollection, sidePanel, exhaustType, width, height, suHeight, leftPart, rightPart);
    }


    #endregion

    #region 华为烟罩
    public void IHw650(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Supply_I_HW_650-1", Aggregator);

        //新风面板螺丝孔数量及间距,最小间距580，距离边缘150 2023/6/20
        const double sideDis = 150d;
        const double minFrontPanelNutDis = 580d;
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
        FNHA0113(swAssyLevel1, suffix, "FNHA0113-1", length, width, sidePanel, uvLightType, bluetooth, marvel, midRoofNutDis);
        //I新风底部CJ孔板
        FNHA0002(swAssyLevel1, suffix, "FNHA0114-1", length, sidePanel, bluetooth, ledLogo, waterCollection, frontPanelNutDis);
        //I新风前面板
        FNHA0003(swAssyLevel1, suffix, "FNHA0120-1", length,555d, midRoofNutDis, frontPanelNutDis);

        //集水翻边
        const double suHeight = 650d;
        const string leftPart = "FNHS0071-1";
        const string rightPart = "FNHS0072-1";
        WaterCollection(swAssyLevel1,suffix,waterCollection,sidePanel,exhaustType,width,height,suHeight,leftPart,rightPart);
    }

    public void FHw650(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection, int supplySpigotNumber, double supplySpigotDis)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "Supply_F_HW_650-1", Aggregator);

        //新风面板螺丝孔数量及间距,最小间距580，距离边缘150 2023/6/20
        const double sideDis = 150d;
        const double minFrontPanelNutDis = 580d;
        var frontPanelNutNumber = Math.Ceiling((length - 2*sideDis) / minFrontPanelNutDis);
        frontPanelNutNumber = frontPanelNutNumber < 2d ? 2d : frontPanelNutNumber;
        var frontPanelNutDis = (length -  2*sideDis) / (frontPanelNutNumber - 1);

        //MidRoof铆螺母孔 2023/3/10
        //修改MidRoof螺丝孔逻辑，以最低450间距计算间距即可
        const double minMidRoofNutDis = 450d;
        var midRoofNutNumber = Math.Ceiling((length - 2*sideDis) / minMidRoofNutDis);
        midRoofNutNumber = midRoofNutNumber < 3d ? 3d : midRoofNutNumber;
        var midRoofNutDis = (length -  2*sideDis)/(midRoofNutNumber-1);

        //新风网孔板加强筋
        if (length > 1599d) swAssyLevel1.UnSuppress(suffix, "FNHA0124-1", Aggregator);
        else swAssyLevel1.Suppress(suffix, "FNHA0124-1");


        //新风主体
        FNHA0108(swAssyLevel1, suffix, "FNHA0108-1", length, width, sidePanel, uvLightType, bluetooth, marvel, midRoofNutDis, supplySpigotNumber, supplySpigotDis);

        //F新风底部CJ孔板,FNHA0005重用FNHA0002方法
        FNHA0002(swAssyLevel1, suffix, "FNHA0093-1", length, sidePanel, bluetooth, ledLogo, waterCollection, frontPanelNutDis);

        //F新风前面板，FNHA0007
        FNHA0007(swAssyLevel1, suffix, "FNHA0107-1", length,555d, midRoofNutDis, frontPanelNutDis);

        //镀锌板
        FNHA0006(swAssyLevel1, suffix, "FNHA0006-1", length);

        //新风滑门导轨
        FNHA0097(swAssyLevel1, suffix, "FNHA0097-1", length);

        #region 新风脖颈
        if (supplySpigotNumber == 1)
        {
            swAssyLevel1.Suppress("LocalLPatternSu250");
        }
        else
        {
            swAssyLevel1.UnSuppress("LocalLPatternSu250");
            var swModelLevel1 = (ModelDoc2)swAssyLevel1;
            swModelLevel1.ChangeDim("Number@LocalLPatternSu250", supplySpigotNumber);
            swModelLevel1.ChangeDim("Dis@LocalLPatternSu250", supplySpigotDis);
        }
        #endregion

        //集水翻边
        const double suHeight = 650d;
        const string leftPart = "FNHS0071-1";
        const string rightPart = "FNHS0072-1";
        WaterCollection(swAssyLevel1, suffix, waterCollection, sidePanel, exhaustType, width, height, suHeight, leftPart, rightPart);
    }
    #endregion

    #region 集水翻边

    private void WaterCollection(AssemblyDoc swAssyLevel1,string suffix, bool waterCollection,SidePanel_e sidePanel,ExhaustType_e exhaustType,double width,double height,double suHeight, string leftPart, string rightPart)
    {
        if (waterCollection)
        {
            if (sidePanel == SidePanel_e.双)
            {
                FNHS0005(swAssyLevel1, suffix, leftPart, width, height, exhaustType, suHeight);
                FNHS0005(swAssyLevel1, suffix, rightPart, width, height, exhaustType, suHeight);
            }
            else if (sidePanel == SidePanel_e.左)
            {
                FNHS0005(swAssyLevel1, suffix, leftPart, width, height, exhaustType, suHeight);
                swAssyLevel1.Suppress(suffix, rightPart);
            }
            else if (sidePanel == SidePanel_e.右)
            {
                swAssyLevel1.Suppress(suffix, leftPart);
                FNHS0005(swAssyLevel1, suffix, rightPart, width, height, exhaustType, suHeight);
            }
            else
            {
                swAssyLevel1.Suppress(suffix, leftPart);
                swAssyLevel1.Suppress(suffix, rightPart);
            }
        }
        else
        {
            swAssyLevel1.Suppress(suffix, leftPart);
            swAssyLevel1.Suppress(suffix, rightPart);
        }
    }
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
            if (sidePanel is SidePanel_e.左 or SidePanel_e.双) swCompLevel2.UnSuppress("JunctionBoxUv");
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


    private void FNHA0003(AssemblyDoc swAssyLevel1, string suffix, string partName, double length,double suHeight, double midRoofNutDis, double frontPanelNutDis)
    {
        swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length - 2d);
        swModelLevel2.ChangeDim("Height@SketchBase", suHeight - 119d);

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
    private void FNHA0007(AssemblyDoc swAssyLevel1, string suffix, string partName, double length,double suHeight, double midRoofNutDis, double frontPanelNutDis)
    {
        swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length - 2d);
        swModelLevel2.ChangeDim("Height@SketchBase", suHeight - 119d);

        #region 新风前面板卡口，距离与铆螺母数量相同，无需重复计算
        swModelLevel2.ChangeDim("Dis@LPatternPlug", midRoofNutDis);
        //蜂窝板压紧结构
        var sideDis = midRoofNutDis / 2d + 150d;
        swModelLevel2.ChangeDim("Side@SketchHandBending", sideDis);
        swModelLevel2.ChangeDim("Dis@LPatternHandBending", midRoofNutDis);
        swModelLevel2.ChangeDim("Side@LPatternHandBending", sideDis-80d);
        swModelLevel2.ChangeDim("Dis@LPatternHandBendingHole", midRoofNutDis);
        swModelLevel2.ChangeDim("Side@LPatternHandBendingHole", sideDis-80d);

        #endregion

        #region 前面板螺丝孔
        swModelLevel2.ChangeDim("Dis@LPatternFrontPanelNut", frontPanelNutDis);
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
    //华为新风滑门导轨使用排风的结构
    private void FNHA0097(AssemblyDoc swAssyLevel1, string suffix, string partName, double length)
    {
        swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@Base-Flange", length - 200d);
    }
    #endregion

    #region 华为烟罩
    private void FNHA0113(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, double width, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, double midRoofNutDis)
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
            if (sidePanel is SidePanel_e.左 or SidePanel_e.双) swCompLevel2.UnSuppress("JunctionBoxUv");
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
        //if (marvel) swCompLevel2.UnSuppress("IrLhc2");
        //else swCompLevel2.Suppress("IrLhc2");
        #endregion
    }
    private void FNHA0108(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, double width, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, double midRoofNutDis, int supplySpigotNumber, double supplySpigotDis)
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
            swModelLevel2.ChangeDim("ToMiddle@SketchSu250", 0);
        }
        else
        {
            swCompLevel2.UnSuppress("LPatternSu250");
            swModelLevel2.ChangeDim("ToMiddle@SketchSu250", suToMiddle);
            swModelLevel2.ChangeDim("Number@LPatternSu250", supplySpigotNumber);
            swModelLevel2.ChangeDim("Dis@LPatternSu250", supplySpigotDis);

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
        //if (marvel) swCompLevel2.UnSuppress("IrLhc2");
        //else swCompLevel2.Suppress("IrLhc2");
        #endregion
    }
    #endregion

}