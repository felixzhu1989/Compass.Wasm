using Compass.Wasm.Shared.ProjectService;
using System.Text;
using ClosedXML.Excel;

namespace Compass.Wasm.Server.ExportExcel;

public class ExportExcelService
{
    #region Csv
    public string GetCsv(IEnumerable<ProjectDto> list)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Id,OdpNumber,Name");
        foreach (var project in list)
        {
            stringBuilder.AppendLine($"{project.Id},{project.OdpNumber},{project.Name}");
        }
        return stringBuilder.ToString();
    }
    #endregion

    #region Excel
    private byte[] ConvertToByte(XLWorkbook workbook)
    {
        var stream=new MemoryStream();
        workbook.SaveAs(stream);
        var content = stream.ToArray();
        return content;
    }

    public byte[] CreateProjectExport(List<ProjectDto> projects)
    {
        var workbook=new XLWorkbook();//create an Excel workbook
        workbook.Properties.Title = "Exprot from projects";
        workbook.Properties.Author = "Compass";
        workbook.Properties.Subject = "Exprot from projects";
        workbook.Properties.Keywords = "Project,Compass,Blazor";
        //添加Worksheet
        CreateProjectWorksheet(workbook, projects);
        //转换成byte[]类型并返回
        return ConvertToByte(workbook);
    }

    public void CreateProjectWorksheet(XLWorkbook package, List<ProjectDto> projects)
    {
        //create and add worksheets to the workbook
        var worksheet = package.Worksheets.Add("Projects");
        worksheet.Cell(1, 1).Value = "Id";
        worksheet.Cell(1, 2).Value = "OdpNumber";
        worksheet.Cell(1, 3).Value = "Name";
        for (int index = 1; index <= projects.Count; index++)
        {
            worksheet.Cell(index + 1, 1).Value = projects[index - 1].Id;
            worksheet.Cell(index + 1, 2).Value = projects[index - 1].OdpNumber;
            worksheet.Cell(index + 1, 3).Value = projects[index - 1].Name;
        }
    }

    #endregion

}