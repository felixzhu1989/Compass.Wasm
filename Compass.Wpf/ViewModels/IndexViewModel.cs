using System;
using Compass.Wasm.Shared.TodoService;
using Compass.Wpf.Common;
using Compass.Wpf.Common.Models;
using Prism.Commands;
using Prism.Ioc;
using System.Collections.ObjectModel;
using System.Linq;
using Compass.Wpf.Service;
using Prism.Services.Dialogs;
using Prism.Regions;
using Compass.Wpf.Extensions;
using ImTools;
using Prism.Events;

namespace Compass.Wpf.ViewModels;

public class IndexViewModel : NavigationViewModel
{
    private readonly IEventAggregator _aggregator;
    private readonly IDialogHostService _dialog;
    private readonly ITodoService _todoService;
    private readonly IMemoService _memoService;
    private readonly IRegionManager _regionManager;

    private ObservableCollection<TaskBar> taskBars;
    public ObservableCollection<TaskBar> TaskBars
    {
        get => taskBars;
        set { taskBars = value; RaisePropertyChanged(); }
    }
    //TodoDtos和MemoDtos包含在Summary中
    //private ObservableCollection<TodoDto> todoDtos;
    //public ObservableCollection<TodoDto> TodoDtos
    //{
    //    get => todoDtos;
    //    set { todoDtos = value; RaisePropertyChanged(); }
    //}
    //private ObservableCollection<MemoDto> memoDtos;
    //public ObservableCollection<MemoDto> MemoDtos
    //{
    //    get => memoDtos;
    //    set { memoDtos = value; RaisePropertyChanged(); }
    //}
    private TodoSummaryDto summary;
    public TodoSummaryDto Summary
    {
        get => summary;
        set { summary = value; RaisePropertyChanged(); }
    }
    private string title;

    public string Title
    {
        get => title;
        set { title = value; RaisePropertyChanged(); }
    }


    public DelegateCommand<string> ExecuteCommand { get; }
    public DelegateCommand<TodoDto> EditTodoCommand { get; }
    public DelegateCommand<MemoDto> EditMemoCommand { get; }
    public DelegateCommand<TodoDto> ToDoCompletedCommand { get; }
    //首页的四个方块选项卡点击后做相应的跳转。
    public DelegateCommand<TaskBar> NavigateCommand { get; }

