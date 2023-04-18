using Compass.PlanService.Infrastructure;
using Compass.Wasm.Shared;
using Microsoft.EntityFrameworkCore.Design;

namespace Compass.ProjectService.Infrastructure;

public class DesignTimeDbContextFactory:IDesignTimeDbContextFactory<PlanDbContext>
{
    public PlanDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = DbContextOptionsBuilderFactory.Create<PlanDbContext>();
        return new PlanDbContext(optionBuilder.Options, null);
    }
}