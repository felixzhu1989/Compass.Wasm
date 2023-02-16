using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Compass.Wpf.Common;
using Compass.Wpf.Identity;
using Compass.Wpf.Service;
using Compass.Wpf.ViewModels;
using Compass.Wpf.ViewModels.Dialogs;
using Compass.Wpf.Views;
using Compass.Wpf.Views.Dialogs;
using DryIoc;
using Microsoft.EntityFrameworkCore.Metadata;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using IView = Compass.Wpf.Views.IView;

namespace Compass.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void OnInitialized()
        {
            var dialog = Container.Resolve<IDialogService>();
            //程序启动时首先启动登录窗口
            dialog.ShowDialog("LoginView", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Application.Current.Shutdown();
                    return;
                }
                //配置默认首页,在应用程序设置中，启动默认首页。
                var service = Current.MainWindow!.DataContext as IConfigureService;
                service!.Configure();
                base.OnInitialized();
            });
        }

        //依赖注入
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //依赖注入容器里注册为导航
            containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
            containerRegistry.RegisterForNavigation<TodoView, TodoViewModel>();
            containerRegistry.RegisterForNavigation<MemoView, MemoViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();

            //设置界面
            containerRegistry.RegisterForNavigation<AboutView>();


            //获取容器，然后注册HttpRestClient，并给构造函数设置默认值
            containerRegistry.GetContainer()
                .Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "apiUrl"));
            containerRegistry.GetContainer().RegisterInstance(@"http://10.9.18.31/", serviceKey: "apiUrl");

            //注册服务
            containerRegistry.Register<ITodoService, TodoService>();
            containerRegistry.Register<IMemoService, MemoService>();
            containerRegistry.Register<ILoginService, LoginService>();

            //注册对话主机服务
            containerRegistry.Register<IDialogHostService, DialogHostService>();

            //注册弹窗
            containerRegistry.RegisterForNavigation<AddTodoView, AddTodoViewModel>();
            containerRegistry.RegisterForNavigation<AddMemoView, AddMemoViewModel>();

            //注册自定义询问窗口
            containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();

            //将登录窗体注册为Dialog，
            containerRegistry.RegisterDialog<LoginView, LoginViewModel>();

        }





        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    //Create a custom principal with an anonymous identity at startup
        //    CustomPrincipal customPrincipal = new CustomPrincipal();
        //    //请务必注意，在应用程序的生存期内，只能调用一次 SetThreadPrincipal
        //    //因此，在最初设置线程的默认标识后，无法重置线程的主体。
        //    AppDomain.CurrentDomain.SetThreadPrincipal(customPrincipal);

        //    base.OnStartup(e);

        //    //Show the login view
        //    AuthenticationViewModel viewModel = new AuthenticationViewModel(new AuthenticationService());
        //    IView loginWindow = new AuthenticationView(viewModel);
        //    loginWindow.Show();
        //}


    }
}
