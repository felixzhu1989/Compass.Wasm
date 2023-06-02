using Compass.Wpf.ApiServices.Plans;
using Compass.Wpf.ApiServices.Todos;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ViewModels;

public class IndexViewModel : NavigationViewModel
{
    #region ctor-首页
    private readonly ITodoService _todoService;
    private readonly IMemoService _memoService;
    private readonly IProjectService _projectService;
    private readonly IMainPlanService _mainPlanService;
    public IndexViewModel(IContainerProvider provider) : base(provider)
    {
        _todoService = provider.Resolve<ITodoService>();
        _memoService = provider.Resolve<IMemoService>();
        _projectService= provider.Resolve<IProjectService>();
        _mainPlanService=provider.Resolve<IMainPlanService>();

        StatusTaskBars = new ObservableCollection<TaskBar>();
        TodoTaskBars = new ObservableCollection<TaskBar>();

        CreateTaskBars();
        ExecuteCommand=new DelegateCommand<string>(Execute);
        EditTodoCommand=new DelegateCommand<TodoDto>(AddTodo);
        EditMemoCommand=new DelegateCommand<MemoDto>(AddMemo);
        ToDoCompletedCommand=new DelegateCommand<TodoDto>(Completed);
        NavigateCommand=new DelegateCommand<TaskBar>(Navigate);
    }
    public DelegateCommand<string> ExecuteCommand { get; }
    public DelegateCommand<TodoDto> EditTodoCommand { get; }
    public DelegateCommand<MemoDto> EditMemoCommand { get; }
    public DelegateCommand<TodoDto> ToDoCompletedCommand { get; }
    //首页的四个方块选项卡点击后做相应的跳转。
    public DelegateCommand<TaskBar> NavigateCommand { get; }
    #endregion

    #region 属性
    private ObservableCollection<TaskBar> statusTaskBars = null!;
    public ObservableCollection<TaskBar> StatusTaskBars
    {
        get => statusTaskBars;
        set { statusTaskBars = value; RaisePropertyChanged(); }
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

    private MainPlanCountDto planCount = null!;
    public MainPlanCountDto PlanCount
    {
        get => planCount;
        set { planCount = value; RaisePropertyChanged(); }
    }

    private string welcomeText = null!;
    public string WelcomeText
    {
        get => welcomeText;
        set { welcomeText = value; RaisePropertyChanged(); }
    }
    #endregion

    #region 主页图标块
    /// <summary>
    /// 创建首页图标块
    /// </summary>
    void CreateTaskBars()
    {
        //计划状态图标
        StatusTaskBars.Add(new TaskBar { Icon = "WrenchClock", Title = MainPlanStatus_e.计划.ToString(), Target = "MainPlansView" });
        StatusTaskBars.Add(new TaskBar { Icon = "DesktopClassic", Title = MainPlanStatus_e.制图.ToString(), Target = "ProjectsView" });
        StatusTaskBars.Add(new TaskBar { Icon = "Cogs", Title = MainPlanStatus_e.生产.ToString(), Target = "ProjectsView" });
        StatusTaskBars.Add(new TaskBar { Icon = "PackageVariant", Title = MainPlanStatus_e.入库.ToString(), Target = "ProjectsView" });
        StatusTaskBars.Add(new TaskBar { Icon = "TruckCargoContainer", Title = MainPlanStatus_e.发货.ToString(), Target = "ProjectsView" });
        StatusTaskBars.Add(new TaskBar { Icon = "TextBoxCheckOutline", Title = MainPlanStatus_e.结束.ToString(), Target = "ProjectsView" });



        //待办图标
        TodoTaskBars.Add(new TaskBar { Icon = "ClockFast", Title = "待办事项汇总", Color = "DarkSeaGreen", Target = "TodoView" });
        TodoTaskBars.Add(new TaskBar { Icon = "AlarmCheck", Title = "已完成待办汇总", Color = "DarkSeaGreen", Target = "TodoView" });
        TodoTaskBars.Add(new TaskBar { Icon = "ChartLine", Title = "待办完成率", Color = "DarkSeaGreen", Target = "TodoView" });
        TodoTaskBars.Add(new TaskBar { Icon = "PlaylistStar", Title = "备忘录汇总", Color = "DarkSeaGreen", Target = "MemoView" });
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
                break;
            case "制图":
                param.Add("Value", 1);
                break;
            case "生产":
                break;
            case "入库":
                break;
            case "发货":
                break;
            case "结束":
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
        var planCountResult = await _mainPlanService.GetCountAsync();
        if (planCountResult.Status)
        {
            PlanCount = planCountResult.Result;
            StatusTaskBars[0].Content=PlanCount.PlanCount.ToString();
            StatusTaskBars[1].Content=PlanCount.DrawingCount.ToString();
            StatusTaskBars[2].Content=PlanCount.ProductionCount.ToString();
            StatusTaskBars[3].Content=PlanCount.WarehousingCount.ToString();
            StatusTaskBars[4].Content=PlanCount.ShippingCount.ToString();
            StatusTaskBars[5].Content=$"{PlanCount.CompletedCount} / {PlanCount.Sum}";
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