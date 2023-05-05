namespace Compass.Wasm.Shared.Projects;

public record ProblemDto
{
    public Guid Id { get; set; }
    //todo：记录问题时向相关方发送电子邮件
    public Guid ProjectId { get; set; }
    public DateTime CreationTime { get; set; }//记录问题的时间（不允许修改）
    public Guid ReportUserId { get; set; }//记录问题的人
    //问题描述
    public Guid ProblemTypeId { get; set; }//问题分类
    public string? Description { get; set; }//问题描述
    public string? DescriptionUrl { get; set; }//上传得附件，多文件

    //todo：pm指定责任人时向相关方发送电子邮件
    public Guid? ResponseUserId { get; set; }//解决问题的责任人，由项目经理指定
    public DateTime? Deadline { get; set; }//责令日期

    //todo：问题解决时向相关方发送电子邮件
    public string? Solution { get; set; }//解决方案
    public string? SolutionUrl { get; set; }//上传得附件，多文件

    //todo：pm宣布问题关闭时向相关方发送电子邮件
    public DateTime? CloseTime { get; set; }//问题解决的时间
    public bool IsClosed { get; set; }//是否结束
}