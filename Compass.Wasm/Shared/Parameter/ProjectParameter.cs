using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Shared.Parameter;

public class ProjectParameter:QueryParameter
{
    public ProjectStatus_e? ProjectStatus { get; set; }
    public Guid? ProjectId { get; set; }
}