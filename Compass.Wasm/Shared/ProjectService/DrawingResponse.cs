namespace Compass.Wasm.Shared.ProjectService;

public record DrawingResponse:IComparable<DrawingResponse>
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string ItemNumber { get; set; }
    public string? DrawingUrl { get; set; }

    public Guid? UserId { get; set; }

    //用于显示
    public bool IsChecked { get; set; }

    public int CompareTo(DrawingResponse? other)
    {
        if (other!=null)
        {
            return ItemNumber.CompareTo(other.ItemNumber); //升序
        }
        return 1;//空值比较大，返回1
    }
}