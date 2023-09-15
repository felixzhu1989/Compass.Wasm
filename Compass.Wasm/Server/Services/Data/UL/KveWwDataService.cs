using Compass.DataService.Domain;
using Compass.Dtos;
using Compass.Wasm.Shared.Data.UL;
using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Services.Data.UL;

public interface IKveWwDataService : IBaseDataGetService<KveWwData>, IBaseDataUpdateService<KveWwData>
{
}

public class KveWwDataService : BaseDataGetService<KveWwData>, IKveWwDataService
{
    private readonly IDataRepository _repository;
    public KveWwDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<KveWwData>> UpdateAsync(Guid id, KveWwData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not KveWwData data) return new ApiResponse<KveWwData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            #endregion



            data.NotifyModified();//通知更新时间
            return new ApiResponse<KveWwData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<KveWwData> { Status = false, Message = e.Message };
        }
    }
}