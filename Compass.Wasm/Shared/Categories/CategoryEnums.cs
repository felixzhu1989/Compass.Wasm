namespace Compass.Wasm.Shared.Categories;
/// <summary>
/// 事业部
/// </summary>
public enum Sbu_e
{
    FS, MA, SBA
}

//物料信息与筛选
public enum Unit_e
{
    PCS,M,SET
}

public enum HoodGroup_e
{
    NA,筒灯, UV系统,ULUV系统,MARVEL系统,水洗系统,CMOD,ANSUL,自制件
}

public enum CeilingGroup_e
{
    NA, 过滤器, 电, 灯具, 风机, 型材, 紧固件, UV系统, MARVEL系统, 水洗系统, ANSUL, 自制件
}

public enum CeilingRule_e
{
    NA,所有项目要,日本项目不要,日本项目单独要
}

//导图模式
public enum ExportWay_e
{
    标准模式,不导图模式,装配体模式,拷贝模式
}