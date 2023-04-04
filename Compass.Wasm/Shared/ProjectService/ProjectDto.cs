namespace Compass.Wasm.Shared.ProjectService;

public class ProjectDto:BaseDto
{
    #region 基本属性
    public string? OdpNumber { get; set; }
    public string? Name { get; set; }
    public DateTime DeliveryDate { get; set; }//交货时间，重要，根据它来排序
    public ProjectType_e ProjectType { get; set; }
    public RiskLevel_e RiskLevel { get; set; }
    public string? SpecialNotes { get; set; }
    #endregion

    #region 文件属性
    public string? ContractUrl { get; set; }
    public string? BomUrl { get; set; }
    public string? AttachmentsUrl { get; set; }
    public string? FinalInspectionUrl { get; set; }
    #endregion


    #region 状态属性
    public ProjectStatus_e ProjectStatus { get; set; }//计划,制图,生产,入库,结束
    //有没有待解决得问题，如果有则另起一行显示异常详细信息
    public bool IsProblemNotResolved { get; set; }
    //是否绑定了生产主计划
    public bool IsBoundMainPlan { get; set; } 
    #endregion

}