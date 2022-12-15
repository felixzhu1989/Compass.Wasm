namespace Compass.Wasm.Shared.ProjectService;

public class ModuleResponse
{
    public Guid Id { get; set; }
    public Guid DrawingId { get; set; }
    public Guid ModelTypeId { get; set; }//标明该分段是属于什么子模型
    public Guid OldModelTypeId { get; set; }
    public string Name { get; set; }
    public string? SpecialNotes { get; set; }
    public bool IsReleased { get; set; }
    public bool IsModuleDataOk { get; set; }
    public double Length { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
}