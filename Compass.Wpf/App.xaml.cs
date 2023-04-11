﻿using System;
using System.Windows;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiService.Hoods;
using Compass.Wpf.BatchWorks;
using Compass.Wpf.BatchWorks.Hoods;
using Compass.Wpf.Common;
using Compass.Wpf.DrawingServices;
using Compass.Wpf.Extensions;
using Compass.Wpf.ViewModels;
using Compass.Wpf.ViewModels.Dialogs;
using Compass.Wpf.ViewModels.Hoods;
using Compass.Wpf.Views;
using Compass.Wpf.Views.Dialogs;
using Compass.Wpf.Views.Hoods;
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
        #region 初始化与登录
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
                regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView", back =>
                {
                    var journal = back.Context.NavigationService.Journal;
                });
            });
        } 
        #endregion

        //依赖注入容器：private readonly IContainerProvider _provider;
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            #region 注册RestSharp服务
            //获取容器，然后注册HttpRestClient，并给构造函数设置默认值
            containerRegistry.GetContainer()
                .Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "apiUrl"));
            //生产环境API
            //containerRegistry.GetContainer().RegisterInstance(@"http://10.9.18.31/", serviceKey: "apiUrl");
            //测试环境API
            containerRegistry.GetContainer().RegisterInstance(@"http://localhost/", serviceKey: "apiUrl"); 
            #endregion

            #region 注册页面服务,View,ViewModel
            containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
            containerRegistry.RegisterForNavigation<TodoView, TodoViewModel>();
            containerRegistry.RegisterForNavigation<MemoView, MemoViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<AboutView>();//设置界面
            containerRegistry.RegisterForNavigation<ProjectsView, ProjectsViewModel>();
            containerRegistry.RegisterForNavigation<DetailView, DetailViewModel>();
            containerRegistry.RegisterForNavigation<ProjectInfoView, ProjectInfoViewModel>();
            containerRegistry.RegisterForNavigation<DrawingView, DrawingViewModel>();
            containerRegistry.RegisterForNavigation<ModulesView, ModulesViewModel>();



            #endregion

            #region 注册页面数据服务
            containerRegistry.Register<ILoginService, LoginService>();
            containerRegistry.Register<IFileUploadService, FileUploadService>();
            containerRegistry.Register<ITodoService, TodoService>();
            containerRegistry.Register<IMemoService, MemoService>();
            containerRegistry.Register<IProjectService, ProjectService>();
            containerRegistry.Register<IDrawingService, DrawingService>();
            containerRegistry.Register<IModuleService, ModuleService>();
            containerRegistry.Register<IModuleDataService, ModuleDataService>();
            containerRegistry.Register<ICutListService, CutListService>();
            containerRegistry.Register<IBatchWorksService, BatchWorksService>();
            containerRegistry.Register<IExportDxfService, ExportDxfService>();
            containerRegistry.Register<IPrintsService, PrintsService>();


            #endregion


            #region 注册HoodsData模型参数页面服务
            containerRegistry.RegisterForNavigation<KviDataView, KviDataViewModel>();
            containerRegistry.RegisterForNavigation<KvfDataView, KvfDataViewModel>();



            #endregion

            #region 模型参数与模型制图数据服务，HoodService
            containerRegistry.Register<IKviDataService, KviDataService>();
            containerRegistry.Register<IKviAutoDrawing, KviAutoDrawing>();
            containerRegistry.Register<IKvfDataService, KvfDataService>();
            containerRegistry.Register<IKvfAutoDrawing, KvfAutoDrawing>();

            #endregion

            #region 制图代码库服务
            containerRegistry.Register<ISldWorksService, SldWorksService>();
            containerRegistry.Register<ISharePartService, SharePartService>();
            containerRegistry.Register<IExhaustService, ExhaustService>();
            containerRegistry.Register<ISupplyService, SupplyService>();
            containerRegistry.Register<ISidePanelService, SidePanelService>();
            containerRegistry.Register<IMidRoofService, MidRoofService>();
            
            



            #endregion

            #region 注册自定义弹窗服务
            //注册对话主机服务
            containerRegistry.Register<IDialogHostService, DialogHostService>();
            //将登录窗体注册为Dialog，
            containerRegistry.RegisterDialog<LoginView, LoginViewModel>();
            //注册通用询问弹窗
            containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();
            //注册其他自定义弹窗
            containerRegistry.RegisterForNavigation<AddTodoView, AddTodoViewModel>();
            containerRegistry.RegisterForNavigation<AddMemoView, AddMemoViewModel>();
            containerRegistry.RegisterForNavigation<CutListView, CutListViewModel>();
            containerRegistry.RegisterForNavigation<JobCardView, JobCardViewModel>();
            containerRegistry.RegisterForNavigation<BatchWorksView, BatchWorksViewModel>();
            

            
            
            
            #endregion
        }
    }
}
