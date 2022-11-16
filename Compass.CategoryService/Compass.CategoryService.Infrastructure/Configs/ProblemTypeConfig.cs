using Compass.CategoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.CategoryService.Infrastructure.Configs;

public class ProblemTypeConfig : IEntityTypeConfiguration<ProblemType>
{
    public void Configure(EntityTypeBuilder<ProblemType> builder)
    {
        builder.HasKey(x => x.Id).IsClustered();
        builder.Property(x => x.Stakeholder).IsRequired();
    }
}