using System;
using Prism.Commands;
using System.CodeDom.Compiler;
using Compass.Wpf.Service;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Windows.Shapes;
using Compass.Wasm.Shared.IdentityService;

namespace Compass.Wpf.ViewModels.Dialogs;

public class LoginViewModel:BindableBase, IDialogAware
{
    private readonly ILoginService _loginService;
    public string Title { get; } = "Compass";
    public event Action<IDialogResult>? RequestClose;
    private string userName;
    public string UserName
    {
        get => userName;
        set { userName = value;RaisePropertyChanged(); }
    }
    private string password;
    public string Password
    {
        get => password;
        set { password = value;RaisePropertyChanged(); }
    }
    public DelegateCommand<string> ExecuteCommand { get; }
    public LoginViewModel(ILoginService loginService)
    {
        _loginService = loginService;

        ExecuteCommand=new DelegateCommand<string>(Execute);
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
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }
        //登录失败提示...

    }
    /// <summary>
    /// 退出操作
    /// </summary>
    private void Logout()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.No));
    }



    public bool CanCloseDialog()
    {
        return true;
    }
    public void OnDialogClosed()
    {
    }
    public void OnDialogOpened(IDialogParameters parameters)
    {
    }
}