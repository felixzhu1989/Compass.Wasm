namespace Compass.Wasm.Shared.CategoryService;

public class AddModelTypeRequest
{
    public Guid ModelId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Length { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
}