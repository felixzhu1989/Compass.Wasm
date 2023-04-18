using Zack.DomainCommons.Models;

namespace Compass.Wasm.Shared.Data;

public class BaseDataEntity : BaseDto, IAggregateRoot, ISoftDelete, IHasDeletionTime,IHasCreationTime, IHasModificationTime
{
    public bool IsDeleted { get; private set; }
    public DateTime? DeletionTime { get; private set; }
    public virtual void SoftDelete()
    {
        IsDeleted = true;
        DeletionTime = DateTime.Now;
    }
    public void NotifyModified()
    {
        LastModificationTime = DateTime.Now;
    }
}