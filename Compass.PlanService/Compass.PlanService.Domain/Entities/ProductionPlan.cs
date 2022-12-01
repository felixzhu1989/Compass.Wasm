using Compass.Wasm.Shared.PlanService;
using Zack.DomainCommons.Models;

namespace Compass.PlanService.Domain.Entities;

public record ProductionPlan:AggregateRootEntity, IAggregateRoot,IHasCreationTime, ISoftDelete
{
    public string SqNumber { get; private set; }//SQFS
    public string Name { get; private set; }
    public ProductionPlanClass ProductionPlanClass { get; private set; }
    public ProductionPlanStatus ProductionPlanStatus { get;private set; }

    public int Quantity { get; private set; }
    public int ItemLine { get; private set; }
    public string ModelSummary { get; private set; }
    public double StdWorkload { get; private set; }

    public string? Remarks { get; private set; }
    public string? SemiFinishedProduct { get; private set; }
    public string? PurchaseParts  { get; private set; }

    public double VtaValue { get; private set; }//换算成人民币RMB
    public DateTime MonthOfInvoice { get; private set; }//开票月份
    public bool Lock { get; set; }//锁定计划

    public DateTime ProductionFinishTime { get; private set; }
    public DateTime DrawingReleaseTarget { get; private set; }
    public DateTime DrawingReleaseActual { get; private set; }
    public DateTime NestingStart { get; private set; }
    public DateTime NestingEnd { get; private set; }
    public DateTime CuttingStart { get; private set; }
    public DateTime CuttingEnd { get; private set; }
    public DateTime BendingStart { get; private set; }
    public DateTime BendingEnd { get; private set; } 
    public DateTime WeldingStart { get; private set; }
    public DateTime WeldingEnd { get; private set; }
    public DateTime AssemblyStart { get; private set; }
    public DateTime AssemblyEnd { get; private set; }
    public DateTime AnsulStart { get; private set; }
    public DateTime AnsulEnd { get; private set; }
    public DateTime PackingDate { get; private set; }




}