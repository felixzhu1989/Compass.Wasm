namespace Compass.Wasm.Shared.ProjectService;

public class AddDrawingPlanRequest
{
    public Guid ProjectId { get; set; }
    public DateTime ReleaseTime { get; set; }
}