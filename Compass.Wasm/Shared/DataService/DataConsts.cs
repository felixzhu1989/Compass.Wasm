namespace Compass.Wasm.Shared.DataService;

public enum SidePanel_e
{
    中,左, 右, 双
}
public enum LightType_e
{
    无灯,长灯, 短灯, 筒灯60, 筒灯140
}
public enum UvLightType_e
{
    No,UVR4S, UVR4L,UVR6S,UVR6L,UVR8S,UVR8L
}
public enum DrainType_e
{
    右油塞, 左油塞, 右排水管, 左排水管,上排水,集油槽
}



#region Ansul
public enum AnsulSide_e
{
    无侧喷, 左侧喷,右侧喷
}
public enum AnsulDetector_e   
{
    无探测器口, 左探测器口, 右探测器口, 双侧探测器口
}
public enum AnsulDetectorEnd_e
{
    无末端探测器, 左末端探测器, 右末端探测器
}


#endregion


#region 烟罩结构
public enum ExhaustType_e
{
    KV,UV,KW,UW,CMOD,M
}

public enum SupplyType_e
{
    I,F,Rectangle,Round
}


#endregion