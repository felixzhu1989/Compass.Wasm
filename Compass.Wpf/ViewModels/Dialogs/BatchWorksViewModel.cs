using Compass.Wpf.Common;
using Prism.Mvvm;
using System.Collections.Generic;
using Prism.Commands;
using Prism.Services.Dialogs;
using MaterialDesignThemes.Wpf;
using Compass.Wpf.BatchWorks;
using Compass.Wasm.Shared.ProjectService;
using System;
using System.Threading.Tasks;

namespace Compass.Wpf.ViewModels.Dialogs;

public class BatchWorksViewModel : BindableBase, IDialogHostAware
{
    private readonly IBatchWorksService _batchWorksService;
    public string DialogHostName { get; set; }
    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }

    #region 数据
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

    public BatchWorksViewModel(IBatchWorksService batchWorksService)
    {
        _batchWorksService = batchWorksService;
        SaveCommand=new DelegateCommand(Execute);
        CancelCommand=new DelegateCommand(Cancel);
    }

    private async void Execute()
    {
        ShowProgressBar =true;
        CanBatchWorks = false;
        
        ProgressTips = $"正在{ActionName}...请稍候，暂时不要执行其他操作...";
        DateTime startTime = DateTime.Now;//计时开始
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
                ProgressTips = $"{ActionName}发生异常!\n{ex.Message}";
                await Task.Delay(5000);
            }
            TimeSpan timeSpan = DateTime.Now - startTime;
            progressTips = $"{progressTips}\n耗时：{timeSpan.TotalSeconds:F2}s";
            ProgressTips = $"{ActionName}完成!";
            await Task.Delay(3000);
            CanBatchWorks = true;
            ShowProgressBar = false;
        });
        
        //关闭弹窗
        Cancel();
    }

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

        ProgressTips = $"请确认开始{ActionName}";
    }

}