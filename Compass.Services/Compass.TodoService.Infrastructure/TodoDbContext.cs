using Compass.TodoService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zack.Infrastructure.EFCore;

namespace Compass.TodoService.Infrastructure;

public class TodoDbContext : BaseDbContext
{
    public DbSet<Todo> Todos { get; set; }
    public DbSet<Memo> Memos { get; set; }

    public TodoDbContext(DbContextOptions<TodoDbContext> options, IMediator? mediator) : base(options, mediator)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.EnableSoftDeletionGlobalFilter();
    }
}