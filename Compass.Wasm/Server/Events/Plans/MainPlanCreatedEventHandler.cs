﻿using AutoMapper;
using Compass.PlanService.Infrastructure;
using Compass.Wasm.Shared.Extensions;
using Compass.Wasm.Shared.Plans;

namespace Compass.Wasm.Server.Events.Plans;

//处理集成事件
[EventName("PlanService.MainPlan.Created")]
public class MainPlanCreatedEventHandler : JsonIntegrationEventHandler<MainPlanCreatedEvent>
{
    private readonly ProjectDbContext _pmDbContext;
    private readonly PlanDbContext _psDbContext;
    private readonly IMapper _mapper;

    public MainPlanCreatedEventHandler(ProjectDbContext pmDbContext, PlanDbContext psDbContext,IMapper mapper)
    {
        _pmDbContext = pmDbContext;
        _psDbContext = psDbContext;
        _mapper = mapper;
    }
    public override async Task HandleJson(string eventName, MainPlanCreatedEvent? eventData)
    {
        //Delay2s
        await Task.Delay(2000);
        //匹配项目名称，相似度最高且大于0.5的关联起来
        var projects = await _pmDbContext.Projects.ToArrayAsync();
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
            var model = await _psDbContext.MainPlans.SingleOrDefaultAsync(x => x.Id.Equals(eventData.Id));
            if (model != null)
            {
                var dto = _mapper.Map<MainPlanDto>(model);
                dto.ProjectId = projects[index].Id;
                model.UpdateStatuses(dto);
                await _psDbContext.SaveChangesAsync();
            }
        }
    }
}