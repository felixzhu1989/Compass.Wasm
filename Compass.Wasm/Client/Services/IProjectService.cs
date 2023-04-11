using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Client.Services;

public interface IProjectService:IBaseService<ProjectDto>
{
    Task<HttpResponseMessage> UploadFilesAsync(Guid id, ProjectDto dto);
}