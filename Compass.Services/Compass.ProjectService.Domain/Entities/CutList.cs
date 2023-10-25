using Compass.Wasm.Shared.Projects;
using Zack.DomainCommons.Models;

namespace Compass.ProjectService.Domain.Entities;

public record CutList:AggregateRootEntity,IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public Guid ModuleId { get; private set; }
    //表内容
    public string? PartDescription { get; private set; }
    public double Length { get; private set; }
    public double Width { get; private set; }
    public double Thickness { get; private set; }
    public int Quantity { get; private set; }
    public string? Material { get; private set; }
    public string? PartNo { get; private set; }
    public string? BendingMark { get; private set; }

    private CutList(){ }
    public CutList(Guid id, Guid moduleId,string partDescription,double length,double width,double thickness,int quantity,string material,string partNo,string bendingMark)
    {
        Id=id; ModuleId=moduleId; PartDescription=partDescription; Length=length; Width=width;Thickness=thickness;Quantity=quantity;Material=material;PartNo=partNo;
        BendingMark=bendingMark;
    }

    public void Update(CutListDto dto)
    {
        ChangePartDescription(dto.PartDescription)
            .ChangeLength(dto.Length).ChangeWidth(dto.Width)
            .ChangeThickness(dto.Thickness).ChangeQuantity(dto.Quantity)
            .ChangeMaterial(dto.Material).ChangePartNo(dto.PartNo)
            .ChangeBendingMark(dto.BendingMark);

        NotifyModified();
    }

    public CutList ChangePartDescription(string? partDescription)
    {
        PartDescription = partDescription;
        return this;
    }
    public CutList ChangeLength(double length)
    {
        Length=length; return this;
    }
    public CutList ChangeWidth(double width)
    {
        Width=width; return this;
    }
    public CutList ChangeThickness(double thickness)
    {
        Thickness=thickness; return this;
    }
    public CutList ChangeQuantity(int quantity)
    {
        Quantity=quantity; return this;
    }
    public CutList ChangeMaterial(string? material)
    {
        Material = material;
        return this;
    }
    public CutList ChangePartNo(string? partNo)
    {
        PartNo = partNo;
        return this;
    }
    public CutList ChangeBendingMark(string? bendingMark)
    {
        BendingMark = bendingMark;
        return this;
    }
}