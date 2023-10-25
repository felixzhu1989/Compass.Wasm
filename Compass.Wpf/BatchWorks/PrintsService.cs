using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Compass.Wasm.Shared.Params;
using Application = Microsoft.Office.Interop.Excel.Application;
using Range = Microsoft.Office.Interop.Excel.Range;
using Worksheet = Microsoft.Office.Interop.Excel.Worksheet;
using ImTools;

namespace Compass.Wpf.BatchWorks;

public interface IPrintsService
{
    Task BatchPrintCutListAsync(List<ModuleDto> moduleDtos);
    Task PrintOneCutListAsync(ModuleDto moduleDto);

    Task BatchPrintJobCardAsync(List<ModuleDto> moduleDtos);
    Task BatchPrintEnJobCardAsync(List<ModuleDto> moduleDtos);
    Task PrintOneJobCardAsync(ModuleDto moduleDto);

    Task BatchPrintFinalAsync(List<ModuleDto> moduleDtos);
    Task PrintOneFinalAsync(ModuleDto moduleDto);
    Task BatchPrintEnFinalAsync(List<ModuleDto> moduleDtos);
    Task PrintOneEnFinalAsync(ModuleDto moduleDto);
    Task BatchPrintEnScreenShotAsync(List<ModuleDto> moduleDtos);

    Task PrintPackingListAsync(PackingListDto packingListDto);
    
    Task ExportPackingInfoAsync(PackingListDto packingListDto);
}


public class PrintsService : IPrintsService
{
    #region ctor
    private readonly IContainerProvider _provider;
    private readonly IEventAggregator _aggregator;
    private readonly IModuleService _moduleService;
    private readonly IFileUploadService _fileUploadService;
    public PrintsService(IContainerProvider provider)
    {
        _provider = provider;
        _aggregator= provider.Resolve<IEventAggregator>();
        _moduleService = provider.Resolve<IModuleService>();
        _fileUploadService = provider.Resolve<IFileUploadService>();
    }
    #endregion

    #region 公共方法实现
    /// <summary>
    /// 批量打印CutList
    /// </summary>
    public async Task BatchPrintCutListAsync(List<ModuleDto> moduleDtos)
    {
        var cutListService = _provider.Resolve<ICutListService>();
        var template = Path.Combine(Environment.CurrentDirectory, "CutList.xlsx");

        //如果报错就添加COM引用，Microsoft Office 16.0 Object Library1.9
        var excelApp = new Application();
        excelApp.Workbooks.Add(template);
        var worksheet = (Worksheet)excelApp.Worksheets[1];

        foreach (var moduleDto in moduleDtos)
        {
            if (!moduleDto.IsCutListOk)
            {
                _aggregator.SendMessage($"{moduleDto.ItemNumber}-{moduleDto.Name}-{moduleDto.ModelName}\t******CutList不OK，跳过打印******", Filter_e.Batch);
                continue;
            }
            _aggregator.SendMessage($"正在打印:\t{moduleDto.ItemNumber}-{moduleDto.Name}-{moduleDto.ModelName}", Filter_e.Batch);
            await GetCutListAndPrint(worksheet, moduleDto, cutListService);
        }

        KillProcess(excelApp);
        excelApp = null;//对象置空
        GC.Collect(); //垃圾回收机制
    }

    /// <summary>
    /// 单独打印CutList
    /// </summary>
    public async Task PrintOneCutListAsync(ModuleDto moduleDto)
    {
        var cutListService = _provider.Resolve<ICutListService>();
        var template = Path.Combine(Environment.CurrentDirectory, "CutList.xlsx");

        //如果报错就添加COM引用，Microsoft Office 16.0 Object Library1.9
        var excelApp = new Application();
        excelApp.Workbooks.Add(template);
        var worksheet = (Worksheet)excelApp.Worksheets[1];
        //调试时预览
        //excelApp.Visible = true;

        await GetCutListAndPrint(worksheet, moduleDto, cutListService);

        KillProcess(excelApp);
        excelApp = null;//对象置空
        GC.Collect(); //垃圾回收机制
    }

