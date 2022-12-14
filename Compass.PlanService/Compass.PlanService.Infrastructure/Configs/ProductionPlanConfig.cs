using Compass.PlanService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.PlanService.Infrastructure.Configs;

public class ProductionPlanConfig:IEntityTypeConfiguration<ProductionPlan>
{
    public void Configure(EntityTypeBuilder<ProductionPlan> builder)
    {
        builder.HasKey(x => x.Id).IsClustered(false);
        builder.HasIndex(x => new { x.ProjectId, x.IsDeleted });//组合索引
        builder.HasIndex(x => new { x.Id, x.IsDeleted });//组合索引
    }
}