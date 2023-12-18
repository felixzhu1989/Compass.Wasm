using Compass.DataService.Domain;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Data.Ceilings;

namespace Compass.Wasm.Server.Services.Data.Ceilings;
public interface IDxfDataService : IBaseDataGetService<DxfData>, IBaseDataUpdateService<DxfData>
{

}
public class DxfDataService:BaseDataGetService<DxfData>,IDxfDataService
{
    private readonly IDataRepository _repository;
    public DxfDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<DxfData>> UpdateAsync(Guid id, DxfData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not DxfData data) return new ApiResponse<DxfData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            data.Marvel = dto.Marvel;
            data.AssyPath=dto.AssyPath;
            #endregion

            data.AccNumber=dto.AccNumber;


            data.NotifyModified();//通知更新时间
            return new ApiResponse<DxfData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<DxfData> { Status = false, Message = e.Message };
        }
    }
}