    /// <summary>
    /// 批量打印JobCard
    /// </summary>
    public async Task BatchPrintJobCardAsync(List<ModuleDto> moduleDtos)
    {
        var template = Path.Combine(Environment.CurrentDirectory, "JobCard.xlsx");
        //如果报错就添加COM引用，Microsoft Office 16.0 Object Library1.9
        var excelApp = new Application();
        excelApp.Workbooks.Add(template);
        var worksheet = (Worksheet)excelApp.Worksheets[1];

        foreach (var moduleDto in moduleDtos)
        {
            if (!moduleDto.IsJobCardOk)
            {
                _aggregator.SendMessage($"{moduleDto.ItemNumber}-{moduleDto.Name}-{moduleDto.ModelName}\t******JobCard不OK，跳过打印******", Filter_e.Batch);
                continue;
            }
            _aggregator.SendMessage($"正在打印:\t{moduleDto.ItemNumber}-{moduleDto.Name}-{moduleDto.ModelName}", Filter_e.Batch);
            await UseExcelPrintJobCard(worksheet, moduleDto);
        }

        KillProcess(excelApp);
        excelApp = null;//对象置空
        GC.Collect(); //垃圾回收机制
    }
    /// <summary>
    /// 批量打印英文JobCard
    /// </summary>
    public async Task BatchPrintEnJobCardAsync(List<ModuleDto> moduleDtos)
    {
        var template = Path.Combine(Environment.CurrentDirectory, "JobCardEn.xlsx");
        //如果报错就添加COM引用，Microsoft Office 16.0 Object Library1.9
        var excelApp = new Application();
        excelApp.Workbooks.Add(template);
        var worksheet = (Worksheet)excelApp.Worksheets[1];

        foreach (var moduleDto in moduleDtos)
        {
            if (!moduleDto.IsJobCardOk)
            {
                _aggregator.SendMessage($"{moduleDto.ItemNumber}-{moduleDto.Name}-{moduleDto.ModelName}\t******JobCard不OK，跳过打印******", Filter_e.Batch);
                continue;
            }
            _aggregator.SendMessage($"正在打印:\t{moduleDto.ItemNumber}-{moduleDto.Name}-{moduleDto.ModelName}", Filter_e.Batch);
            await UseExcelPrintJobCard(worksheet, moduleDto);
        }

        KillProcess(excelApp);
        excelApp = null;//对象置空
        GC.Collect(); //垃圾回收机制
    }


    /// <summary>
    /// 单独打印
    /// </summary>
    public async Task PrintOneJobCardAsync(ModuleDto moduleDto)
    {
        var template = Path.Combine(Environment.CurrentDirectory, "JobCard.xlsx");
        //如果报错就添加COM引用，Microsoft Office 16.0 Object Library1.9
        var excelApp = new Application();
        excelApp.Workbooks.Add(template);
        var worksheet = (Worksheet)excelApp.Worksheets[1];
        //调试时预览
        //excelApp.Visible = true;

        await UseExcelPrintJobCard(worksheet, moduleDto);

        KillProcess(excelApp);
        excelApp = null;//对象置空
        GC.Collect(); //垃圾回收机制
    }


    /// <summary>
    /// 批量打印最终检验
    /// </summary>
    public async Task BatchPrintFinalAsync(List<ModuleDto> moduleDtos)
    {
        var template = Path.Combine(Environment.CurrentDirectory, "FinalInspection.xlsx");
        //如果报错就添加COM引用，Microsoft Office 16.0 Object Library1.9
        var excelApp = new Application();
        excelApp.Workbooks.Add(template);
        var worksheet = (Worksheet)excelApp.Worksheets[1];

        foreach (var moduleDto in moduleDtos)
        {
            if (!moduleDto.IsJobCardOk)
            {
                _aggregator.SendMessage($"{moduleDto.ItemNumber}-{moduleDto.Name}-{moduleDto.ModelName}\t******JobCard不OK，跳过打印******", Filter_e.Batch);
                continue;
            }
            _aggregator.SendMessage($"正在打印:\t{moduleDto.ItemNumber}-{moduleDto.Name}-{moduleDto.ModelName}", Filter_e.Batch);
            await UseExcelPrintFinal(worksheet, moduleDto);
        }

        KillProcess(excelApp);
        excelApp = null;//对象置空
        GC.Collect(); //垃圾回收机制
    }

