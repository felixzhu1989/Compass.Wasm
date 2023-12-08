using Compass.DataService.Domain;
using Compass.Dtos;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Data.Hoods;

namespace Compass.Wasm.Server.Services.Data.Hoods;

public interface IUvfDataService : IBaseDataGetService<UvfData>, IBaseDataUpdateService<UvfData>
{
}

public class UvfDataService : BaseDataGetService<UvfData>, IUvfDataService
{
    private readonly IDataRepository _repository;
    public UvfDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<UvfData>> UpdateAsync(Guid id, UvfData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not UvfData data) return new ApiResponse<UvfData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            data.Marvel = dto.Marvel;
            data.AssyPath=dto.AssyPath;
            #endregion

            #region 排风口参数
            data.MiddleToRight = dto.MiddleToRight;
            data.ExhaustSpigotLength = dto.ExhaustSpigotLength;
            data.ExhaustSpigotWidth = dto.ExhaustSpigotWidth;
            data.ExhaustSpigotHeight = dto.ExhaustSpigotHeight;
            data.ExhaustSpigotNumber = dto.ExhaustSpigotNumber;
            data.ExhaustSpigotDis = dto.ExhaustSpigotDis;
            #endregion

            #region 灯具类型
            data.LightType = dto.LightType;
            data.SpotLightNumber = dto.SpotLightNumber;
            data.SpotLightDistance = dto.SpotLightDistance;
            data.LightToFront = dto.LightToFront;
            #endregion

            #region 其他配置
            data.DrainType = dto.DrainType;
            data.WaterCollection = dto.WaterCollection;
            data.LedLogo = dto.LedLogo;
            data.BackToBack = dto.BackToBack;
            data.BackCj = dto.BackCj;
            data.CjSpigotToRight = dto.CjSpigotToRight;
            data.CoverBoard = dto.CoverBoard;
            #endregion

            #region Ansul基本参数
            data.Ansul = dto.Ansul;
            data.AnsulSide = dto.AnsulSide;
            data.AnsulDetector = dto.AnsulDetector;
            #endregion

            #region Ansul下喷
            data.AnsulDropNumber = dto.AnsulDropNumber;
            data.AnsulDropToFront = dto.AnsulDropToFront;
            data.AnsulDropDis1 = dto.AnsulDropDis1;
            data.AnsulDropDis2 = dto.AnsulDropDis2;
            data.AnsulDropDis3 = dto.AnsulDropDis3;
            data.AnsulDropDis4 = dto.AnsulDropDis4;
            data.AnsulDropDis5 = dto.AnsulDropDis5;
            #endregion

            #region UV灯参数
            data.UvLightType = dto.UvLightType;
            data.Bluetooth = dto.Bluetooth;
            #endregion

            #region 新风口参数
            data.SupplySpigotNumber = dto.SupplySpigotNumber;
            data.SupplySpigotDis = dto.SupplySpigotDis;
            #endregion

            data.NotifyModified();//通知更新时间
            return new ApiResponse<UvfData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<UvfData> { Status = false, Message = e.Message };
        }
    }
}