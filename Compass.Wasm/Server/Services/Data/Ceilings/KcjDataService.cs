using Compass.DataService.Domain;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Data.Ceilings;
using Compass.Wasm.Shared.Data.Hoods;

namespace Compass.Wasm.Server.Services.Data.Ceilings;

public interface IKcjDataService : IBaseDataGetService<KcjData>, IBaseDataUpdateService<KcjData>
{

}
public class KcjDataService:BaseDataGetService<KcjData>,IKcjDataService
{
    private readonly IDataRepository _repository;
    public KcjDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }
    public async Task<ApiResponse<KcjData>> UpdateAsync(Guid id, KcjData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not KcjData data) return new ApiResponse<KcjData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
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

            data.NotifyModified();//通知更新时间
            return new ApiResponse<KcjData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<KcjData> { Status = false, Message = e.Message };
        }
    }
}