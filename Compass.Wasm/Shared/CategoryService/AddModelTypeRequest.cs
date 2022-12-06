namespace Compass.Wasm.Shared.CategoryService;

public class AddModelTypeRequest
{
    public Guid ModelId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}