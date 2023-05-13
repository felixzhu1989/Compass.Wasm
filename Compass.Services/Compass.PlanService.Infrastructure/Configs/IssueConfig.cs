using Compass.PlanService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.PlanService.Infrastructure.Configs;

public class IssueConfig:IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.HasKey(x => x.Id).IsClustered(false);
        builder.HasIndex(x => new { x.MainPlanId, x.IsDeleted });//组合索引
        builder.HasIndex(x => new { x.Id, x.IsDeleted });//组合索引
        builder.Property(x => x.Title).HasConversion<string>();//将枚举值存储为字符串
    }
}