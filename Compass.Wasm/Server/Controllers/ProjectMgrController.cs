using Compass.ProjectService.Domain;
using Compass.ProjectService.Domain.Entities;
using Compass.ProjectService.Infrastructure;
using Compass.Wasm.Client.ProjectService;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UnitOfWork(typeof(PMDbContext))]
    //[Authorize(Roles = "admin,pm,designer")]
    public class ProjectMgrController : ControllerBase
    {
        private readonly PMDomainService _domainService;
        private readonly PMDbContext _dbContext;
        private readonly IPMRepository _repository;
        private readonly IMapper _mapper;

        public ProjectMgrController(PMDomainService domainService,PMDbContext dbContext,IPMRepository repository,IMapper mapper)
        {
            _domainService = domainService;
            _dbContext = dbContext;
            _repository = repository;
            _mapper = mapper;
        }

        #region Project
        [HttpGet("AllProject")]
        public async Task<ProjectResponse[]> FindAllProject()
        {
            //使用AutoMapper将Project转换成ProjectResponse（Dto）
            return await _mapper.ProjectTo<ProjectResponse>( await _repository.GetProjectsAsync()).ToArrayAsync();
        }

        [HttpGet("Project/{id}")]
        public async Task<ActionResult<ProjectResponse?>> FindProjectById([RequiredGuid] Guid id)
        {
            //返回ValueTask的需要await的一下
            var project = await _repository.GetProjectByIdAsync(id);
            if (project == null) return NotFound($"没有Id={id}的Project");
            return _mapper.Map<ProjectResponse>(project);
        }

        [HttpGet("Project/Odp/{odpNumber}")]
        public async Task<ActionResult<ProjectResponse?>> FindProjectByOdp(string odpNumber)
        {
            var project = await _repository.GetProjectByOdpAsync(odpNumber.ToUpper());
            if (project == null) return NotFound($"没有Id={odpNumber.ToUpper()}的Project");
            return _mapper.Map<ProjectResponse>(project);
        }

        [HttpPost("AddProject")]
        public async Task<ActionResult<Guid>> AddProject(AddProjectRequest request)
        {
            var project = new Project(Guid.NewGuid(), request.OdpNumber.ToUpper(), request.Name, request.ProjectType, request.RiskLevel, request.SpecialNotes);
            //包括合同地址
            project.ChangeContractUrl(request.ContractUrl);
            await _dbContext.Projects.AddAsync(project);
            return project.Id;
        }

        [HttpPut("Project/{id}")]
        public async Task<ActionResult> UpdateProject([RequiredGuid] Guid id, ProjectResponse request)
        {
            var project = await _repository.GetProjectByIdAsync(id);
            if (project == null) return NotFound($"没有Id={id}的Project");
            //包括合同地址和Bom地址
            project.ChangeOdpNumber(request.OdpNumber).ChangeName(request.Name)
                .ChangeProjectType(request.ProjectType).ChangeRiskLevel(request.RiskLevel)
                .ChangeContractUrl(request.ContractUrl).ChangeBomUrl(request.BomUrl)
                .ChangeSpecialNotes(request.SpecialNotes);
            return Ok();
        }

        [HttpDelete("Project/{id}")]
        public async Task<ActionResult> DeleteProject([RequiredGuid] Guid id)
        {
            var project = await _repository.GetProjectByIdAsync(id);
            //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
            if (project == null) return NotFound($"没有Id={id}的Project");
            project.SoftDelete();//软删除
            return Ok();
        }
        
        #endregion


        #region Drawing
        [HttpGet("AllDrawing/{projectId}")]
        public async Task<DrawingResponse[]> FindAllDrawing([RequiredGuid] Guid projectId)
        {
            return await _mapper.ProjectTo<DrawingResponse>(await _repository.GetDrawingsByProjectIdAsync(projectId)).ToArrayAsync();
        }
        [HttpGet("Drawing/{id}")]
        public async Task<ActionResult<DrawingResponse?>> FindDrawingById([RequiredGuid] Guid id)
        {
            var drawing = await _repository.GetDrawingByIdAsync(id);
            if (drawing == null) return NotFound($"没有Id={id}的Project");
            return _mapper.Map<DrawingResponse>(drawing);
        }


        #endregion



    }
}
