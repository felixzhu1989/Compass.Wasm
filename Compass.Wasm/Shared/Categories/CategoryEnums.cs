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
    NA, UV系统, MARVEL系统, 水洗系统, 过滤器, 照明, 电, CJ, 型材, 其他, ANSUL, 自制件
}

public enum CeilingRule_e
{
    NA,所有项目要,日本项目不要, 日本项目特有
}

//导图模式
public enum ExportWay_e
{
    标准模式,不导图模式,装配体模式,拷贝文件模式
}
//配件类型
public enum AccType_e
{
    MCP_MCP箱,//
    UCP_UCP箱,//
    UCPGOT_UCP箱GOT,//
    UCPGOTHW_UCP箱GOTHW,
    ULLEDRECT_ULLED整流器盒,
    MU1VV1BOX_MU1VV1盒,
    MU1VV1VV1BOX_MU1VV1VV1盒,
    TCSBOX_加热丝温控开关盒,
    FANBOX4_4孔风机盒,
    UCJFCCOMBI_双层油网,
    UCWUVRACK4S_天花UV灯,
    UCWUVRACK4L_天花UV灯,
    GIMC150_镀锌均流桶,
    GIMC200_镀锌均流桶,
    GIMC250_镀锌均流桶,
    SSMC150_不锈钢均流桶,
    SSMC178_不锈钢均流桶,
    SSMC180_不锈钢均流桶,
    SSMC200_不锈钢均流桶,
    SSMC250_不锈钢均流桶,
    KBOL290W560_排风盒,
    KBOL390W560_排风盒,
    KBOL540W560_排风盒,
    KBOL548W310_排风盒,
    KBOL548W410_排风盒,
    KBOL548W560_排风盒,
    KBOL1048W560_排风盒,
    KBOL1548W560_排风盒,
    KBOL2048W560_排风盒,
}