namespace Compass.Wasm.Shared.Categories;
/// <summary>
/// 事业部
/// </summary>
public enum Sbu_e
{
    FS, MA, SBA
}

//发货清单，装箱清单枚举值
public enum ProductType_e
{
    Hood, Ceiling
}
public enum Unit_e
{
    PCS, M米
}
public enum AccGroup_e
{
    UV, ULUV, Marvel, Ansul, WaterWash, Electric, Light, Fan, Profile, Filter, Fastener, Eto
}
public enum AccRule_e
{
    JapanNoNeed, JapanOnly, All, EtoNoPrint, EtoPrint
}