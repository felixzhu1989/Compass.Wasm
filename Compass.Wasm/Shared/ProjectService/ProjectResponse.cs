namespace Compass.Wasm.Shared.ProjectService;

public record ProjectResponse
{
    public Guid Id { get; set; }
    public string? OdpNumber { get; set; }
    public string? Name { get; set; }
    public DateTime ReceiveDate { get;set; }
    public DateTime DeliveryDate { get;set; }
    public ProjectType ProjectType { get; set; }
    public RiskLevel RiskLevel { get; set; }
    public string? SpecialNotes { get; set; }
    public string? ContractUrl { get; set; }
    public string? BomUrl { get; set; }
    public string? AttachmentsUrl { get;set; }
    public string? FinalInspectionUrl { get; set; }
}