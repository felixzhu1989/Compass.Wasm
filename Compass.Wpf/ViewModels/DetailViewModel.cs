using Compass.Wasm.Shared.CategoryService;
using Compass.Wasm.Shared.Parameter;
using Compass.Wasm.Shared.ProjectService;
using Compass.Wpf.Common;
using Compass.Wpf.Extensions;
using Compass.Wpf.Service;
using Compass.Wpf.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace Compass.Wpf.Views;

public class DetailViewModel : NavigationViewModel
{
    private readonly IEventAggregator _aggregator;
    private readonly IProjectService _service;
    private readonly IModuleService _moduleService;
    private readonly IRegionManager _regionManager;
    private readonly IDialogHostService _dialogHost;

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

    #region 图纸-分段树
    //当前树内容，图纸-分段
    private ObservableCollection<DrawingDto> drawingDtos;
    public ObservableCollection<DrawingDto> DrawingDtos
    {
        get => drawingDtos;
        set { drawingDtos = value; RaisePropertyChanged(); }
    }
    //当前选择得树节点
    private object? _selectedItem;
    public object? SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value);
    }

    //当前分段
    private ModuleDto currentModuleDto;
    public ModuleDto CurrentModuleDto
    {
        get => currentModuleDto;
        set { currentModuleDto = value; RaisePropertyChanged(); }
    }
    #endregion

    #region 右侧展开栏
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
    #endregion

    #region 模型分类列表
    private ObservableCollection<ProductDto> productDtos;
    public ObservableCollection<ProductDto> ProductDtos
    {
        get => productDtos;
        set { productDtos = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<ModelDto> filterModelDtos;
    public ObservableCollection<ModelDto> FilterModelDtos
    {
        get => filterModelDtos;
        set { filterModelDtos = value; RaisePropertyChanged(); }
    }

    //当前选择的模型
    private object? selectedModel;
    public object? SelectedModel
    {
        get => selectedModel;
        set => SetProperty(ref selectedModel, value);
    }
    //提示显示当前选择的模型
    private string showSelectedModel;
    public string ShowSelectedModel
    {
        get => showSelectedModel;
        set
        {
            showSelectedModel = value;
            RaisePropertyChanged();
        }
    }
    //能不能选择模型，编辑时不能选择模型编辑
    private bool canSelectModelType;
    public bool CanSelectModelType
    {
        get => canSelectModelType;
        set { canSelectModelType = value; RaisePropertyChanged(); }
    }


    private string modelTypeSearch;
    /// <summary>
    /// 搜索条件属性
    /// </summary>
    public string ModelTypeSearch
    {
        get => modelTypeSearch;
        set { modelTypeSearch = value; RaisePropertyChanged(); }
    }

    private int selectedSbuIndex;
    /// <summary>
    /// 选中Sbu，用于搜索筛选
    /// </summary>
    public int SelectedSbuIndex
    {
        get => selectedSbuIndex;
        set { selectedSbuIndex = value; RaisePropertyChanged(); }
    }
    private string[] sbu;
    public string[] Sbu
    {
        get => sbu;
        set { sbu = value; RaisePropertyChanged(); }
    }
    //

    #endregion

    #region Command
    public DelegateCommand ProjectInfoCommand { get; }//跳转到项目概况界面
    public DelegateCommand<string> ExecuteCommand { get; }//根据提供的不同参数执行不同的逻辑
    public DelegateCommand<object> SelectedItemChangedCommand { get; }//选择图纸和分段更改
    public DelegateCommand<object> UpdateModuleDataCommand { get; }//编辑ModuleData
    public DelegateCommand<object> SelectedModelChangedCommand { get; }//选择模型更改
    public DelegateCommand ModulesCommand { get; }//跳转到制图界面

    private NavigationParameters ProjectParams { get; set; }=new ();
    public DetailViewModel(IEventAggregator aggregator, IContainerProvider containerProvider, IProjectService service, IModuleService moduleService, IRegionManager regionManager) : base(containerProvider)
    {
        _aggregator = aggregator;
        _service = service;
        _moduleService = moduleService;
        _regionManager = regionManager;
        //给弹窗使用的服务
        _dialogHost = containerProvider.Resolve<IDialogHostService>();
        DrawingDtos = new ObservableCollection<DrawingDto>();
        ProductDtos=new ObservableCollection<ProductDto>();
        FilterModelDtos=new ObservableCollection<ModelDto>();

        ProjectInfoCommand = new DelegateCommand(ProjectInfoNavigate);
        ExecuteCommand = new DelegateCommand<string>(Execute);
        SelectedItemChangedCommand = new DelegateCommand<object>(SelectedItemChanged);
        UpdateModuleDataCommand = new DelegateCommand<object>(UpdateModuleDataNavigate);
        SelectedModelChangedCommand = new DelegateCommand<object>(SelectedModelChanged);
        ModulesCommand = new DelegateCommand(() =>
        {
            //将Project传递给要导航的页面
            _regionManager.Regions[PrismManager.DetailViewRegionName].RequestNavigate("ModulesView", ProjectParams);
        });
    }


    #endregion

    #region 点击项目文字，导航到项目概况

    private void ProjectInfoNavigate()
    {
        _regionManager.Regions[PrismManager.DetailViewRegionName].RequestNavigate("ProjectInfoView", ProjectParams);
    }
    #endregion


    #region 选择图纸树节点的动作
    /// <summary>
    /// 选择树节点时赋值
    /// </summary>
    private void SelectedItemChanged(object obj)
    {
        SelectedItem = obj;
        //todo:右侧容器中填充导航内容



    }
    #endregion

    #region 双击图纸树节点的动作
    private void UpdateModuleDataNavigate(object obj)
    {
        SelectedItem = obj;
        if (SelectedItem is ModuleDto moduleDto)
        {
            if (obj==null||moduleDto.Id.Equals(Guid.Empty)) return;
            var modelName = moduleDto.ModelName.Split('_')[0];
            var target = $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(modelName.ToLower())}DataView";
            //将dto传递给要导航的页面
            NavigationParameters param = new NavigationParameters { { "Value", moduleDto } };
            _regionManager.Regions[PrismManager.DetailViewRegionName].RequestNavigate(target, param);
        }


    }
    #endregion

    #region 选择模型树节点的动作
    private void SelectedModelChanged(object obj)
    {
        SelectedModel= obj;
        if (SelectedModel is ModelTypeDto currentModelType)
        {
            //显示当前的容
            ShowSelectedModel = $"当前选择模型：{currentModelType.ModelName}";
            //给CurrentModuleDto赋值
            CurrentModuleDto.ModelTypeId = currentModelType.Id;
            CurrentModuleDto.ModelName = currentModelType.ModelName;
        }
    }
    #endregion

    #region 增删改查
    private void Execute(string obj)
    {
        switch (obj)
        {
            case "Add": Add(); break;
            case "Update": Update(); break;
            case "Save": Save(); break;
            case "Delete": Delete(); break;
            case "FilterModelTypeTree":
                FilterModelTypeTree(); break;
        }
    }

    private void Add()
    {
        CurrentModuleDto = new ModuleDto();
        if (SelectedItem is DrawingDto drawingDto)
        {
            //拿到drawing得值
            CurrentModuleDto.DrawingId = drawingDto.Id.Value;
        }
        else if (SelectedItem is ModuleDto moduleDto)
        {
            //也可以拿到drawing得值
            CurrentModuleDto.DrawingId = moduleDto.DrawingId;
        }
        else
        {
            //发送全局通知，显示在Snackbar上,告诉用户需要选择节点
            _aggregator.SendMessage("请先选择Item再添加分段！");
            return;
        }
        //弹出右侧栏，填写数据
        IsRightDrawerOpen = true;
        CanSelectModelType = true;
        RightDrawerTitle = "添加分段";
        FilterModelTypeTree();
    }

    private void Update()
    {
        //只可以修改Module，不能修改Drawing
        if (SelectedItem is ModuleDto moduleDto)
        {
            CurrentModuleDto=moduleDto;
            ShowSelectedModel = $"分段模型：{CurrentModuleDto.ModelName}";
        }
        else
        {
            //发送全局通知，显示在Snackbar上,告诉用户需要选择节点
            _aggregator.SendMessage("请选择分段再修改！");
            return;
        }
        //弹出右侧栏，填写数据
        IsRightDrawerOpen = true;
        CanSelectModelType = false;
        RightDrawerTitle = "修改分段";
    }

    private async void Save()
    {
        //数据验证
        //请选择模型，或者分段名称不能为空
        if (string.IsNullOrWhiteSpace(CurrentModuleDto.Name) || CurrentModuleDto.ModelTypeId == null ||
            CurrentModuleDto.ModelTypeId.Equals(Guid.Empty))
        {
            //发送提示
            _aggregator.SendMessage("没有择模型，或者分段名称不能为空");
            return;
        }
        try
        {
            if (CurrentModuleDto.Id!=null&&CurrentModuleDto.Id.Value!=Guid.Empty)//编辑ModuleDto
            {
                var updateResult = await _moduleService.UpdateAsync(CurrentModuleDto.Id.Value, currentModuleDto);
                if (updateResult.Status)
                {
                    var drawingDto =
                        DrawingDtos.FirstOrDefault(x => x.Id.Equals(CurrentModuleDto.DrawingId));
                    if (drawingDto != null)
                    {
                        var dto = drawingDto.ModuleDtos.First(x => x.Id.Equals(CurrentModuleDto.Id));
                        dto.Name = CurrentModuleDto.Name.ToUpper();
                        dto.ModelTypeId = CurrentModuleDto.ModelTypeId;
                        dto.ModelName=CurrentModuleDto.ModelName;
                        dto.SpecialNotes=CurrentModuleDto.SpecialNotes;
                    }
                }
                IsRightDrawerOpen = false;//关闭弹窗
                ShowSelectedModel = string.Empty;//清空模型选择提示信息
                _aggregator.SendMessage($"分段{CurrentModuleDto.Name} {CurrentModuleDto.ModelName}修改成功！");
            }
            else//新增ModuleDto
            {
                var addResult = await _moduleService.AddAsync(CurrentModuleDto);
                if (addResult.Status)
                {
                    //更新界面上分段树的显示，不关闭弹出侧边栏，只给顶部发送添加成功的消息
                    var drawingDto =
                        DrawingDtos.FirstOrDefault(x => x.Id.Equals(CurrentModuleDto.DrawingId));
                    if (drawingDto != null)
                    {
                        addResult.Result.Name= addResult.Result.Name.ToUpper();
                        drawingDto.ModuleDtos.Add(addResult.Result);
                    }
                    //不关闭侧边框，初始化CurrentModuleDto，并重新绑定数据，发送提示
                    _aggregator.SendMessage($"分段{CurrentModuleDto.Name} {CurrentModuleDto.ModelName}添加成功！");
                    CurrentModuleDto.Id=null;//置空id，继续添加下一个分段
                }
            }
        }
        catch (Exception e)
        {
            //发送错误报告
            _aggregator.SendMessage(e.Message);
        }
    }
    private async void Delete()
    {
        //只可以删除Module，不能删除Drawing
        if (SelectedItem is ModuleDto moduleDto)
        {
            //弹窗提示用户确定需要删除吗？
            //删除询问
            var dialogResult = await _dialogHost.Question("删除确认", $"确认删除分段：{moduleDto.Name} {moduleDto.ModelName} 吗?");
            if (dialogResult.Result != ButtonResult.OK) return;
            var deleteResult = await _moduleService.DeleteAsync(moduleDto.Id.Value);
            if (deleteResult.Status)
            {
                var drawingDto =
                    DrawingDtos.FirstOrDefault(x => x.Id.Equals(moduleDto.DrawingId));
                if (drawingDto != null)
                {
                    drawingDto.ModuleDtos.Remove(moduleDto);
                }
            }

        }
    }
    #endregion

    #region 初始化
    private async void GetModuleTreeDataAsync()
    {
        UpdateLoading(true);//打开等待窗口
        ProjectParameter parameter = new() { ProjectId = Project.Id };
        var moduleTreeResult = await _service.GetModuleTreeAsync(parameter);
        if (moduleTreeResult.Status)
        {
            DrawingDtos.Clear();
            DrawingDtos.AddRange(moduleTreeResult.Result);
        }
        UpdateLoading(false);//数据加载完毕后关闭等待窗口
    }

    private async void GetModelTreeDataAsync()
    {
        var modelTypeTreeResult = await _service.GetModelTypeTreeAsync();
        if (modelTypeTreeResult.Status)
        {
            ProductDtos.Clear();
            ProductDtos.AddRange(modelTypeTreeResult.Result);
        }
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        //获取导航传递的参数
        Project = navigationContext.Parameters.ContainsKey("Value")
            ? navigationContext.Parameters.GetValue<ProjectDto>("Value")
            : new ProjectDto();
        ProjectParams.Add("Value", Project);//将导航数据填好，以便后续使用
        ProjectInfoNavigate();
        Title = $"{Project.OdpNumber} - {Project.Name}";
        Sbu = Enum.GetNames(typeof(Sbu_e));
        GetModuleTreeDataAsync();
        GetModelTreeDataAsync();
    }
    #endregion

    #region 筛选模型
    /* 产品模型选择方案
 * Product-Model-ModelType
 * 首先使用api查询到一个总的List<ProductDto> ProductDtos,按照sbu默认为fs分配给List<ProductDto> FilterProductDtos
 * 使用搜索框和sub分类，对FilterProductDtos进行筛选，本地linq查询，不通过api
 */
    private void FilterModelTypeTree()
    {
        FilterModelDtos.Clear();
        //先筛选SBU
        var filterProductDtos = ProductDtos.Where(x => x.Sbu.Equals((Sbu_e)SelectedSbuIndex));
        foreach (var productDto in filterProductDtos)
        {
            FilterModelDtos.AddRange(productDto.ModelDtos.Where(x =>
                string.IsNullOrWhiteSpace(ModelTypeSearch) || x.Name.Contains(ModelTypeSearch.ToUpper())));
        }
    }
    #endregion
}