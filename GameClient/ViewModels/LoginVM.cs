using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameClient.Utility;
using GameClient.Utility.Interface;
using HandyControl.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.ViewModels
{
    public partial class LoginVM(IGameHttpClient httpClient) : ObservableObject
    {

        [ObservableProperty]
        [NotifyCanExecuteChangedFor("LoginCommand")]
        private string? userCode;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor("LoginCommand")]
        private string? password;

        private bool CanLogin => !string.IsNullOrWhiteSpace(UserCode) && !string.IsNullOrWhiteSpace(Password);

        [RelayCommand(CanExecute = "CanLogin")]
        private async Task Login()
        {
            var user = await httpClient.GetUserInfoAsync(UserCode!, Password!);
            if (user is null)
            {
                App.ServiceProvider.GetService<IMessageDisplay>()?.ShowMessage("用户名或密码错误");
            }
            else
            {
                Session.userInfo = user;
                App.ServiceProvider.GetRequiredService<GameViews.MainWindow>().Show();
                App.Current.MainWindow?.Close();
            }
        }
    }
}
