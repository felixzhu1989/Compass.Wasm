namespace Compass.Wasm.Shared.CategoryService;

public class ModelTypeResponse
{
    public Guid Id { get; set; }
    public int SequenceNumber { get; set; }
    public Guid ModelId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}