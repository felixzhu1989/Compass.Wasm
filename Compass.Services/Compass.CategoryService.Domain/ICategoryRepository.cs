using Compass.CategoryService.Domain.Entities;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Projects;

namespace Compass.CategoryService.Domain;

public interface ICategoryRepository
{
    #region Product
    Task<IQueryable<Product>> GetProductsAsync();
    Task<Product?> GetProductByIdAsync(Guid id);

    Task<IQueryable<Product>> GetProductsBySbuAsync(Sbu_e sbu);
    Task<int> GetMaxSeqOfProductsAsync(Sbu_e sbu);//获取最大序号 
    #endregion
    
    #region Model
    Task<IQueryable<Model>> GetModelsAsync();
    Task<Model?> GetModelByIdAsync(Guid id);

    Task<IQueryable<Model>> GetModelsByProductIdAsync(Guid productId);
    Task<int> GetMaxSeqOfModelsAsync(Guid productId);
    #endregion

    #region ModelType
    Task<IQueryable<ModelType>> GetModelTypesAsync();
    Task<ModelType?> GetModelTypeByIdAsync(Guid id);

    Task<IQueryable<ModelType>> GetModelTypesByModelIdAsync(Guid modelId);
    Task<int> GetMaxSeqOfModelTypesAsync(Guid modelId);
    Task<string> GetModelNameByModelTypeIdAsync(Guid modelTypeId);
    #endregion

    #region MaterialItem
    Task<IQueryable<MaterialItem>> GetMaterialItemsAsync();
    Task<MaterialItem?> GetMaterialItemByIdAsync(Guid id);
    Task<MaterialItem?> GetMaterialItemByTypeAsync(string type);
    #endregion

    #region AccCutList
    Task<IQueryable<AccCutList>> GetAccCutListsAsync();
    Task<AccCutList?> GetAccCutListByIdAsync(Guid id);
    #endregion

}