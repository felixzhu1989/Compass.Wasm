using Compass.DataService.Domain;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Data.Ceilings;

namespace Compass.Wasm.Server.Services.Data.Ceilings;

public interface IAnDataService : IBaseDataGetService<AnData>, IBaseDataUpdateService<AnData>
{

}
public class AnDataService:BaseDataGetService<AnData>,IAnDataService 
{
    private readonly IDataRepository _repository;
    public AnDataService(IDataRepository repository) : base(repository)
    {
        _repository = repository;
    }


    public async Task<ApiResponse<AnData>> UpdateAsync(Guid id, AnData dto)
    {
        try
        {
            var model = await _repository.GetModuleDataByIdAsync(id);
            if (model is not AnData data) return new ApiResponse<AnData> { Status = false, Message = "更新数据失败" };

            #region 基本参数
            data.Length = dto.Length;
            data.Width = dto.Width;
            data.Height = dto.Height;
            data.SidePanel = dto.SidePanel;
            data.Marvel = dto.Marvel;
            data.AssyPath=dto.AssyPath;
            #endregion



            #region Ansul基本参数
            data.Ansul = dto.Ansul;
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
            return new ApiResponse<AnData> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<AnData> { Status = false, Message = e.Message };
        }
    }
}