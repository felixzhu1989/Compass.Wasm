using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace Compass.Andro.ApiService
{
    public class UploadResponse
    {
        public Uri RemoteUrl { get; set; }
    }
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
            var fileContent = File.ReadAllBytes(filePath);
            request.AddFile(fileName, fileContent, fileName);
            var response = await client.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<UploadResponse>(response.Content);
        }
    }
}
