using Compass.ProjectService.Domain;
using Compass.ProjectService.Domain.Entities;
using Compass.Wasm.Shared.ProjectService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Compass.ProjectService.Infrastructure;

public class PMRepository : IPMRepository
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
        return Task.FromResult(_context.Projects.OrderByDescending(x => x.DeliveryDate).AsQueryable());
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

    #region DrawingPlan

    public Task<IQueryable<DrawingPlan>> GetDrawingPlansAsync()
    {
        return Task.FromResult(_context.DrawingsPlan.OrderByDescending(x => x.ReleaseTime).AsQueryable());
    }

    public Task<DrawingPlan?> GetDrawingPlanByIdAsync(Guid id)
    {
        return _context.DrawingsPlan.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task<DrawingPlan?> GetDrawingPlanByProjectIdAsync(Guid projectId)
    {
        return _context.DrawingsPlan.SingleOrDefaultAsync(x => x.ProjectId.Equals(projectId));
    }

    public async Task<IEnumerable<Project>> GetProjectsNotDrawingPlannedAsync()
    {
        var projects =await _context.Projects.ToListAsync();//所有项目
        var plannedProjects =await _context.DrawingsPlan.ToListAsync() ;//所有制图计划
        var notDrawingPlannedProjects = projects.Where(x => !plannedProjects.Exists(dp => x.Id.Equals(dp.ProjectId)));
        return notDrawingPlannedProjects;
    }

    public async Task<IEnumerable<Drawing>> GetDrawingsNotAssignedByProjectIdAsync(Guid projectId)
    {
        var drawings = await GetDrawingsByProjectIdAsync(projectId);
        var notAssignedDrawings = new List<Drawing>();
        foreach (var drawing in drawings)
        {
            if (drawing.UserId == null)//空的
            {
                notAssignedDrawings.Add(drawing);
            }
            else if (drawing.UserId.Equals(Guid.Empty))//全部是0
            {
                notAssignedDrawings.Add(drawing);
            }
        }
        return notAssignedDrawings;
    }

    public async Task<Dictionary<Guid?, IQueryable<Drawing>>> GetDrawingsAssignedByProjectIdAsync(Guid projectId)
    {
        var drawings = await GetDrawingsByProjectIdAsync(projectId);
        var userIds = drawings.Where(x => !string.IsNullOrWhiteSpace(x.UserId.ToString())).Select(x => x.UserId).Distinct();
        var assignedDrawings = new Dictionary<Guid?, IQueryable<Drawing>>();
        if (userIds != null && userIds.Count()!=0)
        {
            //userIds.OfType<Guid>()
            foreach (var userId in userIds)
            {
                var items = drawings.Where(x => x.UserId.Equals(userId));
                assignedDrawings.Add(userId, items);
            }
        }
        return assignedDrawings;
    }

    public async Task AssignDrawingsToUserAsync(IEnumerable<Guid> drawingIds, Guid userId)
    {
        foreach (var drawingId in drawingIds)
        {
            var dbDrawing = await GetDrawingByIdAsync(drawingId);
            dbDrawing.ChangeUserId(userId);
        }
    }

    public int GetDrawingsCountByProjectId(Guid projectId)
    {
        return _context.Drawings.Count(x => x.ProjectId.Equals(projectId));
    }

    public int GetDrawingsCountNotAssignedByProjectId(Guid projectId)
    {
        return _context.Drawings.Count(x => x.ProjectId.Equals(projectId)&& string.IsNullOrWhiteSpace(x.UserId.ToString()));
    }
    #endregion


}