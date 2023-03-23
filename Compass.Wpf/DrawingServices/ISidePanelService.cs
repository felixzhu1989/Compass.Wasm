using Compass.Wasm.Shared.DataService;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.DrawingServices;

public interface ISidePanelService
{
    void SidePanelFs(AssemblyDoc swAssyTop, string suffix, SidePanel_e sidePanel, double length,double width,double height, bool backCj, ExhaustType_e exhaustType);
}