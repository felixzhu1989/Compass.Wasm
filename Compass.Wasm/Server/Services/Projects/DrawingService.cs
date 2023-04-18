using AutoMapper;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Projects;

namespace Compass.Wasm.Server.Services.Projects;

public class DrawingService : IDrawingService
{
    private readonly ProjectDomainService _domainService;
    private readonly ProjectDbContext _dbContext;
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly IdentityUserManager _userManager;
    public DrawingService(ProjectDomainService domainService, ProjectDbContext dbContext, IProjectRepository repository, IMapper mapper, IEventBus eventBus, IdentityUserManager userManager)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
        _userManager = userManager;
    }

    #region 基本增删改查
    public async Task<ApiResponse<List<DrawingDto>>> GetAllAsync()
    {
        try
        {
            var models = await _repository.GetDrawingsAsync();
            var orderModels = models.OrderByDescending(x => x.CreationTime);
            var dtos = await _mapper.ProjectTo<DrawingDto>(orderModels).ToListAsync();
            return new ApiResponse<List<DrawingDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<DrawingDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<DrawingDto>> GetSingleAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetDrawingByIdAsync(id);
            if (model != null)
            {
                var dto = _mapper.Map<DrawingDto>(model);
                return new ApiResponse<DrawingDto> { Status = true, Result = dto };
            }
            return new ApiResponse<DrawingDto> { Status = false, Message = "查询数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<DrawingDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<DrawingDto>> AddAsync(DrawingDto dto)
    {
        try
        {
            var model = new Drawing(Guid.NewGuid(), dto.ProjectId, dto.ItemNumber, dto.DrawingUrl, dto.ImageUrl);
            await _dbContext.Drawings.AddAsync(model);
            dto.Id = model.Id;
            return new ApiResponse<DrawingDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<DrawingDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<DrawingDto>> UpdateAsync(Guid id, DrawingDto dto)
    {
        try
        {
            var model = await _repository.GetDrawingByIdAsync(id);
            if (model != null)
            {
                model.Update(dto);
                return new ApiResponse<DrawingDto> { Status = true, Result = dto };
            }
            return new ApiResponse<DrawingDto> { Status = false, Message = "更新数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<DrawingDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<DrawingDto>> DeleteAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetDrawingByIdAsync(id);
            if (model != null)
            {
                model.SoftDelete();//软删除
                return new ApiResponse<DrawingDto> { Status = true };
            }
            return new ApiResponse<DrawingDto> { Status = false, Message = "删除数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<DrawingDto> { Status = false, Message = e.Message };
        }
    }
    #endregion


    #region 扩展的查询功能,WPF




    #endregion


    #region 扩展查询功能，Blazor
    /// <summary>
    /// 根据项目号查询Drawings
    /// </summary>
    public async Task<ApiResponse<List<DrawingDto>>> GetAllByProjectIdAsync(Guid projectId)
    {
        try
        {
            var models = await _repository.GetDrawingsByProjectIdAsync(projectId);
            var dtos = await _mapper.ProjectTo<DrawingDto>(models).ToListAsync();
            return new ApiResponse<List<DrawingDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<DrawingDto>> { Status = false, Message = e.Message };
        }
    }
    #endregion
}