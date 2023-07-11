namespace Compass.Wasm.Shared.Categories;

public class MaterialItemCsv
{
    #region 基本信息
    public string? MtlNumber { get; set; } //物料编码
    public string? Description { get; set; } //中文描述/产品编号
    public string? EnDescription { get; set; } //英文描述
    public string? Type { get; set; } //类型/产品型号
    public string? Unit { get; set; }//PCS
    #endregion

    #region 其他信息(刷新的信息)
    public string? Inventory { get; set; }
    public string? UnitCost { get; set; }
    public string? Order { get; set; }
    #endregion

    #region 筛选信息(不变更的信息)
    public string? Length { get; set; } //长
    public string? Width { get; set; } //宽
    public string? Height { get; set; } //高
    public string? Material { get; set; } //材质

    public string? Hood { get; set; }
    public string? HoodGroup { get; set; }
    public string? Ceiling { get; set; }
    public string? CeilingGroup { get; set; }
    public string? CeilingRule { get; set; }
    public string? CalcRule { get; set; }//计算规则

    public bool NoLabel { get; set; }//不要打印标签，
    public bool OneLabel { get; set; }//打印1张标签，默认false表示需要根据数量Quantity打印多张
    #endregion
}