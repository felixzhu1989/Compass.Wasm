namespace Compass.Wasm.Shared.CategoryService;

public class AddModelRequest
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public double Workload { get; set; }
}