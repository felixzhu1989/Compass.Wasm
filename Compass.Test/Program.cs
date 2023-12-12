//var swApp=SwUtility.ConnectSw();
//var swModel=(ModelDoc2)swApp.ActiveDoc;

#region 获取打印机列表
//Nuget安装System.Drawing.Common包
using System.Drawing.Printing;
using System.Runtime.InteropServices;
var pd = new PrintDocument();
var ps = pd.PrinterSettings;
var defaultPrinter = ps.PrinterName;
Console.WriteLine($"默认打印机:{defaultPrinter}");
Console.WriteLine("打印机列表：");
//使用combobox装它（打印时记住原始打印机，然后设置标签打印机，再恢复原始打印机）
var installedPrinters = PrinterSettings.InstalledPrinters;
foreach (string installedPrinter in installedPrinters)
{
    Console.WriteLine(installedPrinter);
}
//然后是调用windows  打印机操作api的类Externs(将它放在Extension类中)
//调用win api将指定名称的打印机设置为默认打印机
//[DllImport("winspool.drv")]
//public static extern bool SetDefaultPrinter(string printerName);
[DllImport("winspool.drv")]
static extern bool SetDefaultPrinter(string printerName);

SetDefaultPrinter(installedPrinters[3]);


SetDefaultPrinter(defaultPrinter);
#endregion