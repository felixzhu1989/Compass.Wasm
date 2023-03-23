using Compass.Wasm.Shared.DataService;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.DrawingServices;

public interface IExhaustService
{
    void Kv555(AssemblyDoc swAssyTop, string suffix, double length, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector);

    void ExhaustSpigotFs(AssemblyDoc swAssyTop, string suffix, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, bool marvel, bool ansul);







}