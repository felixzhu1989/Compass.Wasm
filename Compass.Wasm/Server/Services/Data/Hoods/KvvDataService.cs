using Compass.DataService.Domain;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Data.Hoods;

namespace Compass.Wasm.Server.Services.Data.Hoods;

public interface IKvvDataService : IBaseDataGetService<KvvData>, IBaseDataUpdateService<KvvData>
{
}

public class KvvDataService:BaseDataGetService<KvvData>,IKvvDataService
{
    private readonly IDataRepository _repository;
    public KvvDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<KvvData>> UpdateAsync(Guid id, KvvData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not KvvData data) return new ApiResponse<KvvData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            #endregion

            #region 排风口参数
            data.MiddleToRight = dto.MiddleToRight;
            data.ExhaustSpigotLength = dto.ExhaustSpigotLength;
            data.ExhaustSpigotWidth = dto.ExhaustSpigotWidth;
            data.ExhaustSpigotHeight = dto.ExhaustSpigotHeight;
            data.ExhaustSpigotNumber = dto.ExhaustSpigotNumber;
            data.ExhaustSpigotDis = dto.ExhaustSpigotDis;
            #endregion

            #region 灯具类型
            data.LightType = dto.LightType;
            #endregion
            
            data.NotifyModified();//通知更新时间
            return new ApiResponse<KvvData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<KvvData> { Status = false, Message = e.Message };
        }
    }
}