    /// <summary>
    /// 单独打印最终检验
    /// </summary>
    public async Task PrintOneFinalAsync(ModuleDto moduleDto)
    {
        var template = Path.Combine(Environment.CurrentDirectory, "FinalInspection.xlsx");
        //如果报错就添加COM引用，Microsoft Office 16.0 Object Library1.9
        var excelApp = new Application();
        excelApp.Workbooks.Add(template);
        var worksheet = (Worksheet)excelApp.Worksheets[1];
        //调试时预览
        //excelApp.Visible = true;

        await UseExcelPrintFinal(worksheet, moduleDto);

        KillProcess(excelApp);
        excelApp = null;//对象置空
        GC.Collect(); //垃圾回收机制
    }

    /// <summary>
    /// 批量打印英文最终检验
    /// </summary>
    public async Task BatchPrintEnFinalAsync(List<ModuleDto> moduleDtos)
    {
        var template = Path.Combine(Environment.CurrentDirectory, "FinalInspectionEn.xlsx");
        //如果报错就添加COM引用，Microsoft Office 16.0 Object Library1.9
        var excelApp = new Application();
        excelApp.Workbooks.Add(template);
        var worksheet = (Worksheet)excelApp.Worksheets[1];

        foreach (var moduleDto in moduleDtos)
        {
            if (!moduleDto.IsJobCardOk)
            {
                _aggregator.SendMessage($"{moduleDto.ItemNumber}-{moduleDto.Name}-{moduleDto.ModelName}\t******JobCard不OK，跳过打印******", Filter_e.Batch);
                continue;
            }
            _aggregator.SendMessage($"正在打印:\t{moduleDto.ItemNumber}-{moduleDto.Name}-{moduleDto.ModelName}", Filter_e.Batch);
            await UseExcelPrintFinal(worksheet, moduleDto);
        }

        KillProcess(excelApp);
        excelApp = null;//对象置空
        GC.Collect(); //垃圾回收机制
    }

    /// <summary>
    /// 单独打印英文最终检验
    /// </summary>
    public async Task PrintOneEnFinalAsync(ModuleDto moduleDto)
    {
        var template = Path.Combine(Environment.CurrentDirectory, "FinalInspectionEn.xlsx");
        //如果报错就添加COM引用，Microsoft Office 16.0 Object Library1.9
        var excelApp = new Application();
        excelApp.Workbooks.Add(template);
        var worksheet = (Worksheet)excelApp.Worksheets[1];
        //调试时预览
        //excelApp.Visible = true;

        await UseExcelPrintFinal(worksheet, moduleDto);


        KillProcess(excelApp);
        excelApp = null;//对象置空
        GC.Collect(); //垃圾回收机制
    }

    /// <summary>
    /// 批量打印英文最终检验
    /// </summary>
    public async Task BatchPrintEnScreenShotAsync(List<ModuleDto> moduleDtos)
    {
        var template = Path.Combine(Environment.CurrentDirectory, "ScreenShotEn.xlsx");
        //如果报错就添加COM引用，Microsoft Office 16.0 Object Library1.9
        var excelApp = new Application();
        excelApp.Workbooks.Add(template);
        var worksheet = (Worksheet)excelApp.Worksheets[1];

        foreach (var moduleDto in moduleDtos)
        {
            if (!moduleDto.IsJobCardOk)
            {
                _aggregator.SendMessage($"{moduleDto.ItemNumber}-{moduleDto.Name}-{moduleDto.ModelName}\t******JobCard不OK，跳过打印******", Filter_e.Batch);
                continue;
            }
            _aggregator.SendMessage($"正在打印:\t{moduleDto.ItemNumber}-{moduleDto.Name}-{moduleDto.ModelName}", Filter_e.Batch);
            await UseExcelPrintScreenShot(worksheet, moduleDto);
        }

        KillProcess(excelApp);
        excelApp = null;//对象置空
        GC.Collect(); //垃圾回收机制
    }


