using System;
using System.Windows;
using Compass.Wpf.Common;
using Compass.Wpf.Extensions;
using Compass.Wpf.Service;
using Compass.Wpf.ViewModels;
using Compass.Wpf.ViewModels.Dialogs;
using Compass.Wpf.Views;
using Compass.Wpf.Views.Dialogs;
using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;

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
        /// <summary>
        /// 初始化时跳转登录页面，并配置默认启动页
        /// </summary>
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
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="containerProvider"></param>
        public static void Logout(IContainerProvider containerProvider, IRegionManager regionManager)
        {
            Current.MainWindow.Hide();
            var dialog = containerProvider.Resolve<IDialogService>();
            dialog.ShowDialog("LoginView", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Environment.Exit(0);
                    return;
                }
                Current.MainWindow.Show();
                //显示页面后重新加载一次Index，以更新界面显示
                regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView");
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
            //containerRegistry.GetContainer().RegisterInstance(@"http://10.9.18.31/", serviceKey: "apiUrl");
            //测试环境
            containerRegistry.GetContainer().RegisterInstance(@"http://localhost/", serviceKey: "apiUrl");

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
    }
}
