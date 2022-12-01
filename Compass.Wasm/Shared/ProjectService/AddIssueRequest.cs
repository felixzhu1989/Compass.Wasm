namespace Compass.Wasm.Shared.ProjectService;

public class AddIssueRequest
{
    public Guid ProjectId { get; set; }
    public Guid ReportUserId { get; set; }//记录问题的人
    public ProjectStatus ProjectStatus { get; set; }
    //问题描述
    public Stakeholder Stakeholder { get; set; }
    public string? Description { get; set; }//问题描述
    public string? DescriptionUrl { get; set; }//上传得附件，多文件
}