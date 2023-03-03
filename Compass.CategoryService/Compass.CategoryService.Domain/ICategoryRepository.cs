using Compass.CategoryService.Domain.Entities;
using Compass.Wasm.Shared.CategoryService;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.CategoryService.Domain;

public interface ICategoryRepository
{
    //ProductCategory
    Task<IQueryable<Product>> GetProductsAsync();
    Task<Product?> GetProductByIdAsync(Guid id);

    Task<IQueryable<Product>> GetProductsBySbuAsync(Sbu_e sbu);
    Task<int> GetMaxSeqOfProductsAsync(Sbu_e sbu);//获取最大序号


    Task<IQueryable<Model>> GetModelsAsync();
    Task<Model?> GetModelByIdAsync(Guid id);

    Task<IQueryable<Model>> GetModelsByProductIdAsync(Guid productId);
    Task<int> GetMaxSeqOfModelsAsync(Guid productId);


    Task<IQueryable<ModelType>> GetModelTypesAsync();
    Task<ModelType?> GetModelTypeByIdAsync(Guid id);

    Task<IQueryable<ModelType>> GetModelTypesByModelIdAsync(Guid modelId);
    Task<int> GetMaxSeqOfModelTypesAsync(Guid modelId);
    Task<string> GetModelNameByModelTypeIdAsync(Guid modelTypeId);


    //ProblemType
    Task<ProblemType?> GetProblemTypeByIdAsync(Guid id);
    Task<IQueryable<ProblemType>> GetProblemTypesAsync(Stakeholder_e stakeholder);
    

}