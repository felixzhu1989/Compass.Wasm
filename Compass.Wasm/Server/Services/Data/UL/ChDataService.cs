using Compass.DataService.Domain;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Data.UL;

namespace Compass.Wasm.Server.Services.Data.UL;

public interface IChDataService : IBaseDataGetService<ChData>, IBaseDataUpdateService<ChData>
{
}

public class ChDataService : BaseDataGetService<ChData>, IChDataService
{
    private readonly IDataRepository _repository;
    public ChDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<ChData>> UpdateAsync(Guid id, ChData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not ChData data) return new ApiResponse<ChData> { Status = false, Message = "更新数据失败" };

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
            return new ApiResponse<ChData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<ChData> { Status = false, Message = e.Message };
        }
    }
}