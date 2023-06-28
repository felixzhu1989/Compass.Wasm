using AutoMapper;
using Compass.DataService.Domain;
using Compass.PlanService.Domain;
using Compass.Wasm.Server.Events;
using Compass.Wasm.Server.ExportExcel;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Params;
using Compass.Wasm.Shared.Projects;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Compass.Wasm.Server.Services.Projects;

public interface IProjectService : IBaseService<ProjectDto>
{
    //扩展的查询功能,WPF
    Task<ApiResponse<List<ProjectDto>?>> GetAllFilterAsync(ProjectParam param);

    Task<ApiResponse<List<DrawingDto>>> GetModuleTreeAsync(ProjectParam param);//用于树结构
    Task<ApiResponse<List<ModuleDto>>> GetModuleListAsync(ProjectParam param);//用于自动作图

    //扩展查询功能，Blazor
    

    //UploadFiles
    Task<ApiResponse<ProjectDto>> UploadFilesAsync(Guid id, ProjectDto dto);
}
public class ProjectService : IProjectService
{
    #region ctor
    private readonly ProjectDbContext _dbContext;
    private readonly IProjectRepository _repository;
    private readonly IPlanRepository _planRepository;
    private readonly IIdentityRepository _identityRepository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly IDataRepository _dataRepository;
    public ProjectService(ProjectDomainService domainService, ProjectDbContext dbContext, IProjectRepository repository, IPlanRepository planRepository,IIdentityRepository identityRepository, IMapper mapper, IEventBus eventBus, ExportExcelService export, IDataRepository dataRepository)
    {
        _dbContext = dbContext;
        _repository = repository;
        _planRepository = planRepository;
        _identityRepository = identityRepository;
        _mapper = mapper;
        _eventBus = eventBus;
        _dataRepository = dataRepository;
    }
    #endregion

