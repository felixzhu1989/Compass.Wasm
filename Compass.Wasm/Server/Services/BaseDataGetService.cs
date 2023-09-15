using Compass.DataService.Domain;
using Compass.Dtos;
using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Services;

public interface IBaseDataGetService<T> where T : class
{
    Task<ApiResponse<T>> GetSingleAsync(Guid id);
}

public interface IBaseDataUpdateService<T> where T : class
{
    Task<ApiResponse<T>> UpdateAsync(Guid id, T dto);
}

public abstract class BaseDataGetService<T> : IBaseDataGetService<T> where T : class
{
    private readonly IDataRepository _repository;
    protected BaseDataGetService(IDataRepository repository)
    {
        _repository = repository;
    }
    public async Task<ApiResponse<T>> GetSingleAsync(Guid id)
    {
        try
        {
            //此处自动获取了需要的子类，使用子类去接收即可
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model != null)
            {
                return new ApiResponse<T> { Status = true, Result = (model as T)! };
            }
            return new ApiResponse<T> { Status = false, Message = "查询数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<T> { Status = false, Message = e.Message };
        }
    }



}