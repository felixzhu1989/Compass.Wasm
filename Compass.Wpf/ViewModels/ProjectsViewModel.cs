using System.Collections.ObjectModel;
using Compass.Wasm.Shared.Parameters;
using Compass.Wasm.Shared.Projects;
using Prism.Ioc;
using Prism.Regions;
using Prism.Commands;
using Compass.Wpf.Extensions;
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
    private int selectedProjectStatus;
    /// <summary>
    /// 选中状态，用于搜索筛选
    /// </summary>
    public int SelectedProjectStatus
    {
        get => selectedProjectStatus;
        set { selectedProjectStatus = value; RaisePropertyChanged(); }
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
        int? status = SelectedProjectStatus == 0 ? null : SelectedProjectStatus-1;
        ProjectParameter parameter = new() { Search = this.Search, ProjectStatus =status==null ? null : (ProjectStatus_e)status };
        var result = await _projectService.GetAllFilterAsync(parameter);
        if (result.Status)
        {
            ProjectDtos.Clear();
            ProjectDtos.AddRange(result.Result);
        }
    }
    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        SelectedProjectStatus = navigationContext.Parameters.ContainsKey("Value")
            ? navigationContext.Parameters.GetValue<int>("Value")
            : 0;
        GetDataAsync();
    } 
    #endregion
}