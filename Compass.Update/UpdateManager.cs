using System.Net;
using System.Xml;

namespace Compass.Update;
/// <summary>
/// 升级管理器核心业务类
/// </summary>
public class UpdateManager
{
    #region 属性
    public UpdateInfo LastUpdateInfo { get; set; }//上次更新的信息
    public UpdateInfo NowUpdateInfo { get; set; }//当前的更新信息
    /// <summary>
    /// 判断是否需要更新，用更新日期比较
    /// </summary>
    public bool NeedUpdate
    {
        get
        {
            var dt1 = Convert.ToDateTime(LastUpdateInfo.UpdateTime);
            var dt2 = Convert.ToDateTime(NowUpdateInfo.UpdateTime);
            return dt2 > dt1;
        }
    }
    /// <summary>
    /// 下载文件缓存目录
    /// </summary>
    public string TempFilePath
    {
        get
        {
            var newTempPath = Path.Combine(Environment.GetEnvironmentVariable("Temp")!, "wpfupdatefiles");
            if (!Directory.Exists(newTempPath)) Directory.CreateDirectory(newTempPath);
            return newTempPath;
        }
    }
    #endregion

    #region ctor
    public UpdateManager()
    {
#pragma warning disable CS4014
        Init();
#pragma warning restore CS4014
    }

    #endregion

    #region 内部实现
    /// <summary>
    /// 初始化
    /// </summary>
    private async Task Init()
    {
        //1.初始化对象属性
        LastUpdateInfo = new UpdateInfo();
        NowUpdateInfo = new UpdateInfo();
        //2.给属性赋值
        GetLastUpdateInfo();
        await GetNewUpdateInfo();
    }
    /// <summary>
    /// 从本地获取上次更新的信息并封装到属性【服务器url,更新时间】
    /// </summary>
    private void GetLastUpdateInfo()
    {
        //封装上次更新的信息
        var myFile = new FileStream("UpdateList.xml", FileMode.Open);
        var xmlReader = new XmlTextReader(myFile);
        while (xmlReader.Read())
        {
            switch (xmlReader.Name)
            {
                case "URLAddress":
                    LastUpdateInfo.UpdateFileUrl = xmlReader.GetAttribute("URL")!;
                    break;
                case "UpdateTime":
                    LastUpdateInfo.UpdateTime = Convert.ToDateTime(xmlReader.GetAttribute("Date"));
                    break;
            }
        }
        xmlReader.Close();
        myFile.Close();
    }
    /// <summary>
    /// 从远程服务器上下载文件，然后获取当前更新的信息并封装到属性【服务器url，更新时间】
    /// </summary>
    private async Task GetNewUpdateInfo()
    {
        //下载最新的更新文件，临时目录
        var fileUrl = Path.Combine(LastUpdateInfo.UpdateFileUrl, "UpdateList.xml");//当前需要下载的文件的URL
        var newFileName = Path.Combine(TempFilePath, "UpdateList.xml");
        
        //从网络下载文件
        var objClient = new WebClient();
        objClient.DownloadFile(fileUrl, newFileName);

        //封装当前更新的信息
        var myFile = new FileStream(newFileName, FileMode.Open);
        var xmlReader = new XmlTextReader(myFile);

        while (xmlReader.Read())
        {
            switch (xmlReader.Name)
            {
                case "URLAddress":
                    NowUpdateInfo.UpdateFileUrl = xmlReader.GetAttribute("URL")!;
                    break;
                case "UpdateTime":
                    NowUpdateInfo.UpdateTime =Convert.ToDateTime(xmlReader.GetAttribute("Date"));
                    break;
                case "UpdateFile":
                    NowUpdateInfo.FileList.Add(xmlReader.GetAttribute("FileName")!);
                    break;
            }
        }
        xmlReader.Close();
        myFile.Close();
    }
    /// <summary>
    /// 下载单个文件
    /// </summary>
    private async Task DownloadFile(HttpClient client, string fileUrl, string newFileName)
    {
        var uri=new Uri(fileUrl);
        var dir = Path.GetDirectoryName(newFileName);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        var response= await client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            await using var stream = File.Create(newFileName);
            var streamFromServer=await response.Content.ReadAsStreamAsync();
            await streamFromServer.CopyToAsync(stream);
        }
    }
    #endregion

    #region 外部调用方法
    /// <summary>
    /// 根据更新文件列表，下载更新文件，并同步显示下载的百分比
    /// </summary>
    public async Task DownloadFiles()
    {
        //从网络下载文件
        var client = new HttpClient();
        foreach (var fileName in NowUpdateInfo.FileList)
        {
            Console.WriteLine($"正在下载{fileName}...");
            var fileUrl = Path.Combine(NowUpdateInfo.UpdateFileUrl, fileName);//当前需要下载的文件的URL
            var newFileName = Path.Combine(TempFilePath, fileName);
            await DownloadFile(client, fileUrl, newFileName);
        }
    }

    /// <summary>
    /// 将下载在临时目录中的文件，复制到应用程序目录
    /// </summary>
    /// <returns></returns>
    public bool CopyFiles()
    {
        //todo：还要包括文件夹
        var files = Directory.GetFiles(TempFilePath);
        foreach (var name in files)
        {
            var currentFile = name.Substring(name.LastIndexOf(@"\") + 1);
            //如果文件在程序目录中已经存在，则删除，避免弹窗中断程序
            Console.WriteLine($"正在拷贝{currentFile}...");
            if (File.Exists(currentFile)) File.Delete(currentFile);
            File.Copy(name, currentFile);
        }
        return true;
    }
    #endregion
}
/// <summary>
/// 升级信息类
/// </summary>
public class UpdateInfo
{
    public DateTime UpdateTime { get; set; }
    public string UpdateFileUrl { get; set; }
    public List<string> FileList { get; set; } = new();
}