﻿using AutoMapper;
using Compass.PlanService.Infrastructure;
using Compass.Wasm.Shared.Extensions;
using Compass.Wasm.Shared.Plans;
using Compass.Wasm.Shared.Projects.Notifs;

namespace Compass.Wasm.Server.Events.NotifHandlers;

//处理Project发出的领域事件，创建新订单后，继续创建跟踪对象
public class ProjectCreatedNotifHandler : NotificationHandler<ProjectCreatedNotif>
{
    private readonly ProjectDbContext _dbContext;
    private readonly PlanDbContext _planDbContext;
    private readonly IEventBus _eventBus;
    private readonly IMapper _mapper;

    public ProjectCreatedNotifHandler(ProjectDbContext dbContext, PlanDbContext planDbContext, IEventBus eventBus,IMapper mapper)
    {
        _dbContext = dbContext;
        _planDbContext = planDbContext;
        _eventBus = eventBus;
        _mapper = mapper;
    }
    protected override void Handle(ProjectCreatedNotif notification)
    {
        //Todo:搜索未绑定的生产计划，匹配项目名称，相似度最高且大于0.5的关联起来
        var mainPlans = _planDbContext.MainPlans.Where(x => x.ProjectId == null).ToArray();
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
                var model = mainPlans[index];
                var dto = _mapper.Map<MainPlanDto>(model);
                dto.ProjectId = notification.Id;
                model.UpdateStatuses(dto);
                _planDbContext.SaveChangesAsync();
            }
        }
    }
}