using Zack.DomainCommons.Models;

namespace Compass.DataService.Domain.Entities;

public record ModuleData: BaseEntity
{
    public Guid ModuleId { get; init; }
    //产品基本属性有长宽高，注意和以前的作图程序不同，这里是总长，
    public double Length { get;private set; }
    public double Width { get;private set; }
    public double Height { get;private set; }

}