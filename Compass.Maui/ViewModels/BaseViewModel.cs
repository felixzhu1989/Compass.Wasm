namespace Compass.Maui.ViewModels;

public partial class BaseViewModel:ObservableObject
{
    private bool isBusy;
    public bool IsBusy
    {
        get => isBusy;
        set
        {
            if (isBusy==value) return;//检查是否为同一个数据，相同则不修改
            isBusy=value;//不同则修改，然后通知修改
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsNotBusy));//同时通知另一个属性更新值
        }
    }

    public bool IsNotBusy => !IsBusy;

    [ObservableProperty]
    private string title;
}