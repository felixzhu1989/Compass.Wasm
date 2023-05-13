using Compass.Wasm.Shared.Projects;
using Zack.DomainCommons.Models;

namespace Compass.ProjectService.Domain.Entities;

public record Lesson : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public Guid ProjectId { get; init; }

    //记录的经验教训(项目总结)描述
    public string? Content { get; private set; }//问题描述
    public string? ContentUrl { get; private set; }//上传得附件，多文件
    
    private Lesson() { }
    public Lesson(Guid id, Guid projectId, string? content, string? contentUrl)
    {
        Id=id;
        ProjectId=projectId;
        Content = content;
        ContentUrl = contentUrl;
    }

    public void Update(LessonDto dto)
    {
        ChangeContent(dto.Content).ChangeContentUrl(dto.ContentUrl);
        NotifyModified();
    }
    public Lesson ChangeContent(string? content)
    {
        Content = content;
        return this;
    }
    public Lesson ChangeContentUrl(string? contentUrl)
    {
        ContentUrl = contentUrl;
        return this;
    }
}