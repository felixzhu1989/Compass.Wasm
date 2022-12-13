using Compass.FileService.Domain;
using Compass.FileService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Compass.FileService.Infrastructure;

public class FileRepository:IFileRepository
{
    private readonly FileDbContext _dbContext;
    public FileRepository(FileDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Task<UploadedItem?> FindFileAsync(long fileSize, string sha256Hash)
    {
        return _dbContext.UploadedItems.FirstOrDefaultAsync(u =>
            u.FileSizeInBytes == fileSize && u.FileSHA256Hash == sha256Hash);
    }
}