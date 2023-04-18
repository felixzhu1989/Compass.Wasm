namespace Compass.Wasm.Shared.Projects;

public record DrawingPlanResponse
{
    public Guid Id { get; set; }
    public DateTime ReleaseTime { get; set; }
}