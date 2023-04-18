using Compass.Wasm.Shared;
using Microsoft.EntityFrameworkCore.Design;

namespace Compass.ProjectService.Infrastructure;

public class DesignTimeDbContextFactory:IDesignTimeDbContextFactory<ProjectDbContext>
{
    public ProjectDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = DbContextOptionsBuilderFactory.Create<ProjectDbContext>();
        return new ProjectDbContext(optionBuilder.Options, null);
    }
}