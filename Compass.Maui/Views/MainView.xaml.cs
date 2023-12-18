using Compass.Maui.ViewModels;

namespace Compass.Maui.Views
{
    public partial class MainView : ContentPage
    {
        public MainView(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext=viewModel;
        }
    }

}
