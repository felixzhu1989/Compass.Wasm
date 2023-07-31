using Compass.DataService.Domain;
using Compass.Wasm.Shared.Data.UL;
using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Services.Data.UL;

public interface IKvcWwDataService : IBaseDataGetService<KvcWwData>, IBaseDataUpdateService<KvcWwData>
{
}

public class KvcWwDataService : BaseDataGetService<KvcWwData>, IKvcWwDataService
{
    private readonly IDataRepository _repository;
    public KvcWwDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<KvcWwData>> UpdateAsync(Guid id, KvcWwData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not KvcWwData data) return new ApiResponse<KvcWwData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            #endregion



            data.NotifyModified();//通知更新时间
            return new ApiResponse<KvcWwData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<KvcWwData> { Status = false, Message = e.Message };
        }
    }
}