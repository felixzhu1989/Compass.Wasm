using Compass.Wasm.Shared.Categories;

namespace Compass.Wasm.Shared.Projects;

public class AccessoryDto : BaseDto
{

    #region 自带属性
    //描述
    private string description;
    public string Description
    {
        get => description;
        set { description = value; OnPropertyChanged(); }
    }
    //类型/部件编号-配件(assembly parts)
    private string type;
    public string Type  
    {
        get => type;
        set { type = value; OnPropertyChanged(); }
    }
    private Unit_e unit;
    public Unit_e Unit
    {
        get => unit;
        set { unit = value; OnPropertyChanged(); }
    }
    //长宽高
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
    //材质
    private string material;
    public string Material
    {
        get => material;
        set { material = value; OnPropertyChanged(); }
    }
    //备注
    private string remark;
    public string Remark
    {
        get => remark;
        set { remark = value; OnPropertyChanged(); }
    }
    //分组
    private AccGroup_e group;
    public AccGroup_e Group
    {
        get => group;
        set { group = value; OnPropertyChanged(); }
    }
    //筛选规则
    private AccRule_e rule;
    public AccRule_e Rule
    {
        get => rule;
        set { rule = value; OnPropertyChanged(); }
    }
    #endregion

    #region 清单中填写
    //托盘号
    private string palletNumber;
    public string PalletNumber
    {
        get => palletNumber;
        set { palletNumber = value; OnPropertyChanged(); }
    }
    //天花烟罩分区
    private string location;
    public string Location
    {
        get => location;
        set { location = value; OnPropertyChanged(); }
    }
    //数量
    private string quantity;
    public string Quantity
    {
        get => quantity;
        set { quantity = value; OnPropertyChanged(); }
    } 
    #endregion
}