using GameLibrary.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Utility.Interface
{
    public interface IMatchHubClientManager : IMatchHubClient
    {
        long PlayerId { get; }

        void MatchClosed(Exception exception);
    }

    public interface IMatchHubServiceManager
    {
        IMatchHubClientManager Client { get; }

        ValueTask<bool> StartAsync(CancellationToken token);

        Task StopAsync();

        Task Match(long playerId);

        Task CancelMatch();

        Task ConfirmMatch();
    }
}
