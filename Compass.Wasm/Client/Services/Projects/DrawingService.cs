using System.Net.Http.Json;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Projects;

namespace Compass.Wasm.Client.Services.Projects;
public interface IDrawingService : IBaseService<DrawingDto>
{
    //扩展查询
    Task<ApiResponse<List<DrawingDto>>> GetAllByProjectIdAsync(Guid projectId);
}
public class DrawingService : BaseService<DrawingDto>, IDrawingService
{
    private readonly HttpClient _http;
    public DrawingService(HttpClient http) : base(http, "Drawing")
    {
        _http = http;
    }
    public Task<ApiResponse<List<DrawingDto>>> GetAllByProjectIdAsync(Guid projectId)
    {
        return _http.GetFromJsonAsync<ApiResponse<List<DrawingDto>>>($"api/Drawing/Project/{projectId}")!;
    }
}