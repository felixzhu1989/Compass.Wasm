using Compass.PlanService.Infrastructure;
using Compass.Wasm.Shared;
using Microsoft.EntityFrameworkCore.Design;

namespace Compass.ProjectService.Infrastructure;

public class DesignTimeDbContextFactory:IDesignTimeDbContextFactory<PSDbContext>
{
    public PSDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = DbContextOptionsBuilderFactory.Create<PSDbContext>();
        return new PSDbContext(optionBuilder.Options, null);
    }
}