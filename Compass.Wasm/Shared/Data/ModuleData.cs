using Zack.DomainCommons.Models;

namespace Compass.Wasm.Shared.Data;

public class ModuleData : BaseDto, IAggregateRoot, ISoftDelete, IHasDeletionTime, IHasCreationTime, IHasModificationTime
{
    public bool IsDeleted { get; private set; }
    public DateTime? DeletionTime { get; private set; }
    public virtual void SoftDelete()
    {
        IsDeleted = true;
        DeletionTime = DateTime.Now;
    }
    public void NotifyModified()
    {
        LastModificationTime = DateTime.Now;
    }


    //Id直接使用ModuleId
    //产品基本属性有长宽高，注意和以前的作图程序不同，这里是总长，
    private double length;
    public double Length
    {
        get => length; set { length = value; OnPropertyChanged(); }
    }

    private double width;
    public double Width
    {
        get => width;
        set { width = value; OnPropertyChanged(); }
    }
    private double height;
    public double Height
    {
        get => height;
        set { height = value; OnPropertyChanged(); }
    }

    //大侧板：左, 右, 双,中
    private SidePanel_e sidePanel;
    public SidePanel_e SidePanel
    {
        get => sidePanel;
        set { sidePanel = value; OnPropertyChanged(); }
    }

    public virtual bool Accept(string model)
    {
        return false;
    }
    public ModuleData ChangeId(Guid id)
    {
        Id = id;
        return this;
    }
}