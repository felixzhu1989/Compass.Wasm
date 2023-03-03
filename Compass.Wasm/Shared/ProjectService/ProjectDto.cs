namespace Compass.Wasm.Shared.ProjectService;

public class ProjectDto:BaseDto
{
    public string? OdpNumber { get; set; }
    public string? Name { get; set; }
    public DateTime DeliveryDate { get; set; }//交货时间，重要，根据它来排序
    public ProjectStatus_e ProjectStatus { get; set; }//计划,制图,生产,入库,结束

    public ProjectType_e ProjectType { get; set; }
    public RiskLevel_e RiskLevel { get; set; }
    public string? SpecialNotes { get; set; }
    public string? ContractUrl { get; set; }
    public string? BomUrl { get; set; }
    public string? AttachmentsUrl { get;set; }
    public string? FinalInspectionUrl { get; set; }
    
}