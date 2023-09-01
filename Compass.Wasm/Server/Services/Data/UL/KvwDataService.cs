using Compass.DataService.Domain;
using Compass.Wasm.Shared.Data.UL;
using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Services.Data.UL;

public interface IKvwDataService : IBaseDataGetService<KvwData>, IBaseDataUpdateService<KvwData>
{
}

public class KvwDataService : BaseDataGetService<KvwData>, IKvwDataService
{
    private readonly IDataRepository _repository;
    public KvwDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<KvwData>> UpdateAsync(Guid id, KvwData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not KvwData data) return new ApiResponse<KvwData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            #endregion



            data.NotifyModified();//通知更新时间
            return new ApiResponse<KvwData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<KvwData> { Status = false, Message = e.Message };
        }
    }
}