using Compass.FileService.Domain;
using Microsoft.Extensions.Options;

namespace Compass.FileService.Infrastructure;

public class SMBStorageClient : IStorageClient
{
    public StorageType StorageType => StorageType.Backup;
    private readonly IOptionsSnapshot<SMBStorageOptions> _options;
    public SMBStorageClient(IOptionsSnapshot<SMBStorageOptions> options)
    {
        _options = options;
    }

    public async Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken = default)
    {
        if (key.StartsWith('/')) throw new ArgumentException("key should not start with /", nameof(key));
        string workingDir = _options.Value.WorkingDir;
        string fullPath = Path.Combine(workingDir, key);
        string? fullDir = Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(fullDir)) Directory.CreateDirectory(fullDir);
        if (File.Exists(fullPath)) File.Delete(fullPath);
        await using Stream outStream = File.OpenWrite(fullPath);
        content.Position=0;
        await content.CopyToAsync(outStream, cancellationToken);//存文件到备份地址上
        return new Uri(fullPath);
    }
}