using Compass.Wasm.Shared;
using Microsoft.EntityFrameworkCore.Design;

namespace Compass.TodoService.Infrastructure;

public class DesignTimeDbContextFactory:IDesignTimeDbContextFactory<TodoDbContext>
{
    public TodoDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = DbContextOptionsBuilderFactory.Create<TodoDbContext>();
        return new TodoDbContext(optionBuilder.Options, null);
    }
}