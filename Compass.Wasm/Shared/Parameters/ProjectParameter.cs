using Compass.Wasm.Shared.Projects;

namespace Compass.Wasm.Shared.Parameters;

public class ProjectParameter:QueryParameter
{
    public ProjectStatus_e? ProjectStatus { get; set; }
    public Guid? ProjectId { get; set; }
}