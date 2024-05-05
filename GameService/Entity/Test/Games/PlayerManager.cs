using GameService.Interfaces.Test.Games;
using System.Collections.Concurrent;

namespace GameService.Entity.Test.Games
{
    public class PlayerManager : IPlayerManager
    {
        readonly ConcurrentDictionary<long, IPlayer> players = new();

        IEnumerable<IPlayer> IPlayerManager.Players => players.Values;

        bool IPlayerManager.TryGetPlayerByConnectionId(string connectionId, out IPlayer? player)
        {
            throw new NotImplementedException();
        }

        IPlayer? IPlayerManager.RemovePlayerByConnectionId(string connectionId)
        {
            throw new NotImplementedException();
        }

        IPlayer IPlayerManager.CreatePlayer(string connectionId, long playerId)
        {
            return new Player(connectionId, playerId);
        }

        void IPlayerManager.AddPlayer(IPlayer player)
        {
            players.AddOrUpdate(player.PlayerId, new Player(player), (_, _) => new Player(player));
        }

        bool IPlayerManager.TryGetPlayerByPlayerId(long playerId, out IPlayer? player)
        {
            return players.TryGetValue(playerId, out player);
        }

        IPlayer? IPlayerManager.RemovePlayerByPlayerId(long playerId)
        {
            throw new NotImplementedException();
        }

        IPlayer IPlayerManager.CreatePlayer(IPlayer player)
        {
            return new Player(player);
        }

        IPlayer? IPlayerManager.PlayerReconnected(long playerId)
        {
            throw new NotImplementedException();
        }

        IPlayer? IPlayerManager.PlayerDisconnected(long playerId)
        {
            throw new NotImplementedException();
        }
    }
}
