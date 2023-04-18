using Compass.Wpf.Extensions;
using Prism.Events;
using System.Windows.Controls;

namespace Compass.Wpf.Views.Dialogs
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView(IEventAggregator aggregator)
        {
            InitializeComponent();
            //注册snackbar提示消息,只订阅来自Login的消息，默认的消息。
            aggregator.RegisterMessage(arg =>
            {
                LoginSnackBar.MessageQueue.Enqueue(arg.Message);
            }, Filter_e.Login);
        }
    }
}
