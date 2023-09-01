using Compass.Wasm.Shared.Data;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.SwServices;

public interface ISupplyService
{
    #region 标准烟罩
    void I555(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection);
    void IFr555(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection);
    void I450(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection);
    void I400(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection);


    void F555(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection, int supplySpigotNumber, double supplySpigotDis);
    void FFr555(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection, int supplySpigotNumber, double supplySpigotDis);

    void F400(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection, int supplySpigotNumber, double supplySpigotDis);

    void BackCj(AssemblyDoc swAssyTop, string suffix, bool backCj, double length, double height, double cjSpigotToRight);
    void BackCjFr(AssemblyDoc swAssyTop, string suffix, bool backCj, double length, double height, double cjSpigotToRight);





    #endregion

    #region 华为烟罩
    void IHw650(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection);
    void FHw650(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, ExhaustType_e exhaustType, SidePanel_e sidePanel, UvLightType_e uvLightType, bool bluetooth, bool marvel, bool ledLogo, bool waterCollection, int supplySpigotNumber, double supplySpigotDis);
    #endregion
}