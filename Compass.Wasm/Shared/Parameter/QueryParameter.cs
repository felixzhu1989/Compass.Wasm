namespace Compass.Wasm.Shared.Parameter;

/// <summary>
/// 查询参数，支持分页，搜索
/// </summary>
public class QueryParameter
{
    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
    public string? Search { get; set; }
}