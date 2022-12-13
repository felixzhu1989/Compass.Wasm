using Compass.CategoryService.Infrastructure;
using Compass.Wasm.Shared;
using Microsoft.EntityFrameworkCore.Design;

namespace Compass.ProjectService.Infrastructure;

public class DesignTimeDbContextFactory:IDesignTimeDbContextFactory<CateDbContext>
{
    public CateDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = DbContextOptionsBuilderFactory.Create<CateDbContext>();
        return new CateDbContext(optionBuilder.Options, null);
    }
}