﻿using Compass.CategoryService.Domain.Entities;
using Compass.Wasm.Shared.CategoryService;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.CategoryService.Domain;

public interface ICateRepository
{
    //ProductCategory
    public Task<Product?> GetProductByIdAsync(Guid id);
    public Task<IQueryable<Product>> GetProductsAsync(Sbu sbu);
    public Task<int> GetMaxSeqOfProductsAsync(Sbu sbu);//获取最大序号
    public Task<Model?> GetModelByIdAsync(Guid id);
    public Task<IQueryable<Model>> GetModelsByProductIdAsync(Guid productId);
    public Task<int> GetMaxSeqOfModelsAsync(Guid productId);
    public Task<ModelType?> GetModelTypeByIdAsync(Guid id);
    public Task<IQueryable<ModelType>> GetModelTypesByProductIdAsync(Guid modelId);
    public Task<int> GetMaxSeqOfModelTypesAsync(Guid modelId);

    //ProblemType
    public Task<ProblemType?> GetProblemTypeByIdAsync(Guid id);
    public Task<IQueryable<ProblemType>> GetProblemTypesAsync(Stakeholder stakeholder);
    

}