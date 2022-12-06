using Zack.DomainCommons.Models;

namespace Compass.CategoryService.Domain.Entities;

public record ModelType : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public int SequenceNumber { get; private set; }
    public Guid ModelId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    private ModelType() { }

    public ModelType(Guid id, Guid modelId, int sequenceNumber, string name,string description)
    {
        Id = id;
        ModelId = modelId;
        SequenceNumber = sequenceNumber;
        Name = name;
        Description = description;
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
}