    /// <summary>
    /// 打印装箱清单
    /// </summary>
    public async Task PrintPackingListAsync(PackingListDto packingListDto)
    {
        var tempPath = packingListDto.Product == Product_e.Hood ? "PackingListHood.xlsx" : "PackingListCeiling.xlsx";
        var template = Path.Combine(Environment.CurrentDirectory, tempPath);
        //如果报错就添加COM引用，Microsoft Office 16.0 Object Library1.9
        var excelApp = new Application();
        excelApp.Workbooks.Add(template);
        var worksheet = (Worksheet)excelApp.Worksheets[1];
        //调试时预览
        //excelApp.Visible = true;

        if (packingListDto.Product == Product_e.Hood)
        {
            await UseExcelPrintPackingListHood(worksheet, packingListDto);
        }
        else
        {
            await UseExcelPrintPackingListCeiling(worksheet, packingListDto);
        }
        
        KillProcess(excelApp);
        excelApp = null;//对象置空
        GC.Collect(); //垃圾回收机制
    }

    /// <summary>
    /// 导出装箱信息表Excel
    /// </summary>
    public async Task ExportPackingInfoAsync(PackingListDto packingListDto)
    {
        
        var template = Path.Combine(Environment.CurrentDirectory, "PackingInfo.xlsx");
        //如果报错就添加COM引用，Microsoft Office 16.0 Object Library1.9
        var excelApp = new Application();
        excelApp.Workbooks.Add(template);
        var worksheet = (Worksheet)excelApp.Worksheets[1];
        
        excelApp.Application.DisplayAlerts = false;

        await UseExcelExportPackingInfo(worksheet, packingListDto);

        //保存Excel到桌面
        var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        var odp = packingListDto.ProjectName.Split('-')[0];
        var fileName = Path.Combine(desktop, $"{odp} PackingInfo装箱信息表.xlsx");
        var workBook = excelApp.ActiveWorkbook;
        workBook.SaveAs(fileName);

        excelApp.Application.DisplayAlerts = true;
        excelApp.Visible = true;
    }

    #endregion

    #region Excel打印内部实现

    #region CutList
    private async Task GetCutListAndPrint(Worksheet worksheet, ModuleDto moduleDto, ICutListService cutListService)
    {
        var param = new CutListParam
        {
            ModuleId = moduleDto.Id.Value
        };
        var result = await cutListService.GetAllByModuleIdAsync(param);
        if (result.Status)
        {
            UseExcelPrintCutList(worksheet, moduleDto, result.Result);
        }
    }

    private void UseExcelPrintCutList(Worksheet worksheet, ModuleDto moduleDto, List<CutListDto> cutListDtos)
    {

        FillCutListTitle(worksheet, moduleDto);

        FillCutListData(worksheet, cutListDtos);
        //调试时预览
        //worksheet.PrintPreview(true);
        //打印
        worksheet.PrintOutEx();
        //清空打印内容,5行到末尾
        var rows = (Range)worksheet.Rows[$"5:{cutListDtos.Count + 4}", Missing.Value];
        rows.Delete(XlDirection.xlDown);
    }

    /// <summary>
    /// 填写CutList项目信息
    /// </summary>
    /// <param name="worksheet"></param>
    /// <param name="moduleDto"></param>
    private void FillCutListTitle(Worksheet worksheet, ModuleDto moduleDto)
    {
        //worksheet.Cells[行,列]
        worksheet.Cells[1, 2] = $"{moduleDto.OdpNumber}-{moduleDto.ProjectName}";
        worksheet.Cells[2, 2] = $"{moduleDto.ItemNumber}({moduleDto.Name})";
        worksheet.Cells[2, 6] = $"{moduleDto.ModelName}({moduleDto.Length}x{moduleDto.Width}x{moduleDto.Height})";
        worksheet.Cells[1, 10] = DateTime.Now.ToShortDateString();
        worksheet.Cells[2, 10] = Environment.UserName;
    }

