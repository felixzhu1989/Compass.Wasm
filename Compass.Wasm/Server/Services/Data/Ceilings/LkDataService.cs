using Compass.DataService.Domain;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Data.Ceilings;

namespace Compass.Wasm.Server.Services.Data.Ceilings;

public interface ILkDataService : IBaseDataGetService<LkData>, IBaseDataUpdateService<LkData>
{

}

public class LkDataService:BaseDataGetService<LkData>,ILkDataService
{
    private readonly IDataRepository _repository;
    public LkDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<LkData>> UpdateAsync(Guid id, LkData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not LkData data) return new ApiResponse<LkData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            data.Marvel = dto.Marvel;
            data.AssyPath=dto.AssyPath;
            #endregion

            #region 灯具类型
            data.TotalLength=dto.TotalLength;
            data.CeilingLightType = dto.CeilingLightType;
            data.LongGlassNumber=dto.LongGlassNumber;
            data.ShortGlassNumber=dto.ShortGlassNumber;

            data.LeftLength=dto.LeftLength;
            data.RightLength=dto.RightLength;
            data.MiddleLength=dto.MiddleLength;
            #endregion

            #region 其他配置
            data.WaterWash=dto.WaterWash;
            data.Japan = dto.Japan;
            #endregion


            data.NotifyModified();//通知更新时间
            return new ApiResponse<LkData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<LkData> { Status = false, Message = e.Message };
        }
    }
}