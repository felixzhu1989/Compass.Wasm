using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Projects;
using Zack.DomainCommons.Models;

namespace Compass.CategoryService.Domain.Entities;

public record AccCutList : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public AccType_e AccType { get; private set; }
    //表内容
    public string? PartDescription { get; private set; }
    public double Length { get; private set; }
    public double Width { get; private set; }
    public double Thickness { get; private set; }
    public int Quantity { get; private set; }
    public string? Material { get; private set; }
    public string? PartNo { get; private set; }
    public string? BendingMark { get; private set; }

    private AccCutList() { }
    public AccCutList(Guid id, AccType_e accType, string partDescription, double length, double width, double thickness, int quantity, string material, string partNo, string bendingMark)
    {
        Id=id; AccType=accType; PartDescription=partDescription; Length=length; Width=width; Thickness=thickness; Quantity=quantity; Material=material; PartNo=partNo;
        BendingMark=bendingMark;
    }

    public void Update(AccCutListDto dto)
    {
        ChangePartDescription(dto.PartDescription)
            .ChangeLength(dto.Length)
            .ChangeWidth(dto.Width)
            .ChangeThickness(dto.Thickness)
            .ChangeQuantity(dto.Quantity)
            .ChangeMaterial(dto.Material)
            .ChangePartNo(dto.PartNo)
            .ChangeBendingMark(dto.BendingMark)
            .ChangeAccType(dto.AccType);

        NotifyModified();
    }
    public AccCutList ChangeAccType(AccType_e accType)
    {
        AccType = accType;
        return this;
    }
    public AccCutList ChangePartDescription(string? partDescription)
    {
        PartDescription = partDescription;
        return this;
    }
    public AccCutList ChangeLength(double length)
    {
        Length=length; return this;
    }
    public AccCutList ChangeWidth(double width)
    {
        Width=width; return this;
    }
    public AccCutList ChangeThickness(double thickness)
    {
        Thickness=thickness; return this;
    }
    public AccCutList ChangeQuantity(int quantity)
    {
        Quantity=quantity; return this;
    }
    public AccCutList ChangeMaterial(string? material)
    {
        Material = material;
        return this;
    }
    public AccCutList ChangePartNo(string? partNo)
    {
        PartNo = partNo;
        return this;
    }
    public AccCutList ChangeBendingMark(string? bendingMark)
    {
        BendingMark = bendingMark;
        return this;
    }
}