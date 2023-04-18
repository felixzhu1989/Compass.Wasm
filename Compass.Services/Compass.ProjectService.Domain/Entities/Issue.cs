using Compass.Wasm.Shared.Projects;
using Zack.DomainCommons.Models;

namespace Compass.ProjectService.Domain.Entities;

public record Issue : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    //todo：记录经验教训时向相关方发送电子邮件?
    public Guid ProjectId { get; init; }
    public Guid ReportUserId { get; init; }//记录经验教训的人
    
    public ProjectStatus_e ProjectStatus { get; init; }//记录经验教训时，项目当前的状态
    //记录的经验教训描述
    public Stakeholder_e Stakeholder { get; set; }//相关方
    public string? Description { get; private set; }//问题描述
    public string? DescriptionUrl { get; private set; }//上传得附件，多文件
    
    private Issue() { }
    public Issue(Guid id, Guid projectId, Guid reportUserId, ProjectStatus_e projectStatus, Stakeholder_e stakeholder, string? description, string? descriptionUrl)
    {
        Id=id;
        ProjectId=projectId;
        ReportUserId = reportUserId;

        ProjectStatus = projectStatus;
        Stakeholder = stakeholder;
        Description = description;
        DescriptionUrl = descriptionUrl;
    }
    public Issue ChangeStakeholder(Stakeholder_e stakeholder)
    {
        Stakeholder = stakeholder;
        return this;
    }
    public Issue ChangeDescription(string? description)
    {
        Description = description;
        return this;
    }
    public Issue ChangeDescriptionUrl(string? descriptionUrl)
    {
        DescriptionUrl = descriptionUrl;
        return this;
    }
}