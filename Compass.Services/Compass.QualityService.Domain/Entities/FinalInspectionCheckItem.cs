using Compass.Wasm.Shared.Quality;
using Zack.DomainCommons.Models;

namespace Compass.QualityService.Domain.Entities;

public record FinalInspectionCheckItem : BaseEntity
{
    public Guid TypeId { get; set; }
    public Guid FinalInspectionId { get; set; }

    public string Comment { get; set; }
    public InspectorComments InspectorComments { get; set; }
}