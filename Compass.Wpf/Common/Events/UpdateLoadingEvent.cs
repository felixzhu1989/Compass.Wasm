using Prism.Events;

namespace Compass.Wpf.Common.Events;

public class UpdateModel
{
    public bool IsOpen { get; set; }
}

public class UpdateLoadingEvent:PubSubEvent<UpdateModel>
{

}