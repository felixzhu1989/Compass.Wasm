using Compass.FileService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zack.Infrastructure.EFCore;

namespace Compass.FileService.Infrastructure;

public class FileDbContext:BaseDbContext
{
    public DbSet<UploadedItem> UploadedItems { get; private set; }
    public FileDbContext(DbContextOptions<FileDbContext> options, IMediator? mediator) : base(options, mediator)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}