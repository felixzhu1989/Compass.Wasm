﻿using Compass.Wasm.Shared.ProjectService;
using Zack.DomainCommons.Models;

namespace Compass.ProjectService.Domain.Entities;
/// <summary>
/// 记录项目进度跟踪信息，实际发生的时间
/// </summary>
public record Tracking : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    //状态表示当前，根据EventBus接收到事件，自动修改
    public ProjectStatus ProjectStatus { get;private set; }//计划,制图,生产,入库,结束
    
    public bool ProblemNotResolved { get;private set; }//有没有待解决得问题，如果有则另起一行显示异常详细信息

    //对象初始化时-> 项目进入计划状态（产生该跟踪记录时必填）
    //实际发生时间，进度相关参数
    public DateTime? DrawingPlanedTime { get; private set; }//制定制图计划的时间->进入制图状态
    public DateTime? ModuleReleaseTime { get; private set; }//第一台发出生产图纸的时间->进入生产状态
    public DateTime? WarehousingTime { get; private set; }//第一台生产完工入库的时间->进入库存状态
    public DateTime? ShippingTime { get; private set; }//项目第一台真实发货的时间->进入发货状态，用减去WarehousingTime，用户计算成品库存时间
    public DateTime? ClosedTime { get; private set; }//所有产品都发货了的时间->进入结束状态（是否需要售后状态？）


    private Tracking() { }
    public Tracking(Guid id)//使用Project的Id作为Tracking的Id
    {
        Id = id;
        ProjectStatus = ProjectStatus.计划;//初始状态是计划状态
    }
    public Tracking ChangeProjectStatus(ProjectStatus projectStatus)
    {
        ProjectStatus = projectStatus;
        return this;
    }
    public Tracking ChangeProblemNotResolved(bool problemNotResolved)
    {
        ProblemNotResolved = problemNotResolved;
        return this;
    }
    public Tracking ChangeDrawingPlanedTime(DateTime drawingPlanedTime)
    {
        DrawingPlanedTime = drawingPlanedTime;
        return this;
    }
    public Tracking ChangeModuleReleaseTime(DateTime moduleReleaseTime)
    {
        ModuleReleaseTime = moduleReleaseTime;
        return this;
    }
    public Tracking ChangeWarehousingTime(DateTime warehousingTime)
    {
        WarehousingTime = warehousingTime;
        return this;
    }
    public Tracking ChangeShippingTime(DateTime shippingTime)
    {
        ShippingTime = shippingTime;
        return this;
    }
    public Tracking ChangeClosedTime(DateTime closedTime)
    {
        ClosedTime = closedTime;
        return this;
    }
}