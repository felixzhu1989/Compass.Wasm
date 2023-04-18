using System.Threading.Tasks;
using Compass.Wasm.Shared.Files;

namespace Compass.Wpf.ApiService;

public interface IFileUploadService
{
    Task<UploadResponse> Upload(string filePath);
}