using Zack.DomainCommons.Models;

namespace Compass.ProjectService.Domain.Entities;

public record Module:AggregateRootEntity,IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public Guid DrawingId { get; private set; }//只能通过聚合根的标识符引用。
    public Guid ModelTypeId { get; private set; }//标明该分段是属于什么什么子模型
    public string Name { get; private set; }
    public string? SpecialNotes { get;private set; }
    public bool IsReleased { get; private set; }//图纸是否已经下发

    private Module() { }
    public Module(Guid id,Guid drawingId,Guid modelTypeId, string name,string? specialNotes)
    {
        Id = id;
        DrawingId = drawingId;
        ModelTypeId = modelTypeId;
        Name = name;
        SpecialNotes = specialNotes;
    }
    public Module ChangeModelTypeId(Guid modelTypeId)
    {
        ModelTypeId = modelTypeId;
        return this;
    }
    public Module ChangeName(string name)
    {
        Name = name;
        return this;
    }
    public Module ChangeSpecialNotes(string? specialNotes)
    {
        SpecialNotes = specialNotes;
        return this;
    }
    public Module ChangeIsReleased(bool isReleased)
    {
        IsReleased = isReleased;
        return this;
    }
}