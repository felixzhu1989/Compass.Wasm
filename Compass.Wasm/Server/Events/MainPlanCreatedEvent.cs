using Compass.Wasm.Server.Services.Plans;
using Compass.Wasm.Server.Services.Projects;
using Compass.Wasm.Shared.Extensions;

namespace Compass.Wasm.Server.Events;

public record MainPlanCreatedEvent(Guid Id, string Name);

//处理集成事件
[EventName("PlanService.MainPlan.Created")]
public class MainPlanCreatedEventHandler : JsonIntegrationEventHandler<MainPlanCreatedEvent>
{
    private readonly IProjectService _projectService;
    private readonly IMainPlanService _mainPlanService;
    public MainPlanCreatedEventHandler(IProjectService projectService,IMainPlanService mainPlanService)
    {
        _projectService = projectService;
        _mainPlanService = mainPlanService;
    }
    public override async Task HandleJson(string eventName, MainPlanCreatedEvent? eventData)
    {
        //Delay2s
        await Task.Delay(2000);
        //匹配项目名称，相似度最高且大于0.5的关联起来
        var projectDtos = (await _projectService.GetAllAsync()).Result;
        var results = projectDtos.Select(project => project.Name.SimilarityWith(eventData.Name)).ToList();
        var maxResult = results.Max();//取最大值
        if (maxResult > 0.5)
        {
            //获取最大值的位置
            var index = results.FindIndex(x => x.Equals(maxResult));
            var result = await _mainPlanService.GetSingleAsync(eventData.Id);
            if (result.Status)
            {
                var dto = result.Result;
                dto.ProjectId = projectDtos[index].Id;
               await _mainPlanService.UpdateStatusesAsync(eventData.Id, dto);
            }
        }
    }
}