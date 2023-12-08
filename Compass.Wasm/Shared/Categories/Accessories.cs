using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Compass.Wasm.Shared.Categories;

public class Accessories: INotifyPropertyChanged
{
    private AccType_e type;
    public AccType_e Type
    {
        get => type;
        set { type = value; OnPropertyChanged(); }
    }
    private int number;
    public int Number
    {
        get => number;
        set { number = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}