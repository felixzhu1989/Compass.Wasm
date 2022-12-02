using Compass.DataService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.DataService.Infrastructure.Configs;

public class KvfDataConfig:IEntityTypeConfiguration<KvfData>
{
    //映射到单独的表格中
    public void Configure(EntityTypeBuilder<KvfData> builder)
    {
        builder.ToTable("KvfData");
        //特征5，有的属性不需要映到数据列，仅在运行时被使用。使用Ignore()来配置忽略这个属性。
        builder.Ignore(x => x.ModelTag);
    }
}