using Compass.CategoryService.Domain;
using Compass.CategoryService.Domain.Entities;
using Compass.Wasm.Shared.CategoryService;
using Compass.Wasm.Shared.ProjectService;
using Microsoft.EntityFrameworkCore;

namespace Compass.CategoryService.Infrastructure;

public class CateRepository : ICateRepository
{
    private readonly CateDbContext _context;
    public CateRepository(CateDbContext context)
    {
        _context = context;
    }

    #region ProductCategory
    public Task<Product?> GetProductByIdAsync(Guid id)
    {
        return _context.Products.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task<IQueryable<Product>> GetProductsAsync(Sbu sbu)
    {
        return Task.FromResult(_context.Products.Where(x => x.Sbu.Equals(sbu)).OrderBy(x => x.SequenceNumber).AsQueryable());
    }

    public async Task<int> GetMaxSeqOfProductsAsync(Sbu sbu)
    {
        var maxSeq = await _context.Query<Product>().Where(x => x.Sbu.Equals(sbu)).MaxAsync(x => (int?)x.SequenceNumber);
        return maxSeq ?? 0;
    }

    public Task<Model?> GetModelByIdAsync(Guid id)
    {
        return _context.Models.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task<IQueryable<Model>> GetModelsByProductIdAsync(Guid productId)
    {
        return Task.FromResult(_context.Models.Where(x => x.ProductId.Equals(productId)).OrderBy(x => x.SequenceNumber).AsQueryable());
    }

    public async Task<int> GetMaxSeqOfModelsAsync(Guid productId)
    {
        var maxSeq = await _context.Query<Model>().Where(x => x.ProductId.Equals(productId)).MaxAsync(x => (int?)x.SequenceNumber);
        return maxSeq ?? 0;
    }

    public Task<ModelType?> GetModelTypeByIdAsync(Guid id)
    {
        return _context.ModelTypes.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task<IQueryable<ModelType>> GetModelTypesByProductIdAsync(Guid modelId)
    {
        return Task.FromResult(_context.ModelTypes.Where(x => x.ModelId.Equals(modelId)).OrderBy(x => x.SequenceNumber).AsQueryable());
    }

    public async Task<int> GetMaxSeqOfModelTypesAsync(Guid modelId)
    {
        var maxSeq = await _context.Query<ModelType>().Where(x => x.ModelId.Equals(modelId)).MaxAsync(x => (int?)x.SequenceNumber);
        return maxSeq ?? 0;
    }

    #endregion

    #region ProblemType
    public Task<ProblemType?> GetProblemTypeByIdAsync(Guid id)
    {
        return _context.ProblemTypes.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task<IQueryable<ProblemType>> GetProblemTypesAsync(Stakeholder stakeholder)
    {
        return Task.FromResult(_context.ProblemTypes.Where(x => x.Stakeholder.Equals(stakeholder)).AsQueryable());
    }
    #endregion
}