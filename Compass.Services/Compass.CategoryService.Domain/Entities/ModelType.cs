using Compass.Wasm.Shared.Categories;
using Zack.DomainCommons.Models;

namespace Compass.CategoryService.Domain.Entities;

public record ModelType : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public int SequenceNumber { get; private set; }
    public Guid ModelId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public double Length { get; private set; }
    public double Width { get; private set; }
    public double Height { get; private set; }
    private ModelType() { }

    public ModelType(Guid id, Guid modelId, int sequenceNumber, string name,string description,double length,double width,double height)
    {
        Id = id;
        ModelId = modelId;
        SequenceNumber = sequenceNumber;
        Name = name;
        Description = description;
        Length=length;
        Width=width;
        Height=height;
    }

    public void Update(ModelTypeDto dto)
    {
        ChangeSequenceNumber(dto.SequenceNumber)
            .ChangeName(dto.Name)
            .ChangeDescription(dto.Description)
            .ChangeLength(dto.Length)
            .ChangeWidth(dto.Width)
            .ChangeHeight(dto.Height);
        NotifyModified();
    }


    public ModelType ChangeSequenceNumber(int sequenceNumber)
    {
        SequenceNumber=sequenceNumber;
        return this;
    }
    public ModelType ChangeName(string name)
    {
        Name=name;
        return this;
    }
    public ModelType ChangeDescription(string description)
    {
        Description=description;
        return this;
    }
    public ModelType ChangeLength(double length)
    {
        Length=length;
        return this;
    }
    public ModelType ChangeWidth(double width)
    {
        Width=width;
        return this;
    }
    public ModelType ChangeHeight(double height)
    {
        Height=height;
        return this;
    }
}