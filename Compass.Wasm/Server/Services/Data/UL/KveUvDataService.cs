using Compass.DataService.Domain;
using Compass.Dtos;
using Compass.Wasm.Shared.Data.UL;
using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Services.Data.UL;

public interface IKveUvDataService : IBaseDataGetService<KveUvData>, IBaseDataUpdateService<KveUvData>
{
}

public class KveUvDataService : BaseDataGetService<KveUvData>, IKveUvDataService
{
    private readonly IDataRepository _repository;
    public KveUvDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<KveUvData>> UpdateAsync(Guid id, KveUvData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not KveUvData data) return new ApiResponse<KveUvData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            data.Marvel = dto.Marvel;
            data.AssyPath=dto.AssyPath;
            #endregion



            data.NotifyModified();//通知更新时间
            return new ApiResponse<KveUvData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<KveUvData> { Status = false, Message = e.Message };
        }
    }
}