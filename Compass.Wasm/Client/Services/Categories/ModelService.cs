using Compass.Wasm.Shared.Categories;

namespace Compass.Wasm.Client.Services.Categories;

public interface IModelService : IBaseService<ModelDto>
{

}

public class ModelService : BaseService<ModelDto>, IModelService
{
    public ModelService(HttpClient http) : base(http, "Model")
    {
    }
}