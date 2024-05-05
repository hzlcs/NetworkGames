using GameService.Interfaces.Test.Games;
using System.Collections.Concurrent;

namespace GameService.Utility
{
    public class PlayerDictionary
    {
        private readonly ConcurrentDictionary<long, IPlayer> players1 = new();
        private readonly ConcurrentDictionary<string, IPlayer> players2 = new();

        public IEnumerable<IPlayer> Players => players2.Values;

        public bool TryGetPlayerByConnectionId(string connectionId, out IPlayer? player)
        {
            return players2.TryGetValue(connectionId, out player);
        }

        public IPlayer? RemovePlayerByConnectionId(string connectionId)
        {
            if(players2.TryRemove(connectionId, out var player))
                players1.TryRemove(player.PlayerId, out _);
            return player;
        }

        public bool TryGetPlayerByPlayerId(long playerId, out IPlayer? player)
        {
            return players1.TryGetValue(playerId, out player);
        }

        public bool Exist(long playerId) => players1.ContainsKey(playerId);
        public bool Exist(string connectionId) => players2.ContainsKey(connectionId);

        public IPlayer? RemovePlayerByPlayerId(long playerId)
        {
            players1.TryRemove(playerId, out var player);
            return player;
        }

        public void AddPlayer(IPlayer player)
        {
            players2.AddOrUpdate(player.ConnectionId, player, (_, _) => player);
            players1.AddOrUpdate(player.PlayerId, player, (_, _) => player);
        }

        public IPlayer? PlayerReconnected(long playerId, string connectionId)
        {
            if (players1.TryGetValue(playerId, out var player))
            {
                player.ConnectionId = connectionId;
                player.LoseConnection = false;
                return player;
            }
            return null;
        }

        public IPlayer? PlayerDisconnected(long playerId)
        {
            if (players1.TryGetValue(playerId, out var player))
            {
                player.LoseConnection = true;
                return player;
            }
            return null;
        }
    }
}
