using Compass.ProjectService.Domain.Entities;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.ProjectService.Domain;
/// <summary>
/// 项目管理相关增删改查
/// </summary>
public interface IPMRepository
{
    //Project
    Task<IQueryable<Project>> GetProjectsAsync();
    Task<Project?> GetProjectByIdAsync(Guid id);
    Task<Project?> GetProjectByOdpAsync(string odpNumber);
    Task<string> GetOdpNumberByIdAsync(Guid id);
    //Drawing
    Task<IQueryable<Drawing>> GetDrawingsByProjectIdAsync(Guid projectId);
    Task<Drawing?> GetDrawingByIdAsync(Guid id);
    //Module
    Task<IQueryable<Module>> GetModulesByDrawingIdAsync(Guid drawingId);
    Task<Module?> GetModuleByIdAsync(Guid id);

}