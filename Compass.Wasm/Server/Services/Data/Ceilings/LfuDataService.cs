using Compass.DataService.Domain;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Data.Ceilings;

namespace Compass.Wasm.Server.Services.Data.Ceilings;

public interface ILfuDataService : IBaseDataGetService<LfuData>, IBaseDataUpdateService<LfuData>
{

}

public class LfuDataService:BaseDataGetService<LfuData>,ILfuDataService 
{
    private readonly IDataRepository _repository;
    public LfuDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<LfuData>> UpdateAsync(Guid id, LfuData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not LfuData data) return new ApiResponse<LfuData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            data.Marvel = dto.Marvel;
            data.AssyPath=dto.AssyPath;
            #endregion

            #region 新风口参数
            data.SupplySpigotNumber=dto.SupplySpigotNumber;
            data.SupplySpigotDis=dto.SupplySpigotDis;
            data.SupplySpigotDia=dto.SupplySpigotDia;
            #endregion

            data.TotalLength=dto.TotalLength;
            data.Japan=dto.Japan; 

            data.NotifyModified();//通知更新时间
            return new ApiResponse<LfuData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<LfuData> { Status = false, Message = e.Message };
        }
    }
}