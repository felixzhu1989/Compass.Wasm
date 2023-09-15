using System.ComponentModel.DataAnnotations;
using Compass.Dtos;
using Compass.Wasm.Server.Services.Projects;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Params;
using Compass.Wasm.Shared.Projects;

namespace Compass.Wasm.Server.Controllers.Projects;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(ProjectDbContext))]
//[Authorize(Roles = "admin,pm,designer")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _service;

    public ProjectController(IProjectService service)
    {
        _service = service;
    }

    #region 标准增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<ProjectDto>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<ProjectDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<ProjectDto>> Add(ProjectDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<ProjectDto>> Update([RequiredGuid] Guid id, ProjectDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<ProjectDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);
    #endregion

    #region 扩展的查询功能,WPF
    /// <summary>
    /// 根据查询条件筛选结果
    /// </summary>
    [HttpGet("Filter")]
    public async Task<ApiResponse<List<ProjectDto>?>> GetAllFilter(ProjectParam param) => await _service.GetAllFilterAsync(param);

    /// <summary>
    /// 查询单个项目的图纸和分段的树结构
    /// </summary>
    [HttpGet("ModuleTree")]
    public async Task<ApiResponse<List<DrawingDto>>> GetModuleTree(ProjectParam param) => await _service.GetModuleTreeAsync(param);

    /// <summary>
    /// 查询单个项目的所有分段，用于自动作图和JobCard
    /// </summary>
    [HttpGet("ModuleList")]
    public async Task<ApiResponse<List<ModuleDto>>> GetModuleList(ProjectParam param) => await _service.GetModuleListAsync(param);

    #endregion


    #region 扩展的查询功能,Blazor
    


    //UploadFiles上传文件
    [HttpPut("UploadFiles/{id}")]
    public async Task<ApiResponse<ProjectDto>> UploadFiles([RequiredGuid] Guid id, ProjectDto dto) => await _service.UploadFilesAsync(id, dto);
    #endregion


    //{
    //    var result = await _repository.GetProjectsAsync(page);
    //    return new PaginationResult<List<ProjectDto>>
    //    {
    //        Data = await _mapper.ProjectTo<ProjectDto>(result.Data).ToListAsync(),
    //        CurrentPage = result.CurrentPage,
    //        Pages = result.Pages
    //    };
    //}



    //[HttpGet("Odp/{odpNumber}")]
    //public async Task<ActionResult<ProjectDto?>> FindByOdp(string odpNumber)
    //{
    //    var project = await _repository.GetProjectByOdpAsync(odpNumber.ToUpper());
    //    if (project == null) return NotFound($"没有Id={odpNumber.ToUpper()}的Project");
    //    return _mapper.Map<ProjectDto>(project);
    //}


    #region 测试导出excel表格
    //https://www.puresourcecode.com/dotnet/blazor/how-to-export-data-to-excel-in-blazor/
    //Install-Package ClosedXML
    /* Also, I recommend to look at ClosedXML.Report repository
     * because it allows you to create very nice report with Excel based on an Excel template.
     *  
     */
    //[HttpGet("ExportExcel")]
    //public async Task<IActionResult> DownloadProjectExport()
    //{
    //    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    //    string fileName = "projects.xlsx";
    //    var result = await _repository.GetProjectsAsync(1);
    //    var projects = await _mapper.ProjectTo<ProjectDto>(result.Data).ToListAsync();
    //    return File(_export.CreateProjectExport(projects), contentType, fileName);
    //}

    #endregion




}