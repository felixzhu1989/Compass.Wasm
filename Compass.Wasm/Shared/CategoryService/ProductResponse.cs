namespace Compass.Wasm.Shared.CategoryService;

public class ProductResponse
{
    public Guid Id { get; set; }
    public int SequenceNumber { get;set; }
    public string Name { get; set; }
    public Sbu Sbu { get; set; }
}