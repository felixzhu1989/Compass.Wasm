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
    public Task<IQueryable<ProductionPlan>> GetProductionPlansAsync(int year,int month, ProductionPlanType_e productionPlanType)
    {
        //只值比较年和月，判断类型，如果为No，则返回全部
        var result = productionPlanType.Equals(ProductionPlanType_e.No) ?
            _context.ProductionPlans.Where(x => x.MonthOfInvoice.Year.Equals(year)&&x.MonthOfInvoice.Month.Equals(month))
            : _context.ProductionPlans.Where(x => x.MonthOfInvoice.Year.Equals(year)&&x.MonthOfInvoice.Month.Equals(month)&&x.ProductionPlanType.Equals(productionPlanType));
        return Task.FromResult(result.OrderBy(x=>x.OdpReleaseTime).AsQueryable());
    }

    public Task<IQueryable<ProductionPlan>> GetProductionPlansAsync(int year, ProductionPlanType_e productionPlanType)
    {
        //只值比较年和月，判断类型，如果为No，则返回全部
        var result = productionPlanType.Equals(ProductionPlanType_e.No) ?
            _context.ProductionPlans.Where(x => x.MonthOfInvoice.Year.Equals(year))
            : _context.ProductionPlans.Where(x => x.MonthOfInvoice.Year.Equals(year)&&x.ProductionPlanType.Equals(productionPlanType));
        return Task.FromResult(result.OrderBy(x => x.OdpReleaseTime).AsQueryable());
    }

    public Task<IQueryable<ProductionPlan>> GetUnbindProductionPlansAsync()
    {
        return Task.FromResult(_context.ProductionPlans.Where(x => x.ProjectId == null).OrderBy(x=>x.SqNumber).AsQueryable());
    }

    public Task<List<Guid?>> GetBoundProductionPlansAsync()
    {
       return _context.ProductionPlans.Where(x => x.ProjectId != null).Select(x => x.ProjectId).ToListAsync();
    }

    public Task<ProductionPlan?> GetProductionPlanByIdAsync(Guid id)
    {
        return _context.ProductionPlans.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task<ProductionPlan?> GetProductionPlanByProjectIdAsync(Guid projectId)
    {
        return _context.ProductionPlans.SingleOrDefaultAsync(x => x.ProjectId.Equals(projectId));
    }

    public Task<CycleTimeResponse> GetCycleTimeByMonthAsync(int year, int month)
    {
        var result = _context.ProductionPlans
            .Where(x => x.MonthOfInvoice.Year.Equals(year)
                        && x.MonthOfInvoice.Month.Equals(month)
                        && !x.ProductionPlanType.Equals(ProductionPlanType_e.KFC));
        return Task.FromResult(GetCycleTime(result));
    }

    public Task<CycleTimeResponse> GetCycleTimeByYearAsync(int year)
    {
        var result = _context.ProductionPlans
            .Where(x => x.MonthOfInvoice.Year.Equals(year)
                        && !x.ProductionPlanType.Equals(ProductionPlanType_e.KFC));
        return Task.FromResult(GetCycleTime(result));
    }

    private CycleTimeResponse GetCycleTime(IQueryable<ProductionPlan> prodPlans)
    {
        CycleTimeResponse response = new();
        if (prodPlans.Any())
        {
            response.ProjectQuantity = prodPlans.Count();
            List<int> factoryCycle = new();
            List<int> productionCycle = new();
            foreach (var plan in prodPlans)
            {
                factoryCycle.Add(plan.ProductionFinishTime.Subtract(plan.OdpReleaseTime).Days);
                if (plan.DrawingReleaseActual != null)
                {
                    productionCycle.Add(plan.ProductionFinishTime.Subtract(plan.DrawingReleaseActual.Value).Days);
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
}