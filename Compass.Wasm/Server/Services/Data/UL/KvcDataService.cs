using Compass.DataService.Domain;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Data.UL;

namespace Compass.Wasm.Server.Services.Data.UL;

public interface IKvcDataService : IBaseDataGetService<KvcData>, IBaseDataUpdateService<KvcData>
{
}

public class KvcDataService : BaseDataGetService<KvcData>, IKvcDataService
{
    private readonly IDataRepository _repository;
    public KvcDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<KvcData>> UpdateAsync(Guid id, KvcData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not KvcData data) return new ApiResponse<KvcData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            data.Marvel = dto.Marvel;
            data.AssyPath=dto.AssyPath;
            #endregion



            data.NotifyModified();//通知更新时间
            return new ApiResponse<KvcData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<KvcData> { Status = false, Message = e.Message };
        }
    }
}