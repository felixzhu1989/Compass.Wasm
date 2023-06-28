using Compass.Wasm.Shared.Params;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ViewModels;

public class ProjectsViewModel : NavigationViewModel
{
    #region ctor-项目列表页面
    private readonly IProjectService _projectService;
    public ProjectsViewModel(IContainerProvider provider) : base(provider)
    {
        _projectService = provider.Resolve<IProjectService>();

        ProjectDtos = new ObservableCollection<ProjectDto>();
        ExecuteCommand = new DelegateCommand<string>(Execute);
        DetailCommand = new DelegateCommand<ProjectDto>(Navigate);
    }
    public DelegateCommand<string> ExecuteCommand { get; }
    public DelegateCommand<ProjectDto> DetailCommand { get; }
    #endregion

    #region 属性
    private ObservableCollection<ProjectDto> projectDtos;
    public ObservableCollection<ProjectDto> ProjectDtos
    {
        get => projectDtos;
        set { projectDtos = value; RaisePropertyChanged(); }
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
    private int selectedStatus;
    /// <summary>
    /// 选中状态，用于搜索筛选
    /// </summary>
    public int SelectedStatus
    {
        get => selectedStatus;
        set { selectedStatus = value; RaisePropertyChanged(); }
    }
    //使用枚举初始化下拉框
    private string[] status = null!;
    public string[] Status
    {
        get => status;
        set { status = value; RaisePropertyChanged(); }
    }
    #endregion

    #region 导航到详细页面
    /// <summary>
    /// 跳转到项目详细页面，传递dto
    /// </summary>
    private void Navigate(ProjectDto obj)
    {
        //将dto传递给要导航的页面
        NavigationParameters param = new NavigationParameters { { "Value", obj } };
        RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("DetailView", back =>
        {
            Journal = back.Context.NavigationService.Journal;
        }, param);
    }
    #endregion

    #region 增删改查
    /// <summary>
    /// 各种执行方法
    /// </summary>
    private void Execute(string obj)
    {
        switch (obj)
        {

            case "Query": GetDataAsync(); break;
                //case "Add": Add(); break;
                //case "Save": Save(); break;
        }
    } 
    #endregion

    #region 导航初始化
    private async void GetDataAsync()
    {
        int? status = SelectedStatus;
        ProjectParam param = new() { Search = this.Search, Status =status==null ? null : (MainPlanStatus_e)status };
        var result = await _projectService.GetAllFilterAsync(param);
        ProjectDtos.Clear();
        if (result.Status)
        {
            ProjectDtos.AddRange(result.Result);
        }
    }
    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        SelectedStatus = navigationContext.Parameters.ContainsKey("Value")
            ? navigationContext.Parameters.GetValue<int>("Value")
            : 0;
        Status = Enum.GetNames(typeof(MainPlanStatus_e));
        GetDataAsync();
    } 
    #endregion
}