    public IndexViewModel(IEventAggregator aggregator, IDialogHostService dialog, IContainerProvider provider) : base(provider)
    {
        _aggregator = aggregator;
        _dialog = dialog;
        _todoService = provider.Resolve<ITodoService>();
        _memoService = provider.Resolve<IMemoService>();
        _regionManager=provider.Resolve<IRegionManager>();
        TaskBars = new ObservableCollection<TaskBar>();
        //TodoDtos = new ObservableCollection<TodoDto>();
        //MemoDtos = new ObservableCollection<MemoDto>();
        CreateTaskBars();
        //CreateTestData();
        ExecuteCommand=new DelegateCommand<string>(Execute);
        EditTodoCommand=new DelegateCommand<TodoDto>(AddTodo);
        EditMemoCommand=new DelegateCommand<MemoDto>(AddMemo);
        ToDoCompletedCommand=new DelegateCommand<TodoDto>(Completed);
        NavigateCommand=new DelegateCommand<TaskBar>(Navigate);

        Title=$"您好，Admin!今天是{DateTime.Now.GetDateTimeFormats('D')[1]}。";
    }
    void CreateTaskBars()
    {
        TaskBars.Add(new TaskBar { Icon = "ClockFast", Title = "待办事项汇总", Color = "#FF0CA0FF", Target = "TodoView" });
        TaskBars.Add(new TaskBar { Icon = "AlarmCheck", Title = "已完成待办汇总", Color = "#FF1ECA3A", Target = "TodoView" });
        TaskBars.Add(new TaskBar { Icon = "ChartLine", Title = "待办完成率", Color = "#FF02C6DC", Target = "TodoView" });
        TaskBars.Add(new TaskBar { Icon = "PlaylistStar", Title = "备忘录汇总", Color = "#FFFFA000", Target = "MemoView" });
    }
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
        }
        _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.Target, param);
    }
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
            var dialogResult = await _dialog.ShowDialog("AddTodoView", param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var dto = dialogResult.Parameters.GetValue<TodoDto>("Value");
                if (dto.Id != null && dto.Id != Guid.Empty)
                {
                    //修改？
                    var updateResult = await _todoService.UpdateAsync(dto.Id.Value, dto);
                    if (updateResult.Status)
                    {
                        var viewDto = Summary.TodoDtos.FirstOrDefault(t => t.Id.Equals(dto.Id));
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
                    var addResult = await _todoService.AddAsync(dto);
                    if (addResult.Status)
                    {
                        Summary.TodoDtos.Add(addResult.Result);//界面显示
                                                               //更新代办事项汇总选项卡
                        Summary.Sum+=1;
                        Summary.CompletedRatio=(Summary.CompletedCount/(double)Summary.Sum).ToString("0%");
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
            var dialogResult = await _dialog.ShowDialog("AddMemoView", param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var dto = dialogResult.Parameters.GetValue<MemoDto>("Value");
                if (dto.Id != null && dto.Id != Guid.Empty)
                {
                    //修改？
                    var updateResult = await _memoService.UpdateAsync(dto.Id.Value, dto);
                    if (updateResult.Status)
                    {
                        var viewDto = Summary.MemoDtos.FirstOrDefault(t => t.Id.Equals(dto.Id.Value));
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
                    var addResult = await _memoService.AddAsync(dto);
                    if (addResult.Status)
                    {
                        Summary.MemoDtos.Add(addResult.Result);
                        //更新备忘汇总选项卡
                        Summary.MemoCount += 1;
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
    private async void Completed(TodoDto obj)
    {
        try
        {
            UpdateLoading(true);
            var updateResult = await _todoService.UpdateAsync(obj.Id.Value, obj);//修改数据
            if (updateResult.Status)
            {
                var todo = Summary.TodoDtos.FirstOrDefault(t => t.Id.Equals(obj.Id));
                if (todo!=null)
                {
                    Summary.TodoDtos.Remove(todo);//从界面列表删除
                    //更新完成率汇总选项卡
                    Summary.CompletedCount++;
                    Summary.CompletedRatio=(Summary.CompletedCount/(double)Summary.Sum).ToString("0%");
                    Refresh();
                }
                //发送全局通知，显示在Snackbar上
                _aggregator.SendMessage("待办事项已完成！");
            }
        }
        finally
        {
            UpdateLoading(false);
        }
    }
    public override async void OnNavigatedTo(NavigationContext navigationContext)
    {
        var summaryResult = await _todoService.GetSummaryAsync();
        if (summaryResult.Status)
        {
            Summary=summaryResult.Result;
            Refresh();
        }
        base.OnNavigatedTo(navigationContext);
    }
    void Refresh()
    {
        TaskBars[0].Content=Summary.Sum.ToString();
        TaskBars[1].Content=Summary.CompletedCount.ToString();
        TaskBars[2].Content=Summary.CompletedRatio;
        TaskBars[3].Content=Summary.MemoCount.ToString();
    }



    //private void CreateTestData()
    //{
    //    TodoDtos = new ObservableCollection<TodoDto>();
    //    MemoDtos = new ObservableCollection<MemoDto>();
    //    for (int i = 0; i < 5; i++)
    //    {
    //        TodoDtos.Add(new TodoDto { Title = $"待办{i}", Content = "正在处理中..." });
    //        MemoDtos.Add(new MemoDto { Title = $"备忘{i}", Content = "备忘事项..." });
    //    }
    //}
}