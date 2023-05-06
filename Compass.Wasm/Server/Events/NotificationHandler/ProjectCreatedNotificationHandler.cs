using Compass.PlanService.Infrastructure;
using Compass.Wasm.Server.Events.Plans;
using Compass.Wasm.Shared.Extensions;
using Compass.Wasm.Shared.Plans;
using Compass.Wasm.Shared.Projects.Notifications;

namespace Compass.Wasm.Server.Events.NotificationHandler;

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
        //Todo:搜索未绑定的生产计划，匹配项目名称，相似度最高且大于0.5的关联起来
        var mainPlans = _psDbContext.MainPlans.Where(x => x.ProjectId == null).ToArray();
        List<float> results = new List<float>();
        foreach (var mainPlan in mainPlans)
        {
            results.Add(mainPlan.Name.SimilarityWith(notification.Name));
        }
        if (results.Count > 0)
        {
            var maxResult = results.Max();//取最大值
            if (maxResult > 0.5)
            {
                //获取最大值的位置
                var index = results.FindIndex(x => x.Equals(maxResult));
                mainPlans[index].ChangeProjectId(notification.Id);
                mainPlans[index].ChangeStatus(MainPlanStatus_e.制图);
                _psDbContext.SaveChangesAsync();
            }
        }
    }
}