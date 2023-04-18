using Zack.DomainCommons.Models;

namespace Compass.ProjectService.Domain.Entities;
/// <summary>
/// 记录项目异常
/// </summary>
public record Problem : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    //todo：记录问题时向相关方发送电子邮件
    public Guid ProjectId { get; init; }
    public Guid ReportUserId { get; init; }//记录问题的人
    //问题描述
    public Guid ProblemTypeId { get; private set; }//问题分类
    public string? Description { get; private set; }//问题描述
    public string? DescriptionUrl { get; private set; }//上传得附件，多文件

    //todo：指定责任人时向相关方发送电子邮件
    public Guid? ResponseUserId { get; private set; }//解决问题的责任人，由项目经理指定
    public DateTime? Deadline { get; set; }//责令日期

    //todo：问题解决时向相关方发送电子邮件
    public string? Solution { get; private set; }//解决方案
    public string? SolutionUrl { get; private set; }//上传得附件，多文件
    public DateTime? CloseTime { get; private set; }//问题解决的时间
    public bool IsClosed { get;private set; }//是否结束

    private Problem() { }
    public Problem(Guid id,Guid projectId,Guid reportUserId, Guid problemTypeId, string? description, string? descriptionUrl)
    {
        Id=id;
        ProjectId=projectId;
        ReportUserId = reportUserId;

        ProblemTypeId = problemTypeId;
        Description = description;
        DescriptionUrl = descriptionUrl;
    }

    public void Update()
    {

        NotifyModified();
    }


    public Problem ChangeProblemTypeId(Guid problemTypeId)
    {
        ProblemTypeId = problemTypeId;
        return this;
    }
    public Problem ChangeDescription(string? description)
    {
        Description = description;
        return this;
    }
    public Problem ChangeDescriptionUrl(string? descriptionUrl)
    {
        DescriptionUrl = descriptionUrl;
        return this;
    }

    public Problem ChangeResponseUserId(Guid? responseUserId)
    {
        ResponseUserId = responseUserId;
        return this;
    }
    public Problem ChangeDeadline(DateTime? deadline)
    {
        Deadline = deadline;
        return this;
    }

    public Problem ChangeSolution(string? solution)
    {
        Solution = solution;
        return this;
    }
    public Problem ChangeSolutionUrl(string? solutionUrl)
    {
        SolutionUrl = solutionUrl;
        return this;
    }
    
    public Problem ChangeCloseTime(DateTime? closeTime)
    {
        CloseTime = closeTime;
        return this;
    }
    public Problem ChangeIsClosed(bool isClosed)
    {
        IsClosed = isClosed;
        return this;
    }
}
