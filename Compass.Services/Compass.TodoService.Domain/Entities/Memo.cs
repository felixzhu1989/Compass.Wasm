using Zack.DomainCommons.Models;

namespace Compass.TodoService.Domain.Entities;

public record Memo : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public string Title { get; private set; }
    public string Content { get; private set; }
    public Guid UserId { get; private set; }

    private Memo() { }
    public Memo(Guid id, string title, string content,Guid userId)
    {
        Id = id;
        Title = title;
        Content = content;
        UserId = userId;
    }
    public Memo ChangeTitle(string title)
    {
        Title = title;
        return this;
    }
    public Memo ChangeContent(string content)
    {
        Content = content;
        return this;
    }
}