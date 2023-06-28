using Compass.Wasm.Shared.Plans;

namespace Compass.Wasm.Shared.Params;

public class ProjectParam:QueryParam
{
    public MainPlanStatus_e? Status { get; set; }
    public Guid? ProjectId { get; set; }
}