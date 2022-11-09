namespace Compass.Wasm.Shared.ProjectService;

public record DrawingResponse
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string ItemNumber { get; set; }
    public string? DrawingUrl { get; set; }

    public Guid? UserId { get; set; }

    //用于显示
    public bool IsChecked { get; set; }
}