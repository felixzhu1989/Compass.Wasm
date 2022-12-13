using Compass.CategoryService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zack.Infrastructure.EFCore;

namespace Compass.CategoryService.Infrastructure;

public class CateDbContext : BaseDbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<ModelType> ModelTypes { get; set; }

    public DbSet<ProblemType> ProblemTypes { get; set; }

    public CateDbContext(DbContextOptions<CateDbContext> options, IMediator? mediator) : base(options, mediator)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.EnableSoftDeletionGlobalFilter();
    }

}