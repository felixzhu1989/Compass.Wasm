using Compass.DataService.Domain;
using Compass.Wasm.Shared.Data.UL;
using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Services.Data.UL;

public interface IKvrDataService : IBaseDataGetService<KvrData>, IBaseDataUpdateService<KvrData>
{
}

public class KvrDataService : BaseDataGetService<KvrData>, IKvrDataService
{
    private readonly IDataRepository _repository;
    public KvrDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<KvrData>> UpdateAsync(Guid id, KvrData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not KvrData data) return new ApiResponse<KvrData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            #endregion



            data.NotifyModified();//通知更新时间
            return new ApiResponse<KvrData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<KvrData> { Status = false, Message = e.Message };
        }
    }
}