using AutoMapper;
using Compass.Dtos;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Params;
using Compass.Wasm.Shared.Projects;

namespace Compass.Wasm.Server.Services.Projects;
public interface ICutListService : IBaseService<CutListDto>
{
    //扩展查询
    Task<ApiResponse<List<CutListDto>>> GetAllByModuleIdAsync(CutListParam param);
}
public class CutListService : ICutListService
{
    private readonly ProjectDomainService _domainService;
    private readonly ProjectDbContext _dbContext;
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    public CutListService(ProjectDomainService domainService, ProjectDbContext dbContext, IProjectRepository repository, IMapper mapper, IEventBus eventBus)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
    }

    #region 基本增删改查
    public async Task<ApiResponse<List<CutListDto>>> GetAllAsync()
    {
        try
        {
            var models = await _repository.GetCutListsAsync();
            var orderModels = models.OrderByDescending(x => x.CreationTime);
            var dtos = await _mapper.ProjectTo<CutListDto>(orderModels).ToListAsync();
            return new ApiResponse<List<CutListDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<CutListDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<CutListDto>> GetSingleAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetCutListByIdAsync(id);
            if (model == null) return new ApiResponse<CutListDto> { Status = false, Message = "查询数据失败" };
            var dto = _mapper.Map<CutListDto>(model);
            return new ApiResponse<CutListDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<CutListDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<CutListDto>> AddAsync(CutListDto dto)
    {
        try
        {
            //为了防止多次导图的失误，这里如果发现已经存在CutList，那么就覆盖更改
            var exitModel = await _dbContext.CutLists.FirstOrDefaultAsync(x => x.ModuleId.Equals(dto.ModuleId) && x.PartNo == dto.PartNo);
            if (exitModel != null)
            {
                return await UpdateAsync(exitModel.Id, dto);
            }
            var model = new CutList(Guid.NewGuid(), dto.ModuleId, dto.PartDescription, dto.Length, dto.Width, dto.Thickness, dto.Quantity, dto.Material, dto.PartNo,dto.BendingMark);
            await _dbContext.CutLists.AddAsync(model);
            dto.Id = model.Id;
            return new ApiResponse<CutListDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<CutListDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<CutListDto>> UpdateAsync(Guid id, CutListDto dto)
    {
        try
        {
            var model = await _repository.GetCutListByIdAsync(id);
            if (model == null) return new ApiResponse<CutListDto> { Status = false, Message = "更新数据失败" };
            model.Update(dto);
            return new ApiResponse<CutListDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<CutListDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<CutListDto>> DeleteAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetCutListByIdAsync(id);
            if (model == null) return new ApiResponse<CutListDto> { Status = false, Message = "删除数据失败" };
            model.SoftDelete();//软删除
            return new ApiResponse<CutListDto> { Status = true };
        }
        catch (Exception e)
        {
            return new ApiResponse<CutListDto> { Status = false, Message = e.Message };
        }
    }
    #endregion

    #region 扩展查询功能，WPF
    public async Task<ApiResponse<List<CutListDto>>> GetAllByModuleIdAsync(CutListParam param)
    {
        try
        {
            var models = await _repository.GetCutListsByModuleIdAsync(param.ModuleId.Value);
            var dtos = await _mapper.ProjectTo<CutListDto>(models).ToListAsync();
            var i = 1;
            dtos.ForEach(x => { x.Index = i++; });
            return new ApiResponse<List<CutListDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<CutListDto>> { Status = false, Message = e.Message };
        }
    }
    #endregion
}