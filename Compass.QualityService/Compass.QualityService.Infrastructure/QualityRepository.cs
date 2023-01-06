using Compass.QualityService.Domain;
using Compass.QualityService.Domain.Entities;
using Compass.Wasm.Shared.CategoryService;
using Microsoft.EntityFrameworkCore;

namespace Compass.QualityService.Infrastructure;

public class QualityRepository:IQualityRepository
{
    private readonly QualityDbContext _context;
    public QualityRepository(QualityDbContext context)
    {
        _context = context;
    }

    public Task<FinalInspectionCheckItemType?> GetFinalInspectionCheckItemTypeByIdAsync(Guid id)
    {
        return _context.FinalInspectionCheckItemTypes.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task<IQueryable<FinalInspectionCheckItemType>> GetFinalInspectionCheckItemTypesAsync()
    {
        return Task.FromResult(_context.FinalInspectionCheckItemTypes.OrderBy(x => x.SequenceNumber).AsQueryable());
    }

    public async Task<int> GetMaxSeqOfFinalInspectionCheckItemTypesAsync()
    {
        var maxSeq = await _context.Query<FinalInspectionCheckItemType>().MaxAsync(x => (int?)x.SequenceNumber);
        return maxSeq ?? 0;
    }
}