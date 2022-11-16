using Zack.DomainCommons.Models;
namespace Compass.ProjectService.Domain.Entities;

public record Drawing : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public Guid ProjectId { get; private set; }//只能通过聚合根的标识符引用。

    public string ItemNumber { get; private set; }
    
    //图纸链接，多张图纸使用回车\n隔开
    public string? DrawingUrl { get; private set; }

    
    public Guid? UserId { get; private set; }


    private Drawing() { }
    public Drawing(Guid id, Guid projectId, string itemNumber,string? drawingUrl)
    {
        Id =id;
        ProjectId = projectId;
        ItemNumber = itemNumber;
        DrawingUrl = drawingUrl;
    }
    public Drawing ChangeItemNumber(string itemNumber)
    {
        ItemNumber= itemNumber;
        return this;
    }
    public Drawing ChangeDrawingUrl(string drawingUrl)
    {
        DrawingUrl = drawingUrl;
        return this;
    }
    public Drawing ChangeUserId(Guid userId)
    {
        UserId= userId;
        return this;
    }
}