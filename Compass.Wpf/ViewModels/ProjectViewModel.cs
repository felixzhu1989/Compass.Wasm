using System;
using System.Collections.ObjectModel;
using Compass.Wasm.Shared.Parameter;
using Compass.Wasm.Shared.ProjectService;
using Compass.Wpf.Service;
using Prism.Ioc;
using Prism.Regions;
using Prism.Commands;
using Compass.Wpf.Extensions;

namespace Compass.Wpf.ViewModels;

public class ProjectViewModel:NavigationViewModel
{
    private readonly IProjectService _service;
    private readonly IRegionManager _regionManager;

    public DelegateCommand<string> ExecuteCommand { get; }
    public DelegateCommand<ProjectDto> DetailCommand { get; }

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


    public ProjectViewModel(IContainerProvider containerProvider,IProjectService service) : base(containerProvider)
    {
        _service = service;
        _regionManager =containerProvider.Resolve<IRegionManager>();

        ProjectDtos = new ObservableCollection<ProjectDto>();
        ExecuteCommand = new DelegateCommand<string>(Execute);
        DetailCommand = new DelegateCommand<ProjectDto>(Detail);
    }
    /// <summary>
    /// 跳转到项目详细页面，传递dto
    /// </summary>
    private void Detail(ProjectDto obj)
    {
        //将dto传递给要导航的页面
        NavigationParameters param = new NavigationParameters { { "Value", obj } };
        _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("DetailView", param);
    }

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



    private async void GetDataAsync()
    {
        UpdateLoading(true);//打开等待窗口
        int? status = SelectedProjectStatus == 0 ? null : SelectedProjectStatus-1;
        ProjectParameter parameter = new() { Search = this.Search,ProjectStatus =status==null?null:(ProjectStatus_e)status };
        var result = await _service.GetAllFilterAsync(parameter);
        if (result.Status)
        {
            ProjectDtos.Clear();
            ProjectDtos.AddRange(result.Result);
        }
        UpdateLoading(false);//数据加载完毕后关闭等待窗口
    }
    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        SelectedProjectStatus = navigationContext.Parameters.ContainsKey("Value")
            ? navigationContext.Parameters.GetValue<int>("Value")
            : 0;
        GetDataAsync();
    }


}