using Compass.Wasm.Shared.CategoryService;
using Zack.DomainCommons.Models;

namespace Compass.CategoryService.Domain.Entities;

public record Product:AggregateRootEntity,IAggregateRoot,IHasCreationTime,ISoftDelete
{
    public int SequenceNumber { get; private set; }
    public string Name { get; private set; }
    public Sbu_e Sbu { get; private set; }

    private Product(){ }
    public Product(Guid id,int sequenceNumber,string name,Sbu_e sbu)
    {
        Id=id;
        SequenceNumber=sequenceNumber;
        Name=name;
        Sbu=sbu;
    }

    public Product ChangeSequenceNumber(int sequenceNumber)
    {
        SequenceNumber=sequenceNumber;
        return this;
    }
    public Product ChangeName(string name)
    {
        Name=name;
        return this;
    }
    public Product ChangeSbu(Sbu_e sbu)
    {
        Sbu=sbu;
        return this;
    }
}