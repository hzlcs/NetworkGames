using GameService.Interfaces.Test.Games;
using System.Collections.Concurrent;

namespace GameService.Entity.Test.Games
{
    public class PlayerManager : IPlayerManager
    {
        readonly ConcurrentDictionary<string, Player> players = new();

        IEnumerable<IPlayer> IPlayerManager.Players => players.Values;

        bool IPlayerManager.TryGetPlayerByConnectionId(string connectionId, out IPlayer? player)
        {
            bool res = players.TryGetValue(connectionId, out var p);
            player = p;
            return res;
        }

        void IPlayerManager.RemovePlayerByConnectionId(string connectionId)
        {
            players.TryRemove(connectionId, out _);
        }

        IPlayer IPlayerManager.CreatePlayer(string connectionId, string playerId)
        {
            return new Player(connectionId, playerId);
        }

        void IPlayerManager.AddPlayer(IPlayer player)
        {
            players.AddOrUpdate(player.ConnectionId, new Player(player), (_, _) => new Player(player));
        }
    }
}
