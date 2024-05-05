using GameLibrary.Network;
using GameLibrary.Utility;
using GameService.Interfaces.Test.Games;
using GameService.Interfaces.Test.Matchs;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace GameService.Entity.Test.Matchs
{
    public class MatchGroupManager : IMatchGroupManager
    {

        private readonly ConcurrentDictionary<string, IMatcher[]> matcherDic = [];
        private readonly ConcurrentDictionary<string, string> matchIdDic = [];
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
                        MatchGroupTimeoutCanceled?.Invoke(matchers);
                    }
                }
            });
        }

        public event MatchGroupTimeoutEventHander? MatchGroupTimeoutCanceled;

        string IMatchGroupManager.Add(IMatcher[] matcher)
        {
            string matchId = Guid.NewGuid().ToString();
            matcherDic.TryAdd(matchId, matcher);
            foreach (var m in matcher)
            {
                matchIdDic.TryAdd(m.ConnectionId, matchId);
            }
            delayQueue.Enqueue(matchId, TimeSpan.FromSeconds(10));
            return matchId;
        }

        IMatcher[]? IMatchGroupManager.Cancel(string connectionId)
        {
            if (!matchIdDic.TryRemove(connectionId, out string? matchId) || matchId is null)
                return null;
            if (!matcherDic.TryRemove(matchId, out IMatcher[]? matchers) || matchers is null)
                return null;
            foreach (var i in matchers)
            {
                i.Confirmed = false;
                matchIdDic.TryRemove(i.ConnectionId, out _);
            }
            return matchers;
        }

        IMatcher[]? IMatchGroupManager.Confirm(string connectionId)
        {
            if (!matchIdDic.TryGetValue(connectionId, out string? matchId) || matchId is null)
                return null;
            if (!matcherDic.TryGetValue(matchId, out IMatcher[]? matchers) || matchers is null)
                return null;
            var matcher = matchers.FirstOrDefault(v => v.ConnectionId == connectionId);
            if (matcher is null)
                return null;
            matcher.Confirmed = true;
            if (!matchers.All(v => v.Confirmed))
                return null;
            if (!matcherDic.TryRemove(matchId, out var ms))
                return null;
            foreach (var m in ms)
                matchIdDic.TryRemove(m.ConnectionId, out _);
            var players = ms.Select(gameRepository.PlayerManager.CreatePlayer).ToArray();
            foreach (var item in players)
            {
                gameRepository.PlayerManager.AddPlayer(item);
            }
            gameRepository.GameManager.CreateGame(players);
            return ms;

        }
    }
}