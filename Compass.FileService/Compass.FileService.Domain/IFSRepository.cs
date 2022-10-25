namespace Compass.FileService.Domain;

public interface IFSRepository
{
    /// <summary>
    /// 查找已经上传的相同大小以及散列值的文件记录（从数据库中查找记录）
    /// </summary>
    Task<UploadedItem?> FindFileAsync(long fileSize, string sha256Hash);
}