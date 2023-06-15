namespace Compass.Wasm.Shared.Plans;

/// <summary>
/// 托盘
/// </summary>
public class PalletDto : BaseDto
{
    public Guid PackingListId { get; set; }
    //托盘号
    private string palletNumber;
    public string PalletNumber
    {
        get => palletNumber;
        set { palletNumber = value; OnPropertyChanged(); }
    }

    #region 技术部填写信息
    //产品编号
    private string itemNumber;
    public string ItemNumber
    {
        get => itemNumber;
        set
        {
            itemNumber = value;
            OnPropertyChanged();
        }
    }
    //产品型号
    private string cType;
    public string ItemType
    {
        get => ItemType;
        set
        {
            ItemType = value;
            OnPropertyChanged();
        }
    }
    //长宽高
    private string itemLength;
    public string ItemLength
    {
        get => itemLength;
        set
        {
            itemLength = value;
            OnPropertyChanged();
        }
    }
    private string itemWidth;
    public string ItemWidth
    {
        get => itemWidth;
        set
        {
            itemWidth = value;
            OnPropertyChanged();
        }
    }
    private string itemHeight;
    public string ItemHeight
    {
        get => itemHeight;
        set
        {
            itemHeight = value;
            OnPropertyChanged();
        }
    }
    #endregion

    #region 生产部填写信息
    //包装长宽高
    private string length;
    public string Length
    {
        get => length;
        set
        {
            length = value;
            OnPropertyChanged();
        }
    }
    private string width;
    public string Width
    {
        get => width;
        set
        {
            width = value;
            OnPropertyChanged();
        }
    }
    private string height;
    public string Height
    {
        get => height;
        set
        {
            height = value;
            OnPropertyChanged();
        }
    }
    private string grossWeight;
    public string GrossWeight
    {
        get => grossWeight;
        set
        {
            grossWeight = value;
            OnPropertyChanged();
        }
    }
    private string netWeight;
    public string NetWeight
    {
        get => netWeight;
        set
        {
            netWeight = value;
            OnPropertyChanged();
        }
    }
    //备注
    private string remark;
    public string Remark
    {
        get => remark;
        set { remark = value; OnPropertyChanged(); }
    }
    #endregion
}