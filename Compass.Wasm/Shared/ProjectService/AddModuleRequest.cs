namespace Compass.Wasm.Shared.ProjectService;

public class AddModuleRequest
{
    public Guid DrawingId { get; set; }
    public Guid ModelTypeId { get; set; }//标明该分段是属于什么子模型
    public string Name { get; set; }
    public string? SpecialNotes { get; set; }
    public double Length { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
}