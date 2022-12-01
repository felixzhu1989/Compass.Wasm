using Compass.DataService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.DataService.Infrastructure.Configs;

public class ModuleDataConfig : IEntityTypeConfiguration<ModuleData>
{
    public void Configure(EntityTypeBuilder<ModuleData> builder)
    {
        //因为SQLServer对于Guid主键默认创建聚集索引，因此会造成每次插入新数据，都会数据库重排。
        //因此我们取消主键的默认的聚集索引
        builder.HasKey(x => x.Id).IsClustered(false);
    }
}