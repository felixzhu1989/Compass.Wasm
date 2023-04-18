using Compass.CategoryService.Infrastructure;
using Compass.Wasm.Shared;
using Microsoft.EntityFrameworkCore.Design;

namespace Compass.ProjectService.Infrastructure;

public class DesignTimeDbContextFactory:IDesignTimeDbContextFactory<CategoryDbContext>
{
    public CategoryDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = DbContextOptionsBuilderFactory.Create<CategoryDbContext>();
        return new CategoryDbContext(optionBuilder.Options, null);
    }
}