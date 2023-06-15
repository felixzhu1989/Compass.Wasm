namespace Compass.Wasm.Shared.Categories;

public class MaterialItemDto:BaseDto
{
    #region 基本信息
    public string? MtlNumber { get; set; } //物料编码
    public string? Description { get; set; } //中文描述/产品编号
    public string? EnDescription { get; set; } //英文描述
    public string? Type { get; set; } //类型/产品型号
    public Unit_e Unit { get; set; }//PCS
    #endregion

    #region 其他信息(刷新的信息)
    public double Inventory { get;  set; }
    public double UnitCost { get;  set; }
    #endregion

    #region 筛选信息(不变更的信息)
    public string? Length { get; set; } //长
    public string? Width { get; set; } //宽
    public string? Height { get; set; } //高
    public string? Material { get; set; } //材质
    public bool Hood { get;  set; }
    public HoodGroup_e HoodGroup { get;  set; }
    public bool Ceiling { get;  set; }
    public CeilingGroup_e CeilingGroup { get;  set; }
    public CeilingRule_e CeilingRule { get;  set; }
    public string? Remark { get;  set; }//备注，计算规则
    #endregion
}