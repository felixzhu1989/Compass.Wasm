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
        //KW/UW烟罩侧边CJ流出安全距离190
        var sideCjEnd = exhaustType == ExhaustType_e.KW || exhaustType == ExhaustType_e.UW ? 190d : 110d;

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

    public void SidePanelHw(AssemblyDoc swAssyTop, string suffix, SidePanel_e sidePanel, double length, double width, double height, bool backCj, ExhaustType_e exhaustType)
    {
        //KV/UV烟罩侧边CJ留出安全距离110
        //KW/UW烟罩侧边CJ流出安全距离190
        var sideCjEnd = exhaustType == ExhaustType_e.KW || exhaustType == ExhaustType_e.UW ? 190d : 110d;

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
}