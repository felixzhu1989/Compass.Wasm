using Compass.CategoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.CategoryService.Infrastructure.Configs;

public class IssueTypeConfig : IEntityTypeConfiguration<IssueType>
{
    public void Configure(EntityTypeBuilder<IssueType> builder)
    {
        builder.HasKey(x => x.Id).IsClustered();
        builder.Property(x => x.Stakeholder).IsRequired();
    }
}