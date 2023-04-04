using System.Threading.Tasks;
using Compass.Wasm.Shared.FileService;

namespace Compass.Wpf.ApiService;

public interface IFileUploadService
{
    Task<UploadResponse> Upload(string filePath);
}