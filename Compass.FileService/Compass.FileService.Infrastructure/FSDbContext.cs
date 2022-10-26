using Compass.FileService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zack.Infrastructure.EFCore;

namespace Compass.FileService.Infrastructure;

public class FSDbContext:BaseDbContext
{
    public DbSet<UploadedItem> UploadedItems { get; private set; }
    public FSDbContext(DbContextOptions options, IMediator? mediator) : base(options, mediator)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}