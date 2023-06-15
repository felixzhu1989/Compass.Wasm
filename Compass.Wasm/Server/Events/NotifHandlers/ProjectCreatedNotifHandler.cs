using AutoMapper;
using Compass.PlanService.Infrastructure;
using Compass.Wasm.Shared.Extensions;
using Compass.Wasm.Shared.Plans;
using Compass.Wasm.Shared.Projects.Notifs;

namespace Compass.Wasm.Server.Events.NotifHandlers;

//处理Project发出的领域事件，创建新订单后
public class ProjectCreatedNotifHandler : NotificationHandler<ProjectCreatedNotif>
{
    private readonly PlanDbContext _planDbContext;
    private readonly IMapper _mapper;
    public ProjectCreatedNotifHandler(PlanDbContext planDbContext,IMapper mapper)
    {
        _planDbContext = planDbContext;
        _mapper = mapper;
    }
    protected override void Handle(ProjectCreatedNotif notification)
    {
        //Todo:搜索未绑定的生产计划，匹配项目名称，相似度最高且大于0.5的关联起来
        var mainPlans = _planDbContext.MainPlans.Where(x => x.ProjectId == null).ToArray();
        var results = mainPlans.Select(mainPlan => mainPlan.Name.SimilarityWith(notification.Name)).ToList();
        if (results.Count > 0)
        {
            var maxResult = results.Max();//取最大值
            if (maxResult > 0.5)
            {
                //获取最大值的位置
                var index = results.FindIndex(x => x.Equals(maxResult));
                var model = mainPlans[index];
                var dto = _mapper.Map<MainPlanDto>(model);
                dto.ProjectId = notification.Id;
                model.UpdateStatuses(dto);
                _planDbContext.SaveChangesAsync();
            }
        }
    }
}