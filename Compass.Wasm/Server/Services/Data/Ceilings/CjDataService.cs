using Compass.DataService.Domain;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Data.Ceilings;

namespace Compass.Wasm.Server.Services.Data.Ceilings;
public interface ICjDataService : IBaseDataGetService<CjData>, IBaseDataUpdateService<CjData>
{

}
public class CjDataService : BaseDataGetService<CjData>, ICjDataService
{
    private readonly IDataRepository _repository;
    public CjDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<CjData>> UpdateAsync(Guid id, CjData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not CjData data) return new ApiResponse<CjData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            data.Marvel = dto.Marvel;
            data.AssyPath=dto.AssyPath;
            #endregion


            #region CJ脖颈参数
            data.CjSpigotDirection=dto.CjSpigotDirection;
            data.CjSpigotToRight=dto.CjSpigotToRight;
            #endregion


            #region 连接排风、BCJ、LKS、Gutter位置参数
            data.LeftBeamType=dto.LeftBeamType;
            data.RightBeamType=dto.RightBeamType;
            data.LeftDbToRight=dto.LeftDbToRight;
            data.RightDbToLeft=dto.RightDbToLeft;
            data.LeftEndDis=dto.LeftEndDis;
            data.RightEndDis=dto.RightEndDis;
            data.BcjSide=dto.BcjSide;
            data.LksSide=dto.LksSide;
            data.GutterSide=dto.GutterSide;
            data.LeftGutterWidth=dto.LeftGutterWidth;
            data.RightGutterWidth=dto.RightGutterWidth;
            #endregion

            #region 连接NOCJ时的参数
            data.NocjSide=dto.NocjSide;
            data.NocjBackSide=dto.NocjBackSide;
            data.DpSide=dto.DpSide;
            //data.DpBackSide=dto.DpBackSide;
            #endregion


            data.NotifyModified();//通知更新时间
            return new ApiResponse<CjData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<CjData> { Status = false, Message = e.Message };
        }
    }
}