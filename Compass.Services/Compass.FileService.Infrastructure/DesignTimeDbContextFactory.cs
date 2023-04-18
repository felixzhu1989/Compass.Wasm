using Compass.Wasm.Shared;
using Microsoft.EntityFrameworkCore.Design;

namespace Compass.FileService.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FileDbContext>
{
    public FileDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = DbContextOptionsBuilderFactory.Create<FileDbContext>();
        return new FileDbContext(optionsBuilder.Options, null);
    }
}