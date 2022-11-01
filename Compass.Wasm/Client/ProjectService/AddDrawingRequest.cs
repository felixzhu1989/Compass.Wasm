namespace Compass.Wasm.Client.ProjectService;

public class AddDrawingRequest
{
    public Guid ProjectId { get; set; }
    public string ItemNumber { get; set; }
    public string? DrawingUrl { get; set; }
}