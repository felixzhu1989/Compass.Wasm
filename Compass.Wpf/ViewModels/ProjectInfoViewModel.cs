using Compass.Wasm.Shared.ProjectService;
using Prism.Ioc;
using Prism.Regions;

namespace Compass.Wpf.ViewModels;

public class ProjectInfoViewModel : NavigationViewModel
{
    #region ctor-项目信息页面
    //todo：做成kickoff邮件，拷贝信息
    public ProjectInfoViewModel(IContainerProvider provider) : base(provider)
    {
    } 
    #endregion

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

    #region 导航与初始化
    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        //获取导航传递的参数
        Project = navigationContext.Parameters.ContainsKey("Value")
            ? navigationContext.Parameters.GetValue<ProjectDto>("Value")
            : new ProjectDto();
        Title = $"{Project.OdpNumber} 项目概况";
    } 
    #endregion
}