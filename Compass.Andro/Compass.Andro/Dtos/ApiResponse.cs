namespace Compass.Andro.Dtos
{
    /// <summary>
    /// api返回结果，添加泛型支持
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
}
