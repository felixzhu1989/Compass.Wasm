using AutoMapper;
using Compass.TodoService.Domain.Entities;
using Compass.TodoService.Domain;
using Compass.TodoService.Infrastructure;
using Compass.Wasm.Shared.TodoService;
using Compass.Wasm.Shared;

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
            var model = new Memo(Guid.NewGuid(), dto.Title, dto.Content);
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



}