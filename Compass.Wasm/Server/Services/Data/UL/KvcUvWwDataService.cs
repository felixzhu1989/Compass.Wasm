using Compass.DataService.Domain;
using Compass.Dtos;
using Compass.Wasm.Shared.Data.UL;
using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Services.Data.UL;

public interface IKvcUvWwDataService : IBaseDataGetService<KvcUvWwData>, IBaseDataUpdateService<KvcUvWwData>
{
}

public class KvcUvWwDataService : BaseDataGetService<KvcUvWwData>, IKvcUvWwDataService
{
    private readonly IDataRepository _repository;
    public KvcUvWwDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<KvcUvWwData>> UpdateAsync(Guid id, KvcUvWwData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not KvcUvWwData data) return new ApiResponse<KvcUvWwData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            #endregion



            data.NotifyModified();//通知更新时间
            return new ApiResponse<KvcUvWwData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<KvcUvWwData> { Status = false, Message = e.Message };
        }
    }
}