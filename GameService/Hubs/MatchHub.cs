using GameLibrary.Network;
using GameService.Interfaces.Test.Games;
using GameService.Interfaces.Test.Matchs;
using GameService.Utility;
using Microsoft.AspNetCore.SignalR;


namespace GameService.Hubs
{
    public sealed class MatchHub : Hub<IMatchHubClient>, IMatchHubService
    {

        private readonly IMatchManager matchManager;
        private readonly ILogger<MatchHub> logger;

        public MatchHub(IMatchManager matchManager, ILoggerFactory loggerFactory)
        {
            this.matchManager = matchManager;
            logger = loggerFactory.CreateLogger<MatchHub>();
        }

        public override Task OnConnectedAsync()
        {
            logger.Debug(Context.ConnectionId + " connected");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            logger.Debug(Context.ConnectionId + " disconnected");
            matchManager.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public void ConfirmMatch()
        {
            matchManager.Confirm(Context.ConnectionId);
        }

        public void CancelMatch()
        {
            matchManager.Cancel(Context.ConnectionId);
        }

        public void Match(long playerId)
        {
            matchManager.Add(Context.ConnectionId, playerId);
        }
    }
}
