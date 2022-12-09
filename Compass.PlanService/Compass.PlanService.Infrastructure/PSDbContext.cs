using Compass.PlanService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zack.Infrastructure.EFCore;

namespace Compass.PlanService.Infrastructure;

public class PSDbContext : BaseDbContext
{
    public DbSet<ProductionPlan> ProductionPlans { get; set; }

    public PSDbContext(DbContextOptions<PSDbContext> options, IMediator? mediator) : base(options, mediator)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.EnableSoftDeletionGlobalFilter();
    }
}