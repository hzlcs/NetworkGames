using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls.Notifications;
using Avalonia.GameClient.Services;
using Avalonia.GameClient.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Avalonia.GameClient.ViewModels;

[ViewModel]
public partial class LoginViewModel(ILoginManager loginManager, IPopupManager popupManager)
    : ViewModelBase, ILoginViewModel
{
    [ObservableProperty] private string userCode = string.Empty;
    [ObservableProperty] private string password = string.Empty;
    [ObservableProperty] private string userName = string.Empty;

    [RelayCommand]
    private async Task Login()
    {
        if (string.IsNullOrEmpty(UserCode))
        {
            popupManager.Popup("Logging in...", NotificationType.Information);
            return;
        }
        var res = await loginManager.LoginAsync(UserCode, Password);
        if (res.IsTrue)
            LoginSuccess?.Invoke();
        
    }

    [RelayCommand]
    private async Task Register()
    {
        
        var res = await loginManager.RegisterAsync(UserName, UserCode, Password);
    }

    public event Action? LoginSuccess;
}