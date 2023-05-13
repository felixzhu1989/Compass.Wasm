using Compass.Wasm.Shared.Projects;
using Compass.Wasm.Shared;
using System.Net.Http.Json;

namespace Compass.Wasm.Client.Services.Projects;
public interface ILessonService : IBaseService<LessonDto>
{
    //扩展查询
    Task<ApiResponse<List<LessonDto>>> GetAllByProjectIdAsync(Guid projectId);
}
public class LessonService: BaseService<LessonDto>, ILessonService
{
    private readonly HttpClient _http;

    public LessonService(HttpClient http) : base(http, "Lesson")
    {
        _http = http;
    }

    public Task<ApiResponse<List<LessonDto>>> GetAllByProjectIdAsync(Guid projectId)
    {
        return _http.GetFromJsonAsync<ApiResponse<List<LessonDto>>>($"api/Lesson/Project/{projectId}")!;
    }
}