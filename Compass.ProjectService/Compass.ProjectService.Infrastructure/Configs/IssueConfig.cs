using Compass.ProjectService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.ProjectService.Infrastructure.Configs;

public class IssueConfig : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.HasKey(x => x.Id).IsClustered(false);
        builder.HasIndex(x => new { x.Id, x.IsDeleted });//组合索引
        builder.HasIndex(x => new { x.ProjectId, x.IsDeleted });//组合索引
    }
}