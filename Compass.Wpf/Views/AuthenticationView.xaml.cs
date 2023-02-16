using Compass.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Compass.Wpf.Views
{
    public interface IView
    {
        IViewModel ViewModel { get; set; }
        void Show();
    }
    /// <summary>
    /// AuthenticationView.xaml 的交互逻辑
    /// </summary>
    public partial class AuthenticationView : Window, IView
    {
        public AuthenticationView(AuthenticationViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }
        #region IView Members
        public IViewModel ViewModel
        {
            get => DataContext as IViewModel;
            set => DataContext = value;
        }
        #endregion
    }
}
