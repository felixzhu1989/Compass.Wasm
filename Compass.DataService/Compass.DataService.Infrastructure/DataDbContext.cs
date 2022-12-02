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
     * TPC：Table Per Concrete Type
     * 在 TPC 映射模式中，所有类型都映射到单个表。 每个表包含对应实体类型上所有属性的列。
     * 这解决了 TPT 策略的一些常见性能问题。
     * 在每个类的config中配置如下
     * builder.UseTpcMappingStrategy();//基类
     * builder.ToTable("XXX");//子类
     */

    public virtual DbSet<ModuleData> ModulesData { get; set; }
    public DbSet<KvfData> KvfData { get; set; }
    public DbSet<UviData> UviData { get; set; }


    public DataDbContext(DbContextOptions<DataDbContext> options, IMediator? mediator) : base(options, mediator)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}