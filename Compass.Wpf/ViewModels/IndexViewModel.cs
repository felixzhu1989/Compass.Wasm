using Compass.Wpf.Common;
using Compass.Wpf.Common.Models;
using Compass.Wpf.Extensions;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Compass.Wasm.Shared.Projects;
using Compass.Wasm.Shared.Todos;
using Compass.Wpf.ApiServices.Todos;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ViewModels;

public class IndexViewModel : NavigationViewModel
{
    #region ctor-首页
    private readonly ITodoService _todoService;
    private readonly IMemoService _memoService;
    private readonly IProjectService _projectService;
    public IndexViewModel(IContainerProvider provider) : base(provider)
    {
        _todoService = provider.Resolve<ITodoService>();
        _memoService = provider.Resolve<IMemoService>();
        _projectService= provider.Resolve<IProjectService>();

        ProjectTaskBars = new ObservableCollection<TaskBar>();
        TodoTaskBars = new ObservableCollection<TaskBar>();

        CreateTaskBars();
        ExecuteCommand=new DelegateCommand<string>(Execute);
        EditTodoCommand=new DelegateCommand<TodoDto>(AddTodo);
        EditMemoCommand=new DelegateCommand<MemoDto>(AddMemo);
        ToDoCompletedCommand=new DelegateCommand<TodoDto>(Completed);
        NavigateCommand=new DelegateCommand<TaskBar>(Navigate);
    }
    #endregion

