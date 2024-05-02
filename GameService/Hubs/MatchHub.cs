using GameLibrary.Network;
using GameService.Interfaces.Test.Games;
using GameService.Interfaces.Test.Matchs;
using Microsoft.AspNetCore.SignalR;

namespace GameService.Hubs
{
    public class MatchHub : Hub, IMatchHubService
    {
        private readonly IMatchManager matchManager;
        private readonly IMatchGroupManager matchGroupManager;

        public MatchHub(IMatchManager matchManager, IMatchGroupManager matchGroupManager)
        {
            this.matchManager = matchManager;
            this.matchGroupManager = matchGroupManager;
            matchManager.OnMatchSuccsee += MatchManager_OnMatchSuccsee;
            matchGroupManager.OnMatchGroupCanceled += MatchGroupManager_OnMatchGroupCanceled;
            matchGroupManager.OnMatchGroupConfirmed += MatchGroupManager_OnMatchGroupConfirm;
        }

        private void MatchGroupManager_OnMatchGroupConfirm(IMatcher[] matchers, IGame game)
        {
            Clients.Users(matchers.Select(v => v.ConnectionId)).SendAsync(nameof(IMatchHubClient.MatchConfirmed), game.GameId);
        }

        private void MatchGroupManager_OnMatchGroupCanceled(IMatcher[] matchers, bool timeout)
        {
            Clients.Users(matchers.Select(v => v.ConnectionId)).SendAsync(nameof(IMatchHubClient.MatchCanceled), timeout);
        }

        private void MatchManager_OnMatchSuccsee(IMatcher[] matchers, string matchId)
        {
            Clients.Users(matchers.Select(v => v.ConnectionId)).SendAsync(nameof(IMatchHubClient.Matched), matchId);
        }

        public void ConfirmMatch(string matchId)
        {
            matchGroupManager.Confirm(matchId, Context.ConnectionId);
        }

        public void CancelMatch(string matchId)
        {
            matchGroupManager.Cancel(matchId, Context.ConnectionId);
        }

        public override Task OnConnectedAsync()
        {
            if (Context.UserIdentifier is null)
            {
                Context.Abort();
                return Task.CompletedTask;
            }
            matchManager.Add(Context.ConnectionId, Context.UserIdentifier);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            matchManager.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
