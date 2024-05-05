using GameClient.Utility.Interface;
using GameLibrary.Network;
using Microsoft.AspNetCore.SignalR.Client;
using System.Reflection;
using System.Windows.Automation.Peers;
using System.Windows.Documents;

namespace GameClient.Utility.Network
{
    public class MatchService : IMatchHubServiceManager
    {
        readonly HubConnection connection;
        public MatchService(IMatchHubClientManager client)
        {
            Client = client;
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:10123/MatchHub", Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets)
                .WithKeepAliveInterval(TimeSpan.FromSeconds(3))
                .Build();
            var method = typeof(IMatchHubClient).GetMethods();
            foreach (var item in method)
            {
                connection.On(item.Name, item.GetParameters().Select(v => v.ParameterType).ToArray(), Listen(item), client);
            }
            connection.Closed += ConnectionClosed;
        }

        private int reconnectCount = 0;

        public IMatchHubClientManager Client { get; }

        private async Task ConnectionClosed(Exception? exception)
        {
            if (exception != null)
            {
                if (++reconnectCount > 3)
                {
                    await connection.DisposeAsync();
                    Client.MatchClosed(exception);
                    reconnectCount = 0;
                    return;
                }
                await connection.StartAsync();
                reconnectCount = 0;
            }
            else
            {
                reconnectCount = 0;
            }
        }

        public async ValueTask<bool> StartAsync(CancellationToken token)
        {
            if(connection.State == HubConnectionState.Connected)           
                return true;           
            try
            {
                await connection.StartAsync(token);
            }
            catch { }
            return connection.State == HubConnectionState.Connected;
        }

        public async Task StopAsync()
        {
            await connection.StopAsync();
        }

        private static Func<object?[], object, Task> Listen(MethodInfo methodInfo)
        {
            return (o, e) => Task.FromResult(methodInfo.Invoke(e, o));
        }

        public async Task CancelMatch()
        {
            await connection.SendAsync(nameof(IMatchHubService.CancelMatch));
        }

        public async Task ConfirmMatch()
        {
            await connection.SendAsync(nameof(IMatchHubService.ConfirmMatch));
        }

        public async Task Match(long playerId)
        {
            await connection.SendAsync(nameof(IMatchHubService.Match), playerId);
        }


    }
}
