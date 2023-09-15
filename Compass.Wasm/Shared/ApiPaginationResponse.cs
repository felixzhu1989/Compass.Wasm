using Compass.Dtos;

namespace Compass.Wasm.Shared;

/// <summary>
/// 分页api返回结果
/// </summary>
public class ApiPaginationResponse<T>:ApiResponse<T>
{
    public int Pages { get; set; }
    public int CurrentPage { get; set; }
}