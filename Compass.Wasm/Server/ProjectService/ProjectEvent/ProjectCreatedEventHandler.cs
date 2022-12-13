using Compass.PlanService.Infrastructure;
using Compass.Wasm.Server.PlanService.ProductionPlanEvent;
using Compass.Wasm.Shared.Extensions;
using Zack.EventBus;

namespace Compass.Wasm.Server.ProjectService.ProjectEvent;

//处理ProjectController发出的集成事件，创建新订单后，继续创建跟踪对象
[EventName("ProjectService.Project.Created")]
public class ProjectCreatedEventHandler : JsonIntegrationEventHandler<ProjectCreatedEvent>
{
    //todo:是否需要发送邮件
    private readonly ProjectDbContext _dbContext;
    private readonly PlanDbContext _psDbContext;
    private readonly IEventBus _eventBus;

    public ProjectCreatedEventHandler(ProjectDbContext dbContext, PlanDbContext psDbContext,IEventBus eventBus)
    {
        _dbContext = dbContext;
        _psDbContext = psDbContext;
        _eventBus = eventBus;
    }
    public override async Task HandleJson(string eventName, ProjectCreatedEvent? eventData)
    {
        //当项目创建时，同时创建跟踪对象
        var tracking = new Tracking(eventData!.Id, eventData.SortDate);
        _dbContext.Trackings.Add(tracking);
        await _dbContext.SaveChangesAsync();
        //搜索未绑定的生产计划，匹配项目名称，相似度最高且大于0.5的关联起来
        var prodPlans = await _psDbContext.ProductionPlans.Where(x => x.ProjectId == null).ToArrayAsync();
        List<float> results = new List<float>();
        foreach (var prodPlan in prodPlans)
        {
            results.Add(prodPlan.Name.SimilarityWith(eventData.Name));
        }
        var maxResult = results.Max();//取最大值
        if (maxResult > 0.5)
        {
            //获取最大值的位置
            var index = results.FindIndex(x => x.Equals(maxResult));
            prodPlans[index].ChangeProjectId(eventData.Id);
            await _psDbContext.SaveChangesAsync();
            //todo:发出集成事件，修改Tracking的排序时间为ProductionFinishTime
            var eData = new BindProjectEvent(eventData.Id, prodPlans[index].ProductionFinishTime);
            _eventBus.Publish("PlanService.ProductionPlan.BindProject", eData);
        }
    }
}