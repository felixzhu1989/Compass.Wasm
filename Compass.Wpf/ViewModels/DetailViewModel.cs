using System.Globalization;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Params;

namespace Compass.Wpf.Views;

public class DetailViewModel : NavigationViewModel
{
    #region ctor-项目详细操作页面
    private readonly IProjectService _projectService;
    private readonly IModuleService _moduleService;

    public DetailViewModel(IContainerProvider provider) : base(provider)
    {
        _projectService = provider.Resolve<IProjectService>();
        _moduleService = provider.Resolve<IModuleService>();

        DrawingDtos = new ObservableCollection<DrawingDto>();
        ProductDtos=new ObservableCollection<ProductDto>();
        FilterModelDtos=new ObservableCollection<ModelDto>();

        ExecuteCommand = new DelegateCommand<string>(Execute);
        SelectedItemChangedCommand = new DelegateCommand<object>(SelectedItemChanged);
        UpdateModuleDataCommand = new DelegateCommand<object>(UpdateModuleDataNavigate);
        SelectedModelChangedCommand = new DelegateCommand<object>(SelectedModelChanged);
        ModulesCommand = new DelegateCommand(ModulesNavigate);
    }
    //Commands
    public DelegateCommand<string> ExecuteCommand { get; }//根据提供的不同参数执行不同的逻辑
    public DelegateCommand<object> SelectedItemChangedCommand { get; }//选择图纸和分段更改
    public DelegateCommand<object> UpdateModuleDataCommand { get; }//编辑ModuleData
    public DelegateCommand<object> SelectedModelChangedCommand { get; }//选择模型更改
    public DelegateCommand ModulesCommand { get; }//跳转到制图界面

    #endregion

    #region 角色控制属性
    private string updateRoles;
    public string UpdateRoles
    {
        get => updateRoles;
        set { updateRoles = value; RaisePropertyChanged(); }
    }
    #endregion

    #region 项目内容属性
    //抬头
    private string title = null!;
    public string Title
    {
        get => title;
        set { title = value; RaisePropertyChanged(); }
    }
    //链接
    private string url;
    public string Url
    {
        get => url;
        set { url = value; RaisePropertyChanged(); }
    }
    //当前页面显示得项目
    private ProjectDto project = null!;
    public ProjectDto Project
    {
        get => project;
        set { project = value; RaisePropertyChanged(); }
    }
    #endregion

    #region 图纸-分段树属性
    //当前树内容，图纸-分段
    private ObservableCollection<DrawingDto> drawingDtos = null!;
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
    private ModuleDto currentModuleDto = null!;
    public ModuleDto CurrentModuleDto
    {
        get => currentModuleDto;
        set { currentModuleDto = value; RaisePropertyChanged(); }
    }
    #endregion

    #region 右侧展开栏属性
    private bool isRightDrawerOpen;
    /// <summary>
    /// 右侧窗口是否展开
    /// </summary>
    public bool IsRightDrawerOpen
    {
        get => isRightDrawerOpen;
        set { isRightDrawerOpen = value; RaisePropertyChanged(); }
    }
    private string rightDrawerTitle = null!;
    public string RightDrawerTitle
    {
        get => rightDrawerTitle;
        set { rightDrawerTitle = value; RaisePropertyChanged(); }
    }
    #endregion

