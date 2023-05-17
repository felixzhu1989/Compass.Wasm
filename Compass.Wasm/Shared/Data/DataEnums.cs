namespace Compass.Wasm.Shared.Data;

public enum SidePanel_e
{
    NA,中,左,右,双
}
public enum LightType_e
{
    NA,长灯, 短灯, 筒灯60, 筒灯140
}
public enum UvLightType_e
{
    NA,UVR4S,UVR4L,UVR6S,UVR6L,UVR8S,UVR8L,Double
}
public enum DrainType_e
{
    NA,右油塞, 左油塞, 右排水管, 左排水管,上排水,集油槽
}

public enum WaterInlet_e
{
    NA,右入水,左入水
}


#region Ansul
public enum AnsulSide_e
{
    NA,无侧喷, 左侧喷,右侧喷
}
public enum AnsulDetector_e   
{
    NA,无探测器口, 左探测器口, 右探测器口, 双侧探测器口
}
public enum AnsulDetectorEnd_e
{
    NA,无末端探测器, 左末端探测器, 右末端探测器
}


#endregion


#region 烟罩结构
public enum ExhaustType_e
{
    NA,KV,UV,KW,UW,CMOD,M,UVHW,UWHW
}

public enum ExhaustHeight_e
{
    E555,E450,E400,E300
}


public enum SupplyType_e
{
    NA,I,F,Rectangle,Round
}


#endregion