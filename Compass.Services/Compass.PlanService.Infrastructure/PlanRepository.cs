using Compass.PlanService.Domain;
using Compass.PlanService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Compass.Wasm.Shared.Plans;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Compass.PlanService.Infrastructure;

public class PlanRepository : IPlanRepository
{
    private readonly PlanDbContext _context;
    public PlanRepository(PlanDbContext context)
    {
        _context = context;
    }

    #region MainPlan
    //MainPlan
    public Task<IQueryable<MainPlan>> GetMainPlansAsync()
    {
        return Task.FromResult(_context.MainPlans.AsQueryable());
    }

    public Task<MainPlan?> GetMainPlanByIdAsync(Guid id)
    {
        return _context.MainPlans.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }
    //扩展查询
    public Task<IQueryable<MainPlan>> GetMainPlansByProjectIdAsync(Guid projectId)
    {
        return Task.FromResult(_context.MainPlans.Where(x => x.ProjectId.Equals(projectId)).OrderBy(x => x.FinishTime).AsQueryable());
    }

    //扩展MainPlan查询
    public Task<CycleTimeDto> GetCycleTimeByMonthAsync(int year, int month)
    {
        var result = _context.MainPlans
            .Where(x => x.MonthOfInvoice.Year.Equals(year)
                        && x.MonthOfInvoice.Month.Equals(month)
                        && !x.MainPlanType.Equals(MainPlanType_e.KFC));
        return Task.FromResult(GetCycleTime(result));
    }

    public Task<CycleTimeDto> GetCycleTimeByYearAsync(int year)
    {
        var result = _context.MainPlans
            .Where(x => x.MonthOfInvoice.Year.Equals(year)
                        && !x.MainPlanType.Equals(MainPlanType_e.KFC));
        return Task.FromResult(GetCycleTime(result));
    }
    private CycleTimeDto GetCycleTime(IQueryable<MainPlan> prodPlans)
    {
        CycleTimeDto response = new();
        if (prodPlans.Any())
        {
            response.ProjectQuantity = prodPlans.Count();
            List<int> factoryCycle = new();
            List<int> productionCycle = new();
            foreach (var plan in prodPlans)
            {
                factoryCycle.Add(plan.FinishTime.Subtract(plan.CreateTime).Days);
                if (plan.DrwReleaseTime != null)
                {
                    productionCycle.Add(plan.FinishTime.Subtract(plan.DrwReleaseTime.Value).Days);
                }
            }
            response.FactoryCycleTime =Math.Ceiling(factoryCycle.Average());
            //如果没有实际发图日期则productionCycle是空的，先判断集合是否有值
            if (productionCycle.Count>0) response.ProductionCycleTime =Math.Ceiling(productionCycle.Average());
        }
        else
        {
            response.ProjectQuantity = 0;
            response.FactoryCycleTime = 0;
            response.ProductionCycleTime = 0;
        }
        return response;
    }
    #endregion


    #region Issue
    public Task<IQueryable<Issue>> GetIssuesAsync()
    {
        return Task.FromResult(_context.Issues.AsQueryable());
    }

    public Task<Issue?> GetIssueByIdAsync(Guid id)
    {
        return _context.Issues.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task<IQueryable<Issue>> GetIssuesByMainPlanIdAsync(Guid mainPlanId)
    {
        return Task.FromResult(_context.Issues.Where(x => x.MainPlanId.Equals(mainPlanId)).OrderBy(x => x.CreationTime).AsQueryable());
    }
    #endregion

}