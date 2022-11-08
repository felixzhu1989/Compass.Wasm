using Compass.ProjectService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Compass.ProjectService.Infrastructure.Configs;

public class DrawingPlanConfig : IEntityTypeConfiguration<DrawingPlan>
{
    public void Configure(EntityTypeBuilder<DrawingPlan> builder)
    {
        //因为SQLServer对于Guid主键默认创建聚集索引，因此会造成每次插入新数据，都会数据库重排。
        //因此我们取消主键的默认的聚集索引
        builder.HasKey(x => x.Id).IsClustered(false);
        builder.HasIndex(x => new { x.ProjectId, x.IsDeleted });//组合索引
        builder.HasIndex(x => new { x.Id, x.IsDeleted });//组合索引
    }
}