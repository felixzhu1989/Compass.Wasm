// See https://aka.ms/new-console-template for more information
using CsvHelper;
using System.Globalization;
using System.Text;
using Compass.Wasm.Shared.Plans;

Console.WriteLine("Hello, World!");

//https://joshclose.github.io/CsvHelper/getting-started/
//Install-Package CsvHelper
var path = @"C:\Users\Administrator\Desktop\MainPlanUpdate.csv";
var mainplans=new List<MainPlanCsv>();
//Excel默认以ANSI格式保存csv文件，读取的时候中文会乱码，应当指定Encoding为GB2312
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
using (var reader = new StreamReader(path, Encoding.GetEncoding("GB2312")))
using (var csv = new CsvReader(reader,CultureInfo.InvariantCulture))
{
    mainplans= csv.GetRecords<MainPlanCsv>().ToList();
}

foreach (var mainplan in mainplans)
{
    Console.WriteLine(mainplan.Name);
}