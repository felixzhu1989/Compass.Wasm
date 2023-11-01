using Compass.DataService.Domain;
using Compass.Dtos;
using Compass.Wasm.Shared.Data.Ceilings;

namespace Compass.Wasm.Server.Services.Data.Ceilings;

public interface IDpDataService : IBaseDataGetService<DpData>, IBaseDataUpdateService<DpData>
{

}

public class DpDataService:BaseDataGetService<DpData>,IDpDataService
{
    private readonly IDataRepository _repository;
    public DpDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<DpData>> UpdateAsync(Guid id, DpData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not DpData data) return new ApiResponse<DpData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            #endregion


            #region CJ脖颈参数
            data.CjSpigotDirection=dto.CjSpigotDirection;
            data.CjSpigotToRight=dto.CjSpigotToRight;
            #endregion

            data.NotifyModified();//通知更新时间
            return new ApiResponse<DpData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<DpData> { Status = false, Message = e.Message };
        }
    }
}