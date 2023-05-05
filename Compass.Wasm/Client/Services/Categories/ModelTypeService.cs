using Compass.Wasm.Shared.Categories;
namespace Compass.Wasm.Client.Services.Categories;

public interface IModelTypeService : IBaseService<ModelTypeDto>
{
}
public class ModelTypeService : BaseService<ModelTypeDto>, IModelTypeService
{
    public ModelTypeService(HttpClient http) : base(http, "ModelType")
    {
    }
}