using Compass.CategoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.CategoryService.Infrastructure.Configs;

public class ModelTypeConfig:IEntityTypeConfiguration<ModelType>
{
    public void Configure(EntityTypeBuilder<ModelType> builder)
    {
        builder.HasKey(x => x.Id).IsClustered(false);
        builder.HasIndex(x => new { x.ModelId, x.IsDeleted });//组合索引
    }
}