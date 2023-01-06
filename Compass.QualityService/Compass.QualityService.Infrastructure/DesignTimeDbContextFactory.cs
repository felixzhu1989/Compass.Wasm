using Compass.QualityService.Infrastructure;
using Compass.Wasm.Shared;
using Microsoft.EntityFrameworkCore.Design;

namespace Compass.ProjectService.Infrastructure;

public class DesignTimeDbContextFactory:IDesignTimeDbContextFactory<QualityDbContext>
{
    public QualityDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = DbContextOptionsBuilderFactory.Create<QualityDbContext>();
        return new QualityDbContext(optionBuilder.Options, null);
    }
}