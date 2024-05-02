namespace GameService.Interfaces.Test.Games
{
    public interface IPlayerManager
    {
        IEnumerable<IPlayer> Players { get; }

        bool TryGetPlayerByConnectionId(string connectionId, out IPlayer? player);

        void RemovePlayerByConnectionId(string connectionId);

        IPlayer CreatePlayer(string connectionId, string playerId);

        void AddPlayer(IPlayer player);
    }
}
