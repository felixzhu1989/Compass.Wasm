using Microsoft.AspNetCore.Identity;
using Zack.DomainCommons.Models;

namespace Compass.IdentityService.Domain.Entities;

public class User:IdentityUser<Guid>,IHasCreationTime,IHasDeletionTime,ISoftDelete
{
    public DateTime CreationTime { get; init; }
    public DateTime? DeletionTime { get;private set; }
    public bool IsDeleted { get;private set; }//软删除
    public User(string userName) : base(userName)
    {
        Id=Guid.NewGuid();
        CreationTime=DateTime.Now;
    }
    public void SoftDelete()
    {
        IsDeleted = true;
        DeletionTime=DateTime.Now;
    }
}