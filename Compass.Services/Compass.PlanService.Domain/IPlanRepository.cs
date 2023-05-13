using Compass.PlanService.Domain.Entities;
using Compass.Wasm.Shared.Plans;

namespace Compass.PlanService.Domain;
public interface IPlanRepository
{
    //MainPlan
    Task<IQueryable<MainPlan>> GetMainPlansAsync();
    Task<MainPlan?> GetMainPlanByIdAsync(Guid id);
    //扩展MainPlan查询
    Task<IQueryable<MainPlan>> GetMainPlansByProjectIdAsync(Guid projectId);
    
    Task<CycleTimeDto> GetCycleTimeByMonthAsync(int year,int month);
    Task<CycleTimeDto> GetCycleTimeByYearAsync(int year);


    //Issue
    Task<IQueryable<Issue>> GetIssuesAsync();
    Task<Issue?> GetIssueByIdAsync(Guid id);
    //扩展Issue查询
    Task<IQueryable<Issue>> GetIssuesByMainPlanIdAsync(Guid mainPlanId);
    //Task<IQueryable<Issue>> GetIssuesByProjectIdAsync(Guid projectId);





}