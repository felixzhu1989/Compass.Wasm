using System;
using Prism.Commands;
using Compass.Wpf.Service;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Compass.Wasm.Shared.IdentityService;
using Compass.Wpf.Common;
using Prism.Events;
using Compass.Wpf.Extensions;
using System.Configuration;

namespace Compass.Wpf.ViewModels.Dialogs;

public class LoginViewModel : BindableBase, IDialogAware
{
    private readonly ILoginService _loginService;//事件聚合器，消息提示
    private readonly IEventAggregator _aggregator;
    public string Title { get; } = "Compass";
    public event Action<IDialogResult>? RequestClose;
    private string userName;
    public string UserName
    {
        get => userName;
        set { userName = value; RaisePropertyChanged(); }
    }
    private string password;
    public string Password
    {
        get => password;
        set { password = value; RaisePropertyChanged(); }
    }
    //是否保存用户信息
    private bool saveUser;
    public bool SaveUser
    {
        get => saveUser;
        set { saveUser = value; RaisePropertyChanged(); }
    }

    public DelegateCommand<string> ExecuteCommand { get; }
    public LoginViewModel(ILoginService loginService, IEventAggregator aggregator)
    {
        _loginService = loginService;
        _aggregator = aggregator;
        SaveUser=true;
        //使用记住的用户名和密码
        UserName = GetSettingString("UserName");
        Password = GetSettingString("Password");
        ExecuteCommand =new DelegateCommand<string>(Execute);
    }
    private void Execute(string obj)
    {
        switch (obj)
        {
            case "Login": Login(); break;
            case "Logout": Logout(); break;
        }
    }
    /// <summary>
    /// 登录操作
    /// </summary>
    private async void Login()
    {
        if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            return;
        var loginResult = await _loginService.LoginAsync(new UserDto
        {
            UserName = this.UserName,
            Password = this.Password
        });
        if (loginResult!=null && loginResult.Status)
        {
            AppSession.Id = loginResult.Result.Id;
            AppSession.UserName = loginResult.Result.UserName;
            AppSession.Roles = loginResult.Result.Roles;
            if (SaveUser)
            {
                //保存用户名和密码
                UpdateSettingString("UserName", UserName);
                UpdateSettingString("Password", Password);
            }
            else
            {
                UpdateSettingString("UserName", "");
                UpdateSettingString("Password", "");
            }
            _aggregator.SendMessage("登录成功，欢迎使用Compass！");//默认即发到主页
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            return;
        }
        //登录失败提示...
        _aggregator.SendMessage(loginResult.Message, "Login");
    }
    /// <summary>
    /// 退出操作
    /// </summary>
    private void Logout()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.No));
    }

    /// <summary>
    /// 读取客户设置,初始化的时候给其赋值,LoginName = GetSettingString("UserName");
    /// </summary>
    public static string GetSettingString(string settingName)
    {
        try
        {
            string settingString = ConfigurationManager.AppSettings[settingName].ToString();
            return settingString;
        }
        catch (Exception)
        {
            return "";
        }
    }
    /// <summary>
    /// 更新设置,在登录成功后调用修改配置的方法,UpdateSettingString("UserName", UserName);
    /// </summary>
    public static void UpdateSettingString(string settingName, string valueName)
    {
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        if (ConfigurationManager.AppSettings[settingName] != null)
        {
            config.AppSettings.Settings.Remove(settingName);
        }
        config.AppSettings.Settings.Add(settingName, valueName);
        config.Save(ConfigurationSaveMode.Modified);
        ConfigurationManager.RefreshSection("appSettings");
    }



    public bool CanCloseDialog()
    {
        return true;
    }
    public void OnDialogClosed()
    {
        Logout();
    }
    public void OnDialogOpened(IDialogParameters parameters)
    {
    }
}