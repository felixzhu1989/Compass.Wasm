using Compass.Wasm.Shared.Categories;

namespace Compass.Wasm.Shared.Plans;

public class PackingItemDto : BaseDto
{
    #region 基本信息

    private Guid? packingListId;
    public Guid? PackingListId
    {
        get => packingListId;
        set { packingListId = value;OnPropertyChanged(); }
    } //关联装箱清单

    //托盘号
    private string palletNumber;
    public string PalletNumber
    {
        get => palletNumber;
        set { palletNumber = value; OnPropertyChanged(); }
    }
    #region 生产部填写信息
    //包装长宽高
    private string palletLength;
    public string PalletLength
    {
        get => palletLength;
        set
        {
            palletLength = value;
            OnPropertyChanged();
        }
    }
    private string palletWidth;
    public string PalletWidth
    {
        get => palletWidth;
        set
        {
            palletWidth = value;
            OnPropertyChanged();
        }
    }
    private string palletHeight;
    public string PalletHeight
    {
        get => palletHeight;
        set
        {
            palletHeight = value;
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
    private string? palletRemark;
    public string? PalletRemark
    {
        get => palletRemark;
        set { palletRemark=value; OnPropertyChanged(); }
    }//备注 

    private int order;
    public int Order
    {
        get => order;
        set { order=value; OnPropertyChanged(); }
    }//排序 
    #endregion


    //从物料模板查询信息
    private string? mtlNumber;
    public string? MtlNumber { get=> mtlNumber;
        set { mtlNumber = value;OnPropertyChanged(); }
    } //物料编码

    private string? description;
    public string? Description 
    { 
        get=> description;  
        set { description=value;OnPropertyChanged(); }
    } //中文描述/产品编号

    private string? enDescription;
    public string? EnDescription
    {
        get=> enDescription; 
        set { enDescription=value; OnPropertyChanged();}
    } //英文描述

    private string? type;
    public string? Type
    {
        get => type;
        set { type=value; OnPropertyChanged(); }
    } //类型/产品型号

    private double quantity;
    public double Quantity
    {
        get => quantity;
        set { quantity=value; OnPropertyChanged(); }
    } //数量

    private Unit_e unit;
    public Unit_e Unit
    {
        get => unit;
        set { unit=value; OnPropertyChanged(); }
    }//PCS

    private string? length;
    public string? Length
    {
        get => length;
        set { length=value; OnPropertyChanged(); }
    } //长

    private string? width;
    public string? Width
    {
        get => width;
        set { width=value; OnPropertyChanged(); }
    } //宽

    private string? height;
    public string? Height
    {
        get => height;
        set { height=value; OnPropertyChanged(); }
    } //高

    private string? material;
    public string? Material
    {
        get => material;
        set { material=value; OnPropertyChanged(); }
    } //材质

    

    private string? calcRule;
    public string? CalcRule
    {
        get => calcRule;
        set { calcRule=value; OnPropertyChanged(); }
    } //计算规则
    private string? remark;
    public string? Remark
    {
        get => remark;
        set { remark=value; OnPropertyChanged(); }
    }//备注 
    #endregion
    #region 状态信息
    private bool pallet;
    public bool Pallet
    {
        get => pallet;
        set { pallet=value; OnPropertyChanged(); }
    }//单独托盘

    private bool noLabel;
    public bool NoLabel
    {
        get => noLabel;
        set { noLabel=value; OnPropertyChanged(); }
    }//打印标签

    private bool oneLabel;
    public bool OneLabel
    {
        get => oneLabel;
        set { oneLabel=value; OnPropertyChanged(); }
    }//打印1张标签，默认false表示需要根据数量Quantity打印多张
    #endregion
}