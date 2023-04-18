using Zack.DomainCommons.Models;

namespace Compass.QualityService.Domain.Entities;

public record FinalInspectionCheckItemType : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public int SequenceNumber { get; private set; }
    public string Name { get; private set; }

    private FinalInspectionCheckItemType(){ }

    public FinalInspectionCheckItemType(Guid id, int sequenceNumber, string name)
    {
        Id = id;
        SequenceNumber = sequenceNumber;
        Name = name;
    }
    public FinalInspectionCheckItemType ChangeSequenceNumber(int sequenceNumber)
    {
        SequenceNumber=sequenceNumber;
        return this;
    }
    public FinalInspectionCheckItemType ChangeName(string name)
    {
        Name=name;
        return this;
    }
}

