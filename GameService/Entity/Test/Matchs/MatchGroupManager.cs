using GameLibrary.Utility;
using GameService.Interfaces.Test.Games;
using GameService.Interfaces.Test.Matchs;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace GameService.Entity.Test.Matchs
{
    public class MatchGroupManager : IMatchGroupManager
    {
        private MatchGroupCancelEventHandler? matchGroupCancelEventHandler;
        event MatchGroupCancelEventHandler IMatchGroupManager.OnMatchGroupCanceled
        {
            add
            {
                matchGroupCancelEventHandler += value;
            }

            remove
            {
                matchGroupCancelEventHandler -= value;
            }
        }

        private MatchGroupConfirmEventHandler? matchGroupConfirmEventHandler;
        event MatchGroupConfirmEventHandler IMatchGroupManager.OnMatchGroupConfirmed
        {
            add
            {
                matchGroupConfirmEventHandler += value;
            }

            remove
            {
                matchGroupConfirmEventHandler -= value;
            }
        }

        private readonly ConcurrentDictionary<string, IMatcher[]> matcherDic = [];
        private readonly DelayQueue<string> delayQueue = new();
        private readonly IGameRepository gameRepository;

        public MatchGroupManager(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
            Task.Run(async () =>
            {
                while (true)
                {
                    var matchId = await delayQueue.DequeueAsync();
                    if (matcherDic.TryRemove(matchId, out var matchers))
                    {
                        matchGroupCancelEventHandler?.Invoke(matchers, true);
                    }
                }
            });
        }

        string IMatchGroupManager.Add(IMatcher[] matcher)
        {
            string matchId = Guid.NewGuid().ToString();
            matcherDic.TryAdd(matchId, matcher);
            delayQueue.Enqueue(matchId, TimeSpan.FromSeconds(10));
            return matchId;
        }

        void IMatchGroupManager.Cancel(string matchId, string connectionId)
        {
            if(matcherDic.TryRemove(matchId, out IMatcher[]? matchers))
                matchGroupCancelEventHandler?.Invoke(matchers, false);
        }

        void IMatchGroupManager.Confirm(string matchId, string connectionId)
        {
            if (!matcherDic.TryGetValue(matchId, out IMatcher[]? matchers) || matchers is null)
                return;
            var matcher = matchers.FirstOrDefault(v => v.ConnectionId == connectionId);
            if (matcher is null)
                return;
            matcher.Confirmed = true;
            if (matchers.All(v => v.Confirmed))
            {
                if (!matcherDic.TryRemove(matchId, out _))
                    return;
                var game = gameRepository.GameManager.CreateGame(matchers.Select(v => v.Player).ToArray());
                matchGroupConfirmEventHandler?.Invoke(matchers, game);
            }
        }
    }
}