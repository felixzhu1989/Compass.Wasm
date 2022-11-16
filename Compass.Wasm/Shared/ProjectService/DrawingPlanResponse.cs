namespace Compass.Wasm.Shared.ProjectService;

public record DrawingPlanResponse
{
    public Guid Id { get; set; }
    public DateTime ReleaseTime { get; set; }
}