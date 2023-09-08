using RestSharp;

namespace Compass.Andro.ApiService
{
    public class BaseRequest
    {
        public Method Method { get; set; }
        public string Route { get; set; }
        public string ContentType { get; set; } = "application/json";
        public object Param { get; set; }
    }
}
