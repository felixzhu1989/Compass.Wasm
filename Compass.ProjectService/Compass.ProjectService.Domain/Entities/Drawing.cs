using Zack.DomainCommons.Models;
namespace Compass.ProjectService.Domain.Entities;

public record Drawing : AggregateRootEntity, IAggregateRoot
{
    public Guid Id { get; init; }
    public string ItemNumber { get; private set; }
    public string? ImageUrl { get; private set; }

    public Guid ProjectId { get; private set; }//只能通过聚合根的标识符引用。
    public Guid? UserId { get; private set; }

    private Drawing() { }
    public Drawing(Guid id, Guid projectId, string itemNumber, string? imageUrl)
    {
        Id =id;
        ItemNumber = itemNumber;
        ImageUrl= imageUrl;
        ProjectId = projectId;
    }

    public Drawing ChangeItemNumber(string itemNumber)
    {
        ItemNumber= itemNumber;
        return this;
    }
    public Drawing ChangeImageUrl(string? imageUrl)
    {
        ImageUrl= imageUrl;
        return this;
    }

    public Drawing ChangeUserId(Guid userId)
    {
        UserId= userId;
        return this;
    }

}