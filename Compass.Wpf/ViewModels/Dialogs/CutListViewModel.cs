using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Prism.Mvvm;
using Compass.Wasm.Shared.Params;

namespace Compass.Wpf.ViewModels.Dialogs;

public class CutListViewModel : BindableBase, IDialogHostAware
{
    #region ctor
    private readonly IContainerProvider _provider;
    private readonly ICutListService _cutListService;
    public readonly IEventAggregator _aggregator;//事件聚合器
    public CutListViewModel(IContainerProvider provider)
    {
        _provider = provider;
        _cutListService = provider.Resolve<ICutListService>();
        _aggregator= _provider.Resolve<EventAggregator>();

        CurrentCutList =new CutListDto();
        CutListDtos = new ObservableCollection<CutListDto>();


        SaveCommand = new DelegateCommand(Execute);//使用Save当作执行打印操作
        CancelCommand=new DelegateCommand(Cancel);
        UpdateItemCommand = new DelegateCommand<CutListDto>(UpdateItem);
        ExecuteItemCommand = new DelegateCommand<string>(ExecuteItem);
    }
    
    public string DialogHostName { get; set; } = "RootDialog";
    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }
    public DelegateCommand<CutListDto> UpdateItemCommand { get; }
    public DelegateCommand<string> ExecuteItemCommand { get; }
    #endregion

    #region 数据属性
    private string title;
    public string Title
    {
        get => title;
        set { title = value; RaisePropertyChanged(); }
    }
    private ModuleDto? moduleDto;
    public ModuleDto? ModuleDto
    {
        get => moduleDto;
        set { moduleDto = value; RaisePropertyChanged(); }
    }
    private ObservableCollection<CutListDto> cutListDtos;
    public ObservableCollection<CutListDto> CutListDtos
    {
        get => cutListDtos;
        set { cutListDtos = value; RaisePropertyChanged(); }
    }

    private bool isRightDrawerOpen;
    /// <summary>
    /// 右侧窗口是否展开
    /// </summary>
    public bool IsRightDrawerOpen
    {
        get => isRightDrawerOpen;
        set { isRightDrawerOpen = value; RaisePropertyChanged(); }
    }

    private CutListDto currentCutList;

    public CutListDto CurrentCutList
    {
        get => currentCutList;
        set { currentCutList = value;RaisePropertyChanged(); }
    }
    #endregion


    #region 主界面命令
    /// <summary>
    /// 执行打印CutList
    /// </summary>
    private void Execute()
    {
        //todo:打印CutList
        var printsService = _provider.Resolve<IPrintsService>();
        Task.Run(async () => { await printsService.PrintOneCutListAsync(ModuleDto); });
        Cancel();//打印完成后关闭dialog
    }

    private void Cancel()
    {
        if (DialogHost.IsDialogOpen(DialogHostName))
            //取消时只返回No，告知操作结束
            DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
    }
    #endregion


    #region 手工修改Cutlist条目
    //弹出修改界面
    private void UpdateItem(CutListDto dto)
    {
        IsRightDrawerOpen = true;
        CurrentCutList= dto;
    }
    private void AddItem()
    {
        IsRightDrawerOpen = true;
        CurrentCutList= new CutListDto
        {
            ModuleId = ModuleDto.Id.Value,
            Thickness = 1,
            Material = "1.0 mm SS304 4B/2B",
        };
    }
    private void CancelItem()
    {
        IsRightDrawerOpen = false;
    }
    private async void ExecuteItem(string obj)
    {
        switch (obj)
        {
            case "AddItem": AddItem();break;
            case "CancelItem": CancelItem();break;
            case "SaveItem":await SaveItem();break;
            case "DeleteItem":await DeleteItem();break;
        }
    }
    //todo：以后再优化_aggregator和删除，目前_aggregator暂时无法提示信息
    private async Task SaveItem()
    {
        //请选择模型，或者分段名称不能为空
        if (string.IsNullOrWhiteSpace(CurrentCutList.PartDescription) ||string.IsNullOrWhiteSpace(CurrentCutList.PartNo))
        {
            //发送提示
            _aggregator.SendMessage("请填写PartDescription零件描述和PartNo零件名称",Filter_e.CutList);
            return;
        }

        if (CurrentCutList.Id !=null && CurrentCutList.Id != Guid.Empty) //编辑
        {
            var updateResult = await _cutListService.UpdateAsync(CurrentCutList.Id.Value, CurrentCutList);
            //更新界面
            if (updateResult.Status)
            {
                _aggregator.SendMessage($"{CurrentCutList.PartDescription} {CurrentCutList.PartNo}修改成功！");

                await RefreshCutListAsync();

                IsRightDrawerOpen = false; //关闭弹窗
            }
        }
        else //新增
        {
            var addResult = await _cutListService.AddAsync(CurrentCutList);
            if (addResult.Status)
            {
                _aggregator.SendMessage($"{CurrentCutList.PartDescription} {CurrentCutList.PartNo}添加成功！");

                await RefreshCutListAsync();

                IsRightDrawerOpen = false; //关闭弹窗
            }
        }

    }
    //删除
    private async Task DeleteItem()
    { 
        await _cutListService.DeleteAsync(CurrentCutList.Id.Value);
        await RefreshCutListAsync();
        IsRightDrawerOpen = false; //关闭弹窗
    }
    #endregion

    #region 初始化
    public async void OnDialogOpen(IDialogParameters parameters)
    {
        ModuleDto = parameters.ContainsKey("Value") ? parameters.GetValue<ModuleDto>("Value") : null;
        if (ModuleDto != null)
        {
            Title = $"{ModuleDto.OdpNumber} / {ModuleDto.ProjectName} ( {ModuleDto.ItemNumber} / {ModuleDto.Name} / {ModuleDto.ModelName} ) ({ModuleDto.Length} x {ModuleDto.Width} x {ModuleDto.Height})";
            await RefreshCutListAsync();
        }
    }

    public async Task RefreshCutListAsync()
    {
        var param = new CutListParam
        {
            ModuleId = ModuleDto.Id.Value
        };
        var result = await _cutListService.GetAllByModuleIdAsync(param);
        if (result.Status)
        {
            CutListDtos =new ObservableCollectionListSource<CutListDto>(result.Result);
        }
    } 
    #endregion
}