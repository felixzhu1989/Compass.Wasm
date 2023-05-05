using Compass.PlanService.Infrastructure;
using Compass.Wasm.Shared.Extensions;

namespace Compass.Wasm.Server.Events.Plans;

//处理集成事件
[EventName("PlanService.MainPlan.Created")]
public class MainPlanCreatedEventHandler : JsonIntegrationEventHandler<MainPlanCreatedEvent>
{
    private readonly ProjectDbContext _pmDbContext;
    private readonly PlanDbContext _psDbContext;
    private readonly IEventBus _eventBus;

    public MainPlanCreatedEventHandler(ProjectDbContext pmDbContext, PlanDbContext psDbContext, IEventBus eventBus)
    {
        _pmDbContext = pmDbContext;
        _psDbContext = psDbContext;
        _eventBus = eventBus;
    }
    public override async Task HandleJson(string eventName, MainPlanCreatedEvent? eventData)
    {
        //Delay2s
        await Task.Delay(2000);
        //搜索未绑定的项目，匹配项目名称，相似度最高且大于0.5的关联起来
        //查找已经绑定的Id
        var ids = await _psDbContext.MainPlans.Where(x => x.ProjectId != null).Select(x => x.ProjectId).ToListAsync();
        //排除已经绑定的Id就是未绑定的项目
        var projects = await _pmDbContext.Projects.Where(x => !ids.Contains(x.Id)).ToArrayAsync();
        List<float> results = new List<float>();
        foreach (var project in projects)
        {
            results.Add(project.Name.SimilarityWith(eventData.Name));
        }
        var maxResult = results.Max();//取最大值
        if (maxResult > 0.5)
        {
            //获取最大值的位置
            var index = results.FindIndex(x => x.Equals(maxResult));
            var prodPlan = await _psDbContext.MainPlans.SingleOrDefaultAsync(x => x.Id.Equals(eventData.Id));
            if (prodPlan != null)
            {
                prodPlan.ChangeProjectId(projects[index].Id);
                await _psDbContext.SaveChangesAsync();
                //todo:发出集成事件，修改Tracking的排序时间为ProductionFinishTime
                var eData = new BindProjectEvent(projects[index].Id, prodPlan.FinishTime);
                _eventBus.Publish("PlanService.MainPlan.BindProject", eData);
            }
        }
    }
}