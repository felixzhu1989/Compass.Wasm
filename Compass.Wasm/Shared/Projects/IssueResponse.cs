namespace Compass.Wasm.Shared.Projects;

public record IssueResponse
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public DateTime CreationTime { get; set; }//记录问题的时间（不允许修改）
    public Guid ReportUserId { get; set; }//记录经验教训的人
    
    public ProjectStatus_e ProjectStatus { get; set; }//记录经验教训时，项目当前的状态
    //记录的经验教训描述
    public Stakeholder_e Stakeholder { get; set; }
    public string? Description { get; set; }//问题描述
    public string? DescriptionUrl { get; set; }//上传得附件，多文件
}