    #region 模型分类列表属性
    private ObservableCollection<ProductDto> productDtos = null!;
    public ObservableCollection<ProductDto> ProductDtos
    {
        get => productDtos;
        set { productDtos = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<ModelDto> filterModelDtos = null!;
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
    private string showSelectedModel = null!;
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


    private string modelTypeSearch = null!;
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
    private string[] sbu = null!;
    public string[] Sbu
    {
        get => sbu;
        set { sbu = value; RaisePropertyChanged(); }
    }
    //

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

    #region 双击图纸树节点的动作 1，修改模型参数 2，修改图纸截图
    private void UpdateModuleDataNavigate(object obj)
    {
        SelectedItem = obj;
        if (SelectedItem is ModuleDto moduleDto)
        {
            if (obj==null||moduleDto.Id.Equals(Guid.Empty)) return;
            var modelName = moduleDto.ModelName.Split('_')[0];
            var target = $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(modelName.ToLower())}DataView";
            //将dto传递给要导航的页面
            var param = new NavigationParameters { { "Value", moduleDto } };
            RegionManager.Regions[PrismManager.DetailViewRegionName].RequestNavigate(target, back =>
            {
                Journal = back.Context.NavigationService.Journal;
            }, param);
        }
        else if (SelectedItem is DrawingDto drawingDto)
        {
            if (obj==null||drawingDto.Id.Equals(Guid.Empty)) return;
            //将dto传递给要导航的页面
            var param = new NavigationParameters { { "Value", drawingDto } };
            RegionManager.Regions[PrismManager.DetailViewRegionName].RequestNavigate("DrawingView", back =>
            {
                Journal = back.Context.NavigationService.Journal;
            }, param);
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
            CurrentModuleDto.Pallet=currentModelType.Pallet;
        }
    }
    #endregion

    #region 增删改查分段模型树
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
            Aggregator.SendMessage("请先选择Item再添加分段！");
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
            Aggregator.SendMessage("请选择分段再修改！");
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
            Aggregator.SendMessage("没有择模型，或者分段名称不能为空");
            return;
        }
        try
        {
            if (CurrentModuleDto.Id!=null&&CurrentModuleDto.Id.Value!=Guid.Empty)//编辑ModuleDto
            {
                //为了避免分段号前后有空格，导致后续创建文件夹时出现错误，此处消除空格
                CurrentModuleDto.Name = CurrentModuleDto.Name.Trim().ToUpper();
                var updateResult = await _moduleService.UpdateAsync(CurrentModuleDto.Id.Value, CurrentModuleDto);
                //更新界面
                if (updateResult.Status)
                {
                    var drawingDto =
                        DrawingDtos.FirstOrDefault(x => x.Id.Equals(CurrentModuleDto.DrawingId));
                    if (drawingDto != null)
                    {
                        var dto = drawingDto.ModuleDtos.First(x => x.Id.Equals(CurrentModuleDto.Id));
                        dto.Name = CurrentModuleDto.Name;
                        dto.ModelTypeId = CurrentModuleDto.ModelTypeId;
                        dto.ModelName=CurrentModuleDto.ModelName;
                        dto.SpecialNotes=CurrentModuleDto.SpecialNotes;
                        dto.Length = CurrentModuleDto.Length;
                        dto.Width = CurrentModuleDto.Width;
                        dto.Height = CurrentModuleDto.Height;
                        dto.SidePanel=CurrentModuleDto.SidePanel;
                        dto.Pallet = CurrentModuleDto.Pallet;
                    }
                }
                IsRightDrawerOpen = false;//关闭弹窗
                ShowSelectedModel = string.Empty;//清空模型选择提示信息
                Aggregator.SendMessage($"分段{CurrentModuleDto.Name} {CurrentModuleDto.ModelName}修改成功！");
            }
            else//新增ModuleDto
            {
                //todo:判断有没有重复
                CurrentModuleDto.Name= CurrentModuleDto.Name.Trim().ToUpper();
                var drawingDto =
                    DrawingDtos.FirstOrDefault(x => x.Id.Equals(CurrentModuleDto.DrawingId));
                if (drawingDto != null)
                {
                  var same=  drawingDto.ModuleDtos.Any(x=>x.Name.Equals(CurrentModuleDto.Name,StringComparison.OrdinalIgnoreCase));
                  if (same)
                  {
                      Aggregator.SendMessage($"请不要重复添加，分段{CurrentModuleDto.Name} {CurrentModuleDto.ModelName}！");
                      return;
                  }
                }
                var addResult = await _moduleService.AddAsync(CurrentModuleDto);
                if (!addResult.Status) return;
                //更新界面上分段树的显示，不关闭弹出侧边栏，只给顶部发送添加成功的消息
                drawingDto.ModuleDtos.Add(addResult.Result);
                //不关闭侧边框，初始化CurrentModuleDto，并重新绑定数据，发送提示
                Aggregator.SendMessage($"分段{CurrentModuleDto.Name} {CurrentModuleDto.ModelName}添加成功！");
                CurrentModuleDto.Id=null;//置空id，继续添加下一个分段
            }
        }
        catch (Exception e)
        {
            //发送错误报告
            Aggregator.SendMessage(e.Message);
        }
    }
    private async void Delete()
    {
        //只可以删除Module，不能删除Drawing
        if (SelectedItem is ModuleDto moduleDto)
        {
            //弹窗提示用户确定需要删除吗？
            //删除询问
            var dialogResult = await DialogHost.Question("删除确认", $"确认删除分段：{moduleDto.Name} {moduleDto.ModelName} 吗?");
            if (dialogResult.Result != ButtonResult.OK) return;
            var deleteResult = await _moduleService.DeleteAsync(moduleDto.Id.Value);
            if (!deleteResult.Status) return;
            var drawingDto =
                DrawingDtos.FirstOrDefault(x => x.Id.Equals(moduleDto.DrawingId));
            if (drawingDto == null) return;
            drawingDto.ModuleDtos.Remove(moduleDto);
        }
    }
    #endregion

    #region 初始化
    private string[] sidePanels = null!;
    public string[] SidePanels
    {
        get => sidePanels;
        set { sidePanels = value; RaisePropertyChanged(); }
    }
    private async void GetModuleTreeDataAsync()
    {
        ProjectParam param = new() { ProjectId = Project.Id };
        var moduleTreeResult = await _projectService.GetModuleTreeAsync(param);
        if (!moduleTreeResult.Status) return;
        DrawingDtos.Clear();
        DrawingDtos.AddRange(moduleTreeResult.Result);
    }

    private async void GetModelTreeDataAsync()
    {
        var modelTypeTreeResult = await _projectService.GetModelTypeTreeAsync();
        if (!modelTypeTreeResult.Status) return;
        ProductDtos.Clear();
        ProductDtos.AddRange(modelTypeTreeResult.Result);
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        //获取导航传递的参数
        Project = navigationContext.Parameters.ContainsKey("Value")
            ? navigationContext.Parameters.GetValue<ProjectDto>("Value")
            : new ProjectDto();
        Title = $"{Project.OdpNumber} - {Project.Name}";
        Url = $"http://10.9.18.31/drawings/{Project.Id}";//点击链接打开网页端
        Sbu = Enum.GetNames(typeof(Sbu_e));
        GetModuleTreeDataAsync();
        GetModelTreeDataAsync();
        SidePanels=Enum.GetNames(typeof(SidePanel_e));
        ModulesNavigate();
        UpdateRoles = "admin,pm,mgr,dsr";
    }
    #endregion

    #region 导航到分段列表
    private void ModulesNavigate()
    {
        var param = new NavigationParameters { { "Value", Project } };
        //将Project传递给要导航的页面
        RegionManager.Regions[PrismManager.DetailViewRegionName].RequestNavigate("ModulesView", back => { Journal = back.Context.NavigationService.Journal; }, param);

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