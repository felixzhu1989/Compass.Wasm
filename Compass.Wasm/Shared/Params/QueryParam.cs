namespace Compass.Wasm.Shared.Params;

/// <summary>
/// 查询参数，支持分页，搜索
/// </summary>
public class QueryParam
{
    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
    public string? Search { get; set; }
}