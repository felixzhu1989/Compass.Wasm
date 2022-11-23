using Compass.ProjectService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zack.Infrastructure.EFCore;

namespace Compass.ProjectService.Infrastructure;

public class PMDbContext:BaseDbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Drawing> Drawings { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<DrawingPlan> DrawingsPlan { get; set; }
    public DbSet<Tracking> Trackings { get; set; }
    public DbSet<Problem> Problems { get; set; }
    public DbSet<Issue> Issues { get; set; }

    public PMDbContext(DbContextOptions<PMDbContext> options, IMediator? mediator) : base(options, mediator)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.EnableSoftDeletionGlobalFilter();
    }
}