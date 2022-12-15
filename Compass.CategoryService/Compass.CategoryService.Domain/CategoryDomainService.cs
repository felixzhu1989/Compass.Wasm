using Compass.CategoryService.Domain.Entities;
using Compass.Wasm.Shared.CategoryService;

namespace Compass.CategoryService.Domain;

public class CategoryDomainService
{
    private readonly ICategoryRepository _repository;
    public CategoryDomainService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Product> AddProductAsync(string name, Sbu sbu)
    {
        int maxSeq = await _repository.GetMaxSeqOfProductsAsync(sbu);
        return new Product(Guid.NewGuid(), maxSeq+1, name, sbu);
    }
    public async Task SortProductsAsync(Sbu sbu,Guid[] sortedProductIds)
    {
        var products = await _repository.GetProductsAsync(sbu);
        var idsInDb = products.Select(x => x.Id);
        if (!idsInDb.SequenceIgnoredEqual(sortedProductIds))
        {
            throw new Exception($"提交的待排序Id中必须是sbu={sbu}分类下所有的Id");
        }
        int seqNum = 1;
        foreach (var productId in sortedProductIds)
        {
            var product = await _repository.GetProductByIdAsync(productId);
            if (product == null) throw new Exception($"productId={productId}不存在");
            product.ChangeSequenceNumber(seqNum);
            seqNum++;
        }
    }
    public async Task<Model> AddModelAsync(Guid productId, string name,double workload)
    {
        int maxSeq = await _repository.GetMaxSeqOfModelsAsync(productId);
        return new Model(Guid.NewGuid(),productId, maxSeq + 1, name,workload);
    }
    public async Task SortModelsAsync(Guid productId, Guid[] sortedModelIds)
    {
        var models = await _repository.GetModelsByProductIdAsync(productId);
        var idsInDb = models.Select(x => x.Id);
        if (!idsInDb.SequenceIgnoredEqual(sortedModelIds))
        {
            throw new Exception($"提交的待排序Id中必须是productId={productId}分类下所有的Id");
        }
        int seqNum = 1;
        foreach (var modelId in sortedModelIds)
        {
            var model = await _repository.GetModelByIdAsync(modelId);
            if (model == null) throw new Exception($"modelId={modelId}不存在");
            model.ChangeSequenceNumber(seqNum);
            seqNum++;
        }
    }
    public async Task<ModelType> AddModelTypeAsync(Guid modelId, string name,string description,double length,double width,double height)
    {
        int maxSeq = await _repository.GetMaxSeqOfModelTypesAsync(modelId);
        return new ModelType(Guid.NewGuid(), modelId, maxSeq + 1, name, description,length,width,height);
    }
    public async Task SortModelTypesAsync(Guid modelId, Guid[] sortedModelTypeIds)
    {
        var modelTypes = await _repository.GetModelTypesByProductIdAsync(modelId);
        var idsInDb = modelTypes.Select(x => x.Id);
        if (!idsInDb.SequenceIgnoredEqual(sortedModelTypeIds))
        {
            throw new Exception($"提交的待排序Id中必须是modelIdId={modelId}分类下所有的Id");
        }
        int seqNum = 1;
        foreach (var modelTypeId in sortedModelTypeIds)
        {
            var modelType = await _repository.GetModelTypeByIdAsync(modelTypeId);
            if (modelType == null) throw new Exception($"modelTypeId={modelTypeId}不存在");
            modelType.ChangeSequenceNumber(seqNum);
            seqNum++;
        }
    }
}