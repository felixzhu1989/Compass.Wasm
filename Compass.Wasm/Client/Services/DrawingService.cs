using System.Net.Http.Json;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Client.Services;

public class DrawingService:BaseService<DrawingDto>,IDrawingService
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