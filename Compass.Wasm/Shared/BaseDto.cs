using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Compass.Wasm.Shared;

public class BaseDto : INotifyPropertyChanged
{
    public Guid? Id { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }
}