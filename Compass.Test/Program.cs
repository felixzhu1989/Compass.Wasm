// See https://aka.ms/new-console-template for more information
using CsvHelper;
using System.Globalization;
using System.Text;
using Compass.Wasm.Shared.Plans;
using Microsoft.Win32;
using System.Security.Claims;

Console.WriteLine("Hello, World!");

////https://joshclose.github.io/CsvHelper/getting-started/
////Install-Package CsvHelper
//var path = @"C:\Users\Administrator\Desktop\MainPlanUpdate.csv";
//var mainplans=new List<MainPlanCsv>();
////Excel默认以ANSI格式保存csv文件，读取的时候中文会乱码，应当指定Encoding为GB2312
//Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
//using (var reader = new StreamReader(path, Encoding.GetEncoding("GB2312")))
//using (var csv = new CsvReader(reader,CultureInfo.InvariantCulture))
//{
//    mainplans= csv.GetRecords<MainPlanCsv>().ToList();
//}

//foreach (var mainplan in mainplans)
//{
//    Console.WriteLine(mainplan.Name);
//}

//计算机\HKEY_CLASSES_ROOT\SldWorks.Application\CLSID（主要修改这个就可以了）
//2023：{b2f1524f-6cfe-4386-b472-ab1148dea4f1}
//var rgrt = Registry.ClassesRoot;
//var clsid = rgrt.OpenSubKey(@"SldWorks.Application\CLSID",true);
//var names = clsid.GetValueNames();
//foreach (var name in names)
//{
//    Console.WriteLine(name);
//}
////Console.WriteLine(clsid.GetValue("(默认)").ToString());
//rgrt.Close();
//clsid.Close();
