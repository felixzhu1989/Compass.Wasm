﻿using Compass.DataService.Domain;
using Compass.Dtos;
using Compass.Wasm.Shared.Data.Ceilings;

namespace Compass.Wasm.Server.Services.Data.Ceilings;
public interface IUcwDataService : IBaseDataGetService<UcwData>, IBaseDataUpdateService<UcwData>
{

}
public class UcwDataService : BaseDataGetService<UcwData>, IUcwDataService
{
    private readonly IDataRepository _repository;
    public UcwDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }
    public async Task<ApiResponse<UcwData>> UpdateAsync(Guid id, UcwData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not UcwData data) return new ApiResponse<UcwData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            #endregion

            #region DP排水腔参数
            data.DpSide = dto.DpSide;
            #endregion

            #region 排风口参数
            data.MiddleToRight = dto.MiddleToRight;
            data.ExhaustSpigotLength = dto.ExhaustSpigotLength;
            data.ExhaustSpigotWidth = dto.ExhaustSpigotWidth;
            data.ExhaustSpigotHeight = dto.ExhaustSpigotHeight;
            data.ExhaustSpigotNumber = dto.ExhaustSpigotNumber;
            data.ExhaustSpigotDis = dto.ExhaustSpigotDis;
            #endregion

            #region 过滤器参数
            data.FilterSide=dto.FilterSide;
            data.FilterLeft=dto.FilterLeft;
            data.FilterRight=dto.FilterRight;
            data.FilterBlindNumber=dto.FilterBlindNumber;
            #endregion

            #region 灯具类型
            data.CeilingLightType = dto.CeilingLightType;
            //data.LightCable = dto.LightCable;
            data.HclSide = dto.HclSide;
            data.HclLeft = dto.HclLeft;
            data.HclRight = dto.HclRight;
            #endregion

            #region 水洗管入口,KCW265时才需要
            data.CeilingWaterInlet=dto.CeilingWaterInlet;
            #endregion

            #region 其他配置
            data.DomeSsp = dto.DomeSsp;
            data.Gutter = dto.Gutter;
            data.GutterWidth = dto.GutterWidth;
            data.Japan = dto.Japan;
            data.Marvel = dto.Marvel;
            #endregion

            #region Ansul基本参数
            data.Ansul = dto.Ansul;
            data.AnsulSide = dto.AnsulSide;
            #endregion

            #region UV灯参数
            data.UvLightType = dto.UvLightType;
            data.BaffleSensorNumber=dto.BaffleSensorNumber;
            data.BaffleSensorDis1 = dto.BaffleSensorDis1;
            data.BaffleSensorDis2 = dto.BaffleSensorDis2;
            #endregion

            data.NotifyModified();//通知更新时间
            return new ApiResponse<UcwData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<UcwData> { Status = false, Message = e.Message };
        }
    }
}