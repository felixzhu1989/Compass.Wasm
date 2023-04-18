namespace Compass.Wasm.Shared.Projects;

public class AddIssueRequest
{
    public Guid ProjectId { get; set; }
    public Guid ReportUserId { get; set; }//记录问题的人
    public ProjectStatus_e ProjectStatus { get; set; }
    //问题描述
    public Stakeholder_e Stakeholder { get; set; }
    public string? Description { get; set; }//问题描述
    public string? DescriptionUrl { get; set; }//上传得附件，多文件
}