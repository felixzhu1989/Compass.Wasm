using Zack.DomainCommons.Models;

namespace Compass.TodoService.Domain.Entities;

public record Todo : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public string Title { get;private set; }
    public string Content { get; private set; }
    public int Status { get; private set; }
    public Guid UserId { get; private set; }

    private Todo() { }
    public Todo(Guid id,string title,string content,int status,Guid userId)
    {
        Id = id;
        Title = title;
        Content = content;
        Status = status;
        UserId = userId;
    }

    public Todo ChangeTitle(string title)
    {
        Title = title;
        return this;
    }
    public Todo ChangeContent(string content)
    {
        Content = content;
        return this;
    }
    public Todo ChangeStatus(int status)
    {
        Status = status;
        return this;
    }
}