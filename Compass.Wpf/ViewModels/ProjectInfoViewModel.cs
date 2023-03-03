using Compass.Wasm.Shared.CategoryService;
using Compass.Wasm.Shared.ProjectService;
using Compass.Wpf.Extensions;
using Prism.Ioc;
using Prism.Regions;
using System;

namespace Compass.Wpf.ViewModels;

public class ProjectInfoViewModel:NavigationViewModel
{

    #region 项目内容
    //抬头
    private string title;
    public string Title
    {
        get => title;
        set { title = value; RaisePropertyChanged(); }
    }
    //当前页面显示得项目
    private ProjectDto project;
    public ProjectDto Project
    {
        get => project;
        set { project = value; RaisePropertyChanged(); }
    }
    #endregion



    public ProjectInfoViewModel(IContainerProvider containerProvider) : base(containerProvider)
    {
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        //获取导航传递的参数
        Project = navigationContext.Parameters.ContainsKey("Value")
            ? navigationContext.Parameters.GetValue<ProjectDto>("Value")
            : new ProjectDto();
        Title = $"{Project.OdpNumber} 项目概况";
    }
}