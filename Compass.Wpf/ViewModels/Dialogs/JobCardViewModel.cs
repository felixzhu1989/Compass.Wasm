using System.Threading.Tasks;
using Compass.Wpf.BatchWorks;
using MaterialDesignThemes.Wpf;
using Prism.Mvvm;

namespace Compass.Wpf.ViewModels.Dialogs;

public class JobCardViewModel : BindableBase, IDialogHostAware
{
    #region ctor
    private readonly IContainerProvider _provider;
    public JobCardViewModel(IContainerProvider provider)
    {
        _provider = provider;
        SaveCommand = new DelegateCommand(PrintJobCard);//使用Save当作执行打印操作
        CancelCommand =new DelegateCommand(Cancel);
        PrintLabelCommand = new DelegateCommand(PrintScreenShot);
        PrintFinalCommand = new DelegateCommand(PrintFinal);
    }

    
    public string DialogHostName { get; set; }
    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }
    public DelegateCommand PrintLabelCommand { get; set; }
    public DelegateCommand PrintFinalCommand { get; set; }
    #endregion

    #region 属性
    private ModuleDto? moduleDto;
    public ModuleDto? ModuleDto
    {
        get => moduleDto;
        set { moduleDto = value; RaisePropertyChanged(); }
    }
    #endregion
    /// <summary>
    /// 打印JobCard
    /// </summary>
    private void PrintJobCard()
    {
        var printsService = _provider.Resolve<IPrintsService>();
        Task.Run(async () => { await printsService.PrintOneJobCardAsync(ModuleDto); });
        Cancel();//打印完成后关闭dialog
    }
    /// <summary>
    /// 打印截图页
    /// </summary>
    private void PrintFinal()
    {
        //todo:待完善


        Cancel();//打印完成后关闭dialog
    }
    /// <summary>
    /// 打印最终检验
    /// </summary>
    private void PrintScreenShot()
    {
        //todo:待完善


        Cancel();//打印完成后关闭dialog
    }




    private void Cancel()
    {
        if (DialogHost.IsDialogOpen(DialogHostName))
            //取消时只返回No，告知操作结束
            DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
    }

    public void OnDialogOpen(IDialogParameters parameters)
    {
        ModuleDto = parameters.ContainsKey("Value") ? parameters.GetValue<ModuleDto>("Value") : null;
    }
}