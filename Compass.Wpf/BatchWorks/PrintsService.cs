using Compass.Wasm.Shared.Parameter;
using Compass.Wasm.Shared.ProjectService;
using Compass.Wpf.Extensions;
using Microsoft.Office.Interop.Excel;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Compass.Wpf.ApiService;
using Microsoft.Office.Core;
using Range = Microsoft.Office.Interop.Excel.Range;
using Worksheet = Microsoft.Office.Interop.Excel.Worksheet;

namespace Compass.Wpf.BatchWorks;

public class PrintsService : IPrintsService
{
    #region ctor
    private readonly IContainerProvider _provider;
    private readonly IEventAggregator _aggregator;
    public PrintsService(IContainerProvider provider)
    {
        _provider = provider;
        _aggregator= provider.Resolve<IEventAggregator>();
    }
    #endregion

    #region 公共方法实现
    /// <summary>
    /// 批量打印CutList
    /// </summary>
    public async Task BatchPrintCutListAsync(List<ModuleDto> moduleDtos)
    {
        var cutListService = _provider.Resolve<ICutListService>();
        var template = Path.Combine(Environment.CurrentDirectory, "TemplateDoc", "CutList.xlsx");

        //如果报错就添加COM引用，Microsoft Office 16.0 Object Library1.9
        var excelApp = new Application();
        excelApp.Workbooks.Add(template);
        var worksheet = (Worksheet)excelApp.Worksheets[1];
        //调试时预览
        //excelApp.Visible = true;

        foreach (var moduleDto in moduleDtos)
        {
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
        var template = Path.Combine(Environment.CurrentDirectory, "TemplateDoc", "CutList.xlsx");

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
    public Task BatchPrintJobCardAsync(List<ModuleDto> moduleDtos)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 单独打印
    /// </summary>
    public async Task PrintOneJobCardAsync(ModuleDto moduleDto)
    {
        var template = Path.Combine(Environment.CurrentDirectory, "TemplateDoc", "JobCard.xlsx");
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

    #endregion

    #region Excel打印内部实现

    #region CutList
    private async Task GetCutListAndPrint(Worksheet worksheet, ModuleDto moduleDto, ICutListService cutListService)
    {
        var param = new CutListParameter
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
        //清空打印内容,11行到末尾
        var rows = (Range)worksheet.Rows[$"11:{cutListDtos.Count + 10}", Missing.Value];
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
        worksheet.Cells[1, 2] = moduleDto.ProjectName;
        worksheet.Cells[2, 2] = moduleDto.OdpNumber;
        worksheet.Cells[3, 2] = moduleDto.ItemNumber;
        worksheet.Cells[4, 2] = moduleDto.Name;
        worksheet.Cells[5, 2] = moduleDto.ModelName;
        var specialNotes = string.IsNullOrEmpty(moduleDto.SpecialNotes) ? "" : moduleDto.SpecialNotes;
        worksheet.Cells[6, 2] = $"{moduleDto.Length}x{moduleDto.Width}x{moduleDto.Height} {specialNotes}";
        worksheet.Cells[7, 2] = DateTime.Now.ToShortDateString();
        worksheet.Cells[8, 2] = Environment.UserName;
    }

    /// <summary>
    /// 填写Cutlist信息
    /// </summary>
    /// <param name="worksheet"></param>
    /// <param name="cutListDtos"></param>
    private void FillCutListData(Worksheet worksheet, List<CutListDto> cutListDtos)
    {
        for (int i = 0; i < cutListDtos.Count; i++)
        {
            worksheet.Cells[i + 11, 1] = cutListDtos[i].PartDescription;
            worksheet.Cells[i + 11, 2] = cutListDtos[i].Length;
            worksheet.Cells[i + 11, 3] = cutListDtos[i].Width;
            worksheet.Cells[i + 11, 4] = cutListDtos[i].Thickness;
            worksheet.Cells[i + 11, 5] = cutListDtos[i].Quantity;
            worksheet.Cells[i + 11, 6] = cutListDtos[i].Material;
            worksheet.Cells[i + 11, 7] = cutListDtos[i].PartNo;
            worksheet.Cells[i + 11, 11] = cutListDtos[i].Index;
            worksheet.Cells[i + 11, 9] = GetSidePanelLength(cutListDtos[i]);
        }
        //设置边框
        Range range = worksheet.Range["A11", $"K{cutListDtos.Count + 10}"];
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
            return dto.Length.Equals(310.67d) ? $"{dto.Width-50}" : $"{dto.Length-50}";
        }
        if (dto.PartNo.Contains("FNHE0005") || dto.PartNo.Contains("FNHE0028") || dto.PartNo.Contains("FNHE0170"))
        {
            //KSA小侧边特殊
            return dto.Length.Equals(300d) ? $"{dto.Width-29}" : $"{dto.Length-29}";
        }
        if (dto.PartNo.Contains("FNHE0012") || dto.PartNo.Contains("FNHE0013") || dto.PartNo.Contains("FNHE0029") || dto.PartNo.Contains("FNHE0030")|| dto.PartNo.Contains("FNHE0038") || dto.PartNo.Contains("FNHE0039")|| dto.PartNo.Contains("FNHE0162") || dto.PartNo.Contains("FNHE0163"))
        {
            //MESH油网侧边
            return dto.Length.Equals(308d) ? $"{dto.Width-46}" : $"{dto.Length-46}";
        }
        if (dto.PartNo.Contains("FNHE0160") || dto.PartNo.Contains("FNHE0161"))
        {
            //KSA小侧边-华为
            return dto.Length.Equals(310.87d) ? $"{dto.Width-50}" : $"{dto.Length-50}";
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
            worksheet.Shapes.AddPicture(imagePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 3893, 610, 260);//左，上，宽度，高度
            worksheet.Shapes.AddPicture(imagePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 4310, 610, 260);//左，上，宽度，高度
        }
        
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