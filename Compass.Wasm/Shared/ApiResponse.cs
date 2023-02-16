namespace Compass.Wasm.Shared;
//添加泛型支持
public class ApiResponse<T>
{
    public bool Status { get; set; }
    public string Message { get; set; }
    public T Result { get; set; }
}