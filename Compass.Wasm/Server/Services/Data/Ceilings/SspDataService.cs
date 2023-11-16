using Compass.DataService.Domain;
using Compass.Dtos;
using Compass.Wasm.Shared.Data.Ceilings;

namespace Compass.Wasm.Server.Services.Data.Ceilings;

public interface ISspDataService : IBaseDataGetService<SspData>, IBaseDataUpdateService<SspData>
{

}

public class SspDataService:BaseDataGetService<SspData>, ISspDataService
{
    private readonly IDataRepository _repository;
    public SspDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<SspData>> UpdateAsync(Guid id, SspData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not SspData data) return new ApiResponse<SspData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            #endregion

            data.LeftType=dto.LeftType;
            data.RightType=dto.RightType;
            data.LeftWidth=dto.LeftWidth;
            data.RightWidth=dto.RightWidth;
            data.MPanelNumber=dto.MPanelNumber;
            data.LedLight=dto.LedLight;


            data.NotifyModified();//通知更新时间
            return new ApiResponse<SspData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<SspData> { Status = false, Message = e.Message };
        }
    }
}