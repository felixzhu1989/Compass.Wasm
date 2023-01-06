using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Compass.Wasm.Server.ExportExcel;
using Compass.Wasm.Server.ProjectService.ProjectEvent;
using Compass.Wasm.Shared.ProjectService;
using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Controllers.ProjectService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(ProjectDbContext))]
//[Authorize(Roles = "admin,pm,designer")]
public class ProjectController : ControllerBase
{
    private readonly ProjectDomainService _domainService;
    private readonly ProjectDbContext _dbContext;
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly ExportExcelService _export;

    public ProjectController(ProjectDomainService domainService, ProjectDbContext dbContext, IProjectRepository repository, IMapper mapper, IEventBus eventBus, ExportExcelService export)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
        _export = export;
    }
    [HttpGet("All/{page}")]
    public async Task<PaginationResult<List<ProjectResponse>>> FindAll(int page)
    {
        var result = await _repository.GetProjectsAsync(page);
        return new PaginationResult<List<ProjectResponse>>
        {
            Data = await _mapper.ProjectTo<ProjectResponse>(result.Data).ToListAsync(),
            CurrentPage = result.CurrentPage,
            Pages = result.Pages
        };
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectResponse?>> FindById([RequiredGuid] Guid id)
    {
        //返回ValueTask的需要await的一下
        var project = await _repository.GetProjectByIdAsync(id);
        if (project == null) return NotFound($"没有Id={id}的Project");
        return _mapper.Map<ProjectResponse>(project);
    }

    [HttpGet("Odp/{odpNumber}")]
    public async Task<ActionResult<ProjectResponse?>> FindByOdp(string odpNumber)
    {
        var project = await _repository.GetProjectByOdpAsync(odpNumber.ToUpper());
        if (project == null) return NotFound($"没有Id={odpNumber.ToUpper()}的Project");
        return _mapper.Map<ProjectResponse>(project);
    }

    [HttpPost("Add")]
    public async Task<ActionResult<Guid>> Add(AddProjectRequest request)
    {
        var project = new Project(Guid.NewGuid(), request.OdpNumber.ToUpper(), request.Name, request.ReceiveDate, request.DeliveryDate, request.ProjectType, request.RiskLevel, request.SpecialNotes);
        //包括合同地址
        project.ChangeContractUrl(request.ContractUrl!);
        await _dbContext.Projects.AddAsync(project);
        var eventData = new ProjectCreatedEvent(project.Id, project.Name, project.DeliveryDate);
        //发布集成事件
        _eventBus.Publish("ProjectService.Project.Created", eventData);

        return project.Id;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, ProjectResponse request)
    {
        var project = await _repository.GetProjectByIdAsync(id);
        if (project == null) return NotFound($"没有Id={id}的Project");
        //包括合同地址和Bom地址
        project.ChangeOdpNumber(request.OdpNumber!.ToUpper()).ChangeName(request.Name!)
            .ChangeReceiveDate(request.ReceiveDate).ChangeDeliveryDate(request.DeliveryDate)
            .ChangeProjectType(request.ProjectType).ChangeRiskLevel(request.RiskLevel)
            .ChangeContractUrl(request.ContractUrl!).ChangeBomUrl(request.BomUrl!)
            .ChangeFinalInspectionUrl(request.FinalInspectionUrl!)
            .ChangeAttachmentsUrl(request.AttachmentsUrl!)
            .ChangeSpecialNotes(request.SpecialNotes!);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([RequiredGuid] Guid id)
    {
        var project = await _repository.GetProjectByIdAsync(id);
        //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
        if (project == null) return NotFound($"没有Id={id}的Project");
        project.SoftDelete();//软删除
        var eventData = new ProjectDeletedEvent(project.Id);
        //发布集成事件
        _eventBus.Publish("ProjectService.Project.Deleted", eventData);
        return Ok();
    }
    #region 测试导出excel表格
    //https://www.puresourcecode.com/dotnet/blazor/how-to-export-data-to-excel-in-blazor/
    //Install-Package ClosedXML
    /* Also, I recommend to look at ClosedXML.Report repository
     * because it allows you to create very nice report with Excel based on an Excel template.
     *  
     */
    [HttpGet("ExportExcel")]
    public async Task<IActionResult> DownloadProjectExport()
    {
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        string fileName = "projects.xlsx";
        var result = await _repository.GetProjectsAsync(1);
        var projects = await _mapper.ProjectTo<ProjectResponse>(result.Data).ToListAsync();
        return File(_export.CreateProjectExport(projects), contentType, fileName);
    }

    #endregion
}