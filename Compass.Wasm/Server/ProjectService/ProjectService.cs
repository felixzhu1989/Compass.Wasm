using AutoMapper;
using Compass.DataService.Domain;
using Compass.Wasm.Server.ExportExcel;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Parameter;
using Compass.Wasm.Shared.ProjectService;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Module = System.Reflection.Module;

namespace Compass.Wasm.Server.ProjectService;

public class ProjectService : IProjectService
{
    private readonly ProjectDomainService _domainService;
    private readonly ProjectDbContext _dbContext;
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly ExportExcelService _export;
    private readonly IDataRepository _dataRepository;

    public ProjectService(ProjectDomainService domainService, ProjectDbContext dbContext, IProjectRepository repository, IMapper mapper, IEventBus eventBus, ExportExcelService export, IDataRepository dataRepository)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
        _export = export;
        _dataRepository = dataRepository;
    }

    #region 基本增删改查
    public async Task<ApiResponse<List<ProjectDto>>> GetAllAsync()
    {
        try
        {
            var models = await _repository.GetProjectsAsync();
            var orderModels = models.OrderByDescending(x => x.DeliveryDate);
            var dtos = await _mapper.ProjectTo<ProjectDto>(orderModels).ToListAsync();
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
            if (model != null)
            {
                var dto = _mapper.Map<ProjectDto>(model);
                return new ApiResponse<ProjectDto> { Status = true, Result = dto };
            }
            return new ApiResponse<ProjectDto> { Status = false, Message = "查询数据失败" };
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
            var model = new Project(Guid.NewGuid(), dto.OdpNumber.ToUpper(), dto.Name, dto.DeliveryDate, dto.ProjectType, dto.RiskLevel, dto.SpecialNotes, dto.ContractUrl);
            await _dbContext.Projects.AddAsync(model);
            dto.Id= model.Id;
            //todo:修改跟踪对象，改成其他的对象
            #region 当项目创建时，同时创建跟踪对象,同一个dbContext的操作应当写在一起
            if (!_dbContext.Trackings.Any(x => x.Id.Equals(dto.Id)))
            {
                var tracking = new Tracking(dto.Id.Value, DateTime.Now.AddDays(20));
                _dbContext.Trackings.Add(tracking);
            }

            #endregion
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
            if (model != null)
            {
                model.Update(dto);
                return new ApiResponse<ProjectDto> { Status = true, Result = dto };
            }
            return new ApiResponse<ProjectDto> { Status = false, Message = "更新数据失败" };
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
            if (model != null)
            {
                model.SoftDelete();//软删除
                return new ApiResponse<ProjectDto> { Status = true };
            }
            return new ApiResponse<ProjectDto> { Status = false, Message = "删除数据失败" };
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
    public async Task<ApiResponse<List<ProjectDto>>> GetAllFilterAsync(ProjectParameter parameter)
    {
        try
        {
            var models = await _repository.GetProjectsAsync();
            //筛选结果，按照发货时间倒序排序
            var filterModels = models.Where(x => (
                string.IsNullOrWhiteSpace(parameter.Search) || x.OdpNumber.Contains(parameter.Search) || x.Name.Contains(parameter.Search)||x.SpecialNotes.Contains(parameter.Search))&& (parameter.ProjectStatus == null || x.ProjectStatus == parameter.ProjectStatus)).OrderByDescending(x => x.DeliveryDate);
            var dtos = await _mapper.ProjectTo<ProjectDto>(filterModels).ToListAsync();
            return new ApiResponse<List<ProjectDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<ProjectDto>> { Status = false, Message = e.Message };
        }
    }

    /// <summary>
    /// 查询项目的统计信息
    /// </summary>
    /// <returns></returns>
    public async Task<ApiResponse<ProjectSummaryDto>> GetSummaryAsync()
    {
        try
        {
            //所有项目
            var dtos = (await GetAllAsync()).Result;
            ProjectSummaryDto summary = new();
            summary.Sum = dtos.Count;//总项目数
            summary.PlanCount = dtos.Count(x => x.ProjectStatus == ProjectStatus_e.计划);
            summary.DrawingCount = dtos.Count(x => x.ProjectStatus == ProjectStatus_e.制图);
            summary.ProductionCount = dtos.Count(x => x.ProjectStatus == ProjectStatus_e.生产);
            summary.WarehousingCount = dtos.Count(x => x.ProjectStatus == ProjectStatus_e.入库);
            summary.ShippingCount = dtos.Count(x => x.ProjectStatus == ProjectStatus_e.发货);
            summary.CompletedCount = dtos.Count(x => x.ProjectStatus == ProjectStatus_e.结束);

            return new ApiResponse<ProjectSummaryDto> { Status = true, Result = summary };
        }
        catch (Exception e)
        {
            return new ApiResponse<ProjectSummaryDto> { Status = false, Message = e.Message };
        }
    }

    /// <summary>
    /// 查询单个项目的图纸和分段的树结构
    /// </summary>
    public async Task<ApiResponse<List<DrawingDto>>> GetModuleTreeAsync(ProjectParameter parameter)
    {
        try
        {
            //先查询项目下的所有图纸Item
            var models = await _repository.GetDrawingsByProjectIdAsync(parameter.ProjectId.Value);
            var dtos = await _mapper.ProjectTo<DrawingDto>(models).ToListAsync();
            foreach (var dto in dtos)
            {
                //再查询图纸下的所有分段Module
                var modules = await _repository.GetModulesByDrawingIdAsync(dto.Id.Value);
                dto.ModuleDtos = new ObservableCollectionListSource<ModuleDto>(await _mapper.ProjectTo<ModuleDto>(modules).ToListAsync());
                //此时Modules中并没有长宽高的信息，这里遍历一下，然后加上去
                foreach (var moduleDto in dto.ModuleDtos)
                {
                    var moduleData = await _dataRepository.GetModuleDataByIdAsync(moduleDto.Id.Value);
                    if (moduleData == null)continue;
                    moduleDto.Length = moduleData.Length;
                    moduleDto.Width = moduleData.Width;
                    moduleDto.Height = moduleData.Height;
                }
            }
            return new ApiResponse<List<DrawingDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<DrawingDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<List<ModuleDto>>> GetModuleListAsync(ProjectParameter parameter)
    {
        try
        {
            var project = await _repository.GetProjectByIdAsync(parameter.ProjectId.Value);
            //先查询项目下的所有图纸Item
            var drawings = await _repository.GetDrawingsByProjectIdAsync(parameter.ProjectId.Value);
            var drawingDtos = await _mapper.ProjectTo<DrawingDto>(drawings).ToListAsync();
            var dtos=new List<ModuleDto>();
            foreach (var drawingDto in drawingDtos)
            {
                //再查询图纸下的所有分段Module
                var modules = await _repository.GetModulesByDrawingIdAsync(drawingDto.Id.Value);
                var moduleDtos = _mapper.ProjectTo<ModuleDto>(modules).ToList();
                //查询时添加图纸的item号，项目ODP号和项目名称
                moduleDtos.ForEach(x =>
                {
                    x.ItemNumber = drawingDto.ItemNumber;
                    x.OdpNumber = project.OdpNumber;
                    x.ProjectName = project.Name;
                });
                dtos.AddRange(moduleDtos);
            }
            //此时Modules中并没有长宽高的信息，这里遍历一下，然后加上去
            foreach (var moduleDto in dtos)
            {
                var moduleData = await _dataRepository.GetModuleDataByIdAsync(moduleDto.Id.Value);
                if(moduleData == null) continue; 
                moduleDto.Length = moduleData.Length;
                moduleDto.Width = moduleData.Width;
                moduleDto.Height = moduleData.Height;
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
    public async Task<ApiResponse<List<ProjectDto>>> GetAllFilterAsync(string? search)
    {
        try
        {
            var models = await _repository.GetProjectsAsync();
            //筛选结果，按照发货时间倒序排序
            var filterModels = models.Where(x =>
                string.IsNullOrWhiteSpace(search) || x.OdpNumber.Contains(search) || x.Name.Contains(search)).OrderByDescending(x => x.DeliveryDate);
            var dtos = await _mapper.ProjectTo<ProjectDto>(filterModels).ToListAsync();
            return new ApiResponse<List<ProjectDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<ProjectDto>> { Status = false, Message = e.Message };
        }
    }
    #endregion


}