    /// <summary>
    /// 填写Cutlist信息
    /// </summary>
    /// <param name="worksheet"></param>
    /// <param name="cutListDtos"></param>
    private void FillCutListData(Worksheet worksheet, List<CutListDto> cutListDtos)
    {
        for (var i = 0; i < cutListDtos.Count; i++)
        {
            worksheet.Cells[i + 5, 1] = cutListDtos[i].PartDescription;
            worksheet.Cells[i + 5, 2] = cutListDtos[i].Length;
            worksheet.Cells[i + 5, 3] = cutListDtos[i].Width;
            worksheet.Cells[i + 5, 4] = cutListDtos[i].Thickness;
            worksheet.Cells[i + 5, 5] = cutListDtos[i].Quantity;
            worksheet.Cells[i + 5, 6] = cutListDtos[i].Material;
            worksheet.Cells[i + 5, 7] = cutListDtos[i].PartNo;
            worksheet.Cells[i + 5, 11] = cutListDtos[i].Index;
            worksheet.Cells[i + 5, 9] = cutListDtos[i].BendingMark;//折弯尺寸备注
        }
        //设置边框
        Range range = worksheet.Range["A5", $"K{cutListDtos.Count + 4}"];
        range.Borders.Value = 1;
        //设置列宽
        ((Range)worksheet.Cells[1, 1]).ColumnWidth = 30;
        ((Range)worksheet.Cells[1, 2]).ColumnWidth = 10;
        ((Range)worksheet.Cells[1, 3]).ColumnWidth = 10;
        ((Range)worksheet.Cells[1, 4]).ColumnWidth = 5;
        ((Range)worksheet.Cells[1, 5]).ColumnWidth = 5;
        ((Range)worksheet.Cells[1, 6]).ColumnWidth = 28;
        ((Range)worksheet.Cells[1, 7]).ColumnWidth = 30;
        ((Range)worksheet.Cells[1, 8]).ColumnWidth = 8;
        ((Range)worksheet.Cells[1, 9]).ColumnWidth = 8;
        ((Range)worksheet.Cells[1, 10]).ColumnWidth = 8;
        ((Range)worksheet.Cells[1, 11]).ColumnWidth = 5;
    }
    #endregion

    #region JobCard
    private async Task UseExcelPrintJobCard(Worksheet worksheet, ModuleDto moduleDto)
    {
        //worksheet.Cells[行,列]
        worksheet.Cells[3, 3] = moduleDto.OdpNumber;
        worksheet.Cells[4, 3] = moduleDto.ProjectName;
        worksheet.Cells[5, 3] = $"{moduleDto.ItemNumber} {moduleDto.Name}";
        worksheet.Cells[6, 3] = moduleDto.ModelName.Split('_')[0];
        worksheet.Cells[7, 3] = moduleDto.ProjectType.ToString();

        worksheet.Cells[11, 7] = moduleDto.DeliveryDate.ToShortDateString();//发货时间

        worksheet.Cells[18, 4] = moduleDto.Name;
        worksheet.Cells[18, 5] = moduleDto.Length;
        worksheet.Cells[18, 6] = moduleDto.Width;
        worksheet.Cells[18, 7] = moduleDto.Height;
        worksheet.Cells[18, 8] = moduleDto.SidePanel.ToString();

        worksheet.Cells[21, 2] = moduleDto.SpecialNotes;
        worksheet.Cells[23, 2] = moduleDto.ProjectSpecialNotes;


        //插入Item图片
        if (!string.IsNullOrEmpty(moduleDto.ImageUrl))
        {
            //保存图片到系统目录中
            await DownloadImage(moduleDto.ImageUrl);

            var imagePath = Path.Combine(Environment.CurrentDirectory, "label.png");
            //将图片插入excel
            worksheet.Shapes.AddPicture(imagePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 3563, 610, 280);//左，上，宽度，高度4240-678
            worksheet.Shapes.AddPicture(imagePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 4015, 610, 280);//左，上，宽度，高度4692-678
        }

        var qrCodePath =await CreateQrCodeImage(moduleDto);

        //wpf imageSource
        //var bs = Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        //imageQRCode.Source = bs;

        //将图片插入excel
        worksheet.Shapes.AddPicture(qrCodePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 560, 35,63, 63);//左，上，宽度，高度
        worksheet.Shapes.AddPicture(qrCodePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 560, 926, 63, 63);
        worksheet.Shapes.AddPicture(qrCodePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 560, 1736, 63, 63);
        worksheet.Shapes.AddPicture(qrCodePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 560, 2564, 63, 63);
        worksheet.Shapes.AddPicture(qrCodePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 560, 3455, 63, 63);//4132-678
        worksheet.Shapes.AddPicture(qrCodePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 560, 3906, 63, 63);//4583-678


        //调试时预览
        //worksheet.PrintPreview(true);
        //打印
        worksheet.PrintOutEx();
    }

