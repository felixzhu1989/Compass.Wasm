namespace Compass.Wasm.Shared.PlanService;

public class CycleTimeResponse
{
    public int Month { get; set; }
    public int ProjectQuantity { get; set; }
    public double FactoryCycleTime { get; set; }
    public double ProductionCycleTime { get; set; }
}