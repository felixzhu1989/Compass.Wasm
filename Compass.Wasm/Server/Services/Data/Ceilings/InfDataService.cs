using Compass.DataService.Domain;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Data.Ceilings;

namespace Compass.Wasm.Server.Services.Data.Ceilings;

public interface IInfDataService : IBaseDataGetService<InfData>, IBaseDataUpdateService<InfData>
{

}
public class InfDataService:BaseDataGetService<InfData>,IInfDataService
{
    private readonly IDataRepository _repository;
    public InfDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<InfData>> UpdateAsync(Guid id, InfData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not InfData data) return new ApiResponse<InfData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            data.Marvel = dto.Marvel;
            data.AssyPath=dto.AssyPath;
            #endregion

            data.NotifyModified();//通知更新时间
            return new ApiResponse<InfData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<InfData> { Status = false, Message = e.Message };
        }
    }
}