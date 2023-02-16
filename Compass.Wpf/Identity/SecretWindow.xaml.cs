using System;
using System.Security.Permissions;
using System.Windows;
using Compass.Wpf.ViewModels;
using Compass.Wpf.Views;

namespace Compass.Wpf.Identity
{
    /// <summary>
    /// SecretWindow.xaml 的交互逻辑
    /// </summary>
    [PrincipalPermission(SecurityAction.Demand)]
    [Obsolete("Obsolete")]
    public partial class SecretWindow : Window, IView
    {
        public SecretWindow()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel {
            get => DataContext as IViewModel;
            set => DataContext = value;
        }
    }
}
