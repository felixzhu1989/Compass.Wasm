using Zack.DomainCommons.Models;

namespace Compass.CategoryService.Domain.Entities;

public record Model:AggregateRootEntity,IAggregateRoot,IHasCreationTime,ISoftDelete
{
    public int SequenceNumber { get; private set; }
    public Guid ProductId { get; private set; }
    public string Name { get; private set; }

    private Model() { }
    public Model(Guid id,Guid productId, int sequenceNumber, string name)
    {
        Id=id;
        ProductId=productId;
        SequenceNumber=sequenceNumber;
        Name=name;
    }

    public Model ChangeSequenceNumber(int sequenceNumber)
    {
        SequenceNumber=sequenceNumber;
        return this;
    }
    public Model ChangeName(string name)
    {
        Name=name;
        return this;
    }
}