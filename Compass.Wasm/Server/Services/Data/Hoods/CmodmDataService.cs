using Compass.DataService.Domain;
using Compass.Dtos;
using Compass.Wasm.Shared.Data.Hoods;

namespace Compass.Wasm.Server.Services.Data.Hoods;

public interface ICmodmDataService : IBaseDataGetService<CmodmData>, IBaseDataUpdateService<CmodmData>
{
}

public class CmodmDataService : BaseDataGetService<CmodmData>, ICmodmDataService
{
    private readonly IDataRepository _repository;
    public CmodmDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<CmodmData>> UpdateAsync(Guid id, CmodmData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not CmodmData data) return new ApiResponse<CmodmData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            #endregion
            

            data.NotifyModified();//通知更新时间
            return new ApiResponse<CmodmData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<CmodmData> { Status = false, Message = e.Message };
        }
    }
}