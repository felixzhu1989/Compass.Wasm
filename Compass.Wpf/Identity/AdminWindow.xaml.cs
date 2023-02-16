using System;
using System.Security.Permissions;
using System.Windows;
using Compass.Wpf.ViewModels;
using Compass.Wpf.Views;

namespace Compass.Wpf.Identity
{
    /// <summary>
    /// AdminWindow.xaml 的交互逻辑
    /// </summary>
    [PrincipalPermission(SecurityAction.Demand, Role = "admin")]
    [Obsolete("Obsolete")]
    public partial class AdminWindow : Window, IView
    {
        public AdminWindow()
        {
            InitializeComponent();
        }
        public IViewModel ViewModel
        {
            get => DataContext as IViewModel;
            set => DataContext = value;
        }
    }
}
