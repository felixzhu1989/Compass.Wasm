using Compass.Wasm.Shared.Plans;
using Zack.DomainCommons.Models;

namespace Compass.PlanService.Domain.Entities;

public record MainPlan : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    #region 基本属性
    public DateTime CreateTime { get; private set; }//创建时间
    public string Number { get; private set; }//SQ/FSO号码
    public string Name { get; private set; }
    public int Quantity { get; private set; }
    public string? ModelSummary { get; private set; }
    public DateTime FinishTime { get; private set; }
    public DateTime DrwReleaseTarget { get; private set; }
    public DateTime MonthOfInvoice { get; private set; }//开票月份，input type=month
    public MainPlanType_e MainPlanType { get; private set; }//海工, ETO, KFC
    public string? Remarks { get; private set; }
    public int Batch { get; private set; }//分批
    #endregion

    #region 记录，暂时不显示的属性
    public int ItemLine { get; private set; }//订单行
    public double Workload { get; private set; }//工作量
    public double Value { get; private set; }//税后价格
    #endregion

    #region 状态属性
    public Guid? ProjectId { get; private set; }//关联项目,可以多个主计划关联到一个订单
    public MainPlanStatus_e Status { get; private set; }//计划,制图,生产,入库,结束

    public DateTime? DrwReleaseTime { get; private set; }
    public DateTime? WarehousingTime { get; private set; }//第一台生产完工入库的时间->进入库存状态
    public DateTime? ShippingTime { get; private set; }//项目第一台真实发货的时间->进入发货状态，用减去WarehousingTime，用户计算成品库存时间

    #endregion

    #region ctor
    private MainPlan() { }
    public MainPlan(Guid id, DateTime createTime, string number, string name, int quantity, string? modelSummary, DateTime finishTime, DateTime drwReleaseTarget, DateTime monthOfInvoice, MainPlanType_e mainPlanType, string? remarks, int batch, int itemLine, double workload, double value)
    {
        Id = id;
        CreateTime = createTime;
        Number = number;
        Name = name;
        Quantity = quantity;
        ModelSummary = modelSummary;
        FinishTime = finishTime;
        DrwReleaseTarget = drwReleaseTarget;
        MonthOfInvoice = monthOfInvoice;
        MainPlanType = mainPlanType;
        Remarks = remarks;
        Batch=batch;
        Status = MainPlanStatus_e.计划;
        ItemLine = itemLine;
        Workload=workload;
        Value = value;
    }
    #endregion

    #region Update
    public void Update(MainPlanDto dto)
    {
        ChangeCreateTime(dto.CreateTime)
            .ChangeNumber(dto.Number)
            .ChangeName(dto.Name)
            .ChangeQuantity(dto.Quantity)
            .ChangeModelSummary(dto.ModelSummary)
            .ChangeFinishTime(dto.FinishTime)
            .ChangeDrwReleaseTarget(dto.DrwReleaseTarget)
            .ChangeMonthOfInvoice(dto.MonthOfInvoice)
            .ChangeMainPlanType(dto.MainPlanType)
            .ChangeRemarks(dto.Remarks)
            .ChangeBatch(dto.Batch)
            .ChangeItemLine(dto.ItemLine)
            .ChangeWorkload(dto.Workload)
            .ChangeValue(dto.Value);
        NotifyModified();
    }

    #region 基本属性
    public MainPlan ChangeCreateTime(DateTime createTime)
    {
        CreateTime = createTime;
        return this;
    }
    public MainPlan ChangeNumber(string number)
    {
        Number = number;
        return this;
    }
    public MainPlan ChangeName(string name)
    {
        Name = name;
        return this;
    }
    public MainPlan ChangeQuantity(int quantity)
    {
        Quantity = quantity;
        return this;
    }
    public MainPlan ChangeModelSummary(string? modelSummary)
    {
        ModelSummary = modelSummary;
        return this;
    }
    public MainPlan ChangeFinishTime(DateTime finishTime)
    {
        FinishTime = finishTime;
        return this;
    }
    public MainPlan ChangeDrwReleaseTarget(DateTime drwReleaseTarget)
    {
        DrwReleaseTarget = drwReleaseTarget;
        return this;
    }
    public MainPlan ChangeMonthOfInvoice(DateTime monthOfInvoice)
    {
        MonthOfInvoice = monthOfInvoice;
        return this;
    }
    public MainPlan ChangeMainPlanType(MainPlanType_e mainPlanType)
    {
        MainPlanType = mainPlanType;
        return this;
    }
    public MainPlan ChangeRemarks(string? remarks)
    {
        Remarks = remarks;
        return this;
    }
    public MainPlan ChangeBatch(int batch)
    {
        Batch = batch;
        return this;
    }
    #endregion

    #region 记录，暂时不显示的属性
    public MainPlan ChangeItemLine(int itemLine)
    {
        ItemLine = itemLine;
        return this;
    }
    public MainPlan ChangeWorkload(double workload)
    {
        Workload=workload;
        return this;
    }
    public MainPlan ChangeValue(double value)
    {
        Value = value;
        return this;
    }
    #endregion

    #endregion

    #region 变更状态属性
    public void UpdateStatuses(MainPlanDto dto)
    {
        ChangeStatus(dto.Status)
            .ChangeProjectId(dto.ProjectId)//MainPlanStatus_e.生产
            .ChangeDrwReleaseTime(dto.DrwReleaseTime)//MainPlanStatus_e.生产
            .ChangeWarehousingTime(dto.WarehousingTime)//MainPlanStatus_e.入库
            .ChangeShippingTime(dto.ShippingTime);//MainPlanStatus_e.发货
                                                  //todo:月度总结的时候，将计划状态更改为结束
        NotifyModified();
    }


    public MainPlan ChangeProjectId(Guid? projectId)
    {
        ProjectId=projectId;
        return this;
    }
    public MainPlan ChangeStatus(MainPlanStatus_e status)
    {
        Status=status;
        return this;
    }

    public MainPlan ChangeDrwReleaseTime(DateTime? drwReleaseTime)
    {
        DrwReleaseTime = drwReleaseTime;
        return this;
    }

    public MainPlan ChangeWarehousingTime(DateTime? warehousingTime)
    {
        WarehousingTime = warehousingTime;
        return this;
    }
    public MainPlan ChangeShippingTime(DateTime? shippingTime)
    {
        ShippingTime = shippingTime;
        return this;
    }
    #endregion
}