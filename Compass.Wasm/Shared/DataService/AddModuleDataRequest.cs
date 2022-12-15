namespace Compass.Wasm.Shared.DataService;

public class AddModuleDataRequest
{
    public string Model { get; set; }
    public Guid Id { get; set; }
    public double Length { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
}