    #region 基本增删改查
    public async Task<ApiResponse<List<ProjectDto>>> GetAllAsync()
    {
        try
        {
            var models = await _repository.GetProjectsAsync();
            var orderModels = models.OrderByDescending(x => x.DeliveryDate);
            var dtos = await _mapper.ProjectTo<ProjectDto>(orderModels).ToListAsync();
            foreach (var dto in dtos.Where(dto => dto.Designer != null && dto.Designer != Guid.Empty))
            {
                var user =await _identityRepository.GetUserByIdAsync(dto.Designer.Value);
                dto.UserName = user.UserName;
            }
            return new ApiResponse<List<ProjectDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<ProjectDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ProjectDto>> GetSingleAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetProjectByIdAsync(id);
            if (model == null) return new ApiResponse<ProjectDto> { Status = false, Message = "查询数据失败" };
            var dto = _mapper.Map<ProjectDto>(model);
            if (dto.Designer != null && dto.Designer != Guid.Empty)
            {
                var user = await _identityRepository.GetUserByIdAsync(dto.Designer.Value);
                dto.UserName =user.UserName;
            }
            return new ApiResponse<ProjectDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<ProjectDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ProjectDto>> AddAsync(ProjectDto dto)
    {
        try
        {
            var model = new Project(Guid.NewGuid(), dto.OdpNumber.ToUpper(), dto.Name, dto.DeliveryDate, dto.ProjectType, dto.RiskLevel, dto.SpecialNotes,dto.Designer);
            await _dbContext.Projects.AddAsync(model);
            dto.Id = model.Id;

            //Todo:发出集成事件，添加待办事项
            var eventData = new ProjectCreatedEvent(dto.OdpNumber, dto.Name,dto.Designer);
            //发布集成事件
            _eventBus.Publish("ProjectService.Project.Created", eventData);

            return new ApiResponse<ProjectDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<ProjectDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ProjectDto>> UpdateAsync(Guid id, ProjectDto dto)
    {
        try
        {
            var model = await _repository.GetProjectByIdAsync(id);
            if (model == null) return new ApiResponse<ProjectDto> { Status = false, Message = "更新数据失败" };
            model.Update(dto);
            return new ApiResponse<ProjectDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<ProjectDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ProjectDto>> DeleteAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetProjectByIdAsync(id);
            if (model == null) return new ApiResponse<ProjectDto> { Status = false, Message = "删除数据失败" };
            model.SoftDelete();//软删除
            return new ApiResponse<ProjectDto> { Status = true };
        }
        catch (Exception e)
        {
            return new ApiResponse<ProjectDto> { Status = false, Message = e.Message };
        }
    }
    #endregion

    #region 扩展的查询功能,WPF
    /// <summary>
    /// 根据筛选条件查询
    /// </summary>
    public async Task<ApiResponse<List<ProjectDto>?>> GetAllFilterAsync(ProjectParam param)
    {
        try
        {
            //查询计划，根据状态查询到Id
            var ids = await _planRepository.GetProjectIdsByStatusAsync(param.Status);
            if (ids.Count == 0)
            {
                return new ApiResponse<List<ProjectDto>?> { Status = false, Message ="当前状态无项目" };
            }
            var models = await _repository.GetProjectsAsync();
            //筛选结果，按照发货时间倒序排序
            var filterModels = models.Where(x => (
                string.IsNullOrWhiteSpace(param.Search) || x.OdpNumber.Contains(param.Search) || x.Name.Contains(param.Search) || x.SpecialNotes.Contains(param.Search)) && ids.Contains(x.Id)).OrderByDescending(x => x.DeliveryDate);
            var dtos = await _mapper.ProjectTo<ProjectDto>(filterModels).ToListAsync();
            foreach (var dto in dtos)
            {
                dto.Status = param.Status;
                if (dto.Designer == null || dto.Designer == Guid.Empty) continue;
                var user = await _identityRepository.GetUserByIdAsync(dto.Designer.Value);
                dto.UserName = user.UserName;
            }
            return new ApiResponse<List<ProjectDto>?> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<ProjectDto>?> { Status = false, Message = e.Message };
        }
    }

    /// <summary>
    /// 查询单个项目的图纸和分段的树结构
    /// </summary>
    public async Task<ApiResponse<List<DrawingDto>>> GetModuleTreeAsync(ProjectParam param)
    {
        try
        {
            //先查询项目下的所有图纸Item
            var models = await _repository.GetDrawingsByProjectIdAsync(param.ProjectId.Value);
            var dtos = await _mapper.ProjectTo<DrawingDto>(models).ToListAsync();
            foreach (var dto in dtos)
            {
                dto.IsDrawingOk = !string.IsNullOrEmpty(dto.DrawingUrl);
                //再查询图纸下的所有分段Module
                var modules = await _repository.GetModulesByDrawingIdAsync(dto.Id.Value);
                dto.ModuleDtos = new ObservableCollectionListSource<ModuleDto>(await _mapper.ProjectTo<ModuleDto>(modules).ToListAsync());
                //此时Modules中并没有长宽高的信息，这里遍历一下，然后加上去
                foreach (var moduleDto in dto.ModuleDtos)
                {
                    var moduleData = await _dataRepository.GetModuleDataByIdAsync(moduleDto.Id.Value);
                    if (moduleData == null) continue;
                    moduleDto.Length = moduleData.Length;
                    moduleDto.Width = moduleData.Width;
                    moduleDto.Height = moduleData.Height;
                    moduleDto.SidePanel = moduleData.SidePanel;

                    moduleDto.IsDrawingOk = dto.IsDrawingOk;
                    moduleDto.DrawingUrl = dto.DrawingUrl;
                }
            }
            return new ApiResponse<List<DrawingDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<DrawingDto>> { Status = false, Message = e.Message };
        }
    }

    /// <summary>
    /// 查询单个项目中所有的图纸
    /// </summary>
    public async Task<ApiResponse<List<ModuleDto>>> GetModuleListAsync(ProjectParam param)
    {
        try
        {
            var project = await _repository.GetProjectByIdAsync(param.ProjectId.Value);
            //先查询项目下的所有图纸Item
            var drawings = await _repository.GetDrawingsByProjectIdAsync(param.ProjectId.Value);
            var drawingDtos = await _mapper.ProjectTo<DrawingDto>(drawings).ToListAsync();

            var dtos = new List<ModuleDto>();
            foreach (var drawingDto in drawingDtos)
            {
                drawingDto.IsDrawingOk = !string.IsNullOrEmpty(drawingDto.DrawingUrl);
                //再查询图纸下的所有分段Module
                var modules = await _repository.GetModulesByDrawingIdAsync(drawingDto.Id.Value);
                var moduleDtos = _mapper.ProjectTo<ModuleDto>(modules).ToList();
                //查询时添加图纸的item号，项目ODP号和项目名称，判断截图是否存在

                moduleDtos.ForEach(x =>
                {
                    x.ItemNumber = drawingDto.ItemNumber;
                    x.Batch = drawingDto.Batch;
                    x.OdpNumber = project.OdpNumber;
                    x.ProjectName = project.Name;
                    x.IsJobCardOk = x.IsModuleDataOk && !string.IsNullOrEmpty(drawingDto.ImageUrl);
                    x.IsDrawingOk = drawingDto.IsDrawingOk;
                    x.DrawingUrl = drawingDto.DrawingUrl;
                    x.ImageUrl = drawingDto.ImageUrl;
                    x.ProjectType = project.ProjectType;
                    x.DeliveryDate = project.DeliveryDate;
                    x.ProjectSpecialNotes = project.SpecialNotes;

                });
                dtos.AddRange(moduleDtos);
            }
            //此时Modules中并没有长宽高的信息，这里遍历一下，然后加上去
            foreach (var moduleDto in dtos)
            {
                var moduleData = await _dataRepository.GetModuleDataByIdAsync(moduleDto.Id.Value);
                if (moduleData == null) continue;
                moduleDto.Length = moduleData.Length;
                moduleDto.Width = moduleData.Width;
                moduleDto.Height = moduleData.Height;
                moduleDto.SidePanel = moduleData.SidePanel;
            }
            return new ApiResponse<List<ModuleDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<ModuleDto>> { Status = false, Message = e.Message };
        }
    }



    #endregion

    #region 扩展查询功能，Blazor
    

    /// <summary>
    /// 上传项目文件
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<ApiResponse<ProjectDto>> UploadFilesAsync(Guid id, ProjectDto dto)
    {
        try
        {
            var model = await _repository.GetProjectByIdAsync(id);
            if (model == null) return new ApiResponse<ProjectDto> { Status = false, Message = "更新数据失败" };
            model.UploadFiles(dto);
            return new ApiResponse<ProjectDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<ProjectDto> { Status = false, Message = e.Message };
        }
    }
    #endregion

}