namespace Compass.Wasm.Shared.Data;

public enum SidePanel_e
{
    NA, 中, 左, 右, 双
}
public enum LightType_e
{
    NA, 长灯, 短灯, 筒灯, HCL, 飞利浦三防灯
}
public enum UvLightType_e
{
    NA, UVR4S, UVR4L, UVR6S, UVR6L, UVR8S, UVR8L, Double
}
public enum DrainType_e
{
    NA, 右油塞, 左油塞, 右排水管, 左排水管, 上排水, 集油槽
}
public enum WaterInlet_e
{
    NA, 右入水管, 左入水管
}


#region Ansul
public enum AnsulSide_e
{
    NA, 无侧喷, 左侧喷, 右侧喷
}
public enum AnsulDetector_e
{
    NA, 无探测器口, 左探测器口, 右探测器口, 两探测器口
}
public enum AnsulDetectorEnd_e
{
    NA, 无末端探测器, 左末端探测器, 右末端探测器
}


#endregion


#region 烟罩结构
public enum ExhaustType_e
{
    NA, KV, UV, KW, UW, CMOD, M
}

public enum ExhaustHeight_e
{
    NA, E555, E450, E400, E300
}


public enum SupplyType_e
{
    NA, I, F, 方形, 圆形
}


#endregion


#region 天花烟罩
//排风腔
public enum FilterType_e
{
    NA, FC, KSA
}
public enum FilterSide_e
{
    NA, 无过滤器侧板, 左过滤器侧板, 右过滤器侧板, 两过滤器侧板
}
public enum CeilingLightType_e
{
    NA, 日光灯, 筒灯, HCL
}

public enum LightCable_e
{
    NA, 无出线孔, 左出线孔, 右出线孔, 两出线孔
}
public enum HclSide_e
{
    NA, 无HCL侧板, 左HCL侧板, 右HCL侧板, 两HCL侧板
}


public enum CeilingWaterInlet_e
{
    NA, 前入水管, 上入水管
}
//CJ
public enum CjSpigotDirection_e
{
    NA, CJ脖颈朝前, CJ脖颈朝上
}
public enum BeamType_e
{
    NA, KUCJDB800, KUCJSB535, KCJSB290, KCJSB265, UCJSB385, KUCWDB800, KUCWSB535, KCWSB265
}
public enum BcjSide_e
{
    NA, 无BCJ腔, 左BCJ腔, 右BCJ腔, 两BCJ腔
}
public enum LksSide_e
{
    NA, 无LK灯腔, 左LK灯腔, 右LK灯腔, 两LK灯腔
}
public enum GutterSide_e
{
    NA, 无Ansul腔, 左Ansul腔, 右Ansul腔, 两Ansul腔
}
public enum NocjSide_e
{
    NA, 无NOCJ腔, 左NOCJ腔, 右NOCJ腔, 两NOCJ腔
}
//背面NOCJ位置
public enum NocjBackSide_e
{
    NA, 无背面NOCJ腔, 左背面NOCJ腔, 右背面NOCJ腔, 两背面NOCJ腔
}
public enum DpSide_e
{
    NA, 无DP腔, 左DP腔, 右DP腔, 两DP腔
}
public enum DpBackSide_e
{
    NA, 无背面DP腔, 左背面DP腔, 右背面DP腔, 两背面DP腔
}

public enum DpDrainType_e
{
    NA, 无排水管, 左排水管,右排水管
}


public enum PanelType_e
{
    W,Z
}


#endregion