    private async Task<string> CreateQrCodeImage(ModuleDto moduleDto)
    {
        //生成二维码插入Excel并上传
        var content = moduleDto.Id.ToString();
        var bmp = content.GetQrCodeBitmap();//获取QrCode的扩展方法
        var qrCodePath = Path.Combine(Environment.CurrentDirectory, "moduleidqrcode.png");
        bmp.Save(qrCodePath);

        //判断module的qrcode是否存在，不存在则上传，存在则不上传
        if (!string.IsNullOrEmpty(moduleDto.ImageUrl)) return qrCodePath;

        var result = await _fileUploadService.Upload(qrCodePath);
        moduleDto.QrCodeUrl = result.RemoteUrl.ToString();
        await _moduleService.UpdateAsync(moduleDto.Id.Value, moduleDto);

        return qrCodePath;
    }

    /// <summary>
    /// 从网络下载图片保存在系统目录
    /// </summary>
    private async Task DownloadImage(string imageUrl)
    {
        var client = new HttpClient();
        var imageByte = await client.GetByteArrayAsync(imageUrl);
        await using var stream = new FileStream("label.png", FileMode.Create);
        await stream.WriteAsync(imageByte, 0, imageByte.Length);
        stream.Close();
    }
    #endregion
    
    #region 最终检验

    private async Task UseExcelPrintFinal(Worksheet worksheet, ModuleDto moduleDto)
    {
        worksheet.Cells[3, 3] = moduleDto.OdpNumber;
        worksheet.Cells[4, 3] = moduleDto.ProjectName;
        worksheet.Cells[5, 3] = $"{moduleDto.ItemNumber} {moduleDto.Name}";
        worksheet.Cells[6, 3] = moduleDto.ModelName.Split('_')[0];
        
        var qrCodePath = await CreateQrCodeImage(moduleDto);

        //将图片插入excel
        worksheet.Shapes.AddPicture(qrCodePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 560, 35, 63, 63);//左，上，宽度，高度

        worksheet.PrintOutEx();
    }

    private async Task UseExcelPrintScreenShot(Worksheet worksheet, ModuleDto moduleDto)
    {
        worksheet.Cells[3, 3] = moduleDto.OdpNumber;
        worksheet.Cells[4, 3] = moduleDto.ProjectName;
        worksheet.Cells[5, 3] = $"{moduleDto.ItemNumber} {moduleDto.Name}";
        worksheet.Cells[6, 3] = moduleDto.ModelName.Split('_')[0];

        worksheet.Cells[7, 3] = moduleDto.Length;
        worksheet.Cells[7, 5] = moduleDto.Width;
        worksheet.Cells[7, 7] = moduleDto.Height;

        var qrCodePath = await CreateQrCodeImage(moduleDto);

        //将图片插入excel
        worksheet.Shapes.AddPicture(qrCodePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 560, 36, 63, 63);//相差4,097
        worksheet.Shapes.AddPicture(qrCodePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 560, 487, 63, 63);

        //插入Item图片
        if (!string.IsNullOrEmpty(moduleDto.ImageUrl))
        {
            //保存图片到系统目录中
            await DownloadImage(moduleDto.ImageUrl);

            var imagePath = Path.Combine(Environment.CurrentDirectory, "label.png");
            //将图片插入excel
            worksheet.Shapes.AddPicture(imagePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 144, 610, 280);//左，上，宽度，高度
            worksheet.Shapes.AddPicture(imagePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 596, 610, 280);//左，上，宽度，高度
        }

        worksheet.PrintOutEx();
    }

    #endregion
    
