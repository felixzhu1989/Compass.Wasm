namespace Compass.Wasm.Shared.ProjectService;

public class AddModuleRequest
{
    public Guid DrawingId { get; set; }
    public Guid ModelId { get; set; }//标明该分段是属于什么什么模型
    public string Name { get; set; }
    public string? SpecialNotes { get; set; }
}