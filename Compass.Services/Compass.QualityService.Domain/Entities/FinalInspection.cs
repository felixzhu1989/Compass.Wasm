using Zack.DomainCommons.Models;

namespace Compass.QualityService.Domain.Entities;

public record FinalInspection : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    //Id由ModuleId来
    public Guid InspectedBy { get; set; }
    public DateTime InspectedDate { get; set; }
    public string Conclusion { get; set; }

    public List<FinalInspectionCheckItem> CheckItems { get; set; }

}