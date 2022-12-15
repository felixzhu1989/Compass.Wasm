using Compass.CategoryService.Domain.Entities;
using Compass.Wasm.Shared.CategoryService;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.CategoryService.Domain;

public interface ICategoryRepository
{
    //ProductCategory
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<IQueryable<Product>> GetProductsAsync(Sbu sbu);
    Task<int> GetMaxSeqOfProductsAsync(Sbu sbu);//获取最大序号
    Task<Model?> GetModelByIdAsync(Guid id);
    Task<IQueryable<Model>> GetModelsByProductIdAsync(Guid productId);
    Task<int> GetMaxSeqOfModelsAsync(Guid productId);
    Task<ModelType?> GetModelTypeByIdAsync(Guid id);
    Task<IQueryable<ModelType>> GetModelTypesByProductIdAsync(Guid modelId);
    Task<int> GetMaxSeqOfModelTypesAsync(Guid modelId);
    Task<string> GetModelNameByModelTypeIdAsync(Guid modelTypeId);

    //ProblemType
    Task<ProblemType?> GetProblemTypeByIdAsync(Guid id);
    Task<IQueryable<ProblemType>> GetProblemTypesAsync(Stakeholder stakeholder);
    

}