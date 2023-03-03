using AutoMapper;
using Compass.TodoService.Domain.Entities;
using Compass.TodoService.Domain;
using Compass.TodoService.Infrastructure;
using Compass.Wasm.Shared.TodoService;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Parameter;

namespace Compass.Wasm.Server.TodoService;

public class MemoService:IMemoService
{
    private readonly TodoDomainService _domainService;
    private readonly TodoDbContext _dbContext;
    private readonly ITodoRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    public MemoService(TodoDomainService domainService, TodoDbContext dbContext, ITodoRepository repository, IMapper mapper, IEventBus eventBus)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
    }

    #region 基本增删改查
    public async Task<ApiResponse<List<MemoDto>>> GetAllAsync()
    {
        try
        {
            var models = await _repository.GetMemosAsync();
            var dtos = await _mapper.ProjectTo<MemoDto>(models).ToListAsync();
            return new ApiResponse<List<MemoDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<MemoDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<MemoDto>> GetSingleAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetMemoByIdAsync(id);
            if (model != null)
            {
                var dto = _mapper.Map<MemoDto>(model);
                return new ApiResponse<MemoDto> { Status = true, Result = dto };
            }
            return new ApiResponse<MemoDto> { Status = false, Message = "查询数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<MemoDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<MemoDto>> AddAsync(MemoDto dto)
    {
        try
        {
            var model = new Memo(Guid.NewGuid(), dto.Title, dto.Content,dto.UserId);
            await _dbContext.Memos.AddAsync(model);
            dto.Id= model.Id;
            return new ApiResponse<MemoDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<MemoDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<MemoDto>> UpdateAsync(Guid id,MemoDto dto)
    {
        try
        {
            var model = await _repository.GetMemoByIdAsync(id);
            if (model != null)
            {
                model.ChangeTitle(dto.Title).ChangeContent(dto.Content);
                return new ApiResponse<MemoDto> { Status = true, Result = dto };
            }
            return new ApiResponse<MemoDto> { Status = false, Message = "更新数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<MemoDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<MemoDto>> DeleteAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetMemoByIdAsync(id);
            if (model != null)
            {
                model.SoftDelete();//软删除
                return new ApiResponse<MemoDto> { Status = true };
            }
            return new ApiResponse<MemoDto> { Status = false, Message = "删除数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<MemoDto> { Status = false, Message = e.Message };
        }
    }
    #endregion

    #region 增加了特定用户的基本增查
    public async Task<ApiResponse<List<MemoDto>>> GetUserAllAsync(Guid userId)
    {

        try
        {
            var models = await _repository.GetMemosAsync();
            var userModels = models.Where(x => x.UserId.Equals(userId));
            var dtos = await _mapper.ProjectTo<MemoDto>(userModels).ToListAsync();
            return new ApiResponse<List<MemoDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<MemoDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<MemoDto>> UserAddAsync(MemoDto dto, Guid userId)
    {
        try
        {
            var model = new Memo(Guid.NewGuid(), dto.Title, dto.Content,  userId);
            await _dbContext.Memos.AddAsync(model);
            dto.Id= model.Id;
            return new ApiResponse<MemoDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<MemoDto> { Status = false, Message = e.Message };
        }
    }
    #endregion

    /// <summary>
    /// 根据筛选条件查询
    /// </summary>
    public async Task<ApiResponse<List<MemoDto>>> GetAllFilterAsync(QueryParameter parameter, Guid userId)
    {
        try
        {
            var models = await _repository.GetMemosAsync();
            //筛选结果，按照创建时间排序
            var filterModels = models.Where(x =>(
                string.IsNullOrWhiteSpace(parameter.Search) || x.Title.Contains(parameter.Search) || x.Content.Contains(parameter.Search))&&x.UserId.Equals(userId)).OrderBy(x => x.CreationTime);
            var dtos = await _mapper.ProjectTo<MemoDto>(filterModels).ToListAsync();
            return new ApiResponse<List<MemoDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<MemoDto>> { Status = false, Message = e.Message };
        }
    }
}