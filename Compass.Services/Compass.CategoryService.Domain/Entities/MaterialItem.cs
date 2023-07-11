using Compass.Wasm.Shared.Categories;
using Zack.DomainCommons.Models;

namespace Compass.CategoryService.Domain.Entities;

//物料信息模板
public record MaterialItem : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    #region 基本信息(首次添加的信息)
    public string? MtlNumber { get; private set; } //物料编码
    public string? Description { get; private set; } //中文描述/产品编号
    public string? EnDescription { get; private set; } //英文描述
    public string? Type { get; private set; } //类型/产品型号
    public Unit_e Unit { get; private set; }//PCS
    #endregion

    #region 库存信息(刷新的信息)
    public double Inventory { get; private set; }
    public double UnitCost { get; private set; }
    #endregion

    #region 其他信息和筛选信息(不常变更的信息)
    public string? Length { get; private set; } //长
    public string? Width { get; private set; } //宽
    public string? Height { get; private set; } //高
    public string? Material { get; private set; } //材质


    public bool Hood { get; private set; }
    public HoodGroup_e HoodGroup { get; private set; }
    public bool Ceiling { get; private set; }
	public CeilingGroup_e CeilingGroup { get; private set; }
	public CeilingRule_e CeilingRule { get; private set; }
    public string? CalcRule { get; private set; }//计算规则

    public bool NoLabel { get; private set; }//不要打印标签，
    public bool OneLabel { get; private set; }//打印1张标签，默认false表示需要根据数量Quantity打印多张

    public int Order { get; private set; } //排序
    #endregion

    #region ctor
    private MaterialItem() { }
    public MaterialItem(Guid id,string? mtlNumber,string? description,string enDescription,string? type,Unit_e unit)
    {
        Id=id;
        MtlNumber = mtlNumber;
        Description=description;
        EnDescription= enDescription;
        Type= type;
        Unit= unit;
    }
    #endregion

    #region update
    public void Update(MaterialItemDto dto)
    {
        ChangeMtlNumber(dto.MtlNumber)
            .ChangeDescription(dto.Description)
            .ChangeEnDescription(dto.EnDescription)
            .ChangeType(dto.Type)
            .ChangeUnit(dto.Unit);
        NotifyModified();
    }
    public MaterialItem ChangeMtlNumber(string? mtlNumber)
    {
        MtlNumber = mtlNumber;
        return this;
    }
    public MaterialItem ChangeDescription(string? description)
    {
        Description = description;
        return this;
    }
    public MaterialItem ChangeEnDescription(string? enDescription)
    {
        EnDescription = enDescription;
        return this;
    }
     public MaterialItem ChangeType(string? type)
    {
        Type = type;
        return this;
    }
    public MaterialItem ChangeUnit(Unit_e unit)
    {
        Unit = unit;
        return this;
    }
    #endregion

    #region update Inventory
    public void UpdateInventory(MaterialItemDto dto)
    {
        ChangeInventory(dto.Inventory)
            .ChangeUnitCost(dto.UnitCost);
        NotifyModified();
    }
    public MaterialItem ChangeInventory(double inventory)
    {
        Inventory = inventory;
        return this;
    }
    public MaterialItem ChangeUnitCost(double unitCost)
    {
        UnitCost = unitCost;
        return this;
    }


    #endregion

    #region update 其他信息和筛选信息
    public void UpdateOther(MaterialItemDto dto)
    {
        ChangeLength(dto.Length)
            .ChangeWidth(dto.Width)
            .ChangeHeight(dto.Height)
            .ChangeMaterial(dto.Material)
            .ChangeHood(dto.Hood)
            .ChangeHoodGroup(dto.HoodGroup)
            .ChangeCeiling(dto.Ceiling)
            .ChangeCeilingGroup(dto.CeilingGroup)
            .ChangeCeilingRule(dto.CeilingRule)
            .ChangeCalcRule(dto.Remark)
            .ChangeNoLabel(dto.NoLabel)
            .ChangeOneLabel(dto.OneLabel)
            .ChangeOrder(dto.Order);
        NotifyModified();
    }
    public MaterialItem ChangeLength(string? length)
    {
        Length = length;
        return this;
    }
    public MaterialItem ChangeWidth(string? width)
    {
        Width = width;
        return this;
    }
    public MaterialItem ChangeHeight(string? height)
    {
        Height = height;
        return this;
    }
    public MaterialItem ChangeMaterial(string? material)
    {
        Material = material;
        return this;
    }

    public MaterialItem ChangeHood(bool hood)
    {
        Hood = hood;
        return this;
    }
     public MaterialItem ChangeHoodGroup(HoodGroup_e hoodGroup)
    {
        HoodGroup = hoodGroup;
        return this;
    }
    public MaterialItem ChangeCeiling(bool ceiling)
    {
        Ceiling = ceiling;
        return this;
    }
    public MaterialItem ChangeCeilingGroup(CeilingGroup_e ceilingGroup)
    {
        CeilingGroup = ceilingGroup;
        return this;
    }
    public MaterialItem ChangeCeilingRule(CeilingRule_e ceilingRule)
    {
        CeilingRule = ceilingRule;
        return this;
    }

    public MaterialItem ChangeCalcRule(string? calcRule)
    {
        CalcRule = calcRule;
        return this;
    }

    public MaterialItem ChangeNoLabel(bool noLabel)
    {
        NoLabel = noLabel;
        return this;
    }
    public MaterialItem ChangeOneLabel(bool oneLabel)
    {
        OneLabel = oneLabel;
        return this;
    }
    public MaterialItem ChangeOrder(int order)
    {
        Order = order;
        return this;
    }
    #endregion
}