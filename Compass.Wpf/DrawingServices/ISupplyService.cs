using Compass.Wasm.Shared.DataService;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.DrawingServices;

public interface ISupplyService
{
    void I555(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection);

    void F555(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection,int supplySpigotNumber, double supplySpigotDis);

    void BackCj(AssemblyDoc swAssyTop, string suffix,bool backCj, double length, double height, double cjSpigotToRight);
}