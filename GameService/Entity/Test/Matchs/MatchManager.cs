using GameService.Interfaces.Test.Games;
using GameService.Interfaces.Test.Matchs;
using System.Collections.Concurrent;

namespace GameService.Entity.Test.Matchs
{
    public class MatchManager : IMatchManager
    {
        public MatchManager(IGameRepository gameRepository, IMatchGroupManager matchGroupManager)
        {
            this.gameRepository = gameRepository;
            this.matchGroupManager = matchGroupManager;
            matchGroupManager.OnMatchGroupCanceled += MatchGroupManager_OnMatchGroupCanceled;
        }

        private IMatchManager Manager => (IMatchManager)this;

        private void MatchGroupManager_OnMatchGroupCanceled(IMatcher[] matchers, bool timeout)
        {
            foreach (var i in matchers.Where(v => v.Confirmed))
                Manager.Add(i.Player);
        }

        private readonly MatchQueue queue = new();
        private readonly IGameRepository gameRepository;
        private readonly IMatchGroupManager matchGroupManager;
        private GameMatchedEventHandler? action;

        event GameMatchedEventHandler IMatchManager.OnMatchSuccsee
        {
            add
            {
                action += value;
            }

            remove
            {
                action -= value;
            }
        }

        IMatcher IMatchManager.Add(string connectionId, string playerId)
        {
            IPlayer player = gameRepository.PlayerManager.CreatePlayer(connectionId, playerId);
            return Manager.Add(player);
        }

        void IMatchManager.Remove(string connectionId)
        {
            queue.Remove(connectionId);
        }

        string IMatchManager.CreateMatchId()
        {
            return Guid.NewGuid().ToString();
        }

        IMatcher IMatchManager.Add(IPlayer player)
        {
            IMatcher current = new Matcher(player);
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
                string matchId = matchGroupManager.Add(gamers);
                action?.Invoke(gamers, matchId);
            }
            return current;
        }

        private class MatchQueue
        {
            private readonly List<IMatcher> list = [];
            private readonly Dictionary<string, IMatcher> dic = [];

            public void Enqueue(IMatcher item)
            {
                lock (list)
                {
                    if(!dic.ContainsKey(item.ConnectionId) && !item.Disposed)
                        list.Add(item);
                }
            }
            public IMatcher? Dequeue()
            {
                lock (list)
                {
                    IMatcher? item = list.ElementAtOrDefault(0);
                    while(item is not null && item.Disposed)
                    {
                        list.RemoveAt(0);
                        dic.Remove(item.ConnectionId);
                        item = list.ElementAtOrDefault(0);
                    }
                    return item;
                }
            }

            public void BackQueue(IMatcher item)
            {
                lock (list)
                {
                    list.Insert(0, item);
                }
            }

            public IMatcher? Remove(string connectionId)
            {
                lock (list)
                {
                    if(dic.TryGetValue(connectionId, out var matcher))
                    {
                        dic.Remove(connectionId);
                        matcher.Dispose();
                        return matcher;
                    }
                    return null;
                }
            }
        }

    }
}
