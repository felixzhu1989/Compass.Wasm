using Prism.Mvvm;
using System.Collections.Generic;
using MaterialDesignThemes.Wpf;
using Compass.Wpf.BatchWorks;
using System.Threading.Tasks;

namespace Compass.Wpf.ViewModels.Dialogs;

public class BatchWorksViewModel : BindableBase, IDialogHostAware
{
    #region ctor
    private readonly IBatchWorksService _batchWorksService;
    private readonly IEventAggregator _aggregator;
    public string DialogHostName { get; set; }
    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }
    public BatchWorksViewModel(IContainerProvider provider)
    {
        _batchWorksService = provider.Resolve<IBatchWorksService>();
        _aggregator= provider.Resolve<IEventAggregator>();
        //注册接收消息
        _aggregator.RegisterMessage(arg =>
        {
            Message+=$"{arg.Message}\n";
        }, Filter_e.Batch);
        SaveCommand =new DelegateCommand(Execute);
        CancelCommand=new DelegateCommand(Cancel);
    }
    #endregion

    #region 数据属性
    private List<ModuleDto> moduleDtos;
    public List<ModuleDto> ModuleDtos
    {
        get => moduleDtos;
        set { moduleDtos = value; RaisePropertyChanged(); }
    }
    private BatchWorksAction_e actionName;

    public BatchWorksAction_e ActionName
    {
        get => actionName;
        set { actionName = value; RaisePropertyChanged(); }
    }
    #endregion

    #region Progress属性
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

    #region Message
    private string message;
    public string Message
    {
        get => message;
        set
        {
            message = value;
            RaisePropertyChanged();
        }
    }
    #endregion

    
    //执行批量操作框架
    private async void Execute()
    {
        ShowProgressBar =true;
        CanBatchWorks = false;
        _aggregator.SendMessage($"正在{ActionName}...请稍候，暂时不要执行其他操作...", Filter_e.Batch);
        var startTime = DateTime.Now;//计时开始
        await Task.Delay(100);
        //开启新线程作图，防止卡死界面，todo:观察后续，如果会中途丢失sw连接，则需要切换回来卡界面
        await Task.Run(async () =>
        {
            try
            {
                switch (ActionName)
                {
                    case BatchWorksAction_e.自动作图:
                        await _batchWorksService.BatchDrawingAsync(ModuleDtos);
                        break;
                    case BatchWorksAction_e.导出DXF图:
                        await _batchWorksService.BatchExportDxfAsync(ModuleDtos);
                        break;
                    case BatchWorksAction_e.打印CutList:
                        await _batchWorksService.BatchPrintCutListAsync(ModuleDtos);
                        break;
                    case BatchWorksAction_e.打印JobCard:
                        await _batchWorksService.BatchPrintJobCardAsync(ModuleDtos);
                        break;
                }
            }
            catch (Exception ex)
            {
                _aggregator.SendMessage($"{ActionName}发生异常!\n{ex.Message}", Filter_e.Batch);
                await Task.Delay(8000);
            }
            var timeSpan = DateTime.Now - startTime;
            _aggregator.SendMessage($"耗时：{timeSpan.TotalSeconds:F2}s", Filter_e.Batch);
            _aggregator.SendMessage($"{ActionName}完成!", Filter_e.Batch);
            await Task.Delay(3000);
            CanBatchWorks = true;
            ShowProgressBar = false;
        });
        
        //关闭弹窗
        Cancel();
    }

    //取消关闭弹窗
    private void Cancel()
    {
        if (DialogHost.IsDialogOpen(DialogHostName))
            //取消时只返回No，告知操作结束
            DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
    }

    public void OnDialogOpen(IDialogParameters parameters)
    {
        ModuleDtos = parameters.ContainsKey("Value") ? parameters.GetValue<List<ModuleDto>>("Value") : new List<ModuleDto>();
        ActionName= parameters.ContainsKey("ActionName") ? parameters.GetValue<BatchWorksAction_e>("ActionName") : BatchWorksAction_e.自动作图;
        _aggregator.SendMessage($"请确认开始{ActionName}...", Filter_e.Batch);
    }
}