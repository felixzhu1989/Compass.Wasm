using Zack.DomainCommons.Models;

namespace Compass.Wasm.Shared.DataService.Entities;

public  record ModuleData: BaseEntity
{

    //Id直接使用ModuleId
    //产品基本属性有长宽高，注意和以前的作图程序不同，这里是总长，
    public double Length { get;set; }
    public double Width { get;set; }
    public double Height { get;set; }

    public virtual bool Accept(string model)
    {
        return false;
    }
    public ModuleData ChangeId(Guid id)
    {
        Id = id;
        return this;
    }
}