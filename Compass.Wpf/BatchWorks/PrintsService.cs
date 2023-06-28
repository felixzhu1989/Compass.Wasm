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
using Compass.Wasm.Shared.Projects;

namespace Compass.Wpf.BatchWorks;

public interface IPrintsService
{
    Task BatchPrintCutListAsync(List<ModuleDto> moduleDtos);
    Task PrintOneCutListAsync(ModuleDto moduleDto);

    Task BatchPrintJobCardAsync(List<ModuleDto> moduleDtos);
    Task PrintOneJobCardAsync(ModuleDto moduleDto);

    Task PrintPackingListAsync(PackingListDto packingListDto);

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
    /// 打印装箱清单
    /// </summary>
    public async Task PrintPackingListAsync(PackingListDto packingListDto)
    {

        var template = Path.Combine(Environment.CurrentDirectory, "PackingList.xlsx");
        //如果报错就添加COM引用，Microsoft Office 16.0 Object Library1.9
        var excelApp = new Application();
        excelApp.Workbooks.Add(template);
        var worksheet = (Worksheet)excelApp.Worksheets[1];
        //调试时预览
        //excelApp.Visible = true;

        await UseExcelPrintPackingList(worksheet, packingListDto);

        KillProcess(excelApp);
        excelApp = null;//对象置空
        GC.Collect(); //垃圾回收机制


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
            worksheet.Cells[i + 5, 9] = GetSidePanelLength(cutListDtos[i]);
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

    /// <summary>
    /// 计算KSA、MESH小侧板长度
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    private string GetSidePanelLength(CutListDto dto)
    {
        if (dto.PartNo.Length<8) return "";

        if (dto.PartNo.Contains("FNHE0003") || dto.PartNo.Contains("FNHE0004") || dto.PartNo.Contains("FNHE0026") || dto.PartNo.Contains("FNHE0027"))
        {
            //普通KSA小侧边
            return dto.Length.Equals(310.67d) ? $"{dto.Width-50.13d}" : $"{dto.Length-50.13d}";
        }

        if (dto.PartNo.Contains("FNHE0005") || dto.PartNo.Contains("FNHE0028") || dto.PartNo.Contains("FNHE0170"))
        {
            //普通KSA小侧边特殊
            return dto.Length.Equals(300d) ? $"{dto.Width-29}" : $"{dto.Length-29}";
        }
        

        if (dto.PartNo.Contains("FNHE0012") || dto.PartNo.Contains("FNHE0013") || dto.PartNo.Contains("FNHE0029") || dto.PartNo.Contains("FNHE0030")|| dto.PartNo.Contains("FNHE0038") || dto.PartNo.Contains("FNHE0039"))
        {
            //普通MESH油网侧边
            return dto.Length.Equals(308d) ? $"{dto.Width-46.58407}" : $"{dto.Length-46.58407}";
        }



        

        if (dto.PartNo.Contains("FNHE0160") || dto.PartNo.Contains("FNHE0161"))
        {
            //华为KSA小侧边
            return dto.Length.Equals(310.87d) ? $"{dto.Width-48.49}" : $"{dto.Length-48.49}";
        }
        if (dto.PartNo.Contains("FNHE0162") || dto.PartNo.Contains("FNHE0163"))
        {
            //华为MESH油网侧边
            return dto.Length.Equals(308d) ? $"{dto.Width-45.39}" : $"{dto.Length-45.39}";
        }



        return "";
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
            worksheet.Shapes.AddPicture(imagePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 4240, 610, 280);//左，上，宽度，高度
            worksheet.Shapes.AddPicture(imagePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 4692, 610, 280);//左，上，宽度，高度
        }

        //生成二维码插入Excel并上传
        var content = moduleDto.Id.ToString();
        var bmp = content.GetQrCodeBitmap();//获取QrCode的扩展方法
        var qrCodePath = Path.Combine(Environment.CurrentDirectory, "moduleidqrcode.png");
        bmp.Save(qrCodePath);

        //判断module的qrcode是否存在，不存在则上传，存在则不上传
        if (string.IsNullOrEmpty(moduleDto.ImageUrl))
        {
            var result = await _fileUploadService.Upload(qrCodePath);
            moduleDto.QrCodeUrl = result.RemoteUrl.ToString();
            await _moduleService.UpdateAsync(moduleDto.Id.Value, moduleDto);
        }

        //wpf imageSource
        //var bs = Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        //imageQRCode.Source = bs;

        //将图片插入excel
        worksheet.Shapes.AddPicture(qrCodePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 560, 35,63, 63);//左，上，宽度，高度
        worksheet.Shapes.AddPicture(qrCodePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 560, 926, 63, 63);
        worksheet.Shapes.AddPicture(qrCodePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 560, 1736, 63, 63);
        worksheet.Shapes.AddPicture(qrCodePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 560, 2564, 63, 63);
        worksheet.Shapes.AddPicture(qrCodePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 560, 3454, 63, 63);
        worksheet.Shapes.AddPicture(qrCodePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 560, 4132, 63, 63);
        worksheet.Shapes.AddPicture(qrCodePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 560, 4583, 63, 63);


        //调试时预览
        //worksheet.PrintPreview(true);
        //打印
        worksheet.PrintOutEx();
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


    #region PackingList
    private async Task UseExcelPrintPackingList(Worksheet worksheet, PackingListDto packingListDto)
    {
        //worksheet.Cells[行,列]
        worksheet.Cells[2, 2] = packingListDto.ProjectName;
        worksheet.Cells[3, 2] = packingListDto.PackingType;
        worksheet.Cells[4, 2] = packingListDto.DeliveryType;

        worksheet.Cells[2, 10] = packingListDto.Product.ToString();
        worksheet.Cells[3, 10] = DateTime.Now.ToShortDateString();
        worksheet.Cells[4, 10] = Environment.UserName;

        var items = packingListDto.PackingItemDtos;

        for (var i = 0; i < items.Count; i++)
        {
            worksheet.Cells[i + 7, 2] = items[i].Description;
            worksheet.Cells[i + 7, 3] = items[i].Type;
            worksheet.Cells[i + 7, 4] = items[i].Quantity;
            worksheet.Cells[i + 7, 5] = items[i].Unit.ToString();
            worksheet.Cells[i + 7, 6] = items[i].Length;
            worksheet.Cells[i + 7, 7] = items[i].Width;
            worksheet.Cells[i + 7, 8] = items[i].Height;
            worksheet.Cells[i + 7, 9] = items[i].Material;
            worksheet.Cells[i + 7, 10] = items[i].Remark;
        }
        //设置边框
        Range range = worksheet.Range["A7", $"K{items.Count + 6}"];
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