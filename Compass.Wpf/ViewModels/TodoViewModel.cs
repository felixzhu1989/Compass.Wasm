using Compass.Wasm.Shared.Params;
using Compass.Wpf.ApiServices.Todos;

namespace Compass.Wpf.ViewModels;

public class TodoViewModel : NavigationViewModel
{
    #region ctor-待办事项页面
    private readonly ITodoService _todoService;
    public TodoViewModel(ITodoService service, IContainerProvider provider) : base(provider)
    {
        _todoService = provider.Resolve<ITodoService>();
        TodoDtos =new ObservableCollection<TodoDto>();
        ExecuteCommand = new DelegateCommand<string>(Execute);
        SelectedCommand = new DelegateCommand<TodoDto>(Selected);
        DeleteCommand = new DelegateCommand<TodoDto>(Delete);
    }
    public DelegateCommand<string> ExecuteCommand { get; }//根据提供的不同参数执行不同的逻辑
    public DelegateCommand<TodoDto> SelectedCommand { get; }
    public DelegateCommand<TodoDto> DeleteCommand { get; }
    #endregion

    #region 属性
    private bool isRightDrawerOpen;
    /// <summary>
    /// 右侧窗口是否展开
    /// </summary>
    public bool IsRightDrawerOpen
    {
        get => isRightDrawerOpen;
        set { isRightDrawerOpen = value; RaisePropertyChanged(); }
    }
    private string rightDrawerTitle;
    public string RightDrawerTitle
    {
        get => rightDrawerTitle;
        set { rightDrawerTitle = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<TodoDto> todoDtos;
    public ObservableCollection<TodoDto> TodoDtos
    {
        get => todoDtos;
        set { todoDtos = value; RaisePropertyChanged(); }
    }
    private TodoDto currentDto;
    /// <summary>
    /// 当前Todo对象，添加或编辑
    /// </summary>
    public TodoDto CurrentDto
    {
        get => currentDto;
        set { currentDto = value; RaisePropertyChanged(); }
    }

    private string search;
    /// <summary>
    /// 搜索条件属性
    /// </summary>
    public string Search
    {
        get => search;
        set { search = value; RaisePropertyChanged(); }
    }
    private int selectedIndex;
    /// <summary>
    /// 选中状态，用于搜索筛选
    /// </summary>
    public int SelectedIndex
    {
        get => selectedIndex;
        set { selectedIndex = value; RaisePropertyChanged(); }
    }
    #endregion

    #region 增删改
    /// <summary>
    /// 各种执行方法
    /// </summary>
    private void Execute(string obj)
    {
        switch (obj)
        {
            case "Add": Add(); break;
            case "Query": GetDataAsync(); break;
            case "Save": Save(); break;
        }
    }

    /// <summary>
    /// 添加待办
    /// </summary>
    private void Add()
    {
        CurrentDto = new TodoDto();
        IsRightDrawerOpen=true;
        RightDrawerTitle = "添加待办";
    }
    /// <summary>
    /// 选中待办弹出修改界面
    /// </summary>
    /// <param name="obj"></param>
    private async void Selected(TodoDto obj)
    {
        try
        {
            UpdateLoading(true);//等待进度条
            var todoResult = await _todoService.GetFirstOrDefault(obj.Id.Value);
            if (todoResult.Status)
            {
                CurrentDto = todoResult.Result;
                IsRightDrawerOpen = true;//展开右边的窗口
                RightDrawerTitle = "修改待办";
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            UpdateLoading(false);
        }
    }
    /// <summary>
    /// 保存待办
    /// </summary>
    private async void Save()
    {
        //标题和内容不能为空
        if (string.IsNullOrWhiteSpace(CurrentDto.Title)||string.IsNullOrWhiteSpace(CurrentDto.Content)) return;
        try
        {
            UpdateLoading(true);
            if (CurrentDto.Id!=null&&currentDto.Id.Value!=Guid.Empty)//编辑ToDo
            {
                var updateResult = await _todoService.UpdateAsync(CurrentDto.Id.Value, CurrentDto);
                if (updateResult.Status)
                {
                    var dto = TodoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                    if (dto != null)
                    {
                        //更新界面显示
                        dto.Title = CurrentDto.Title;
                        dto.Content = CurrentDto.Content;
                        dto.Status = CurrentDto.Status;
                    }
                }
            }
            else//新增ToDo
            {
                var addResult = await _todoService.UserAddAsync(CurrentDto);
                if (addResult.Status)
                {
                    //更新界面显示
                    TodoDtos.Add(addResult.Result);
                }
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            IsRightDrawerOpen = false;
            UpdateLoading(false);
        }
    }

    /// <summary>
    /// 删除待办
    /// </summary>
    /// <param name="obj"></param>
    private async void Delete(TodoDto obj)
    {
        try
        {
            //删除询问
            var dialogResult = await DialogHost.Question("温馨提示", $"确认删除待办事项：{obj.Title}?");
            if (dialogResult.Result != ButtonResult.OK) return;

            UpdateLoading(true);
            var deleteResult = await _todoService.DeleteAsync(obj.Id.Value);
            if (deleteResult.Status)
            {
                var model = TodoDtos.FirstOrDefault(T => T.Id.Equals(obj.Id));
                if (model != null) TodoDtos.Remove(model);
            }
        }
        finally
        {
            UpdateLoading(false);
        }
    }
    #endregion

    #region 初始化
    /// <summary>
    /// 获取数据
    /// </summary>
    private async void GetDataAsync()
    {
        UpdateLoading(true);//打开等待窗口
        int? status = SelectedIndex == 0 ? null : SelectedIndex == 1 ? 0 : 1;
        TodoParam param = new()
        {
            Search = this.Search,
            Status = status
        };
        var result = await _todoService.GetAllFilterAsync(param);
        if (result.Status)
        {
            TodoDtos.Clear();
            TodoDtos.AddRange(result.Result);
        }
        UpdateLoading(false);//数据加载完毕后关闭等待窗口
    }
    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        SelectedIndex = navigationContext.Parameters.ContainsKey("Value") ? navigationContext.Parameters.GetValue<int>("Value") : 0;
        GetDataAsync();
    }
    #endregion
}