using System.Net.Http.Json;
using Compass.Wasm.Shared.Projects;

namespace Compass.Wasm.Client.Services;

public class ProjectService:BaseService<ProjectDto>, IProjectService
{
    private readonly HttpClient _http;
    public ProjectService(HttpClient http) : base(http, "Project")
    {
        _http = http;
    }
    public Task<HttpResponseMessage> UploadFilesAsync(Guid id, ProjectDto dto)
    {
        return _http.PutAsJsonAsync($"api/Project/UploadFiles/{id}", dto);

    }
}