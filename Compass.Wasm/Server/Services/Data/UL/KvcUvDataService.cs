using Compass.DataService.Domain;
using Compass.Wasm.Shared.Data.UL;
using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Services.Data.UL;

public interface IKvcUvDataService : IBaseDataGetService<KvcUvData>, IBaseDataUpdateService<KvcUvData>
{
}

public class KvcUvDataService : BaseDataGetService<KvcUvData>, IKvcUvDataService
{
    private readonly IDataRepository _repository;
    public KvcUvDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<KvcUvData>> UpdateAsync(Guid id, KvcUvData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not KvcUvData data) return new ApiResponse<KvcUvData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            #endregion



            data.NotifyModified();//通知更新时间
            return new ApiResponse<KvcUvData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<KvcUvData> { Status = false, Message = e.Message };
        }
    }
}