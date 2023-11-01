using System.Collections.Generic;
using System.IO;
using Compass.Wasm.Shared.Params;

namespace Compass.Wpf.ViewModels;

public class ModulesViewModel : NavigationViewModel
{
    #region ctor-分段列表页面
    private readonly IProjectService _projectService;
    public ModulesViewModel(IContainerProvider provider) : base(provider)
    {
        _projectService = provider.Resolve<IProjectService>();
        AllModules = new ObservableCollection<ModuleDto>();
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
    private int? selectedBatch;
    /// <summary>
    /// 选中状态，用于搜索筛选
    /// </summary>
    public int? SelectedBatch
    {
        get => selectedBatch;
        set { selectedBatch = value; RaisePropertyChanged(); }
    }
    //使用枚举初始化下拉框
    private int[] batchs = null!;
    public int[] Batchs
    {
        get => batchs;
        set { batchs = value; RaisePropertyChanged(); }
    }
    #endregion

    #region DataGrid数据属性

    private ObservableCollection<ModuleDto> allModules;
    public ObservableCollection<ModuleDto> AllModules 
    { 
        get=> allModules; 
        set { allModules=value;RaisePropertyChanged(); }
    }
    
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
            case "Batch": BatchChanges();break;
            case "AutoDrawing": AutoDrawing(); break;
            case "ExportDxf": ExportDxf(); break;
            case "PrintCutList": PrintCutList(); break;
            case "PrintJobCard": PrintJobCard();break;
            case "PrintEnJobCard": PrintEnJobCard(); break;
            case "SyncFiles": SyncFiles();break;
            case "PackingList": PackingListNavigate();break;

            case "PrintFinal": PrintFinal(); break;
            case "PrintEnFinal": PrintEnFinal(); break;
            case "PrintEnScreenShot": PrintEnScreenShot();break;

        }
    }

    #region 同步文件
    private void SyncFiles()
    {
        //弹窗，同步本地和公共盘的文件





    } 
    #endregion

    #region 批量操作
    

    private async void AutoDrawing()
    {
        const BatchWorksAction_e actionName = BatchWorksAction_e.自动作图;
        await BatchWorks(actionName);
        GetModuleDtosDataAsync();
    }

    private async void ExportDxf()
    {
        const BatchWorksAction_e actionName = BatchWorksAction_e.导出DXF图;
        await BatchWorks(actionName);
    }

    private async void PrintCutList()
    {
        const BatchWorksAction_e actionName = BatchWorksAction_e.打印CutList;
        await BatchWorks(actionName);
    }

    private async void PrintJobCard()
    {
        const BatchWorksAction_e actionName = BatchWorksAction_e.打印JobCard;
        await BatchWorks(actionName);
    }
    private async void PrintEnJobCard()
    {
        const BatchWorksAction_e actionName = BatchWorksAction_e.打印英文JobCard;
        await BatchWorks(actionName);
    }

    private async void PrintFinal()
    {
        const BatchWorksAction_e actionName = BatchWorksAction_e.打印最终检验页;
        await BatchWorks(actionName);
    }
    private async void PrintEnFinal()
    {
        const BatchWorksAction_e actionName = BatchWorksAction_e.打印英文最终检验页;
        await BatchWorks(actionName);
    }

    private async void PrintEnScreenShot()
    {
        const BatchWorksAction_e actionName = BatchWorksAction_e.打印英文截图页;
        await BatchWorks(actionName);
    }



    #endregion

    #region 装箱清单
    private void PackingListNavigate()
    {
        if(Project.Id==null||Project.Id==Guid.Empty||SelectedBatch==null)return;
        var packingListParam = new PackingListParam
        {
            ProjectId = Project.Id,
            Batch = SelectedBatch
        };
        var param = new NavigationParameters { { "Value", packingListParam } };
        RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("PackingListView", back =>
        {
            Journal = back.Context.NavigationService.Journal;
        }, param);
    }
    #endregion



    private async Task BatchWorks(BatchWorksAction_e actionName)
    {
        CanBatchWorks = false;
        //获取勾选的ModuleDto
        var selectedModuleDto = ModuleDtos.Where(x => x.IsSelected).ToList();
        //判断是否勾选分段
        if (!selectedModuleDto.Any())
        {
            Aggregator.SendMessage($"请勾选分段再开始{actionName}");
            CanBatchWorks = true;
            return;
        }
        //弹出模式窗口，向弹窗传递信息
        var param = new DialogParameters
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
        var param = new ProjectParam { ProjectId = Project.Id };
        var moduleDtosResult = await _projectService.GetModuleListAsync(param);
        if (moduleDtosResult.Status)
        {
            AllModules.Clear();
            AllModules.AddRange(moduleDtosResult.Result);
            Batchs = AllModules.Select(x => x.Batch).Distinct().ToArray();
            if (Batchs.Length != 0)
            {
                SelectedBatch = Batchs[0];
            }
        }
        else
        {
            AllModules = new ObservableCollection<ModuleDto>();
        }
        //绑定勾选数据变更
        foreach (var model in AllModules)
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
        BatchChanges();
    }
    /// <summary>
    /// 分批变更时
    /// </summary>
    private void BatchChanges()
    {
        //查询分批的分段模型
        ModuleDtos.Clear();
        if (Batchs.Length == 0)
        {
            ModuleDtos = new ObservableCollection<ModuleDto>();//初始化
        }
        else
        {
            var dtos = AllModules.Where(x => x.Batch == SelectedBatch);
            ModuleDtos.AddRange(dtos);
        }
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