using System.IO;
using System.Threading.Tasks;
using Compass.Wasm.Shared.Files;
using Newtonsoft.Json;
using RestSharp;

namespace Compass.Wpf.ApiService;

public interface IFileUploadService
{
    Task<UploadResponse> Upload(string filePath);
}

public class FileUploadService : IFileUploadService
{
    public async Task<UploadResponse> Upload(string filePath)
    {
        var client = new RestClient("http://10.9.18.31");

        var request = new RestRequest("api/Uploader/RestSharp", Method.Post);
        var fileName = Path.GetFileName(filePath);
        var fileContent = await File.ReadAllBytesAsync(filePath);
        request.AddFile(fileName, fileContent, fileName);
        //获取token，给请求添加token
        //var token = AppSession.Token;
        //if (!string.IsNullOrEmpty(token))
        //{
        //    request.AddOrUpdateHeader("Authorization", $"Bearer {token.Replace("\"", "")}");
        //}
        var response =await client.ExecuteAsync(request);
        return JsonConvert.DeserializeObject<UploadResponse>(response.Content!)!;
    }
}