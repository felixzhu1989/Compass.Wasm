using Compass.Wasm.Shared.PlanService;
using Zack.DomainCommons.Models;

namespace Compass.PlanService.Domain.Entities;

public record ProductionPlan : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public DateTime OdpReleaseTime { get; private set; }//创建时间
    public string SqNumber { get; private set; }//SQ号码
    public string Name { get; private set; }
    public int Quantity { get; private set; }
    public string? ModelSummary { get; private set; }
    public DateTime ProductionFinishTime { get; private set; }
    public DateTime DrawingReleaseTarget { get; private set; }
    public DateTime MonthOfInvoice { get; private set; }//开票月份，input type=month
    public ProductionPlanType_e ProductionPlanType { get; private set; }//海工, ETO, KFC
    public string? Remarks { get; private set; }

    public Guid? ProjectId { get; private set; }//关联项目
    public DateTime? DrawingReleaseActual { get; private set; }
    //public double StdWorkload { get; private set; }
    //public double VtaValue { get; private set; }//换算成人民币RMB
    //public int ItemLine { get; private set; }
    //public bool Lock { get;private set; }//锁定计划
    //public ProductionPlanStatus ProductionPlanStatus { get; private set; }

    //public DateTime NestingStart { get; private set; }
    //public DateTime NestingEnd { get; private set; }
    //public DateTime CuttingStart { get; private set; }
    //public DateTime CuttingEnd { get; private set; }
    //public DateTime BendingStart { get; private set; }
    //public DateTime BendingEnd { get; private set; }
    //public DateTime WeldingStart { get; private set; }
    //public DateTime WeldingEnd { get; private set; }
    //public DateTime AssemblyStart { get; private set; }
    //public DateTime AssemblyEnd { get; private set; }
    //public DateTime AnsulStart { get; private set; }
    //public DateTime AnsulEnd { get; private set; }
    //public DateTime PackingDate { get; private set; }

    private ProductionPlan(){}
    public ProductionPlan(Guid id, DateTime odpReleaseTime, string sqNumber, string name, int quantity,string? modelSummary, DateTime productionFinishTime,DateTime drawingReleaseTarget,DateTime monthOfInvoice, ProductionPlanType_e productionPlanType, string? remarks)
    {
        Id = id;
        OdpReleaseTime = odpReleaseTime;
        SqNumber = sqNumber;
        Name = name;
        Quantity = quantity;
        ModelSummary = modelSummary;
        ProductionFinishTime = productionFinishTime;
        DrawingReleaseTarget = drawingReleaseTarget;
        MonthOfInvoice = monthOfInvoice;
        ProductionPlanType = productionPlanType;
        Remarks = remarks;
    }
    public ProductionPlan ChangeOdpReleaseTime(DateTime odpReleaseTime)
    {
        OdpReleaseTime = odpReleaseTime;
        return this;
    }
    public ProductionPlan ChangeSqNumber(string sqNumber)
    {
        SqNumber = sqNumber;
        return this;
    }
    public ProductionPlan ChangeName(string name)
    {
        Name = name;
        return this;
    }
    public ProductionPlan ChangeQuantity(int quantity)
    {
        Quantity = quantity;
        return this;
    }
    public ProductionPlan ChangeModelSummary(string? modelSummary)
    {
        ModelSummary = modelSummary;
        return this;
    }
    public ProductionPlan ChangeProductionFinishTime(DateTime productionFinishTime)
    {
        ProductionFinishTime = productionFinishTime;
        return this;
    }
    public ProductionPlan ChangeDrawingReleaseTarget(DateTime drawingReleaseTarget)
    {
        DrawingReleaseTarget = drawingReleaseTarget;
        return this;
    }
    public ProductionPlan ChangeMonthOfInvoice(DateTime monthOfInvoice)
    {
        MonthOfInvoice = monthOfInvoice;
        return this;
    }

    public ProductionPlan ChangeProductionPlanType(ProductionPlanType_e productionPlanType)
    {
        ProductionPlanType = productionPlanType;
        return this;
    }
    public ProductionPlan ChangeRemarks(string? remarks)
    {
        Remarks = remarks;
        return this;
    }


    public ProductionPlan ChangeProjectId(Guid? projectId)
    {
        ProjectId=projectId;
        return this;
    }
    public ProductionPlan ChangeDrawingReleaseActual(DateTime? drawingReleaseActual)
    {
        DrawingReleaseActual = drawingReleaseActual;
        return this;
    }
}