using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.SwServices;

public interface IExhaustService
{
    #region 标准555烟罩

    void ExhaustSpigotFs(AssemblyDoc swAssyLevel1, string suffix, double length, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, bool marvelSpigot,bool marvelRail, bool ansul, ExhaustType_e exhaustType);
    void Kv555(AssemblyDoc swAssyTop, string suffix, double length,double height, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector);
    void KvFr555(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, LightType_e lightType, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector);
    void Kv450(AssemblyDoc swAssyTop, string suffix, double length, double height, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector);
    void KvFr450(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, LightType_e lightType, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector);
    void Uv555(AssemblyDoc swAssyTop, string suffix, double length, double height, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector);
    void UvFr555(AssemblyDoc swAssyTop, string suffix, double length,double width, double height, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, LightType_e lightType, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector);

    void Uv450(AssemblyDoc swAssyTop, string suffix, double length, double height, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector);
    void UvFr450(AssemblyDoc swAssyTop, string suffix, double length, double width, double height, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, LightType_e lightType, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector);



    void Kw555(AssemblyDoc swAssyTop, string suffix, double length, double height, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, WaterInlet_e waterInlet);
    void Uw555(AssemblyDoc swAssyTop, string suffix, double length, double height, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, WaterInlet_e waterInlet);

    void Cmod555(AssemblyDoc swAssyTop, string suffix, double length, double height, SidePanel_e sidePanel,  double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, WaterInlet_e waterInlet);


    #endregion

    #region 华为烟罩
    void UvHw650(AssemblyDoc swAssyTop, string suffix, double length, double height, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, AnsulDetector_e ansulDetector);
    void UwHw650(AssemblyDoc swAssyTop, string suffix, double length, double height, SidePanel_e sidePanel, UvLightType_e uvLightType, double middleToRight, int exhaustSpigotNumber, double exhaustSpigotLength, double exhaustSpigotWidth, double exhaustSpigotHeight, double exhaustSpigotDis, DrainType_e drainType, bool waterCollection, bool backToBack, bool marvel, bool ansul, AnsulSide_e ansulSide, WaterInlet_e waterInlet);
    #endregion

    #region KVV烟罩

    void Kvv555(AssemblyDoc swAssyTop, string suffix, double length, double height, double panelAngle, double panelHeight);

    #endregion



}