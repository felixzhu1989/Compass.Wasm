using Compass.Wasm.Shared.Data;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.SwServices;

public interface ISidePanelService
{
    void SidePanelFs(AssemblyDoc swAssyTop, string suffix, SidePanel_e sidePanel, double length,double width,double height, bool backCj, ExhaustType_e exhaustType);
    void SidePanelHw(AssemblyDoc swAssyTop, string suffix, SidePanel_e sidePanel, double length, double width, double height, bool backCj, ExhaustType_e exhaustType);

    void SidePanelNeq(AssemblyDoc swAssyTop, string suffix, SidePanel_e sidePanel, double length, double width, double height,double suHeight, bool backCj, ExhaustType_e exhaustType);

    void SidePanelKvv(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, double panelAngle,
        double panelHeight,double insidePanelWidth);
}