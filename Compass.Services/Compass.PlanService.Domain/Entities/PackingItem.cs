using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Plans;
using Zack.DomainCommons.Models;

namespace Compass.PlanService.Domain.Entities;

//打包清单：配件->打印横向A4给生产，生产现场打印标签
public record PackingItem : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    #region 基本信息
    public Guid PackingListId { get; init; } //关联装箱清单

    //从物料模板查询信息
    public string? MtlNumber { get; private set; } //物料编码
    public string? Description { get;private set; } //中文描述/产品编号
    public string? EnDescription { get;private set; } //英文描述
    public string? Type { get; private set; } //类型/产品型号
    public double Quantity { get; private set; } //数量
    public Unit_e Unit { get; private set; }//PCS
    public string? Length { get; private set; } //长
    public string? Width { get; private set; } //宽
    public string? Height { get; private set; } //高
    public string? Material { get; private set; } //材质
    public string? Remark { get; private set; }//备注
    public string? CalcRule { get; private set; } //计算规则

    #endregion
    #region 状态信息
    public bool Pallet { get; private set; }//单独托盘，查询烟罩分段时设置为true，自定义物料时设定
    public bool NoLabel { get; private set; }//不要打印标签，
    public bool OneLabel { get; private set; }//打印1张标签，默认false表示需要根据数量Quantity打印多张
    #endregion

    #region ctor
    private PackingItem() { }
    public PackingItem(Guid id,Guid packingListId,string? mtlNumber,string? description,string? enDescription,string? type,double quantity,Unit_e unit,string? length,string? width,string? height,string? material,string? remark,string? calcRule,bool pallet,bool noLabel,bool oneLabel)
    {
        Id=id;
        PackingListId=packingListId;
        MtlNumber = mtlNumber;
        Description=description;
        EnDescription=enDescription;
        Type=type;
        Quantity=quantity;
        Unit=unit;
        Length=length;
        Width=width;
        Height=height;
        Material=material;
        Remark=remark;
        CalcRule=calcRule;
        Pallet=pallet;
        NoLabel=noLabel;
        OneLabel=oneLabel;
    }
    #endregion

    #region update
    public void Update(PackingItemDto dto)
    {
        ChangeMtlNumber(dto.MtlNumber)
            .ChangeDescription(dto.Description)
            .ChangeEnDescription(dto.EnDescription)
            .ChangeType(dto.Type)
            .ChangeQuantity(dto.Quantity)
            .ChangeUnit(dto.Unit)
            .ChangeSize(dto.Length, dto.Width, dto.Height)
            .ChangeMaterial(dto.Material)
            .ChangeRemark(dto.Remark)
            .ChangeCalcRule(dto.CalcRule)
            .ChangePallet(dto.Pallet)
            .ChangeNoLabel(dto.NoLabel)
            .ChangeOneLabel(dto.OneLabel);
        NotifyModified();
    }

    public PackingItem ChangeMtlNumber(string? mtlNumber)
    {
        MtlNumber = mtlNumber;
        return this;
    }
    public PackingItem ChangeDescription(string? description)
    {
        Description=description;
        return this;
    }

    public PackingItem ChangeEnDescription(string? enDescription)
    {
        EnDescription=enDescription;
        return this;
    }
    public PackingItem ChangeType(string? type)
    {
        Type=type;
        return this;
    }
    public PackingItem ChangeQuantity(double quantity)
    {
        Quantity=quantity;
        return this;
    }
    public PackingItem ChangeUnit(Unit_e unit)
    {
        Unit=unit;
        return this;
    }
    public PackingItem ChangeSize(string? length, string? width, string? height)
    {
        Length=length;
        Width=width;
        Height=height;
        return this;
    }
    
    public PackingItem ChangeMaterial(string? material)
    {
        Material=material;
        return this;
    }
    public PackingItem ChangeRemark(string? remark)
    {
        Remark=remark;
        return this;
    }
    public PackingItem ChangeCalcRule(string? calcRule)
    {
        CalcRule=calcRule;
        return this;
    }
    public PackingItem ChangePallet(bool pallet)
    {
        Pallet=pallet;
        return this;

    }
    public PackingItem ChangeNoLabel(bool noLabel)
    {
        NoLabel=noLabel;
        return this;

    }
    public PackingItem ChangeOneLabel(bool oneLabel)
    {
        OneLabel=oneLabel;
        return this;
    }
    #endregion
}