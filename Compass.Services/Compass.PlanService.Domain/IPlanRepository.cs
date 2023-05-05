using Compass.PlanService.Domain.Entities;
using Compass.Wasm.Shared.Plans;

namespace Compass.PlanService.Domain;
public interface IPlanRepository
{
    //MainPlan
    Task<IQueryable<MainPlan>> GetMainPlansAsync();
    Task<MainPlan?> GetMainPlanByIdAsync(Guid id);


    //扩展MainPlan查询
    Task<IQueryable<MainPlan>> GetFilterMainPlansAsync(int year,int month, MainPlanType_e productionPlanType);



    Task<IQueryable<MainPlan>> GetMainPlansAsync(int year, MainPlanType_e productionPlanType);
    Task<IQueryable<MainPlan>> GetUnbindMainPlansAsync();
    Task<List<Guid?>> GetBoundMainPlansAsync();

    
    Task<MainPlan?> GetMainPlanByProjectIdAsync(Guid projectId);
    Task<CycleTimeDto> GetCycleTimeByMonthAsync(int year,int month);
    Task<CycleTimeDto> GetCycleTimeByYearAsync(int year);
}