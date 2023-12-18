using Compass.Wasm.Shared.Projects;
using Compass.Wasm.Shared;
using AutoMapper;

namespace Compass.Wasm.Server.Services.Projects;
public interface ILessonService : IBaseService<LessonDto>
{
    //扩展查询
    Task<ApiResponse<List<LessonDto>>> GetAllByProjectIdAsync(Guid projectId);


}
public class LessonService:ILessonService
{
    private readonly ProjectDbContext _dbContext;
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;

    public LessonService(ProjectDomainService domainService, ProjectDbContext dbContext, IProjectRepository repository, IMapper mapper, IEventBus eventBus, IdentityUserManager userManager)
    {
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
    }
    #region 基本增删改查
    public async Task<ApiResponse<List<LessonDto>>> GetAllAsync()
    {
        try
        {
            var models = await _repository.GetLessonsAsync();
            var orderModels = models.OrderByDescending(x => x.CreationTime);
            var dtos = await _mapper.ProjectTo<LessonDto>(orderModels).ToListAsync();
            return new ApiResponse<List<LessonDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<LessonDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<LessonDto>> GetSingleAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetLessonByIdAsync(id);
            if (model == null) return new ApiResponse<LessonDto> { Status = false, Message = "查询数据失败" };
            var dto = _mapper.Map<LessonDto>(model);
            return new ApiResponse<LessonDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<LessonDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<LessonDto>> AddAsync(LessonDto dto)
    {
        try
        {
            var model = new Lesson(Guid.NewGuid(), dto.ProjectId, dto.Content, dto.ContentUrl);
            await _dbContext.Lessons.AddAsync(model);
            dto.Id = model.Id;
            return new ApiResponse<LessonDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<LessonDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<LessonDto>> UpdateAsync(Guid id, LessonDto dto)
    {
        try
        {
            var model = await _repository.GetLessonByIdAsync(id);
            if (model == null) return new ApiResponse<LessonDto> { Status = false, Message = "更新数据失败" };
            model.Update(dto);
            return new ApiResponse<LessonDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<LessonDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<LessonDto>> DeleteAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetLessonByIdAsync(id);
            if (model == null) return new ApiResponse<LessonDto> { Status = false, Message = "删除数据失败" };
            model.SoftDelete();//软删除
            return new ApiResponse<LessonDto> { Status = true };
        }
        catch (Exception e)
        {
            return new ApiResponse<LessonDto> { Status = false, Message = e.Message };
        }
    }
    #endregion

    #region 扩展查询功能，Blazor
    public async Task<ApiResponse<List<LessonDto>>> GetAllByProjectIdAsync(Guid projectId)
    {
        try
        {
            var models = await _repository.GetLessonsByProjectIdAsync(projectId);
            var dtos = await _mapper.ProjectTo<LessonDto>(models).ToListAsync();
            return new ApiResponse<List<LessonDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<LessonDto>> { Status = false, Message = e.Message };
        }
    } 
    #endregion
}