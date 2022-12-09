using Compass.PlanService.Domain;
using Compass.PlanService.Domain.Entities;
using Compass.Wasm.Shared.PlanService;
using Microsoft.EntityFrameworkCore;

namespace Compass.PlanService.Infrastructure;

public class PSRepository : IPSRepository
{
    private readonly PSDbContext _context;
    public PSRepository(PSDbContext context)
    {
        _context = context;
    }
    public Task<IQueryable<ProductionPlan>> GetProductionPlansAsync(int year,int month, ProductionPlanType productionPlanType)
    {
        //只值比较年和月，判断类型，如果为No，则返回全部
        var result = productionPlanType.Equals(ProductionPlanType.No) ?
            _context.ProductionPlans.Where(x => x.MonthOfInvoice.Year.Equals(year)&&x.MonthOfInvoice.Month.Equals(month))
            : _context.ProductionPlans.Where(x => x.MonthOfInvoice.Year.Equals(year)&&x.MonthOfInvoice.Month.Equals(month)&&x.ProductionPlanType.Equals(productionPlanType));
        return Task.FromResult(result.OrderBy(x=>x.OdpReleaseTime).AsQueryable());
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
}