namespace Compass.Wasm.Shared.ProjectService;

public class AddProblemRequest
{
    public Guid ProjectId { get; set; }
    public Guid ReportUserId { get; set; }//记录问题的人
    //问题描述
    public Guid ProblemTypeId { get; set; }//问题分类
    public string? Description { get; set; }//问题描述
    public string? DescriptionUrl { get; set; }//上传得附件，多文件
}