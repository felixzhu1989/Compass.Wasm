using Compass.PlanService.Domain.Entities;
using Compass.Wasm.Shared.Plans;

namespace Compass.PlanService.Domain;
public interface IPlanRepository
{
    Task<IQueryable<ProductionPlan>> GetProductionPlansAsync(int year,int month, ProductionPlanType_e productionPlanType);
    Task<IQueryable<ProductionPlan>> GetProductionPlansAsync(int year, ProductionPlanType_e productionPlanType);
    Task<IQueryable<ProductionPlan>> GetUnbindProductionPlansAsync();
    Task<List<Guid?>> GetBoundProductionPlansAsync();

    Task<ProductionPlan?> GetProductionPlanByIdAsync(Guid id);
    Task<ProductionPlan?> GetProductionPlanByProjectIdAsync(Guid projectId);
    Task<CycleTimeResponse> GetCycleTimeByMonthAsync(int year,int month);
    Task<CycleTimeResponse> GetCycleTimeByYearAsync(int year);
}