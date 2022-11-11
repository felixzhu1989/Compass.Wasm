using Zack.DomainCommons.Models;

namespace Compass.ProjectService.Domain.Entities;
/// <summary>
/// 记录项目异常日志
/// </summary>
public record Issue : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    //todo：记录问题时向相关方发送电子邮件
    public Guid ProjectId { get; private set; }
    public Guid IssueTypeId { get; private set; }//问题分类
    public DateTime RecordTime { get; init; }//记录问题的时间（不允许修改）
    public string Description { get; private set; }//问题描述
   
    //todo：指定责任人时向相关方发送电子邮件
    public Guid UserId { get; private set; }//解决问题的责任人，由项目经理指定

    //todo：问题解决时向相关方发送电子邮件
    public string Solution { get; private set; }//解决方案
    public DateTime CloseTime { get; private set; }//问题解决的时间
}
