﻿using Compass.Wpf.BatchWorks;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Prism.Mvvm;
using System.Threading.Tasks;
using Compass.Wasm.Shared.Params;
using Compass.Wpf.ApiServices.Projects;
using Compass.Wasm.Shared.Projects;

namespace Compass.Wpf.ViewModels.Dialogs;

public class CutListViewModel : BindableBase, IDialogHostAware
{
    #region ctor
    private readonly IContainerProvider _provider;
    private readonly ICutListService _cutListService;
    public readonly IDialogHostService _dialogHost;
    public CutListViewModel(IContainerProvider provider)
    {
        _provider = provider;
        _cutListService = provider.Resolve<ICutListService>();
        _dialogHost = provider.Resolve<IDialogHostService>();
        SaveCommand = new DelegateCommand(Execute);//使用Save当作执行打印操作
        CancelCommand=new DelegateCommand(Cancel);
        UpdateItem = new DelegateCommand<CutListDto>(UpdateCutListItem);
        DeleteItem = new DelegateCommand<CutListDto>(DeleteCutListItem);
    }

    

    public string DialogHostName { get; set; } = "RootDialog";
    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }
    public DelegateCommand<CutListDto> UpdateItem { get; }
    public DelegateCommand<CutListDto> DeleteItem { get; }
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

    //弹出修改界面
    private void UpdateCutListItem(CutListDto dto)
    {
        IsRightDrawerOpen = true;
    }
    //删除
    private async void DeleteCutListItem(CutListDto obj)
    {
        CutListDtos.Remove(obj);
        await _cutListService.DeleteAsync(obj.Id.Value);
    }

    public async void OnDialogOpen(IDialogParameters parameters)
    {
        ModuleDto = parameters.ContainsKey("Value") ? parameters.GetValue<ModuleDto>("Value") : null;
        if (ModuleDto != null)
        {
            Title = $"{ModuleDto.OdpNumber} / {ModuleDto.ProjectName} ( {ModuleDto.ItemNumber} / {ModuleDto.Name} / {ModuleDto.ModelName} ) ({ModuleDto.Length} x {ModuleDto.Width} x {ModuleDto.Height})";
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
    }
}