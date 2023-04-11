using Compass.Wasm.Shared.ProjectService;
using Prism.Ioc;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Compass.Wasm.Shared.Parameter;
using Compass.Wpf.ApiService;
using Compass.Wpf.Extensions;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Diagnostics;

namespace Compass.Wpf.ViewModels;

public class ModulesViewModel : NavigationViewModel
{
    #region ctor-分段列表页面
    private readonly IProjectService _projectService;
    public ModulesViewModel(IContainerProvider provider) : base(provider)
    {
        _projectService = provider.Resolve<IProjectService>();
        ModuleDtos =new ObservableCollection<ModuleDto>();
        ExecuteCommand = new DelegateCommand<string>(Execute);
        ShowCutListCommand = new DelegateCommand<ModuleDto>(ShowCutList);
        ShowFilesCommand=new DelegateCommand<ModuleDto>(ShowFiles);
        ShowJobCardCommand = new DelegateCommand<ModuleDto>(ShowJobCard);
    }
    public DelegateCommand<string> ExecuteCommand { get; }
    public DelegateCommand<ModuleDto> ShowCutListCommand { get; }
    public DelegateCommand<ModuleDto> ShowFilesCommand { get; }
    public DelegateCommand<ModuleDto> ShowJobCardCommand { get; }

    #endregion

    #region 项目内容属性
    //当前页面显示得项目
    private ProjectDto project;
    public ProjectDto Project
    {
        get => project;
        set { project = value; RaisePropertyChanged(); }
    }
    #endregion

    #region DataGrid数据属性
    private ObservableCollection<ModuleDto> moduleDtos;
    public ObservableCollection<ModuleDto> ModuleDtos
    {
        get => moduleDtos;
        set { moduleDtos = value; RaisePropertyChanged(); }
    }

    public bool? IsAllModuleDtosSelected
    {
        get
        {
            var selected = ModuleDtos.Select(item => item.IsSelected).Distinct().ToList();
            return selected.Count == 1 ? selected.Single() : null;
        }
        set
        {
            if (value.HasValue)
            {
                SelectAll(value.Value, ModuleDtos);
                RaisePropertyChanged();
            }
        }
    }
    private static void SelectAll(bool select, IEnumerable<ModuleDto> models)
    {
        foreach (var model in models)
        {
            model.IsSelected = select;
        }
    }

    #endregion

    #region 执行操作属性
    private bool canBatchWorks = true;
    public bool CanBatchWorks
    {
        get => canBatchWorks;
        set
        {
            canBatchWorks = value;
            RaisePropertyChanged();
        }
    }

    #endregion

    #region 显示CutList弹窗
    private async void ShowCutList(ModuleDto obj)
    {
        DialogParameters param = new DialogParameters { { "Value", obj } };
        await DialogHost.ShowDialog("CutListView", param);
    }
    #endregion

    #region 显示JobCard弹窗
    private async void ShowJobCard(ModuleDto obj)
    {
        DialogParameters param = new DialogParameters { { "Value", obj } };
        await DialogHost.ShowDialog("JobCardView", param);
    }
    #endregion

    #region 显示分段模型所在文件夹
    private void ShowFiles(ModuleDto obj)
    {
        if(!Directory.Exists(obj.PackDir))return;
        var psi = new ProcessStartInfo("Explorer.exe")
        {
            Arguments =obj.PackDir
        };
        Process.Start(psi);
    }
    #endregion

    #region 执行批量操作动作，弹出批量操作弹窗
    private void Execute(string obj)
    {
        switch (obj)
        {
            case "AutoDrawing": AutoDrawing(); break;
            case "ExportDxf": ExportDxf(); break;
            case "PrintCutList": PrintCutList(); break;
            case "PrintJobCard": PrintJobCard();break;
        }
    }
    private async void AutoDrawing()
    {
        var actionName = BatchWorksAction_e.自动作图;
        await BatchWorks(actionName);
    }

    private async void ExportDxf()
    {
        var actionName = BatchWorksAction_e.导出DXF图;
        await BatchWorks(actionName);
    }

    private async void PrintCutList()
    {
        var actionName = BatchWorksAction_e.打印CutList;
        await BatchWorks(actionName);
    }

    private async void PrintJobCard()
    {
        var actionName = BatchWorksAction_e.打印JobCard;
        await BatchWorks(actionName);
    }
    private async Task BatchWorks(BatchWorksAction_e actionName)
    {
        CanBatchWorks = false;
        //获取勾选的ModuleDto
        List<ModuleDto> selectedModuleDto = ModuleDtos.Where(x => x.IsSelected).ToList();
        //判断是否勾选分段
        if (!selectedModuleDto.Any())
        {
            Aggregator.SendMessage($"请勾选分段再开始{actionName}");
            CanBatchWorks = true;
            return;
        }
        //弹出模式窗口，向弹窗传递信息
        DialogParameters param = new DialogParameters
        {
            { "Value", selectedModuleDto },
            { "ActionName", actionName }
        };
        await DialogHost.ShowDialog("BatchWorksView", param);
        CanBatchWorks = true;
    }
    #endregion

    #region 初始化
    private async void GetModuleDtosDataAsync()
    {
        ProjectParameter parameter = new() { ProjectId = Project.Id };
        var moduleDtosResult = await _projectService.GetModuleListAsync(parameter);
        if (moduleDtosResult.Status)
        {
            ModuleDtos.Clear();
            ModuleDtos.AddRange(moduleDtosResult.Result);
        }
        //绑定勾选数据变更
        foreach (var model in ModuleDtos)
        {
            model.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(ModuleDto.IsSelected))
                    RaisePropertyChanged(nameof(IsAllModuleDtosSelected));
            };
            //给IsFilesOk和PackDir赋值
            var packPath = model.GetPackPath(out _,out var packDir);
            if (!File.Exists(packPath)) continue;
            model.IsFilesOk=true;
            model.PackDir=packDir;
        }
        IsAllModuleDtosSelected = false;
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        //获取导航传递的参数
        Project = navigationContext.Parameters.ContainsKey("Value")
            ? navigationContext.Parameters.GetValue<ProjectDto>("Value")
            : new ProjectDto();
        GetModuleDtosDataAsync();
        CanBatchWorks = true;
    } 
    #endregion
}