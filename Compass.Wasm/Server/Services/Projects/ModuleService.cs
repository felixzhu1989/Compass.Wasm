using AutoMapper;
using Compass.DataService.Domain;
using Compass.Dtos;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Projects;

namespace Compass.Wasm.Server.Services.Projects;
public interface IModuleService : IBaseService<ModuleDto>
{
    //扩展查询
    Task<ApiResponse<List<ModuleDto>>> GetAllByDrawingIdAsync(Guid drawingId);

}
public class ModuleService : IModuleService
{
    private readonly ProjectDomainService _domainService;
    private readonly ProjectDbContext _dbContext;
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly IdentityUserManager _userManager;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDataRepository _dataRepository;
    public ModuleService(ProjectDomainService domainService, ProjectDbContext dbContext, IProjectRepository repository, IMapper mapper, IEventBus eventBus, IdentityUserManager userManager, ICategoryRepository categoryRepository, IDataRepository dataRepository)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
        _userManager = userManager;
        _categoryRepository = categoryRepository;
        _dataRepository = dataRepository;

    }

    #region 基本增删改查
    public async Task<ApiResponse<List<ModuleDto>>> GetAllAsync()
    {
        try
        {
            var models = await _repository.GetModulesAsync();
            var orderModels = models.OrderByDescending(x => x.CreationTime);
            var dtos = await _mapper.ProjectTo<ModuleDto>(orderModels).ToListAsync();
            return new ApiResponse<List<ModuleDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<ModuleDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ModuleDto>> GetSingleAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetModuleByIdAsync(id);
            if (model == null) return new ApiResponse<ModuleDto> { Status = false, Message = "查询数据失败" };
            var dto = _mapper.Map<ModuleDto>(model);
            return new ApiResponse<ModuleDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<ModuleDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ModuleDto>> AddAsync(ModuleDto dto)
    {
        try
        {
            var model = new Compass.ProjectService.Domain.Entities.Module(Guid.NewGuid(), dto.DrawingId, dto.ModelTypeId.Value, dto.Name.ToUpper(), dto.ModelName, dto.SpecialNotes, dto.Length, dto.Width, dto.Height, dto.SidePanel,dto.Pallet,dto.Marvel,dto.ExportWay);
            await _dbContext.Modules.AddAsync(model);
            dto.Id = model.Id;
            return new ApiResponse<ModuleDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<ModuleDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ModuleDto>> UpdateAsync(Guid id, ModuleDto dto)
    {
        try
        {
            var model = await _repository.GetModuleByIdAsync(id);
            if (model == null) return new ApiResponse<ModuleDto> { Status = false, Message = "更新数据失败" };
            model.Update(dto);//这里面发出领域事件
            return new ApiResponse<ModuleDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<ModuleDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ModuleDto>> DeleteAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetModuleByIdAsync(id);
            if (model == null) return new ApiResponse<ModuleDto> { Status = false, Message = "删除数据失败" };
            model.SoftDelete();//软删除
            return new ApiResponse<ModuleDto> { Status = true };
        }
        catch (Exception e)
        {
            return new ApiResponse<ModuleDto> { Status = false, Message = e.Message };
        }
    }
    #endregion

    #region 扩展的查询功能,WPF




    #endregion


    #region 扩展查询功能，Blazor
    /// <summary>
    /// 根据图纸号查询Module
    /// </summary>
    public async Task<ApiResponse<List<ModuleDto>>> GetAllByDrawingIdAsync(Guid drawingId)
    {
        try
        {
            var models = await _repository.GetModulesByDrawingIdAsync(drawingId);
            var dtos = await _mapper.ProjectTo<ModuleDto>(models).ToListAsync();
            return new ApiResponse<List<ModuleDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<ModuleDto>> { Status = false, Message = e.Message };
        }
    }
    #endregion

}