namespace Compass.Wasm.Shared;

public class PaginationResult<T>
{
    public T? Data { get; set; }
    public int Pages { get; set; }
    public int CurrentPage { get; set; }
}