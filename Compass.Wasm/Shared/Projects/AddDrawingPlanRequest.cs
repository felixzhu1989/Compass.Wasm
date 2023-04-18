namespace Compass.Wasm.Shared.Projects;

public class AddDrawingPlanRequest
{
    public Guid ProjectId { get; set; }
    public DateTime ReleaseTime { get; set; }
}