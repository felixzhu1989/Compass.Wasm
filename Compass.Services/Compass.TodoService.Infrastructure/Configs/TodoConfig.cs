using Compass.TodoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.TodoService.Infrastructure.Configs;

public class TodoConfig : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.HasKey(x => x.Id).IsClustered(false);
        builder.HasIndex(x => new { x.Id, x.IsDeleted });//组合索引
        //builder.HasIndex(x => new { x.ProjectId, x.IsDeleted });//组合索引
    }
}