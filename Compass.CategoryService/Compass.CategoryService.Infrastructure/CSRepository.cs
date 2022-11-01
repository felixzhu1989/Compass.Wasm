using Compass.CategoryService.Domain;
using Compass.CategoryService.Domain.Entities;
using Compass.Wasm.Shared.CategoryService;
using Microsoft.EntityFrameworkCore;

namespace Compass.CategoryService.Infrastructure;

public class CSRepository : ICSRepository
{
    private readonly CSDbContext _context;
    public CSRepository(CSDbContext context)
    {
        _context = context;
    }
    public Task<Product?> GetProductByIdAsync(Guid id)
    {
        return _context.Products.SingleOrDefaultAsync(x=>x.Id.Equals(id));
    }

    public Task<IQueryable<Product>> GetProductsAsync(Sbu sbu)
    {
        return Task.FromResult(_context.Products.Where(x=>x.Sbu.Equals(sbu)).OrderBy(x => x.SequenceNumber).AsQueryable());
    }

    public async Task<int> GetMaxSeqOfProductsAsync(Sbu sbu)
    {
        var maxSeq = await _context.Query<Product>().Where(x=>x.Sbu.Equals(sbu)).MaxAsync(x => (int?)x.SequenceNumber);
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
}