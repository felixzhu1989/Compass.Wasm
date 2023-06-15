using Compass.Wasm.Shared.Plans;

namespace Compass.Wasm.Shared.Projects;

public class ProjectDto:BaseDto
{
    #region 基本属性
    public string? OdpNumber { get; set; }
    public string? Name { get; set; }
    public DateTime DeliveryDate { get; set; }//交货时间，重要，根据它来排序
    public ProjectType_e ProjectType { get; set; }
    public RiskLevel_e RiskLevel { get; set; }
    public string? SpecialNotes { get; set; }
    public Guid? Designer { get; set; }//制图人，由项目经理指定
    #endregion

    #region 文件属性
    public string? ContractUrl { get; set; }
    public string? BomUrl { get; set; }
    public string? AttachmentsUrl { get; set; }
    public string? FinalInspectionUrl { get; set; }
    #endregion


    #region 状态属性
    public MainPlanStatus_e? Status { get; set; }//计划,制图,生产,入库,结束
    #endregion

    #region 扩展查询属性
    public string? UserName { get; set; }

    public List<MainPlanDto> MainPlanDtos { get; set; } = new();
    public List<DrawingDto> DrawingDtos { get; set; } = new();

    public bool AllIssueClosed { get; set; }
    public List<LessonDto> LessonDtos { get; set; } = new();
    #endregion

}