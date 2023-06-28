namespace Compass.Wasm.Shared.Categories;

public class MaterialItemDto:BaseDto
{
    #region 基本信息
    private string mtlNumber;
    public string? MtlNumber 
    { 
        get=> mtlNumber;
        set { mtlNumber=value;OnPropertyChanged(); }
    } //物料编码

    private string? description;
    public string? Description
    {
        get => description;
        set { description = value; OnPropertyChanged();}
    }//中文描述/产品编号


    private string? enDescription;
    public string? EnDescription
    {
        get => enDescription;
        set { enDescription = value; OnPropertyChanged(); }
    }//英文描述

    private string? type;
    public string? Type
    {
        get => type;
        set { type = value; OnPropertyChanged(); }
    } //类型/产品型号

    private Unit_e unit;
    public Unit_e Unit { get=>unit; set { unit=value;OnPropertyChanged(); } }//PCS
    #endregion

    #region 其他信息(刷新的信息)

    private double inventory;
    public double Inventory { get=>inventory;  set { inventory=value;OnPropertyChanged(); } }
    private double unitCost;
    public double UnitCost { get=>unitCost;  set { unitCost=value;OnPropertyChanged(); } }
    #endregion

    #region 筛选信息(不变更的信息)

    private string? length;
    public string? Length { get=>length; set { length=value; OnPropertyChanged();} } //长
    private string? width;
    public string? Width { get=>width; set { width=value;OnPropertyChanged(); } } //宽
    private  string? height;
    public string? Height { get=>height; set { height=value;OnPropertyChanged(); } } //高
    private string? material;
    public string? Material { get=>material; set{material=value;OnPropertyChanged();} } //材质
    
    public bool Hood { get;  set; }
    public HoodGroup_e HoodGroup { get;  set; }
    public bool Ceiling { get;  set; }
    public CeilingGroup_e CeilingGroup { get;  set; }
    public CeilingRule_e CeilingRule { get;  set; }

    private string? calcRule;
    public string? CalcRule { get=> calcRule;  set { calcRule=value; OnPropertyChanged();} }//计算规则
    private bool noLabel;
    public bool NoLabel { get=>noLabel; set { noLabel=value;OnPropertyChanged(); } }//不要打印标签，
    private bool oneLabel;
    public bool OneLabel { get=>oneLabel; set { oneLabel=value;OnPropertyChanged(); } }//打印1张标签，默认false表示需要根据数量Quantity打印多张

    #endregion

    #region 扩展查询信息
    private double quantity;
    public double Quantity
    {
        get => quantity;
        set { quantity=value; OnPropertyChanged(); }
    } //数量
    private string? remark;
    public string? Remark
    {
        get => remark;
        set { remark=value; OnPropertyChanged(); }
    }//备注
    #endregion


}