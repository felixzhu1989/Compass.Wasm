using Compass.DataService.Domain;
using Compass.Wasm.Shared.Data.UL;
using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Services.Data.UL;

public interface IKveDataService : IBaseDataGetService<KveData>, IBaseDataUpdateService<KveData>
{
}

public class KveDataService : BaseDataGetService<KveData>, IKveDataService
{
    private readonly IDataRepository _repository;
    public KveDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<KveData>> UpdateAsync(Guid id, KveData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not KveData data) return new ApiResponse<KveData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            data.Marvel = dto.Marvel;
            data.AssyPath=dto.AssyPath;
            #endregion



            data.NotifyModified();//通知更新时间
            return new ApiResponse<KveData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<KveData> { Status = false, Message = e.Message };
        }
    }
}