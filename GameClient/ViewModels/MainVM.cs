using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameClient.Utility;
using GameClient.Utility.Interface;
using GameClient.Utility.Network;
using GameLibrary.Core.Users;
using GameLibrary.Network;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.ViewModels
{
    public partial class MainVM(IMessageDisplay messageDisplay) : ObservableObject, IMatchHubClientManager
    {

        private static UserInfo UserInfo => Session.userInfo;

        public string Title => UserInfo.UserName;

        long IMatchHubClientManager.PlayerId => Session.userInfo.UserId;

        private readonly Lazy<IMatchHubServiceManager> service = new(App.ServiceProvider.GetRequiredService<IMatchHubServiceManager>);

        public IMatchHubServiceManager Service => service.Value;

        void IMatchHubClient.MatchCanceled()
        {

        }

        async void IMatchHubClient.MatchConfirmed()
        {
            await Service.StopAsync();
        }

        void IMatchHubClient.Matched()
        {

        }

        void IMatchHubClientManager.MatchClosed(Exception exception)
        {
            
        }

        [RelayCommand]
        private async Task StartMatch()
        {
            CancellationTokenSource source = new (5000);
            bool connect = await Service.StartAsync(source.Token);
            if(!connect)
            {
                messageDisplay.ShowMessage("连接失败");
                return;
            }
            await Service.Match(UserInfo.UserId);
        }


        
    }
}
