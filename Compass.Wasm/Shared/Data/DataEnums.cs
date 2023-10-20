namespace Compass.Wasm.Shared.Data;

public enum SidePanel_e
{
    NA,中,左,右,双
}
public enum LightType_e
{
    NA,长灯, 短灯, 筒灯,HCL,飞利浦三防灯
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
    NA,右入水管,左入水管
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
    NA,KV,UV,KW,UW,CMOD,M
}

public enum ExhaustHeight_e
{
    E555,E450,E400,E300
}


public enum SupplyType_e
{
    NA,I,F,方形,圆形
}


#endregion


#region 天花烟罩
public enum FilterType_e
{
    FC,KSA
}
public enum FilterSide_e
{
    NA, 无过滤器侧板, 左过滤器侧板, 右过滤器侧板, 两过滤器侧板
}
public enum CeilingLightType_e
{
    NA, 日光灯, 筒灯,HCL
}

public enum LightCable_e
{
    NA, 无出线孔, 左出线孔, 右出线孔, 两出线孔
}
public enum HclSide_e
{
    NA, 无HCL侧板, 左HCL侧板, 右HCL侧板, 两HCL侧板
}
public enum DpSide_e
{
    NA,无DP腔,左侧DP腔, 右侧DP腔,两侧DP腔
}

public enum CeilingWaterInlet_e
{
    NA, 前入水管, 上入水管
}

#endregion