using Compass.ProjectService.Domain;
using Compass.ProjectService.Domain.Entities;
using Compass.Wasm.Shared.ProjectService;
using Microsoft.EntityFrameworkCore;

namespace Compass.ProjectService.Infrastructure;

public class PMRepository: IPMRepository
{
    private readonly PMDbContext _context;
    public PMRepository(PMDbContext context)
    {
        _context = context;
    }
    #region Project
    public Task<IQueryable<Project>> GetProjectsAsync()
    {
        //return _context.Projects.OrderByDescending(x => x.CreationTime).ToListAsync();
        return Task.FromResult(_context.Projects.OrderByDescending(x => x.CreationTime).AsQueryable());
    }
    public Task<Project?> GetProjectByIdAsync(Guid id)
    {
        return _context.Projects.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }
   
    public Task<Project?> GetProjectByOdpAsync(string odpNumber)
    {
        return _context.Projects.SingleOrDefaultAsync(x => x.OdpNumber.Contains(odpNumber));
    }

    public async Task<string> GetOdpNumberByIdAsync(Guid id)
    {
        var project = await _context.Projects.SingleOrDefaultAsync(x => x.Id.Equals(id));
        return project.OdpNumber;
    }
    #endregion

    

    #region Drawing
    public Task<IQueryable<Drawing>> GetDrawingsByProjectIdAsync(Guid projectId)
    {
        return Task.FromResult(_context.Drawings.Where(x => x.ProjectId.Equals(projectId)).AsQueryable());
    }

    public Task<Drawing?> GetDrawingByIdAsync(Guid id)
    {
        return _context.Drawings.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }
    #endregion

    #region Module
    public Task<IQueryable<Module>> GetModulesByDrawingIdAsync(Guid drawingId)
    {
        return Task.FromResult(_context.Modules.Where(x => x.DrawingId.Equals(drawingId)).AsQueryable());
    }

    public Task<Module?> GetModuleByIdAsync(Guid id)
    {
        return _context.Modules.SingleOrDefaultAsync(x => x.Id.Equals(id));
    } 
    #endregion
}