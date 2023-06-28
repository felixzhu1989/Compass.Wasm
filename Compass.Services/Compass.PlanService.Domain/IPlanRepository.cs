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
    Task<MainPlan?> GetMainPlanByProjectIdAndBatchAsync(Guid projectId, int batch);
    Task<List<Guid?>> GetProjectIdsByStatusAsync(MainPlanStatus_e? status);
    

    Task<CycleTimeDto> GetCycleTimeByMonthAsync(int year,int month);
    Task<CycleTimeDto> GetCycleTimeByYearAsync(int year);


    //Issue
    Task<IQueryable<Issue>> GetIssuesAsync();
    Task<Issue?> GetIssueByIdAsync(Guid id);
    //扩展Issue查询
    Task<IQueryable<Issue>> GetIssuesByMainPlanIdAsync(Guid mainPlanId);
    //Task<IQueryable<Issue>> GetIssuesByProjectIdAsync(Guid projectId);

    //PackingList
    Task<IQueryable<PackingList>> GetPackingListsAsync();
    Task<PackingList?> GetPackingListByIdAsync(Guid id);
    //扩展PackingList查询
    Task<PackingList?> GetPackingListByMainPlanIdAsync(Guid mainPlanId);

    //PackingItem
    Task<IQueryable<PackingItem>> GetPackingItemsAsync();
    Task<PackingItem?> GetPackingItemByIdAsync(Guid id);
    //扩展PackingItem查询
    Task<IQueryable<PackingItem>> GetPackingItemsByListIdAsync(Guid packingListId);

}