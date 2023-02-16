using System.Collections.ObjectModel;
using AutoMapper;
using Compass.TodoService.Domain;
using Compass.TodoService.Domain.Entities;
using Compass.TodoService.Infrastructure;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Parameter;
using Compass.Wasm.Shared.TodoService;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Compass.Wasm.Server.TodoService;


public class TodoService:ITodoService
{
    private readonly TodoDomainService _domainService;
    private readonly TodoDbContext _dbContext;
    private readonly ITodoRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    public TodoService(TodoDomainService domainService, TodoDbContext dbContext, ITodoRepository repository, IMapper mapper, IEventBus eventBus)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
    }

    #region 基本增删改查
    public async Task<ApiResponse<List<TodoDto>>> GetAllAsync()
    {
        try
        {
            var models = await _repository.GetTodosAsync();
            var dtos = await _mapper.ProjectTo<TodoDto>(models).ToListAsync();
            return new ApiResponse<List<TodoDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<TodoDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<TodoDto>> GetSingleAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetTodoByIdAsync(id);
            if (model != null)
            {
                var dto = _mapper.Map<TodoDto>(model);
                return new ApiResponse<TodoDto> { Status = true, Result = dto };
            }
            return new ApiResponse<TodoDto> { Status = false, Message = "查询数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<TodoDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<TodoDto>> AddAsync(TodoDto dto)
    {
        try
        {
            var model = new Todo(Guid.NewGuid(), dto.Title, dto.Content, dto.Status);
            await _dbContext.Todos.AddAsync(model);
            dto.Id= model.Id;
            return new ApiResponse<TodoDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<TodoDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<TodoDto>> UpdateAsync(Guid id,TodoDto dto)
    {
        try
        {
            var model = await _repository.GetTodoByIdAsync(id);
            if (model != null)
            {
                model.ChangeTitle(dto.Title).ChangeContent(dto.Content).ChangeStatus(dto.Status);
                return new ApiResponse<TodoDto> { Status = true, Result = dto };
            }
            return new ApiResponse<TodoDto> { Status = false, Message = "更新数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<TodoDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<TodoDto>> DeleteAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetTodoByIdAsync(id);
            if (model != null)
            {
                model.SoftDelete();//软删除
                return new ApiResponse<TodoDto> { Status = true };
            }
            return new ApiResponse<TodoDto> { Status = false, Message = "删除数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<TodoDto> { Status = false, Message = e.Message };
        }
    } 
    #endregion

    /// <summary>
    /// 根据筛选条件查询
    /// </summary>
    public async Task<ApiResponse<List<TodoDto>>> GetAllFilterAsync(TodoParameter parameter)
    {
        try
        {
            var models = await _repository.GetTodosAsync();
            //筛选结果，按照创建时间排序
            var filterModels = models.Where(x =>
                (string.IsNullOrWhiteSpace(parameter.Search) || x.Title.Contains(parameter.Search) || x.Content.Contains(parameter.Search)) &&
                (parameter.Status == null || x.Status == parameter.Status)).OrderBy(x=>x.CreationTime);
            var dtos = await _mapper.ProjectTo<TodoDto>(filterModels).ToListAsync();
            return new ApiResponse<List<TodoDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<TodoDto>> { Status = false, Message = e.Message };
        }
    }

    /// <summary>
    /// 汇总所有的信息
    /// </summary>
    /// <returns></returns>
    public async Task<ApiResponse<TodoSummaryDto>> GetSummary()
    {
        try
        {
            //待办事项结果
            var todos = await _repository.GetTodosAsync();
            //备忘结果
            var memos = await _repository.GetMemosAsync();
            TodoSummaryDto summary = new();
            summary.Sum = todos.Count();//汇总待办事项数量
            summary.CompletedCount = todos.Count(x => x.Status == 1);//统计完成待办事项数量
            summary.CompletedRatio = (summary.CompletedCount / (double)summary.Sum).ToString("0%");//完成率
            summary.MemoCount=memos.Count();//汇总备忘录数量
            summary.TodoDtos = new ObservableCollection<TodoDto>(_mapper.ProjectTo<TodoDto>(todos.Where(x=>x.Status==0)));//只需要未完成的项目
            summary.MemoDtos = new ObservableCollection<MemoDto>(_mapper.ProjectTo<MemoDto>(memos));
            return new ApiResponse<TodoSummaryDto>{Status = true,Result = summary};
        }
        catch (Exception e)
        {
            return new ApiResponse<TodoSummaryDto> { Status = false, Message = e.Message };
        }
    }
}