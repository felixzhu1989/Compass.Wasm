using Zack.DomainCommons.Models;

namespace Compass.CategoryService.Domain.Entities;

public record Model:AggregateRootEntity,IAggregateRoot,IHasCreationTime,ISoftDelete
{
    public int SequenceNumber { get; private set; }
    public Guid ProductId { get; private set; }
    public string Name { get; private set; }
    public double Workload { get; private set; }

    private Model() { }
    public Model(Guid id,Guid productId, int sequenceNumber, string name,double workload)
    {
        Id=id;
        ProductId=productId;
        SequenceNumber=sequenceNumber;
        Name=name;
        Workload=workload;
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
    public Model ChangeWorkload(double workload)
    {
        Workload=workload;
        return this;
    }
}