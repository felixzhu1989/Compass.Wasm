using AutoMapper;
using Compass.Wasm.Client.ProjectService;
using Compass.Wasm.Shared.ProjectService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers.ProjectService
{
    [Route("api/[controller]")]
    [ApiController]
    [UnitOfWork(typeof(PMDbContext))]
    //[Authorize(Roles = "admin,pm")]
    public class DrawingPlanController : ControllerBase
    {
        private readonly PMDomainService _domainService;
        private readonly PMDbContext _dbContext;
        private readonly IPMRepository _repository;
        private readonly IMapper _mapper;

        public DrawingPlanController(PMDomainService domainService, PMDbContext dbContext, IPMRepository repository, IMapper mapper)
        {
            _domainService = domainService;
            _dbContext = dbContext;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("All")]
        public async Task<DrawingPlanResponse[]> FindAll()
        {
            return await _mapper.ProjectTo<DrawingPlanResponse>(await _repository.GetDrawingPlansAsync()).ToArrayAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DrawingPlanResponse?>> FindById([RequiredGuid] Guid id)
        {
            var drawingPlan = await _repository.GetDrawingPlanByIdAsync(id);
            if (drawingPlan == null) return NotFound($"没有Id={id}的DrawingPlan");
            return _mapper.Map<DrawingPlanResponse>(drawingPlan);
        }

        [HttpGet("ProjectId/{projectId}")]
        public async Task<ActionResult<DrawingPlanResponse?>> FindByProjectId([RequiredGuid] Guid projectId)
        {
            var drawingPlan = await _repository.GetDrawingPlanByProjectIdAsync(projectId);
            if (drawingPlan == null) return NotFound($"没有ProjectId={projectId}的DrawingPlan");
            return _mapper.Map<DrawingPlanResponse>(drawingPlan);
        }

        [HttpGet("ProjectsNotPlanned")]
        public async Task<ProjectResponse[]> FindProjectsNotPlanned()
        {
            //使用AutoMapper将Project转换成ProjectResponse（Dto）
            var projects = await _repository.GetProjectsNotDrawingPlannedAsync();
            var response = new List<ProjectResponse>();
            foreach (var project in projects)
            {
                response.Add(new ProjectResponse{Id = project.Id,OdpNumber = project.OdpNumber,Name = project.Name,DeliveryDate = project.DeliveryDate});
            }
            return response.ToArray();

            //return await _mapper.ProjectTo<ProjectResponse>(projects.AsQueryable()).ToArrayAsync();
        }

        [HttpGet("DrawingsNotAssigned/{projectId}")]
        public async Task<DrawingResponse[]> FindDrawingsNotAssigned([RequiredGuid] Guid projectId)
        {
            var drawings = await _repository.GetDrawingsNotAssignedByProjectIdAsync(projectId);
            var response = new List<DrawingResponse>();
            foreach (var drawing in drawings)
            {
                response.Add(new DrawingResponse{Id = drawing.Id,ItemNumber = drawing.ItemNumber,DrawingUrl = drawing.DrawingUrl});
            }
            return response.ToArray();

            //return await _mapper.ProjectTo<DrawingResponse>(await _repository.GetDrawingsNotAssignedByProjectIdAsync(projectId)).ToArrayAsync();
        }





        [HttpPost("Add")]
        public async Task<ActionResult<Guid>> Add(AddDrawingPlanRequest request)
        {
            var drawingPlan = new DrawingPlan(Guid.NewGuid(), request.ProjectId, request.ReleaseTime);
            await _dbContext.DrawingsPlan.AddAsync(drawingPlan);
            return drawingPlan.Id;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update([RequiredGuid] Guid id, DrawingPlanResponse request)
        {
            var drawingPlan = await _repository.GetDrawingPlanByIdAsync(id);
            if (drawingPlan == null) return NotFound($"没有Id={id}的DrawingPlan");
            drawingPlan.ChangeReleaseTime(request.ReleaseTime);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([RequiredGuid] Guid id)
        {
            var drawingPlan = await _repository.GetDrawingPlanByIdAsync(id);
            //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
            if (drawingPlan == null) return NotFound($"没有Id={id}的DrawingPlan");
            drawingPlan.SoftDelete();//软删除
            return Ok();
        }






    }
}