    #region PackingListCeiling
    private async Task UseExcelPrintPackingListHood(Worksheet worksheet, PackingListDto packingListDto)
    {
        worksheet.PageSetup.LeftHeader = $"项目名称: {packingListDto.ProjectName}";
        //worksheet.Cells[行,列]
        worksheet.Cells[2, 2] = packingListDto.PackingType;
        worksheet.Cells[3, 2] = packingListDto.DeliveryType;

        worksheet.Cells[2, 12] = DateTime.Now.ToShortDateString();
        worksheet.Cells[3, 12] = Environment.UserName;

        var items = packingListDto.PackingItemDtos.Where(x=>x.Pallet==true).ToArray();
        var pCount= items.Length;
        for (var i = 0; i < pCount; i++)
        {
            worksheet.Cells[i + 6, 1] = items[i].PalletNumber;
            worksheet.Cells[i + 6, 2] = items[i].MtlNumber;
            worksheet.Cells[i + 6, 3] = items[i].Type;
            worksheet.Cells[i + 6, 4] = items[i].Length;
            worksheet.Cells[i + 6, 5] = items[i].Width;
            worksheet.Cells[i + 6, 6] = items[i].Height;
            worksheet.Cells[i + 6, 12] = items[i].Remark;
        }

        //隔开3行打印配件accessories
        var accs = packingListDto.PackingItemDtos.Where(x => x.Pallet==false).ToArray();
        var aCount=accs.Length;
        worksheet.Cells[6+pCount+2, 1] = "配件数量";
        for (int j= 0; j < aCount; j++)
        {
            worksheet.Cells[j + 6+pCount+3, 1] = $"{accs[j].Quantity} {accs[j].Unit}";
            worksheet.Cells[j + 6+pCount+3, 2] = accs[j].MtlNumber;
            worksheet.Cells[j + 6+pCount+3, 3] = accs[j].Type;
            worksheet.Cells[j + 6+pCount+3, 4] = accs[j].Length;
            worksheet.Cells[j + 6+pCount+3, 5] = accs[j].Width;
            worksheet.Cells[j + 6+pCount+3, 6] = accs[j].Height;
            worksheet.Cells[j + 6+pCount+3, 12] = accs[j].Description;
        }


        //设置边框
        Range range = worksheet.Range["A6", $"L{pCount+aCount+3 + 5}"];
        range.Borders.Value = 1;
        //设置列宽
        //((Range)worksheet.Cells[1, 1]).ColumnWidth = 15;
        //((Range)worksheet.Cells[1, 2]).ColumnWidth = 30;
        //((Range)worksheet.Cells[1, 3]).ColumnWidth = 10;
        //((Range)worksheet.Cells[1, 4]).ColumnWidth = 5;
        //((Range)worksheet.Cells[1, 5]).ColumnWidth = 5;
        //((Range)worksheet.Cells[1, 6]).ColumnWidth = 28;
        //((Range)worksheet.Cells[1, 7]).ColumnWidth = 30;
        //((Range)worksheet.Cells[1, 8]).ColumnWidth = 8;
        //((Range)worksheet.Cells[1, 9]).ColumnWidth = 8;
        //((Range)worksheet.Cells[1, 10]).ColumnWidth = 8;

        //调试时预览
        //worksheet.PrintPreview(true);
        //打印
        worksheet.PrintOutEx();
        //清空打印内容,11行到末尾
        var rows = (Range)worksheet.Rows[$"6:{pCount +aCount+3+ 5}", Missing.Value];
        rows.Delete(XlDirection.xlDown);
    }


    private async Task UseExcelPrintPackingListCeiling(Worksheet worksheet, PackingListDto packingListDto)
    {
        worksheet.PageSetup.LeftHeader = $"项目名称: {packingListDto.ProjectName}";
        //worksheet.Cells[行,列]
        worksheet.Cells[2, 2] = packingListDto.PackingType;
        worksheet.Cells[3, 2] = packingListDto.DeliveryType;

        worksheet.Cells[2, 10] = DateTime.Now.ToShortDateString();
        worksheet.Cells[3, 10] = Environment.UserName;

        var items = packingListDto.PackingItemDtos;

        for (var i = 0; i < items.Count; i++)
        {
            worksheet.Cells[i + 6, 1] = items[i].PalletNumber;
            worksheet.Cells[i + 6, 2] = items[i].Description;
            worksheet.Cells[i + 6, 3] = items[i].Type;
            worksheet.Cells[i + 6, 4] = items[i].Quantity;
            worksheet.Cells[i + 6, 5] = items[i].Unit.ToString();
            worksheet.Cells[i + 6, 6] = items[i].Length;
            worksheet.Cells[i + 6, 7] = items[i].Width;
            worksheet.Cells[i + 6, 8] = items[i].Height;
            worksheet.Cells[i + 6, 9] = items[i].Material;
            worksheet.Cells[i + 6, 10] = items[i].Remark;
        }
        //设置边框
        Range range = worksheet.Range["A6", $"J{items.Count + 5}"];
        range.Borders.Value = 1;
        //设置列宽
        //((Range)worksheet.Cells[1, 1]).ColumnWidth = 15;
        //((Range)worksheet.Cells[1, 2]).ColumnWidth = 30;
        //((Range)worksheet.Cells[1, 3]).ColumnWidth = 10;
        //((Range)worksheet.Cells[1, 4]).ColumnWidth = 5;
        //((Range)worksheet.Cells[1, 5]).ColumnWidth = 5;
        //((Range)worksheet.Cells[1, 6]).ColumnWidth = 28;
        //((Range)worksheet.Cells[1, 7]).ColumnWidth = 30;
        //((Range)worksheet.Cells[1, 8]).ColumnWidth = 8;
        //((Range)worksheet.Cells[1, 9]).ColumnWidth = 8;
        //((Range)worksheet.Cells[1, 10]).ColumnWidth = 8;

        //调试时预览
        //worksheet.PrintPreview(true);
        //打印
        worksheet.PrintOutEx();
        //清空打印内容,11行到末尾
        var rows = (Range)worksheet.Rows[$"7:{items.Count + 6}", Missing.Value];
        rows.Delete(XlDirection.xlDown);
    }
    #endregion

