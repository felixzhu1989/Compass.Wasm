﻿using Compass.DataService.Domain;
using Compass.Wasm.Shared.Data.UL;
using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Services.Data.UL;

public interface IKveUvWwDataService : IBaseDataGetService<KveUvWwData>, IBaseDataUpdateService<KveUvWwData>
{
}

public class KveUvWwDataService : BaseDataGetService<KveUvWwData>, IKveUvWwDataService
{
    private readonly IDataRepository _repository;
    public KveUvWwDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<KveUvWwData>> UpdateAsync(Guid id, KveUvWwData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not KveUvWwData data) return new ApiResponse<KveUvWwData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            #endregion



            data.NotifyModified();//通知更新时间
            return new ApiResponse<KveUvWwData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<KveUvWwData> { Status = false, Message = e.Message };
        }
    }
}