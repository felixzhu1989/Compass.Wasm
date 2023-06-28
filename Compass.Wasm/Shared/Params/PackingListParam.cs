namespace Compass.Wasm.Shared.Params;

public class PackingListParam:QueryParam
{
    public Guid? ProjectId { get; set; }
    public int? Batch { get; set; }
    public Guid? MainPlanId { get; set; }
}