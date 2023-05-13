using Compass.PlanService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zack.Infrastructure.EFCore;

namespace Compass.PlanService.Infrastructure;

public class PlanDbContext : BaseDbContext
{
    public DbSet<MainPlan> MainPlans { get; set; }
    public DbSet<Issue> Issues { get; set; }

    public PlanDbContext(DbContextOptions<PlanDbContext> options, IMediator? mediator) : base(options, mediator)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.EnableSoftDeletionGlobalFilter();
    }
}