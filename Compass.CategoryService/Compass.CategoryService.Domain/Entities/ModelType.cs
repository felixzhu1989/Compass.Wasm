using Zack.DomainCommons.Models;

namespace Compass.CategoryService.Domain.Entities;

public record ModelType : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public int SequenceNumber { get; private set; }
    public Guid ModelId { get; private set; }
    public string Name { get; private set; }

    private ModelType() { }

    public ModelType(Guid id, Guid modelId, int sequenceNumber, string name)
    {
        Id = id;
        ModelId = modelId;
        SequenceNumber = sequenceNumber;
        Name = name;
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
}