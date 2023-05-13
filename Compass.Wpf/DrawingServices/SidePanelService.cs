using Compass.Wasm.Shared.Data;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.DrawingServices;

public class SidePanelService : BaseDrawingService, ISidePanelService
{
    public SidePanelService(IContainerProvider provider) : base(provider)
    {
    }
    public void SidePanelFs(AssemblyDoc swAssyTop, string suffix, SidePanel_e sidePanel, double length, double width, double height, bool backCj, ExhaustType_e exhaustType)
    {
        //计算侧向CJ孔的数量
        var netWidth = backCj ? width - 90 : width;
        //水洗减去380，普通减去305
        int sideCjNo = (int)((netWidth -(exhaustType == ExhaustType_e.KW || exhaustType == ExhaustType_e.UW ? 380d : 305d)) / 32d)+1;

        //todo:是否需要合并M型烟罩
        var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "SidePanel_Fs-1", Aggregator);


        if (sidePanel == SidePanel_e.双)
        {
            FNHS0001(swAssyLevel1, suffix, "FNHS0001-1", width, height, backCj, sideCjNo);
            FNHS0002(swAssyLevel1, suffix, "FNHS0002-1", width, height, backCj);
            FNHS0001(swAssyLevel1, suffix, "FNHS0003-1", width, height, backCj, sideCjNo);
            FNHS0002(swAssyLevel1, suffix, "FNHS0004-1", width, height, backCj);
            swAssyLevel1.ChangeDim("Length@DistanceLeft", length/2d);
            swAssyLevel1.ChangeDim("Length@DistanceRight", length/2d);
        }
        else if (sidePanel == SidePanel_e.左)
        {
            FNHS0001(swAssyLevel1, suffix, "FNHS0001-1", width, height, backCj, sideCjNo);
            FNHS0002(swAssyLevel1, suffix, "FNHS0002-1", width, height, backCj);
            swAssyLevel1.Suppress(suffix, "FNHS0003-1");
            swAssyLevel1.Suppress(suffix, "FNHS0004-1");
            swAssyLevel1.ChangeDim("Length@DistanceLeft", length/2d);
        }
        else if (sidePanel == SidePanel_e.右)
        {
            swAssyLevel1.Suppress(suffix, "FNHS0001-1");
            swAssyLevel1.Suppress(suffix, "FNHS0002-1");
            FNHS0001(swAssyLevel1, suffix, "FNHS0003-1", width, height, backCj, sideCjNo);
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

    private void FNHS0001(AssemblyDoc swAssyLevel1, string suffix, string partName, double width, double height, bool backCj, int sideCjNo)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", width);
        swModelLevel2.ChangeDim("Height@SketchBase", height);
        swModelLevel2.ChangeDim("CjSide@CjSide", sideCjNo);
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


}