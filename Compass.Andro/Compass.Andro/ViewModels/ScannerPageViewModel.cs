using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Compass.Andro.ViewModels
{
    public class ScannerPageViewModel : ViewModelBase
    {
        #region ctor
        public ScannerPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Scanner Page";
            
        }
        
        #endregion

        #region 属性
        private string code;
        public string Code
        {
            get => code;
            set => SetProperty(ref code, value);
        }

        #endregion

        

    }
}
