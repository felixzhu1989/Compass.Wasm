using Compass.QualityService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zack.Infrastructure.EFCore;

namespace Compass.QualityService.Infrastructure;

public class QualityDbContext : BaseDbContext
{
    public DbSet<FinalInspection> FinalInspections { get; set; }
    public DbSet<FinalInspectionCheckItem> FinalInspectionCheckItems { get; set; }
    public DbSet<FinalInspectionCheckItemType> FinalInspectionCheckItemTypes { get; set; }


    public QualityDbContext(DbContextOptions<QualityDbContext> options, IMediator? mediator) : base(options, mediator)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.EnableSoftDeletionGlobalFilter();
    }
}