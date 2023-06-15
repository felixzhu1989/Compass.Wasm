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


    //Drawing
    Task<IQueryable<Drawing>> GetDrawingsAsync();
    Task<Drawing?> GetDrawingByIdAsync(Guid id);
    //扩展Drawing查询
    Task<IQueryable<Drawing>> GetDrawingsByProjectIdAsync(Guid projectId);

    

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
    

    //Lesson
    Task<IQueryable<Lesson>> GetLessonsAsync();
    Task<Lesson?> GetLessonByIdAsync(Guid id);
    //扩展Lesson查询
    Task<IQueryable<Lesson>> GetLessonsByProjectIdAsync(Guid projectId);
}