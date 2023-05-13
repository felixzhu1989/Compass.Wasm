namespace Compass.Wpf.Common.Events;
public class MessageModel
{
    public Filter_e Filter { get; set; }
    public string Message { get; set; }
}
public class MessageEvent:PubSubEvent<MessageModel>
{
}
