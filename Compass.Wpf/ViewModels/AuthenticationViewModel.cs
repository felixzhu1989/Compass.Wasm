using Compass.Wpf.Identity;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading;
using System.Windows.Controls;
using Compass.Wasm.Shared.IdentityService;
using Compass.Wpf.Identity;
using Compass.Wpf.Views;

namespace Compass.Wpf.ViewModels;

public interface IViewModel { }
public class AuthenticationViewModel : BindableBase, IViewModel
{
    private readonly IAuthenticationService _authenticationService;
    private readonly DelegateCommand<object> _loginCommand;
    private readonly DelegateCommand _logoutCommand;
    private readonly DelegateCommand<object> _showViewCommand;
    private string _username;
    private string _status;

    public AuthenticationViewModel(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
        _loginCommand = new DelegateCommand<object>(Login, CanLogin);
        _logoutCommand = new DelegateCommand(Logout, CanLogout);
        _showViewCommand = new DelegateCommand<object>(ShowView);
    }

    #region Properties
    public string Username
    {
        get => _username;
        set { _username = value; RaisePropertyChanged(); }
    }

    public string AuthenticatedUser
    {
        get
        {
            if (IsAuthenticated)
                return string.Format("Signed in as {0}. {1}",
                    Thread.CurrentPrincipal.Identity.Name,
                    Thread.CurrentPrincipal.IsInRole("admin") ? "You are an administrator!"
                        : "You are NOT a member of the administrators group.");
            return "Not authenticated!";
        }
    }
    public string Status
    {
        get => _status;
        set { _status = value; RaisePropertyChanged(); }
    }
    #endregion

    #region Commands
    public DelegateCommand<object> LoginCommand => _loginCommand;
    public DelegateCommand LogoutCommand => _logoutCommand;
    public DelegateCommand<object> ShowViewCommand => _showViewCommand;
    #endregion

    private void Login(object parameter)
    {
        PasswordBox passwordBox = parameter as PasswordBox;
        string clearTextPassword = passwordBox.Password;
        try
        {
            //Validate credentials through the authentication service
            //Mark
            UserDto user = _authenticationService.AuthenticateUser(Username, clearTextPassword);

            //Get the current principal object
            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            if (customPrincipal == null)
                throw new ArgumentException("The application's default thread principal must be set to a CustomPrincipal object on startup.");

            //Authenticate the user
            //todo:获取token
            customPrincipal.Identity = new CustomIdentity(user.UserName, user.Email, user.Roles,"Token");

            //Update UI
            RaisePropertyChanged("AuthenticatedUser");
            RaisePropertyChanged("IsAuthenticated");
            _loginCommand.RaiseCanExecuteChanged();
            _logoutCommand.RaiseCanExecuteChanged();
            Username = string.Empty; //reset
            passwordBox.Password = string.Empty; //reset
            Status = string.Empty;
        }
        catch (UnauthorizedAccessException)
        {
            Status = "Login failed! Please provide some valid credentials.";
        }
        catch (Exception ex)
        {
            Status = string.Format("ERROR: {0}", ex.Message);
        }
    }

    private bool CanLogin(object parameter)
    {
        return !IsAuthenticated;
    }

    private void Logout()
    {
        CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
        if (customPrincipal != null)
        {
            customPrincipal.Identity = new AnonymousIdentity();
            RaisePropertyChanged("AuthenticatedUser");
            RaisePropertyChanged("IsAuthenticated");
            _loginCommand.RaiseCanExecuteChanged();
            _logoutCommand.RaiseCanExecuteChanged();
            Status = string.Empty;
        }
    }

    private bool CanLogout()
    {
        return IsAuthenticated;
    }

    public bool IsAuthenticated => Thread.CurrentPrincipal.Identity.IsAuthenticated;

    private void ShowView(object parameter)
    {
        try
        {
            Status = string.Empty;
            IView view;
            if (parameter == null)
                view = new SecretWindow();
            else
                view = new AdminWindow();

            view.Show();
        }
        catch (SecurityException)
        {
            Status = "You are not authorized!";
        }
    }
}