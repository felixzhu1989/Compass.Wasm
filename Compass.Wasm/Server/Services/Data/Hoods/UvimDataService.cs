using Compass.DataService.Domain;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Data.Hoods;

namespace Compass.Wasm.Server.Services.Data.Hoods;
public interface IUvimDataService : IBaseDataGetService<UvimData>, IBaseDataUpdateService<UvimData>
{
}

public class UvimDataService : BaseDataGetService<UvimData>, IUvimDataService
{
    private readonly IDataRepository _repository;
    public UvimDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }
    public async Task<ApiResponse<UvimData>> UpdateAsync(Guid id, UvimData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not UvimData data) return new ApiResponse<UvimData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            #endregion



            data.NotifyModified();//通知更新时间
            return new ApiResponse<UvimData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<UvimData> { Status = false, Message = e.Message };
        }
    }
}