using Compass.PlanService.Domain;
using Compass.PlanService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Compass.Wasm.Shared.Plans;

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


    //扩展MainPlan查询
    public Task<IQueryable<MainPlan>> GetFilterMainPlansAsync(int year, int month, MainPlanType_e productionPlanType)
    {
        //只值比较年和月，判断类型，如果为No，则返回全部
        var result = productionPlanType.Equals(MainPlanType_e.NA) ?
            _context.MainPlans.Where(x => x.MonthOfInvoice.Year.Equals(year)&&x.MonthOfInvoice.Month.Equals(month))
            : _context.MainPlans.Where(x => x.MonthOfInvoice.Year.Equals(year)&&x.MonthOfInvoice.Month.Equals(month)&&x.MainPlanType.Equals(productionPlanType));
        return Task.FromResult(result.OrderBy(x => x.CreateTime).AsQueryable());
    }

    public Task<IQueryable<MainPlan>> GetMainPlansAsync(int year, MainPlanType_e productionPlanType)
    {
        //只值比较年和月，判断类型，如果为No，则返回全部
        var result = productionPlanType.Equals(MainPlanType_e.NA) ?
            _context.MainPlans.Where(x => x.MonthOfInvoice.Year.Equals(year))
            : _context.MainPlans.Where(x => x.MonthOfInvoice.Year.Equals(year)&&x.MainPlanType.Equals(productionPlanType));
        return Task.FromResult(result.OrderBy(x => x.CreateTime).AsQueryable());
    }

    public Task<IQueryable<MainPlan>> GetUnbindMainPlansAsync()
    {
        return Task.FromResult(_context.MainPlans.Where(x => x.ProjectId == null).OrderBy(x => x.Number).AsQueryable());
    }

    public Task<List<Guid?>> GetBoundMainPlansAsync()
    {
        return _context.MainPlans.Where(x => x.ProjectId != null).Select(x => x.ProjectId).ToListAsync();
    }



    public Task<MainPlan?> GetMainPlanByProjectIdAsync(Guid projectId)
    {
        return _context.MainPlans.SingleOrDefaultAsync(x => x.ProjectId.Equals(projectId));
    }

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
}