using Compass.Wasm.Shared.Plans;

namespace Compass.Wasm.Shared.Parameters;

public class ProjectParameter:QueryParameter
{
    public MainPlanStatus_e? Status { get; set; }
    public Guid? ProjectId { get; set; }
}