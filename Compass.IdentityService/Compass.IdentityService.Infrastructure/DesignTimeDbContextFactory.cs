using Compass.Wasm.Shared;
using Microsoft.EntityFrameworkCore.Design;

namespace Compass.IdentityService.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = DbContextOptionsBuilderFactory.Create<IdentityDbContext>();
        return new IdentityDbContext(optionsBuilder.Options);
    }
}