    #region PackingInfo
    private async Task UseExcelExportPackingInfo(Worksheet worksheet, PackingListDto packingListDto)
    {
        //表的页眉页脚
        worksheet.PageSetup.LeftHeader = $"项目: {packingListDto.ProjectName}";
        worksheet.PageSetup.LeftFooter = $"填表: {Environment.UserName}";
        worksheet.PageSetup.CenterFooter = $"完工: {packingListDto.FinishTime.ToShortDateString()}";
        worksheet.PageSetup.RightFooter = $"打印: {DateTime.Now.ToShortDateString()}";

        var items = packingListDto.PackingItemDtos;
        var itemsCount = items.Count;
        //首先复制表格[1,7][8-14][15-21]...
        for (int i = 0; i < itemsCount-1; i++)
        {
            worksheet.Range["A1:E7"].Copy(worksheet.Range[$"A{8+i*7}"]);
            //itemsCount不用-1，这样会多出一张空白的，用于填写配件
        }
        //填写数据
        //worksheet.Cells[行,列]
        for (int i = 0; i < items.Count; i++)
        {
            worksheet.Cells[2+i*7, 1]=items[i].PalletNumber;
            worksheet.Cells[6+i*7, 1]=items[i].MtlNumber;
            worksheet.Cells[2+i*7, 3]=items[i].PalletLength;
            worksheet.Cells[3+i*7, 3]=items[i].PalletWidth;
            worksheet.Cells[4+i*7, 3]=items[i].PalletHeight;
            worksheet.Cells[5+i*7, 3]=$"产品长(L): {items[i].Length}";
            worksheet.Cells[6+i*7, 3]=$"产品宽(W): {items[i].Width}";
            worksheet.Cells[7+i*7, 3]=$"产品高(H): {items[i].Height}";
            worksheet.Cells[2+i*7, 4]=items[i].GrossWeight;
            worksheet.Cells[4+i*7, 4]=items[i].NetWeight;
            worksheet.Cells[6+i*7, 4]=items[i].Type.Equals("托盘")?"": items[i].Type;
            worksheet.Cells[2+i*7, 5]=items[i].Remark;
        }
    }

   
    #endregion

    #endregion

    #region Excel进程关闭
    /// <summary>
    /// 引用Windows句柄，获取程序PID
    /// </summary>
    [DllImport("User32.dll")]
    private static extern int GetWindowThreadProcessId(IntPtr hwnd, out int pid);
    /// <summary>
    /// 杀掉生成的进程
    /// </summary>
    /// <param name="appObject">进程程对象</param>
    private void KillProcess(Microsoft.Office.Interop.Excel.Application appObject)
    {
        IntPtr hwnd = new IntPtr(appObject.Hwnd);
        try
        {
            GetWindowThreadProcessId(hwnd, out var pid);
            var p = System.Diagnostics.Process.GetProcessById(pid);
            if (p != null)
            {
                p.Kill();
                p.Dispose();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("进程关闭失败！异常信息：" + ex);
        }
    }
    #endregion
}