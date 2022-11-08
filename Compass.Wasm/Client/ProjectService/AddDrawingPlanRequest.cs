namespace Compass.Wasm.Client.ProjectService;

public class AddDrawingPlanRequest
{
    public Guid ProjectId { get; set; }
    public DateTime ReleaseTime { get; set; }
}