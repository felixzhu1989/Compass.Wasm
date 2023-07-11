using System.Collections.Generic;
using System.IO;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Extensions;
using Compass.Wpf.ApiServices.Categories;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace Compass.Wpf.ViewModels;

public class MaterialItemsViewModel : NavigationViewModel
{
    #region ctor
    private readonly IMaterialItemService _materialItemService;
    public MaterialItemsViewModel(IContainerProvider provider) : base(provider)
    {
        _materialItemService = provider.Resolve<IMaterialItemService>();

        MaterialItemDtos = new ObservableCollection<MaterialItemDto>();
        OpenCsvCommand = new DelegateCommand(OpenCsv);
        UpdateCommand = new DelegateCommand<string>(Update);
    }
    public DelegateCommand OpenCsvCommand { get; }
    public DelegateCommand<string> UpdateCommand { get; }
    #endregion

    #region 属性
    private ObservableCollection<MaterialItemDto> materialItemDtos;
    public ObservableCollection<MaterialItemDto> MaterialItemDtos
    {
        get => materialItemDtos;
        set { materialItemDtos = value; RaisePropertyChanged(); }
    }
    private string search;
    /// <summary>
    /// 搜索条件属性
    /// </summary>
    public string Search
    {
        get => search;
        set { search = value; RaisePropertyChanged(); }
    }

    private bool canUpdateInfo;
    /// <summary>
    /// 管理员才能更新
    /// </summary>
    public bool CanUpdateInfo
    {
        get => canUpdateInfo;
        set { canUpdateInfo = value; RaisePropertyChanged(); }
    }





    #endregion



    private void OpenCsv()
    {
        var path = Path.Combine(Environment.CurrentDirectory, "MaterialItem.csv");
        var excelApp = new Application();
        excelApp.Workbooks.Open(path);
        excelApp.Visible=true;
    }

    private async void Update(string obj)
    {
        var path = Path.Combine(Environment.CurrentDirectory, "MaterialItem.csv");
        List<MaterialItemCsv> mtlItems;
        try
        {
            mtlItems= path.GetRecords<MaterialItemCsv>();
        }
        //StreamReader的异常
        catch (Exception e)
        {
            Aggregator.SendMessage($"请关闭Excel后再执行更新，{e.Message}");
            return;
        }
        var dtos = (await _materialItemService.GetAllAsync()).Result;
        foreach (var itemDto in mtlItems)
        {
            //判断编号是否相同
            var dto = dtos.FirstOrDefault(x => x.MtlNumber.Equals(itemDto.MtlNumber)) ?? new MaterialItemDto{Id = Guid.Empty};
            switch (obj)
            {
                case "Info"://更新基本信息，没有则添加，有则修改
                    dto.MtlNumber=itemDto.MtlNumber;
                    dto.Description=itemDto.Description;
                    dto.EnDescription=itemDto.EnDescription;
                    dto.Type=itemDto.Type;
                    dto.Unit=itemDto.Unit.ToEnum<Unit_e>();
                    if (dto.Id == Guid.Empty)
                    {
                        await _materialItemService.AddAsync(dto);
                    }
                    else
                    {
                        await _materialItemService.UpdateAsync(dto.Id.Value, dto);
                    }
                    break;
                case "Inventory":
                    if (dto.Id != Guid.Empty)
                    {
                        dto.Inventory=itemDto.Inventory.ToDouble();
                        dto.UnitCost = itemDto.UnitCost.ToDouble();
                        await _materialItemService.UpdateInventoryAsync(dto.Id.Value, dto);
                    }
                    break;
                case "Other":
                    if (dto.Id != Guid.Empty)
                    {
                        dto.Length=itemDto.Length;
                        dto.Width = itemDto.Width;
                        dto.Height = itemDto.Height;
                        dto.Material=itemDto.Material;
                        dto.Hood=itemDto.Hood.ToBool();
                        dto.HoodGroup = itemDto.HoodGroup.ToEnum<HoodGroup_e>();
                        dto.Ceiling = itemDto.Ceiling.ToBool();
                        dto.CeilingGroup = itemDto.CeilingGroup.ToEnum<CeilingGroup_e>();
                        dto.CeilingRule = itemDto.CeilingRule.ToEnum<CeilingRule_e>();
                        dto.CalcRule=itemDto.CalcRule;
                        dto.NoLabel=itemDto.NoLabel;
                        dto.OneLabel=itemDto.OneLabel;
                        dto.Order = itemDto.Order.ToInt();
                        await _materialItemService.UpdateOtherAsync(dto.Id.Value, dto);
                    }
                    break;
            }
        }

        Aggregator.SendMessage("更新完毕！");
        GetDataAsync();
    }

    #region 导航初始化
    private async void GetDataAsync()
    {
        var result = await _materialItemService.GetTop50Async();
        if (!result.Status) return;
        MaterialItemDtos.Clear();
        MaterialItemDtos.AddRange(result.Result);
    }
    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        CanUpdateInfo = AppSession.Roles.Contains("admin");
        GetDataAsync();
    }
    #endregion
}