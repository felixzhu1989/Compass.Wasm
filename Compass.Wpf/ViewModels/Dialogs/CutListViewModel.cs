using Compass.Wasm.Shared.ProjectService;
using Compass.Wpf.Common;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Compass.Wasm.Shared.Parameter;
using Compass.Wpf.BatchWorks;
using Compass.Wpf.Service;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MaterialDesignThemes.Wpf;
using Prism.Ioc;

namespace Compass.Wpf.ViewModels.Dialogs;

public class CutListViewModel : BindableBase, IDialogHostAware
{
    private readonly IContainerProvider _containerProvider;
    private readonly ICutListService _cutListService;
    public string DialogHostName { get; set; }
    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }
    public DelegateCommand ExecuteCommand { get; }
    #region 数据
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

    public CutListViewModel(IContainerProvider containerProvider)
    {
        _containerProvider = containerProvider;
        _cutListService = containerProvider.Resolve<ICutListService>();
        ExecuteCommand = new DelegateCommand(Execute);

        CancelCommand=new DelegateCommand(Cancel);
    }
    //todo:是否需要手工修改Cutlist条目


    private void Execute()
    {
        //todo:打印CutList
        var printsService = _containerProvider.Resolve<IPrintsService>();
        Task.Run(async () => { await printsService.PrintOneCutListAsync(ModuleDto); });
        Cancel();
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