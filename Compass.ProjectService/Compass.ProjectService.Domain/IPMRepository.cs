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
    Task<IQueryable<Drawing>> GetDrawingsByUserIdAsync(Guid userId);
    Task<Drawing?> GetDrawingByIdAsync(Guid id);
    Task<bool> DrawingExistsInProjectAsync(Guid projectId);
    Task<int> GetTotalDrawingsCountInProjectAsync(Guid projectId);
    Task<int> GetNotAssignedDrawingsCountInProjectAsync(Guid projectId);


    //Module
    Task<IQueryable<Module>> GetModulesByDrawingIdAsync(Guid drawingId);
    Task<Module?> GetModuleByIdAsync(Guid id);
    Task<bool> ModuleExistsInDrawing(Guid drawingId);

    //DrawingPlan
    Task<IQueryable<DrawingPlan>> GetDrawingPlansAsync();
    Task<DrawingPlan?> GetDrawingPlanByIdAsync(Guid id);

    //编制计划时查找那些项目还没有编制计划
    Task<IEnumerable<Project>> GetProjectsNotDrawingPlannedAsync();
    //判断项目中是否有图纸没有分配人员
    Task<bool> IsDrawingsNotAssignedByProjectIdAsync(Guid projectId);
    //编制计划时查找项目中那些图纸没有分配人员
    Task<IEnumerable<Drawing>> GetDrawingsNotAssignedByProjectIdAsync(Guid projectId);
    //编制计划时查找项目中那些图纸已经分配人员，并按照人员分组
    Task<Dictionary<Guid, IQueryable<Drawing>>> GetDrawingsAssignedByProjectIdAsync(Guid projectId);
    Task AssignDrawingsToUserAsync(IEnumerable<Guid> drawingIds, Guid userId);

   




    //Tracking
    Task<IQueryable<Tracking>> GetTrackingsAsync();
    Task<Tracking?> GetTrackingByIdAsync(Guid id);

    //Problem
    Task<IQueryable<Problem>> GetProblemsAsync();
    Task<IQueryable<Problem>> GetProblemsByProjectIdAsync(Guid projectId);
    Task<Problem?> GetProblemByIdAsync(Guid id);
    Task<IQueryable<Problem>> GetNotResolvedProblemsByProjectIdAsync(Guid projectId);

    //Issue
    Task<IQueryable<Issue>> GetIssuesAsync();
    Task<IQueryable<Issue>> GetIssuesByProjectIdAsync(Guid projectId);
    Task<Issue?> GetIssueByIdAsync(Guid id);
}