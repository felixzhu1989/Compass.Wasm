using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Compass.Wasm.Shared.ProjectService;
using Microsoft.Office.Interop.Excel;
using Excel=Microsoft.Office.Interop.Excel;
using Worksheet= Microsoft.Office.Interop.Excel.Worksheet;
using Range=Microsoft.Office.Interop.Excel.Range;
using Prism.Ioc;
using Compass.Wasm.Shared.Parameter;
using Compass.Wpf.Service;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using DocumentFormat.OpenXml.Spreadsheet;
using Action = System.Action;

namespace Compass.Wpf.BatchWorks;

public class PrintsService:IPrintsService
{
    private readonly IContainerProvider _containerProvider;
    public PrintsService(IContainerProvider containerProvider)
    {
        _containerProvider = containerProvider;
    }
    public async Task BatchPrintCutListAsync(List<ModuleDto> moduleDtos)
    {
        var cutListService = _containerProvider.Resolve<ICutListService>();
        var template = Path.Combine(Environment.CurrentDirectory, "TemplateDoc", "CutList.xlsx");

        //如果报错就添加COM引用，Microsoft Office 16.0 Object Library1.9
        var excelApp = new Application();
        excelApp.Workbooks.Add(template);
        var worksheet = (Worksheet)excelApp.Worksheets[1];
        //调试时预览
        //excelApp.Visible = true;

        foreach (var moduleDto in moduleDtos)
        {
            await GetCutListAndPrint(worksheet, moduleDto, cutListService);
        }

        KillProcess(excelApp);
        excelApp = null;//对象置空
        GC.Collect(); //垃圾回收机制
    }
    public async Task PrintOneCutListAsync(ModuleDto moduleDto)
    {
        var cutListService = _containerProvider.Resolve<ICutListService>();
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

    


    private void UsePrintDialog(Visual visual)
    {
        //效果不理想
        PrintDialog printDialog = new PrintDialog();
        printDialog.PageRangeSelection = PageRangeSelection.AllPages;
        printDialog.UserPageRangeEnabled = true;
        var result = printDialog.ShowDialog();
        if (result.Value)
        {
            printDialog.PrintVisual(visual, "CutList");
        }
    }


    private async Task GetCutListAndPrint(Worksheet worksheet,ModuleDto moduleDto,ICutListService cutListService)
    {
        CutListParameter param = new CutListParameter
        {
            ModuleId = moduleDto.Id.Value
        };
        var result = await cutListService.GetAllByModuleIdAsync(param);
        if (result.Status)
        {
            UseExcelPrint(worksheet, moduleDto, result.Result);
        }
    }

    private void UseExcelPrint(Worksheet worksheet, ModuleDto moduleDto, List<CutListDto> cutListDtos)
    {
        FillTitle(worksheet, moduleDto);

        FillData(worksheet, cutListDtos);

        //调试时预览
        //worksheet.PrintPreview(true);
        //打印
        worksheet.PrintOutEx();
        //清空打印内容,11行到末尾
        var rows=worksheet.Rows[$"11:{cutListDtos.Count + 10}",Missing.Value];
        rows.Delete(XlDirection.xlDown);
    }

    private void FillTitle(Worksheet worksheet,ModuleDto moduleDto)
    {
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

    private void FillData(Worksheet worksheet, List<CutListDto> cutListDtos)
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
        worksheet.Cells[1, 1].ColumnWidth = 30;
        worksheet.Cells[1, 2].ColumnWidth = 10;
        worksheet.Cells[1, 3].ColumnWidth = 10;
        worksheet.Cells[1, 4].ColumnWidth = 5;
        worksheet.Cells[1, 5].ColumnWidth = 5;
        worksheet.Cells[1, 6].ColumnWidth = 28;
        worksheet.Cells[1, 7].ColumnWidth = 30;
        worksheet.Cells[1, 8].ColumnWidth = 8;
        worksheet.Cells[1, 9].ColumnWidth = 8;
        worksheet.Cells[1, 10].ColumnWidth = 8;
        worksheet.Cells[1, 11].ColumnWidth = 5;
    }

    private string GetSidePanelLength(CutListDto dto)
    {
        if(dto.PartNo.Length<8) return "";
        
        if (dto.PartNo.Contains("FNHE0003") || dto.PartNo.Contains("FNHE0004") || dto.PartNo.Contains("FNHE0026") || dto.PartNo.Contains("FNHE0027"))
        {
            //普通KSA小侧边
            return dto.Length.Equals(310.67d) ? $"{dto.Width-50}": $"{dto.Length-50}";
        }
        if(dto.PartNo.Contains("FNHE0005") || dto.PartNo.Contains("FNHE0028") || dto.PartNo.Contains("FNHE0170")) 
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
}