// See https://aka.ms/new-console-template for more information

using Compass.Update;

Console.WriteLine("程序正在升级，请稍候...");
var updateMgr = new UpdateManager();
//var fileList = updateMgr.NowUpdateInfo.FileList;
await updateMgr.DownloadFiles();
var status=updateMgr.CopyFiles();
if (status)
{
    //启动主程序
    System.Diagnostics.Process.Start("Compass.Wpf.exe");
}