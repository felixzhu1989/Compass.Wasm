using Compass.DataService.Domain;
using Compass.Dtos;
using Compass.Wasm.Shared.Data.Ceilings;

namespace Compass.Wasm.Server.Services.Data.Ceilings;

public interface ILpDataService : IBaseDataGetService<LpData>, IBaseDataUpdateService<LpData>
{

}

public class LpDataService:BaseDataGetService<LpData>,ILpDataService
{
    private readonly IDataRepository _repository;
    public LpDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<LpData>> UpdateAsync(Guid id, LpData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not LpData data) return new ApiResponse<LpData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            data.Marvel = dto.Marvel;
            data.AssyPath=dto.AssyPath;
            #endregion

            data.LeftWidth=dto.LeftWidth;
            data.RightWidth=dto.RightWidth;
            data.ZPanelNumber=dto.ZPanelNumber;
            data.LedLight=dto.LedLight;

            data.NotifyModified();//通知更新时间
            return new ApiResponse<LpData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<LpData> { Status = false, Message = e.Message };
        }
    }
}