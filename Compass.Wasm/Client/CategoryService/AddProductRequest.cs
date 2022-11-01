using Compass.Wasm.Shared.CategoryService;
namespace Compass.Wasm.Client.CategoryService;
public class AddProductRequest
{
    public string Name { get; set; }
    public Sbu Sbu { get; set; }
}