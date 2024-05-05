using GameLibrary.Network;
using GameService.Hubs;
using GameService.Interfaces.Test.Games;
using GameService.Interfaces.Test.Matchs;
using GameService.Utility;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace GameService.Entity.Test.Matchs
{
    public class MatchManager : IMatchManager
    {
        public MatchManager(
            IMatchGroupManager matchGroupManager,
            IHubContext<MatchHub> hubContext,
            ILoggerFactory loggerFactory)
        {
            this.matchGroupManager = matchGroupManager;
            this.hubContext = hubContext;
            logger = loggerFactory.CreateLogger<MatchManager>();
            this.matchGroupManager.MatchGroupTimeoutCanceled += MatchGroupManager_OnMatchGroupCanceled;
        }

        private IMatchManager Manager => this;

        private void MatchGroupManager_OnMatchGroupCanceled(IMatcher[] matchers)
        {
            foreach (var i in matchers.Where(v => v.Confirmed))
                Manager.Add(i);
            hubContext.Clients.Clients(matchers.Select(v => v.ConnectionId)).SendAsync(nameof(IMatchHubClient.MatchCanceled));
        }

        private readonly MatchQueue queue = new();
        private readonly IMatchGroupManager matchGroupManager;
        private readonly IHubContext<MatchHub> hubContext;
        private readonly ILogger<MatchManager> logger;

        IMatcher IMatchManager.Add(string connectionId, long playerId)
        {
            if (queue.TryGetMatcher(connectionId, out var m))
                return m!;
            IMatcher matcher = new Matcher(connectionId, playerId);
            return Manager.Add(matcher);
        }

        void IMatchManager.Remove(string connectionId)
        {
            queue.Remove(connectionId);
            matchGroupManager.Cancel(connectionId);
        }

        string IMatchManager.CreateMatchId()
        {
            return Guid.NewGuid().ToString();
        }

        IMatcher IMatchManager.Add(IMatcher matcher)
        {
            IMatcher current = matcher;
            IMatcher? other = queue.Dequeue();
            if (other is null)
            {
                queue.Enqueue(current);
            }
            else
            {
                IMatcher[] gamers = [current, other];
                foreach (var m in gamers)
                    m.Matched = true;
                matchGroupManager.Add(gamers);
                logger.Info($"{matcher.PlayerId} send {current.PlayerId} {other.PlayerId}");
                hubContext.Clients.Clients(gamers.Select(v => v.ConnectionId)).SendAsync(nameof(IMatchHubClient.Matched));
            }
            return current;
        }

        public void Confirm(string connectionId)
        {
            var matchers = matchGroupManager.Confirm(connectionId);
            if (matchers != null)
            {
                var clients = hubContext.Clients.Clients(matchers.Select(v => v.ConnectionId));
                clients.SendAsync(nameof(IMatchHubClient.MatchConfirmed));
            }
        }

        public void Cancel(string connectionId)
        {
            var matchers = matchGroupManager.Cancel(connectionId);
            if (matchers != null)
                hubContext.Clients.Clients(matchers.Select(v => v.ConnectionId)).SendAsync(nameof(IMatchHubClient.MatchCanceled));
        }


        private class MatchQueue
        {
            private readonly List<IMatcher> list = [];
            private readonly Dictionary<string, IMatcher> dic = [];
            private readonly Dictionary<long, IMatcher> playerDic = [];
            public void Enqueue(IMatcher item)
            {
                lock (list)
                {

                    if (!dic.ContainsKey(item.ConnectionId) && !item.Disposed)
                    {
                        list.Add(item);
                        dic.Add(item.ConnectionId, item);
                        playerDic.Add(item.PlayerId, item);
                    }
                }
            }

            public bool TryGetMatcher(string connectionId, out IMatcher? matcher)
            {
                lock (list)
                {
                    return dic.TryGetValue(connectionId, out matcher);
                }
            }

            public IMatcher? Dequeue()
            {
                lock (list)
                {

                    IMatcher? item = null;
                    while (list.Count > 0)
                    {
                        item = list[0];
                        list.RemoveAt(0);
                        dic.Remove(item.ConnectionId);
                        playerDic.Remove(item.PlayerId);
                        if (!item.Disposed)                     
                            break;                       
                    }
                    return item;
                }
            }

            public void BackQueue(IMatcher item)
            {
                lock (list)
                {
                    list.Insert(0, item);
                    dic.Add(item.ConnectionId, item);
                    playerDic.Add(item.PlayerId, item);
                }
            }

            public IMatcher? Remove(string connectionId)
            {
                lock (list)
                {
                    if (dic.TryGetValue(connectionId, out var matcher))
                    {
                        dic.Remove(connectionId);
                        playerDic.Remove(matcher.PlayerId);
                        matcher.Dispose();
                        return matcher;
                    }
                    return null;
                }
            }
        }

    }
}
