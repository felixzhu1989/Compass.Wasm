using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Client.Services;

public class ProjectService:BaseService<ProjectDto>, IProjectService
{
    public ProjectService(HttpClient http) : base(http, "Project")
    {
    }


}