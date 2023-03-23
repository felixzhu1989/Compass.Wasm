using Compass.ProjectService.Domain.Entities;
using Compass.Wasm.Shared;

namespace Compass.ProjectService.Domain;
/// <summary>
/// 项目管理相关增删改查
/// </summary>
public interface IProjectRepository
{
    //Project
    Task<IQueryable<Project>> GetProjectsAsync();
    Task<Project?> GetProjectByIdAsync(Guid id);
    //扩展Project查询


    Task<Project?> GetProjectByOdpAsync(string odpNumber);
    Task<string> GetOdpNumberByIdAsync(Guid id);
    Task<IQueryable<Project>> GetUnbindProjectsAsync(List<Guid?> ids);

    //Drawing
    Task<IQueryable<Drawing>> GetDrawingsAsync();
    Task<Drawing?> GetDrawingByIdAsync(Guid id);
    //扩展Drawing查询
    Task<IQueryable<Drawing>> GetDrawingsByProjectIdAsync(Guid projectId);



    Task<IQueryable<Drawing>> GetDrawingsByUserIdAsync(Guid userId);
    Task<bool> DrawingExistsInProjectAsync(Guid projectId);
    Task<int> GetTotalDrawingsCountInProjectAsync(Guid projectId);
    Task<int> GetNotAssignedDrawingsCountInProjectAsync(Guid projectId);


    //Module
    Task<IQueryable<Module>> GetModulesAsync();
    Task<Module?> GetModuleByIdAsync(Guid id);
    //扩展Module查询
    Task<IQueryable<Module>> GetModulesByDrawingIdAsync(Guid drawingId);


    Task<bool> ModuleExistsInDrawing(Guid drawingId);
    Task<string?> GetDrawingUrlByModuleIdAsync(Guid id);

    //CutList
    Task<IQueryable<CutList>> GetCutListsAsync();
    Task<CutList?> GetCutListByIdAsync(Guid id);
    //扩展CutList查询
    Task<IQueryable<CutList>> GetCutListsByModuleIdAsync(Guid moduleId);








    //DrawingPlan
    Task<ApiPaginationResponse<IQueryable<DrawingPlan>>> GetDrawingPlansAsync(int page);
    Task<DrawingPlan?> GetDrawingPlanByIdAsync(Guid id);

    //编制计划时查找那些项目还没有编制计划
    Task<IEnumerable<Project>> GetProjectsNotDrawingPlannedAsync();
    //判断项目中是否有图纸没有分配人员
    //Task<bool> IsDrawingsNotAssignedByProjectIdAsync(Guid projectId);
    //编制计划时查找项目中那些图纸没有分配人员
    Task<IEnumerable<Drawing>> GetDrawingsNotAssignedByProjectIdAsync(Guid projectId);
    //编制计划时查找项目中那些图纸已经分配人员，并按照人员分组
    Task<Dictionary<Guid, IQueryable<Drawing>>> GetDrawingsAssignedByProjectIdAsync(Guid projectId);
    Task AssignDrawingsToUserAsync(IEnumerable<Guid> drawingIds, Guid userId);

   




    //Tracking
    Task<ApiPaginationResponse<IQueryable<Tracking>>> GetTrackingsAsync(int page);
    Task<Tracking?> GetTrackingByIdAsync(Guid id);
    //搜索针对Tracking
    Task<ApiPaginationResponse<IQueryable<Tracking>>> SearchTrackingsAsync(string searchText,int page);
    Task<List<string>> GetProjectSearchSuggestions(string searchText);

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