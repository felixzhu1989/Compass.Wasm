using System;
using Compass.Wasm.Shared.ProjectService;
using Prism.Ioc;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Compass.Wpf.Service;
using Compass.Wasm.Shared.Parameter;
using Compass.Wpf.BatchWorks;
using Compass.Wpf.Extensions;
using Prism.Commands;
using Prism.Events;
using System.CodeDom.Compiler;

namespace Compass.Wpf.ViewModels;

public class BatchWorksViewModel : NavigationViewModel
{
    private readonly IEventAggregator _aggregator;
    private readonly IProjectService _service;
    private readonly IBatchWorksService _batchWorksService;

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

    #region DataGrid数据
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

    #region Progress
    private bool showProgressBar;
    public bool ShowProgressBar
    {
        get => showProgressBar;
        set
        {
            showProgressBar = value;
            RaisePropertyChanged();
        }
    }
    private string progressTips;
    public string ProgressTips
    {
        get => progressTips;
        set
        {
            progressTips = value;
            RaisePropertyChanged();
        }
    }



    #endregion

    public DelegateCommand<string> ExecuteCommand { get; }

    public BatchWorksViewModel(IEventAggregator aggregator,IContainerProvider containerProvider,IProjectService service) : base(containerProvider)
    {
        _aggregator = aggregator;
        _service = service;
        _batchWorksService = containerProvider.Resolve<IBatchWorksService>();
        ModuleDtos =new ObservableCollection<ModuleDto>();
        ExecuteCommand = new DelegateCommand<string>(Execute);

    }

    private void Execute(string obj)
    {
        switch (obj)
        {
            case "AutoDrawing": AutoDrawing(); break;
            
        }
    }

    //最终只要一串Guid来参与制图
    private async void AutoDrawing()
    {
        _aggregator.SendMessage($"{Project.OdpNumber} 自动作图开始");
        ShowProgressBar =true;
        ProgressTips = "正在作图，请勿导航到其他页面！";
        //获取勾选的ModuleDto
        List<ModuleDto> selectedModuleDto = ModuleDtos.Where(x => x.IsSelected).ToList();
        await _batchWorksService.BatchDrawingAsync(selectedModuleDto);
        await Task.Delay(1000);
        ProgressTips = "作图完成！";
        await Task.Delay(1000);
        ShowProgressBar = false;
        _aggregator.SendMessage($"{Project.OdpNumber} 自动作图完成");
    }



    #region 初始化
    private async void GetModuleDtosDataAsync()
    {
        ProjectParameter parameter = new() { ProjectId = Project.Id };
        var moduleDtosResult = await _service.GetModuleListAsync(parameter);
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
        Title = $"{Project.OdpNumber}-AutoDrawing";
        ShowProgressBar = false;
        GetModuleDtosDataAsync();
    } 
    #endregion
}