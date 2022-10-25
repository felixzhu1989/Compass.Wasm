using Compass.FileService.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Compass.FileService.Infrastructure;
/// <summary>
/// 实现IStorageClient，将文件存储在wwwroot文件夹下
/// </summary>
public class MockCloudStorageClient : IStorageClient
{
    public StorageType StorageType => StorageType.Public;
    private readonly IWebHostEnvironment _hostEnv;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public MockCloudStorageClient(IWebHostEnvironment hostEnv, IHttpContextAccessor httpContextAccessor)
    {
        _hostEnv = hostEnv;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken = default)
    {
        if (key.StartsWith('/')) throw new ArgumentException("Key should not start with /", nameof(key));
        //获取当前web服务器的wwwroot路径
        string workingDir = Path.Combine(_hostEnv.ContentRootPath, "wwwroot");
        string fullPath = Path.Combine(workingDir, key);//key是DomainService中拼接了日期路径的文件名
        string? fullDir = Path.GetDirectoryName(fullPath);//get the directory，获取路径（剔除文件名） 
        //automatically create dir，如果不存在就创建这个路径
        if (!Directory.Exists(fullDir)) Directory.CreateDirectory(fullDir);
        //再判断文件是否存在，同名文件？尝试删除
        if (File.Exists(fullPath)) File.Delete(fullPath);
        await using Stream outStream = File.OpenWrite(fullPath);
        content.Position=0;
        await content.CopyToAsync(outStream, cancellationToken);
        var req = _httpContextAccessor.HttpContext.Request;
        string url = $"{req.Scheme}://{req.Host}/{key}";
        //string url = $"{req.Scheme}://localhost/{key}";
        return new Uri(url);
    }
}