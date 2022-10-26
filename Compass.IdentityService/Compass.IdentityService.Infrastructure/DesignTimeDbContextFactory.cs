using Compass.Wasm.Shared;
using Microsoft.EntityFrameworkCore.Design;

namespace Compass.IdentityService.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdDbContext>
{
    public IdDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = DbContextOptionsBuilderFactory.Create<IdDbContext>();
        return new IdDbContext(optionsBuilder.Options);
    }
}