using Compass.Wasm.Shared.Parameter;
using Compass.Wasm.Shared.ProjectService;
using Compass.Wpf.BatchWorks;
using Compass.Wpf.Common;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Compass.Wpf.ApiService;

namespace Compass.Wpf.ViewModels.Dialogs;

public class CutListViewModel : BindableBase, IDialogHostAware
{
    #region ctor
    private readonly IContainerProvider _provider;
    private readonly ICutListService _cutListService;
    public CutListViewModel(IContainerProvider provider)
    {
        _provider = provider;
        _cutListService = provider.Resolve<ICutListService>();
        SaveCommand = new DelegateCommand(Execute);//使用Save当作执行打印操作
        CancelCommand=new DelegateCommand(Cancel);
    }
    public string DialogHostName { get; set; } = "RootDialog";
    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }
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
    #endregion
    
    //todo:是否需要手工修改Cutlist条目
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

    public async void OnDialogOpen(IDialogParameters parameters)
    {
        ModuleDto = parameters.ContainsKey("Value") ? parameters.GetValue<ModuleDto>("Value") : null;
        if (ModuleDto != null)
        {
            Title = $"{ModuleDto.OdpNumber} / {ModuleDto.ProjectName} ( {ModuleDto.ItemNumber} / {ModuleDto.Name} / {ModuleDto.ModelName} ) ({ModuleDto.Length} x {ModuleDto.Width} x {ModuleDto.Height})";
            CutListParameter param = new CutListParameter
            {
                ModuleId = ModuleDto.Id.Value
            };
            var result = await _cutListService.GetAllByModuleIdAsync(param);
            if (result.Status)
            {
                CutListDtos =new ObservableCollectionListSource<CutListDto>(result.Result);
            }
        }
    }
}