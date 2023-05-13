using Compass.Wpf.ApiService;

namespace Compass.Wpf.ApiServices.Projects;
public interface IDrawingService : IBaseService<DrawingDto>
{
}
public class DrawingService : BaseService<DrawingDto>, IDrawingService
{
    public DrawingService(HttpRestClient client) : base(client, "Drawing")
    {
    }
}