    #region 属性
    private ObservableCollection<TaskBar> projectTaskBars = null!;
    public ObservableCollection<TaskBar> ProjectTaskBars
    {
        get => projectTaskBars;
        set { projectTaskBars = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<TaskBar> todoTaskBars = null!;
    public ObservableCollection<TaskBar> TodoTaskBars
    {
        get => todoTaskBars;
        set { todoTaskBars = value; RaisePropertyChanged(); }
    }
    //ToDoDtos和MemoDtos包含在TodoSummary中
    private TodoSummaryDto todoSummary = null!;
    public TodoSummaryDto TodoSummary
    {
        get => todoSummary;
        set { todoSummary = value; RaisePropertyChanged(); }
    }

    private ProjectSummaryDto projectSummary = null!;
    public ProjectSummaryDto ProjectSummary
    {
        get => projectSummary;
        set { projectSummary = value; RaisePropertyChanged(); }
    }

    private string welcomeText = null!;
    public string WelcomeText
    {
        get => welcomeText;
        set { welcomeText = value; RaisePropertyChanged(); }
    }
    #endregion

    #region Commands
    public DelegateCommand<string> ExecuteCommand { get; }
    public DelegateCommand<TodoDto> EditTodoCommand { get; }
    public DelegateCommand<MemoDto> EditMemoCommand { get; }
    public DelegateCommand<TodoDto> ToDoCompletedCommand { get; }
    //首页的四个方块选项卡点击后做相应的跳转。
    public DelegateCommand<TaskBar> NavigateCommand { get; }
    #endregion

    #region 主页图标块
    /// <summary>
    /// 创建首页图标块
    /// </summary>
    void CreateTaskBars()
    {
        TodoTaskBars.Add(new TaskBar { Icon = "ClockFast", Title = "待办事项汇总", Color = "#FF0CA0FF", Target = "TodoView" });
        TodoTaskBars.Add(new TaskBar { Icon = "AlarmCheck", Title = "已完成待办汇总", Color = "#FF1ECA3A", Target = "TodoView" });
        TodoTaskBars.Add(new TaskBar { Icon = "ChartLine", Title = "待办完成率", Color = "#FF02C6DC", Target = "TodoView" });
        TodoTaskBars.Add(new TaskBar { Icon = "PlaylistStar", Title = "备忘录汇总", Color = "#FFFFA000", Target = "MemoView" });

        ProjectTaskBars.Add(new TaskBar { Icon = "WrenchClock", Title = ProjectStatus_e.计划.ToString(), Target = "ProjectsView" });
        ProjectTaskBars.Add(new TaskBar { Icon = "DesktopClassic", Title = ProjectStatus_e.制图.ToString(), Target = "ProjectsView" });
        ProjectTaskBars.Add(new TaskBar { Icon = "Cogs", Title = ProjectStatus_e.生产.ToString(), Target = "ProjectsView" });
        ProjectTaskBars.Add(new TaskBar { Icon = "PackageVariant", Title = ProjectStatus_e.入库.ToString(), Target = "ProjectsView" });
        ProjectTaskBars.Add(new TaskBar { Icon = "TruckCargoContainer", Title = ProjectStatus_e.发货.ToString(), Target = "ProjectsView" });
        ProjectTaskBars.Add(new TaskBar { Icon = "TextBoxCheckOutline", Title = ProjectStatus_e.结束.ToString(), Target = "ProjectsView" });
    }

    /// <summary>
    /// 图标块点击后导航
    /// </summary>
    /// <param name="obj"></param>
    private void Navigate(TaskBar obj)
    {
        if (string.IsNullOrEmpty(obj.Target)) return;
        NavigationParameters param = new NavigationParameters();
        switch (obj.Title)
        {
            case "已完成待办汇总":
                //添加跳转时传递的参数
                param.Add("Value", 2);
                break;
            case "待办完成率":
                param.Add("Value", 1);
                break;

            //计划,制图,生产,入库,发货,结束
            case "计划":
                param.Add("Value", 1);
                break;
            case "制图":
                param.Add("Value", 2);
                break;
            case "生产":
                param.Add("Value", 3);
                break;
            case "入库":
                param.Add("Value", 4);
                break;
            case "发货":
                param.Add("Value", 5);
                break;
            case "结束":
                param.Add("Value", 6);
                break;
        }
        RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.Target, back =>
        {
            Journal = back.Context.NavigationService.Journal;
        }, param);
    }
    #endregion

    #region 待办与备忘
    /// <summary>
    /// 添加待办或备忘
    /// </summary>
    /// <param name="obj"></param>
    private void Execute(string obj)
    {
        switch (obj)
        {
            case "AddTodo":
                AddTodo(null);
                break;
            case "AddMemo":
                AddMemo(null);
                break;
        }
    }

    /// <summary>
    /// 添加代办事项
    /// </summary>
    async void AddTodo(TodoDto? model)
    {
        try
        {
            UpdateLoading(true);
            DialogParameters param = new DialogParameters();
            if (model!=null) param.Add("Value", model);
            var dialogResult = await DialogHost.ShowDialog("AddTodoView", param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var dto = dialogResult.Parameters.GetValue<TodoDto>("Value");
                if (dto.Id != null && dto.Id != Guid.Empty)
                {
                    //修改？
                    var updateResult = await _todoService.UpdateAsync(dto.Id.Value, dto);
                    if (updateResult.Status)
                    {
                        var viewDto = TodoSummary.TodoDtos.FirstOrDefault(t => t.Id.Equals(dto.Id));
                        if (viewDto!=null)
                        {
                            viewDto.Title=dto.Title;
                            viewDto.Content=dto.Content;
                            viewDto.Status = dto.Status;
                        }
                    }
                }
                else
                {
                    //新增
                    var addResult = await _todoService.UserAddAsync(dto);
                    if (addResult.Status)
                    {
                        TodoSummary.TodoDtos.Add(addResult.Result);//界面显示
                                                                   //更新代办事项汇总选项卡
                        TodoSummary.Sum+=1;
                        TodoSummary.CompletedRatio=(TodoSummary.CompletedCount/(double)TodoSummary.Sum).ToString("0%");
                        Refresh();
                    }
                }
            }
        }
        finally
        {
            UpdateLoading(false);
        }
    }

    /// <summary>
    /// 添加备忘录
    /// </summary>
    async void AddMemo(MemoDto? model)
    {
        try
        {

            UpdateLoading(true);
            DialogParameters param = new DialogParameters();
            if (model != null) param.Add("Value", model);
            var dialogResult = await DialogHost.ShowDialog("AddMemoView", param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var dto = dialogResult.Parameters.GetValue<MemoDto>("Value");
                if (dto.Id != null && dto.Id != Guid.Empty)
                {
                    //修改？
                    var updateResult = await _memoService.UpdateAsync(dto.Id.Value, dto);
                    if (updateResult.Status)
                    {
                        var viewDto = TodoSummary.MemoDtos.FirstOrDefault(t => t.Id.Equals(dto.Id.Value));
                        if (viewDto != null)
                        {
                            viewDto.Title = dto.Title;
                            viewDto.Content = dto.Content;
                        }
                    }
                }
                else
                {
                    //新增
                    var addResult = await _memoService.UserAddAsync(dto);
                    if (addResult.Status)
                    {
                        TodoSummary.MemoDtos.Add(addResult.Result);
                        //更新备忘汇总选项卡
                        TodoSummary.MemoCount += 1;
                        Refresh();
                    }
                }
            }
        }
        finally
        {
            UpdateLoading(false);
        }
    }

    /// <summary>
    /// 完成待办事项
    /// </summary>
    /// <param name="obj"></param>
    private async void Completed(TodoDto obj)
    {
        try
        {
            UpdateLoading(true);
            var updateResult = await _todoService.UpdateAsync(obj.Id.Value, obj);//修改数据
            if (updateResult.Status)
            {
                var todo = TodoSummary.TodoDtos.FirstOrDefault(t => t.Id.Equals(obj.Id));
                if (todo!=null)
                {
                    TodoSummary.TodoDtos.Remove(todo);//从界面列表删除
                    //更新完成率汇总选项卡
                    TodoSummary.CompletedCount++;
                    TodoSummary.CompletedRatio=(TodoSummary.CompletedCount/(double)TodoSummary.Sum).ToString("0%");
                    Refresh();
                }
                //发送全局通知，显示在Snackbar上
                Aggregator.SendMessage("待办事项已完成！");
            }
        }
        finally
        {
            UpdateLoading(false);
        }
    }

    /// <summary>
    /// 添加和完成待办后更新界面显示
    /// </summary>
    void Refresh()
    {
        TodoTaskBars[0].Content=TodoSummary.Sum.ToString();
        TodoTaskBars[1].Content=TodoSummary.CompletedCount.ToString();
        TodoTaskBars[2].Content=TodoSummary.CompletedRatio;
        TodoTaskBars[3].Content=TodoSummary.MemoCount.ToString();
    }
    #endregion

    #region 初始化信息和导航
    private async void InitToDoSummary()
    {
        var todoSummaryResult = await _todoService.GetSummaryAsync();
        if (todoSummaryResult.Status)
        {
            TodoSummary=todoSummaryResult.Result;
            Refresh();
        }
    }

    private async void InitProjectSummary()
    {
        var projectSummaryResult = await _projectService.GetSummaryAsync();
        if (projectSummaryResult.Status)
        {
            ProjectSummary = projectSummaryResult.Result;
            ProjectTaskBars[0].Content=ProjectSummary.PlanCount.ToString();
            ProjectTaskBars[1].Content=ProjectSummary.DrawingCount.ToString();
            ProjectTaskBars[2].Content=ProjectSummary.ProductionCount.ToString();
            ProjectTaskBars[3].Content=ProjectSummary.WarehousingCount.ToString();
            ProjectTaskBars[4].Content=ProjectSummary.ShippingCount.ToString();
            ProjectTaskBars[5].Content=$"{ProjectSummary.CompletedCount} / {ProjectSummary.Sum}";
        }
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        WelcomeText=$"您好，{AppSession.UserName}! 今天是{DateTime.Now.GetDateTimeFormats('D')[1]}。";

        InitToDoSummary();

        InitProjectSummary();

        base.OnNavigatedTo(navigationContext);
    }
    #endregion


}