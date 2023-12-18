using Compass.DataService.Domain;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Data.Ceilings;

namespace Compass.Wasm.Server.Services.Data.Ceilings;

public interface IUcjDataService : IBaseDataGetService<UcjData>, IBaseDataUpdateService<UcjData>
{

}
public class UcjDataService:BaseDataGetService<UcjData>,IUcjDataService
{
    private readonly IDataRepository _repository;
    public UcjDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<UcjData>> UpdateAsync(Guid id, UcjData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not UcjData data) return new ApiResponse<UcjData> { Status = false, Message = "更新数据失败" };

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

            #region 过滤器参数
            data.FilterType = dto.FilterType;
            data.FilterSide=dto.FilterSide;
            data.FilterLeft=dto.FilterLeft;
            data.FilterRight=dto.FilterRight;
            data.FilterBlindNumber=dto.FilterBlindNumber;
            #endregion

            #region 灯具类型
            data.CeilingLightType = dto.CeilingLightType;
            data.LightCable = dto.LightCable;
            data.HclSide = dto.HclSide;
            data.HclLeft = dto.HclLeft;
            data.HclRight = dto.HclRight;
            #endregion

            #region 灯腔侧板参数
            data.TotalLength=dto.TotalLength;
            data.LongGlassNumber=dto.LongGlassNumber;
            data.ShortGlassNumber=dto.ShortGlassNumber;

            data.LeftLength=dto.LeftLength;
            data.RightLength=dto.RightLength;
            data.MiddleLength=dto.MiddleLength;
            #endregion

            #region 其他配置
            data.DomeSsp = dto.DomeSsp;
            data.Gutter = dto.Gutter;
            data.GutterWidth = dto.GutterWidth;
            data.Japan = dto.Japan;
            #endregion

            #region Ansul基本参数
            data.Ansul = dto.Ansul;
            data.AnsulSide = dto.AnsulSide;
            data.AnsulDetector = dto.AnsulDetector;
            #endregion

            #region Ansul探测器
            data.AnsulDetectorEnd = dto.AnsulDetectorEnd;
            data.AnsulDetectorNumber=dto.AnsulDetectorNumber;
            data.AnsulDetectorDis1 = dto.AnsulDetectorDis1;
            data.AnsulDetectorDis2 = dto.AnsulDetectorDis2;
            data.AnsulDetectorDis3 = dto.AnsulDetectorDis3;
            data.AnsulDetectorDis4 = dto.AnsulDetectorDis4;
            data.AnsulDetectorDis5 = dto.AnsulDetectorDis5;
            #endregion

            #region UV灯参数
            data.UvLightType=dto.UvLightType;
            #endregion

            data.NotifyModified();//通知更新时间
            return new ApiResponse<UcjData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<UcjData> { Status = false, Message = e.Message };
        }
    }
}