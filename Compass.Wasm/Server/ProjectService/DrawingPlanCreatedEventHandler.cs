﻿using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Server.ProjectService;
//处理DrawingPlanController发出的集成事件，创建制图计划后，将项目跟踪得项目状态修改成制图
[EventName("ProjectService.DrawingPlan.Created")]
public class DrawingPlanCreatedEventHandler : JsonIntegrationEventHandler<DrawingPlanCreatedEvent>
{
    //todo:是否需要发送邮件
    private readonly PMDbContext _dbContext;
    public DrawingPlanCreatedEventHandler(PMDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public override async Task HandleJson(string eventName, DrawingPlanCreatedEvent? eventData)
    {
        var tracking = await _dbContext.Trackings.SingleAsync(x => x.Id.Equals(eventData.ProjectId) );
        tracking.ChangeDrawingPlanedTime(DateTime.Now).ChangeProjectStatus(ProjectStatus.制图);
        await _dbContext.SaveChangesAsync();
    }
}