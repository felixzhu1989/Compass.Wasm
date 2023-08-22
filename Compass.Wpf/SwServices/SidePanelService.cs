using Compass.Wasm.Shared.Data;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.SwServices;

public class SidePanelService : BaseSwService, ISidePanelService
{
    public SidePanelService(IContainerProvider provider) : base(provider)
    {
    }
    public void SidePanelFs(AssemblyDoc swAssyTop, string suffix, SidePanel_e sidePanel, double length, double width, double height, bool backCj, ExhaustType_e exhaustType)
    {
        //KV/UV烟罩侧边CJ留出安全距离110
        //KV/UV450烟罩安全距离140
        //KW/UW烟罩侧边CJ流出安全距离190
        var sideCjEnd = 110d;
        if (height == 555d)
        {
            sideCjEnd= exhaustType is ExhaustType_e.KW or ExhaustType_e.UW ? 190d : 110d;
        }
        else if (height ==450d)
        {
            sideCjEnd = 140d;
        }
        if (backCj) sideCjEnd += 90d;//有BackCj时需要让侧面CJ孔避让排风腔

        //todo:是否需要合并M型烟罩
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "SidePanel_Fs-1", Aggregator);


        if (sidePanel == SidePanel_e.双)
        {
            FNHS0001(swAssyLevel1, suffix, "FNHS0001-1", width, height, backCj, sideCjEnd);
            FNHS0002(swAssyLevel1, suffix, "FNHS0002-1", width, height, backCj);
            FNHS0001(swAssyLevel1, suffix, "FNHS0003-1", width, height, backCj, sideCjEnd);
            FNHS0002(swAssyLevel1, suffix, "FNHS0004-1", width, height, backCj);
            swAssyLevel1.ChangeDim("Length@DistanceLeft", length/2d);
            swAssyLevel1.ChangeDim("Length@DistanceRight", length/2d);
        }
        else if (sidePanel == SidePanel_e.左)
        {
            FNHS0001(swAssyLevel1, suffix, "FNHS0001-1", width, height, backCj, sideCjEnd);
            FNHS0002(swAssyLevel1, suffix, "FNHS0002-1", width, height, backCj);
            swAssyLevel1.Suppress(suffix, "FNHS0003-1");
            swAssyLevel1.Suppress(suffix, "FNHS0004-1");
            swAssyLevel1.ChangeDim("Length@DistanceLeft", length/2d);
        }
        else if (sidePanel == SidePanel_e.右)
        {
            swAssyLevel1.Suppress(suffix, "FNHS0001-1");
            swAssyLevel1.Suppress(suffix, "FNHS0002-1");
            FNHS0001(swAssyLevel1, suffix, "FNHS0003-1", width, height, backCj, sideCjEnd);
            FNHS0002(swAssyLevel1, suffix, "FNHS0004-1", width, height, backCj);
            swAssyLevel1.ChangeDim("Length@DistanceRight", length/2d);
        }
        else
        {
            swAssyLevel1.Suppress(suffix, "FNHS0001-1");
            swAssyLevel1.Suppress(suffix, "FNHS0002-1");
            swAssyLevel1.Suppress(suffix, "FNHS0003-1");
            swAssyLevel1.Suppress(suffix, "FNHS0004-1");
        }
    }

    public void SidePanelFr(AssemblyDoc swAssyTop, string suffix, SidePanel_e sidePanel, double length, double width, double height, bool backCj, ExhaustType_e exhaustType)
    {
        //KV/UV烟罩侧边CJ留出安全距离110
        //KV/UV450烟罩安全距离140
        //KW/UW烟罩侧边CJ流出安全距离190
        var sideCjEnd = 110d;
        if (height == 555d)
        {
            sideCjEnd= exhaustType is ExhaustType_e.KW or ExhaustType_e.UW ? 190d : 110d;
        }
        else if (height ==450d)
        {
            sideCjEnd = 140d;
        }
        if (backCj) sideCjEnd += 90d;//有BackCj时需要让侧面CJ孔避让排风腔

        //todo:是否需要合并M型烟罩
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "SidePanel_Fr-1", Aggregator);


        if (sidePanel == SidePanel_e.双)
        {
            FNHS0073(swAssyLevel1, suffix, "FNHS0073-1", width, height, backCj, sideCjEnd);
            FNHS0075(swAssyLevel1, suffix, "FNHS0075-1", width, height, backCj);
            FNHS0073(swAssyLevel1, suffix, "FNHS0074-1", width, height, backCj, sideCjEnd);
            FNHS0075(swAssyLevel1, suffix, "FNHS0076-1", width, height, backCj);
            swAssyLevel1.ChangeDim("Length@DistanceLeft", length/2d);
            swAssyLevel1.ChangeDim("Length@DistanceRight", length/2d);
        }
        else if (sidePanel == SidePanel_e.左)
        {
            FNHS0073(swAssyLevel1, suffix, "FNHS0073-1", width, height, backCj, sideCjEnd);
            FNHS0075(swAssyLevel1, suffix, "FNHS0075-1", width, height, backCj);
            swAssyLevel1.Suppress(suffix, "FNHS0003-1");
            swAssyLevel1.Suppress(suffix, "FNHS0004-1");
            swAssyLevel1.ChangeDim("Length@DistanceLeft", length/2d);
        }
        else if (sidePanel == SidePanel_e.右)
        {
            swAssyLevel1.Suppress(suffix, "FNHS0001-1");
            swAssyLevel1.Suppress(suffix, "FNHS0002-1");
            FNHS0073(swAssyLevel1, suffix, "FNHS0074-1", width, height, backCj, sideCjEnd);
            FNHS0075(swAssyLevel1, suffix, "FNHS0076-1", width, height, backCj);
            swAssyLevel1.ChangeDim("Length@DistanceRight", length/2d);
        }
        else
        {
            swAssyLevel1.Suppress(suffix, "FNHS0073-1");
            swAssyLevel1.Suppress(suffix, "FNHS0075-1");
            swAssyLevel1.Suppress(suffix, "FNHS0074-1");
            swAssyLevel1.Suppress(suffix, "FNHS0076-1");
        }
    }

    public void SidePanelHw(AssemblyDoc swAssyTop, string suffix, SidePanel_e sidePanel, double length, double width, double height, bool backCj, ExhaustType_e exhaustType)
    {
        //KV/UV烟罩侧边CJ留出安全距离110
        //KW/UW烟罩侧边CJ流出安全距离190
        var sideCjEnd = exhaustType is ExhaustType_e.KW or ExhaustType_e.UW ? 190d : 110d;

        //todo:是否需要合并M型烟罩
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "SidePanel_Hw-1", Aggregator);


        if (sidePanel == SidePanel_e.双)
        {
            FNHS0067(swAssyLevel1, suffix, "FNHS0067-1", width, height, backCj, sideCjEnd);
            FNHS0068(swAssyLevel1, suffix, "FNHS0068-1", width, height, backCj);
            FNHS0067(swAssyLevel1, suffix, "FNHS0069-1", width, height, backCj, sideCjEnd);
            FNHS0068(swAssyLevel1, suffix, "FNHS0070-1", width, height, backCj);
            swAssyLevel1.ChangeDim("Length@DistanceLeft", length/2d);
            swAssyLevel1.ChangeDim("Length@DistanceRight", length/2d);
        }
        else if (sidePanel == SidePanel_e.左)
        {
            FNHS0067(swAssyLevel1, suffix, "FNHS0067-1", width, height, backCj, sideCjEnd);
            FNHS0068(swAssyLevel1, suffix, "FNHS0068-1", width, height, backCj);
            swAssyLevel1.Suppress(suffix, "FNHS0069-1");
            swAssyLevel1.Suppress(suffix, "FNHS0070-1");
            swAssyLevel1.ChangeDim("Length@DistanceLeft", length/2d);
        }
        else if (sidePanel == SidePanel_e.右)
        {
            swAssyLevel1.Suppress(suffix, "FNHS0067-1");
            swAssyLevel1.Suppress(suffix, "FNHS0068-1");
            FNHS0067(swAssyLevel1, suffix, "FNHS0069-1", width, height, backCj, sideCjEnd);
            FNHS0068(swAssyLevel1, suffix, "FNHS0070-1", width, height, backCj);
            swAssyLevel1.ChangeDim("Length@DistanceRight", length/2d);
        }
        else
        {
            swAssyLevel1.Suppress(suffix, "FNHS0067-1");
            swAssyLevel1.Suppress(suffix, "FNHS0068-1");
            swAssyLevel1.Suppress(suffix, "FNHS0069-1");
            swAssyLevel1.Suppress(suffix, "FNHS0070-1");
        }
    }


    public void SidePanelNeq(AssemblyDoc swAssyTop, string suffix, SidePanel_e sidePanel, double length, double width, double height,double suHeight, bool backCj, ExhaustType_e exhaustType)
    {
        //KV/UV烟罩侧边CJ留出安全距离30
        //KW/UW烟罩侧边CJ流出安全距离110
        var sideCjEnd = exhaustType is ExhaustType_e.KW or ExhaustType_e.UW ? 110d : 30d;
        //
        var bottom = 77d;
        if (height == 555d)
        {
            bottom=exhaustType is ExhaustType_e.CMOD? 175d: exhaustType is ExhaustType_e.KW or ExhaustType_e.UW ? 150d : 77d;
        }
        else if(height ==450d)
        {
            bottom = 106d;
        }
        if (backCj) bottom += 90d;



        //todo:是否需要合并M型烟罩
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "SidePanel_Neq-1", Aggregator);
        if (sidePanel == SidePanel_e.双)
        {
            FNHS0012(swAssyLevel1, suffix, "FNHS0012-1", width, height,suHeight, backCj, sideCjEnd,bottom);
            FNHS0013(swAssyLevel1, suffix, "FNHS0013-1", width, height, suHeight, backCj, bottom);

            FNHS0012(swAssyLevel1, suffix, "FNHS0014-1", width, height, suHeight, backCj, sideCjEnd, bottom);
            FNHS0013(swAssyLevel1, suffix, "FNHS0015-1", width, height, suHeight, backCj, bottom);

            swAssyLevel1.ChangeDim("Length@DistanceLeft", length/2d);
            swAssyLevel1.ChangeDim("Length@DistanceRight", length/2d);
        }
        else if (sidePanel == SidePanel_e.左)
        {
            FNHS0012(swAssyLevel1, suffix, "FNHS0012-1", width, height, suHeight, backCj, sideCjEnd, bottom);
            FNHS0013(swAssyLevel1, suffix, "FNHS0013-1", width, height, suHeight, backCj, bottom);
            swAssyLevel1.Suppress(suffix, "FNHS0014-1");
            swAssyLevel1.Suppress(suffix, "FNHS0015-1");
            swAssyLevel1.ChangeDim("Length@DistanceLeft", length/2d);
        }
        else if (sidePanel == SidePanel_e.右)
        {
            swAssyLevel1.Suppress(suffix, "FNHS0012-1");
            swAssyLevel1.Suppress(suffix, "FNHS0013-1");
            FNHS0012(swAssyLevel1, suffix, "FNHS0014-1", width, height, suHeight, backCj, sideCjEnd, bottom);
            FNHS0013(swAssyLevel1, suffix, "FNHS0015-1", width, height, suHeight, backCj, bottom);
            swAssyLevel1.ChangeDim("Length@DistanceRight", length/2d);
        }
        else
        {
            swAssyLevel1.Suppress(suffix, "FNHS0012-1");
            swAssyLevel1.Suppress(suffix, "FNHS0013-1");
            swAssyLevel1.Suppress(suffix, "FNHS0014-1");
            swAssyLevel1.Suppress(suffix, "FNHS0015-1");
        }
    }

    public void SidePanelKvv(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, double panelAngle, double panelHeight,double insidePanelWidth)
    {
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "SidePanel_Kvv-1", Aggregator);
        swAssyLevel1.ChangeDim("Length@DistanceLeft", length/2d);
        swAssyLevel1.ChangeDim("Length@DistanceRight", length/2d);
        FNHS0035(swAssyLevel1, suffix, "FNHS0035-1", width, height);
        FNHS0034(swAssyLevel1, suffix, "FNHS0034-1", width, height, panelAngle, panelHeight, insidePanelWidth);
    }

    #region 标准烟罩
    private void FNHS0001(AssemblyDoc swAssyLevel1, string suffix, string partName, double width, double height, bool backCj, double sideCjEnd)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", width);
        swModelLevel2.ChangeDim("Height@SketchBase", height);
        swModelLevel2.ChangeDim("End@CjSide", sideCjEnd);
        if (backCj) swCompLevel2.UnSuppress("BackCj");
        else swCompLevel2.Suppress("BackCj");
    }
    private void FNHS0002(AssemblyDoc swAssyLevel1, string suffix, string partName, double width, double height, bool backCj)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", width);
        swModelLevel2.ChangeDim("Height@SketchBase", height);
        swCompLevel2.Suppress("FI555");
        swCompLevel2.Suppress("FI450");
        swCompLevel2.Suppress("FI400");
        if (height.Equals(555d)||height.Equals(650d)) swCompLevel2.UnSuppress("FI555");
        else if (height.Equals(450d)) swCompLevel2.UnSuppress("FI450");
        else if (height.Equals(400d)) swCompLevel2.UnSuppress("FI400");
        if (backCj) swCompLevel2.UnSuppress("BackCj");
        else swCompLevel2.Suppress("BackCj");
    }
    #endregion

    #region 法国烟罩大侧板
    private void FNHS0073(AssemblyDoc swAssyLevel1, string suffix, string partName, double width, double height, bool backCj, double sideCjEnd)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", width);
        swModelLevel2.ChangeDim("Height@SketchBase", height);
        swModelLevel2.ChangeDim("End@CjSide", sideCjEnd);
        //if (backCj) swCompLevel2.UnSuppress("BackCj");
        //else swCompLevel2.Suppress("BackCj");
    }
    private void FNHS0075(AssemblyDoc swAssyLevel1, string suffix, string partName, double width, double height, bool backCj)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", width);
        swModelLevel2.ChangeDim("Height@SketchBase", height);
        //swCompLevel2.Suppress("FI555");
        //swCompLevel2.Suppress("FI450");
        //swCompLevel2.Suppress("FI400");
        //if (height.Equals(555d)||height.Equals(650d)) swCompLevel2.UnSuppress("FI555");
        //else if (height.Equals(450d)) swCompLevel2.UnSuppress("FI450");
        //else if (height.Equals(400d)) swCompLevel2.UnSuppress("FI400");
        //if (backCj) swCompLevel2.UnSuppress("BackCj");
        //else swCompLevel2.Suppress("BackCj");
        //todo:处理MidRoof铆钉孔



    }


    #endregion


    #region 华为烟罩
    private void FNHS0067(AssemblyDoc swAssyLevel1, string suffix, string partName, double width, double height, bool backCj, double sideCjEnd)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", width);
        swModelLevel2.ChangeDim("Height@SketchBase", height);
        swModelLevel2.ChangeDim("End@CjSide", sideCjEnd);
        //if (backCj) swCompLevel2.UnSuppress("BackCj");
        //else swCompLevel2.Suppress("BackCj");
    }
    private void FNHS0068(AssemblyDoc swAssyLevel1, string suffix, string partName, double width, double height, bool backCj)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", width);
        swModelLevel2.ChangeDim("Height@SketchBase", height);
        //swCompLevel2.Suppress("FI555");
        //swCompLevel2.Suppress("FI450");
        //swCompLevel2.Suppress("FI400");
        //if (height.Equals(555d)||height.Equals(650d)) swCompLevel2.UnSuppress("FI555");
        //else if (height.Equals(450d)) swCompLevel2.UnSuppress("FI450");
        //else if (height.Equals(400d)) swCompLevel2.UnSuppress("FI400");
        //if (backCj) swCompLevel2.UnSuppress("BackCj");
        //else swCompLevel2.Suppress("BackCj");
    }
    #endregion

    #region 斜侧板

    private void FNHS0012(AssemblyDoc swAssyLevel1, string suffix, string partName, double width, double height,double suHeight, bool backCj, double sideCjEnd,double bottom)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", width);
        swModelLevel2.ChangeDim("Height@SketchBase", height);
        swModelLevel2.ChangeDim("SuHeight@SketchBase", suHeight);
        swModelLevel2.ChangeDim("Bottom@SketchBase", bottom);
        swModelLevel2.ChangeDim("End@CjSide", sideCjEnd);

        //if (backCj) swCompLevel2.UnSuppress("BackCj");
        //else swCompLevel2.Suppress("BackCj");
    }

    private void FNHS0013(AssemblyDoc swAssyLevel1, string suffix, string partName, double width, double height, double suHeight, bool backCj, double bottom)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", width);
        swModelLevel2.ChangeDim("Height@SketchBase", height);
        swModelLevel2.ChangeDim("SuHeight@SketchBase", suHeight);
        swModelLevel2.ChangeDim("Bottom@SketchBase", bottom);
        
        //swCompLevel2.Suppress("FI450");
        swCompLevel2.Suppress("FI400");
        if (suHeight.Equals(400d)) swCompLevel2.UnSuppress("FI400");
        
        //if (backCj) swCompLevel2.UnSuppress("BackCj");
        //else swCompLevel2.Suppress("BackCj");
    }



    #endregion

    #region KVV大侧板
    private void FNHS0035(AssemblyDoc swAssyLevel1, string suffix, string partName, double width, double height)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Width@SketchBase", width);
        swModelLevel2.ChangeDim("Height@SketchBase", height);
    }
    private void FNHS0034(AssemblyDoc swAssyLevel1, string suffix, string partName, double width, double height, double panelAngle, double panelHeight, double insidePanelWidth)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Width@SketchBase", width-3.5d);
        swModelLevel2.ChangeDim("Height@SketchBase", height-1d);
        swModelLevel2.ChangeDim("Angle@SketchPanelHoles", panelAngle*1000d);
        swModelLevel2.ChangeDim("Height@SketchPanelHoles", panelHeight);
        swModelLevel2.ChangeDim("Width@SketchMidRoofHoles", insidePanelWidth);
    }
    #endregion

}