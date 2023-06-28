using Compass.Wasm.Shared.Categories;
using MaterialDesignThemes.Wpf;
using Prism.Mvvm;

namespace Compass.Wpf.ViewModels.Dialogs;

public class AddSplMaterialViewModel : BindableBase, IDialogHostAware
{
    #region ctor
    public readonly IEventAggregator _aggregator;
    public AddSplMaterialViewModel(IContainerProvider provider)
    {
        _aggregator = provider.Resolve<IEventAggregator>();
        SaveCommand =new DelegateCommand(Save);
        CancelCommand=new DelegateCommand(Cancel);
    }
    public string DialogHostName { get; set; }
    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }
    #endregion

    #region 属性
    private PackingItemDto packingItemDto;
    public PackingItemDto PackingItemDto
    {
        get => packingItemDto;
        set { packingItemDto = value; RaisePropertyChanged(); }
    }
    private string[] units = null!;
    public string[] Units
    {
        get => units;
        set { units = value; RaisePropertyChanged(); }
    }
    #endregion
    private void Cancel()
    {
        if (DialogHost.IsDialogOpen(DialogHostName))
            //取消时只返回No，告知操作结束
            DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
    }
    private void Save()
    {
        //验证数据已填写
        if (string.IsNullOrWhiteSpace(PackingItemDto.MtlNumber) ||
            string.IsNullOrWhiteSpace(PackingItemDto.Description))
        {
            _aggregator.SendMessage("请完善信息的填写，料号和描述必填");
            return;
        }

        if (!DialogHost.IsDialogOpen(DialogHostName)) return;
        DialogParameters param = new() { { "Value", PackingItemDto } };
        //保存时传递参数
        DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
    }

    public void OnDialogOpen(IDialogParameters parameters)
    {
        if(!parameters.ContainsKey("Value"))Cancel();
        PackingItemDto= parameters.GetValue<PackingItemDto>("Value");
        Units = Enum.GetNames(typeof(Unit_e));
    }
}