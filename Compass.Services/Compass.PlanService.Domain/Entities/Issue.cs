using Compass.Wasm.Shared.Plans;
using Zack.DomainCommons.Models;

namespace Compass.PlanService.Domain.Entities;

public record Issue : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    #region 基本属性
    //todo：记录问题时向相关方发送电子邮件
    public Guid MainPlanId { get; init; }//关联主计划
    public Guid ReporterId { get; init; }//记录问题的人
    //问题描述
    public IssueTitle_e Title { get; private set; }//标题
    public string? Content { get; private set; }//内容
    public string? ContentUrl { get; private set; }//上传附件，多文件 
    #endregion
    
    #region 状态属性
    public Guid? ResponderId { get; private set; }//解决问题的责任人，由项目经理指定
    public DateTime? Deadline { get; set; }//责令日期

    //todo：问题解决时向相关方发送电子邮件
    public string? Solution { get; private set; }//解决方案
    public string? SolutionUrl { get; private set; }//上传得附件，多文件
    public DateTime? CloseTime { get; private set; }//问题解决的时间
    public bool IsClosed { get; private set; }//是否结束 
    #endregion

    #region ctor
    private Issue() { }
    public Issue(Guid id, Guid mainPlanId, Guid reporterId, IssueTitle_e title, string? content, string? contentUrl)
    {
        Id = id;
        MainPlanId = mainPlanId;
        ReporterId = reporterId;
        Title = title;
        Content = content;
        ContentUrl = contentUrl;
    }
    #endregion

    #region Update
    public void Update(IssueDto dto)
    {
        ChangeTitle(dto.Title).ChangeContent(dto.Content).ChangeContentUrl(dto.ContentUrl);
        NotifyModified();
    }

    public Issue ChangeTitle(IssueTitle_e title)
    {
        Title=title;
        return this;
    }
    public Issue ChangeContent(string? conent)
    {
        Content=conent;
        return this;
    }
    public Issue ChangeContentUrl(string? contentUrl)
    {
        ContentUrl=contentUrl;
        return this;
    }
    #endregion

    #region UpdateStatus
    public void UpdateStatuses(IssueDto dto)
    {
        ChangeResponderId(dto.ResponderId)
            .ChangeDeadline(dto.Deadline)
            .ChangeSolution(dto.Solution)
            .ChangeSolutionUrl(dto.SolutionUrl)
            .ChangeCloseTime(dto.CloseTime)
            .ChangeIsClosed(dto.IsClosed);
        NotifyModified();
    }
    public Issue ChangeResponderId(Guid? responderId)
    {
        ResponderId=responderId;
        return this;
    }
    public Issue ChangeDeadline(DateTime? deadline)
    {
        Deadline=deadline;
        return this;
    }
    public Issue ChangeSolution(string? solution)
    {
        Solution=solution;
        return this;
    }
    public Issue ChangeSolutionUrl(string? solutionUrl)
    {
        SolutionUrl=solutionUrl;
        return this;
    }
    public Issue ChangeCloseTime(DateTime? closeTime)
    {
        CloseTime=closeTime;
        return this;
    }
    public Issue ChangeIsClosed(bool isClosed)
    {
        IsClosed=isClosed;
        return this;
    }
    #endregion
}