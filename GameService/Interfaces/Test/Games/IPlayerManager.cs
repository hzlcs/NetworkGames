namespace GameService.Interfaces.Test.Games
{
    public interface IPlayerManager
    {
        IEnumerable<IPlayer> Players { get; }

        bool TryGetPlayerByConnectionId(string connectionId, out IPlayer? player);

        bool TryGetPlayerByPlayerId(long playerId, out IPlayer? player);

        IPlayer? PlayerReconnected(long playerId);

        IPlayer? PlayerDisconnected(long playerId);

        IPlayer? RemovePlayerByConnectionId(string connectionId);

        IPlayer? RemovePlayerByPlayerId(long playerId);

        IPlayer CreatePlayer(string connectionId, long playerId);

        IPlayer CreatePlayer(IPlayer player);

        void AddPlayer(IPlayer player);
    }
}
