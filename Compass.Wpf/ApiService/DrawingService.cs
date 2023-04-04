using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wpf.ApiService;

public class DrawingService: BaseService<DrawingDto>, IDrawingService
{
    public DrawingService(HttpRestClient client) : base(client, "Drawing")
    {
    }
}