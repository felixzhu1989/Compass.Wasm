using Compass.PlanService.Domain;
using Compass.PlanService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Compass.Wasm.Shared.Plans;

namespace Compass.PlanService.Infrastructure;

public class PlanRepository : IPlanRepository
{
    #region ctor
    private readonly PlanDbContext _context;
    public PlanRepository(PlanDbContext context)
    {
        _context = context;
    } 
    #endregion

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
    public Task<MainPlan?> GetMainPlanByProjectIdAndBatchAsync(Guid projectId,int batch)
    {
        return _context.MainPlans
            .SingleOrDefaultAsync(x => x.ProjectId.Equals(projectId)
                                       && x.Batch==batch);
    }
    public Task<List<Guid?>> GetProjectIdsByStatusAsync(MainPlanStatus_e? status)
    {
        var plans = _context.MainPlans.Where(x => x.Status == status);
        var ids = plans.Select(x => x.ProjectId).ToList();
        return Task.FromResult(ids);
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

    #region PackingList
    public Task<IQueryable<PackingList>> GetPackingListsAsync()
    {
        return Task.FromResult(_context.PackingLists.AsQueryable());
    }

    public Task<PackingList?> GetPackingListByIdAsync(Guid id)
    {
        return _context.PackingLists.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task<PackingList?> GetPackingListByMainPlanIdAsync(Guid mainPlanId)
    {
        return _context.PackingLists.SingleOrDefaultAsync(x => x.MainPlanId.Equals(mainPlanId));
    } 
    #endregion



}