using Compass.PlanService.Infrastructure;
using Compass.Wasm.Server.PlanService.ProductionPlanEvent;
using Compass.Wasm.Shared.Extensions;
using Compass.Wasm.Shared.ProjectService.Notification;

namespace Compass.Wasm.Server.ProjectService;

//处理Project发出的领域事件，创建新订单后，继续创建跟踪对象
public class ProjectCreatedNotificationHandler : NotificationHandler<ProjectCreatedNotification>
{
    private readonly ProjectDbContext _dbContext;
    private readonly PlanDbContext _psDbContext;
    private readonly IEventBus _eventBus;

    public ProjectCreatedNotificationHandler(ProjectDbContext dbContext, PlanDbContext psDbContext, IEventBus eventBus)
    {
        _dbContext = dbContext;
        _psDbContext = psDbContext;
        _eventBus = eventBus;
    }
    protected override void Handle(ProjectCreatedNotification notification)
    {
        //当项目创建时，同时创建跟踪对象，如果跟踪对象存在那么不执行
        if (!_dbContext.Trackings.Any(x => x.Id.Equals(notification.Id)))
        {
            var tracking = new Tracking(notification.Id, notification.SortDate);
            _dbContext.Trackings.Add(tracking);
        }

        //Todo:搜索未绑定的生产计划，匹配项目名称，相似度最高且大于0.5的关联起来
        var prodPlans = _psDbContext.ProductionPlans.Where(x => x.ProjectId == null).ToArray();
        List<float> results = new List<float>();
        foreach (var prodPlan in prodPlans)
        {
            results.Add(prodPlan.Name.SimilarityWith(notification.Name));
        }
        if (results.Count > 0)
        {
            var maxResult = results.Max();//取最大值
            if (maxResult > 0.5)
            {
                //获取最大值的位置
                var index = results.FindIndex(x => x.Equals(maxResult));
                prodPlans[index].ChangeProjectId(notification.Id);
                _psDbContext.SaveChangesAsync();
                //todo:发出集成事件，修改Tracking的排序时间为ProductionFinishTime
                var eData = new BindProjectEvent(notification.Id, prodPlans[index].ProductionFinishTime);
                _eventBus.Publish("PlanService.ProductionPlan.BindProject", eData);
            }
        }
    }
}