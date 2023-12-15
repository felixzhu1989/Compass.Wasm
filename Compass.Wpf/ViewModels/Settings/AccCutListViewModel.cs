using System.Collections.Generic;
using System.IO;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Extensions;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace Compass.Wpf.ViewModels.Settings;

public class AccCutListViewModel : NavigationViewModel
{
    /* SQL
     * insert into AccCutLists (Id,AccType,PartDescription,Length,Width,Thickness,Quantity,Material,PartNo,IsDeleted,CreationTime)
    values 
    ('D22D2BD4-BE12-421C-2665-4C1A24A5D016','UCWUVRACK4S_天花UV灯','UCW特殊UVR骨架后S',804,73,1,1,'1.0 mm SS304 2B/2B','UCWUVRACK4S_6200300102',0,'2023-10-11 15:21:45.4102842')
     */
    #region ctor
    private readonly IAccCutListService _accCutListService;
    public AccCutListViewModel(IContainerProvider provider) : base(provider)
    {
        _accCutListService = provider.Resolve<IAccCutListService>();
        AccCutListDtos = new ObservableCollection<AccCutListDto>();
        OpenCsvCommand = new DelegateCommand(OpenCsv);
        UpdateCommand = new DelegateCommand(Update);
    }
    public DelegateCommand OpenCsvCommand { get; }
    public DelegateCommand UpdateCommand { get; }
    #endregion
    #region 属性
    public ObservableCollection<AccCutListDto> AccCutListDtos { get; }
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

    #region 打开CSV文件，更新列表
    private void OpenCsv()
    {
        var path = Path.Combine(Environment.CurrentDirectory, "AccCutList.csv");
        var excelApp = new Application();
        excelApp.Workbooks.Open(path);
        excelApp.Visible=true;
    }
    private async void Update()
    {
        var path = Path.Combine(Environment.CurrentDirectory, "AccCutList.csv");
        List<AccCutListCsv> accCutLists;
        try
        {
            accCutLists= path.GetRecords<AccCutListCsv>();
        }
        //StreamReader的异常
        catch (Exception e)
        {
            Aggregator.SendMessage($"请关闭Excel后再执行更新，{e.Message}");
            return;
        }
        var dtos = (await _accCutListService.GetAllAsync()).Result;
        foreach (var item in accCutLists)
        {
            //判断文件号是否相同，保证文件必须唯一
            var dto = dtos.FirstOrDefault(x => x.PartNo.Equals(item.PartNo)) ??
                      new AccCutListDto { Id = Guid.Empty };
            dto.AccType = item.AccType.ToEnum<AccType_e>();
            dto.PartDescription=item.PartDescription;
            dto.Length = item.Length.ToDouble();
            dto.Width = item.Width.ToDouble();
            dto.Thickness=item.Thickness.ToDouble();
            dto.Quantity = item.Quantity.ToInt();
            dto.Material = item.Material;
            dto.PartNo=item.PartNo;
            dto.BendingMark=item.BendingMark;

            if (dto.Id == Guid.Empty)
            {
                await _accCutListService.AddAsync(dto);
            }
            else
            {
                await _accCutListService.UpdateAsync(dto.Id.Value, dto);
            }
        }

        Aggregator.SendMessage("更新完毕！");
        GetDataAsync();
    }
    #endregion

    #region 导航初始化
    private async void GetDataAsync()
    {
        var result = await _accCutListService.GetAllAsync();
        if (!result.Status) return;
        AccCutListDtos.Clear();
        AccCutListDtos.AddRange(result.Result);
    }
    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        CanUpdateInfo = AppSession.Roles.Contains("admin");
        GetDataAsync();
    }
    #endregion
}