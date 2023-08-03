﻿using Compass.PlanService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.PlanService.Infrastructure.Configs;

public class PackingItemConfig:IEntityTypeConfiguration<PackingItem>
{
    public void Configure(EntityTypeBuilder<PackingItem> builder)
    {
        builder.HasKey(x => x.Id).IsClustered(false);
        builder.HasIndex(x => new { x.PackingListId, x.IsDeleted });//组合索引
        builder.HasIndex(x => new { x.Id, x.IsDeleted });//组合索引
        //将枚举值存储为字符串
        builder.Property(x => x.Unit).HasConversion<string>();
    }
}