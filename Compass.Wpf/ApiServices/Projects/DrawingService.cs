using Compass.Wasm.Shared.Projects;
using Compass.Wpf.ApiService;

namespace Compass.Wpf.ApiServices.Projects;

public class DrawingService : BaseService<DrawingDto>, IDrawingService
{
    public DrawingService(HttpRestClient client) : base(client, "Drawing")
    {
    }
}