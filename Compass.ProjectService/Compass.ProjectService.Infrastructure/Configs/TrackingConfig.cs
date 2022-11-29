using Compass.ProjectService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.ProjectService.Infrastructure.Configs;

public class TrackingConfig:IEntityTypeConfiguration<Tracking>
{
    public void Configure(EntityTypeBuilder<Tracking> builder)
    {
        builder.HasKey(x => x.Id).IsClustered(false);
        builder.HasIndex(x => new { x.Id, x.IsDeleted });//组合索引

        ////特征5，有的属性不需要映到数据列，仅在运行时被使用。使用Ignore()来配置忽略这个属性。
        //builder.Ignore(u => u.DeliveryDate);
    }
}