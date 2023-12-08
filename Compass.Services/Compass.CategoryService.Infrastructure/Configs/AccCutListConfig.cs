using Compass.CategoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.CategoryService.Infrastructure.Configs;

public class AccCutListConfig : IEntityTypeConfiguration<AccCutList>
{
    public void Configure(EntityTypeBuilder<AccCutList> builder)
    {
        builder.HasKey(x => x.Id).IsClustered(false);
        builder.HasIndex(x => new { x.Id, x.IsDeleted });//组合索引
        //将枚举值存储为字符串
        builder.Property(x => x.AccType).HasConversion<string>();
    }
}