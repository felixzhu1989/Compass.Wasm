using Compass.Wasm.Shared.Data;
using Compass.Wasm.Shared.Data.Hoods;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zack.Infrastructure.EFCore;

namespace Compass.DataService.Infrastructure;

public class DataDbContext : BaseDbContext
{

    /* https://learn.microsoft.com/zh-cn/ef/core/modeling/inheritance
     * https://www.cnblogs.com/dotnet261010/p/8018266.html
     * EF 可以将 .NET 类型层次结构映射到数据库。
     * 这允许你像往常一样使用基类型和派生类型在代码中编写 .NET 实体，
     * 并让 EF 无缝创建适当的数据库架构、发出查询等。
     * Table per Class Hierarchy(TPH)继承
     * 共享列
     * 默认情况下，当层次结构中的两个同级实体类型具有同名的属性时，它们将映射到两个单独的列。 
     * 但是，如果它们的类型相同，则可以映射到相同的数据库列
     * modelBuilder.Entity<Blog>().Property(b => b.Url).HasColumnName("Url"); 
     * 可以通过foreach批量配置：property.SetColumnName(property.Name);
     */
    public DbSet<ModuleData> ModulesData { get; set; }

    #region 标准烟罩
    public DbSet<KviData> KviData { get; set; }
    public DbSet<KvfData> KvfData { get; set; }
    public DbSet<UviData> UviData { get; set; }
    public DbSet<UvfData> UvfData { get; set; }

    public DbSet<KwiData> KwiData { get; set; }
    public DbSet<KwfData> KwfData { get; set; }
    public DbSet<UwiData> UwiData { get; set; }
    public DbSet<UwfData> UwfData { get; set; }

    public DbSet<KvvData> KvvData { get; set; }

    #endregion





    public DataDbContext(DbContextOptions<DataDbContext> options, IMediator? mediator) : base(options, mediator)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        //批量配置列名，配置共享列
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                property.SetColumnName(property.Name);//将属性名设置为列名
            }
        }
    }
}