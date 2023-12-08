using Compass.CategoryService.Domain;
using Compass.CategoryService.Domain.Entities;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Projects;
using Microsoft.EntityFrameworkCore;

namespace Compass.CategoryService.Infrastructure;

public class CategoryRepository : ICategoryRepository
{
    #region ctor
    private readonly CategoryDbContext _context;
    public CategoryRepository(CategoryDbContext context)
    {
        _context = context;
    } 
    #endregion

    #region ProductCategory
    public Task<IQueryable<Product>> GetProductsAsync()
    {
        return Task.FromResult(_context.Products.OrderBy(x => x.SequenceNumber).AsQueryable());
    }
    public Task<Product?> GetProductByIdAsync(Guid id)
    {
        return _context.Products.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }



    public Task<IQueryable<Product>> GetProductsBySbuAsync(Sbu_e sbu)
    {
        return Task.FromResult(_context.Products.Where(x => x.Sbu.Equals(sbu)).OrderBy(x => x.SequenceNumber).AsQueryable());
    }
    public async Task<int> GetMaxSeqOfProductsAsync(Sbu_e sbu)
    {
        var maxSeq = await _context.Query<Product>().Where(x => x.Sbu.Equals(sbu)).MaxAsync(x => (int?)x.SequenceNumber);
        return maxSeq ?? 0;
    }


    public Task<IQueryable<Model>> GetModelsAsync()
    {
        return Task.FromResult(_context.Models.AsQueryable());
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

    public Task<IQueryable<ModelType>> GetModelTypesAsync()
    {
        return Task.FromResult(_context.ModelTypes.AsQueryable());
    }
    public Task<ModelType?> GetModelTypeByIdAsync(Guid id)
    {
        return _context.ModelTypes.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task<IQueryable<ModelType>> GetModelTypesByModelIdAsync(Guid modelId)
    {
        return Task.FromResult(_context.ModelTypes.Where(x => x.ModelId.Equals(modelId)).OrderBy(x => x.SequenceNumber).AsQueryable());
    }

    public async Task<int> GetMaxSeqOfModelTypesAsync(Guid modelId)
    {
        var maxSeq = await _context.Query<ModelType>().Where(x => x.ModelId.Equals(modelId)).MaxAsync(x => (int?)x.SequenceNumber);
        return maxSeq ?? 0;
    }

    public async Task<string> GetModelNameByModelTypeIdAsync(Guid modelTypeId)
    {
        var modelType = await _context.ModelTypes.SingleOrDefaultAsync(x => x.Id.Equals(modelTypeId));
        var model = await _context.Models.SingleOrDefaultAsync(x => x.Id.Equals(modelType.ModelId));
        return model.Name;
    }
    #endregion

    #region MaterialItem
    public Task<IQueryable<MaterialItem>> GetMaterialItemsAsync()
    {
        return Task.FromResult(_context.MaterialItems.AsQueryable());
    }
    public Task<MaterialItem?> GetMaterialItemByIdAsync(Guid id)
    {
        return _context.MaterialItems.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }
    public Task<MaterialItem?> GetMaterialItemByTypeAsync(string type)
    {
        return _context.MaterialItems.SingleOrDefaultAsync(x => x.Type.ToUpper()==type.ToUpper());
    }
    #endregion

    #region AccCutList
    public Task<IQueryable<AccCutList>> GetAccCutListsAsync()
    {
        return Task.FromResult(_context.AccCutLists.AsQueryable());
    }

    public Task<AccCutList?> GetAccCutListByIdAsync(Guid id)
    {
        return _context.AccCutLists.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }
    #endregion
}