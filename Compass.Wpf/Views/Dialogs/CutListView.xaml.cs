namespace Compass.Wpf.Views.Dialogs;

/// <summary>
/// CutListView.xaml 的交互逻辑
/// </summary>
public partial class CutListView : UserControl
{
    public CutListView(IEventAggregator aggregator)
    {
        InitializeComponent();
        //注册snackbar提示消息,只订阅来自Login的消息，默认的消息。
        aggregator.RegisterMessage(arg =>
        {
            CutListSnackBar.MessageQueue.Enqueue(arg.Message);
        }, Filter_e.CutList);
    }
}