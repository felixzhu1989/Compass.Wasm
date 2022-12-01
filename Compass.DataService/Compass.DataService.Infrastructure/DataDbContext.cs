using Compass.DataService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zack.Infrastructure.EFCore;

namespace Compass.DataService.Infrastructure;

public class DataDbContext : BaseDbContext
{

    /* https://learn.microsoft.com/zh-cn/ef/core/modeling/inheritance
     * EF 可以将 .NET 类型层次结构映射到数据库。
     * 这允许你像往常一样使用基类型和派生类型在代码中编写 .NET 实体，
     * 并让 EF 无缝创建适当的数据库架构、发出查询等。
     */

    public DbSet<ModuleData> ModulesData { get; set; }
    public DbSet<KvfData> KvfsData { get; set; }

    public DataDbContext(DbContextOptions<DataDbContext> options, IMediator? mediator) : base(options, mediator)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}