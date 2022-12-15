using Compass.DataService.Domain;
using Compass.Wasm.Shared.DataService.Entities;
using Microsoft.EntityFrameworkCore;

namespace Compass.DataService.Infrastructure;

public class DataRepository : IDataRepository
{
    private readonly DataDbContext _context;
    public DataRepository(DataDbContext context)
    {
        _context = context;
    }
    public Task<List<ModuleData>> GetModulesDataAsync()
    {
        
       return _context.ModulesData.ToListAsync();
    }
    public Task<ModuleData?> GetModuleDataByIdAsync(Guid id)
    {
        return _context.ModulesData.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }
}