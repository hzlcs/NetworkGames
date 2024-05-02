using GameService.Interfaces.Test.Games;

namespace GameService.Entity.Test.Games
{
    public class Player : IPlayer
    {
        private readonly string connectionId;
        private readonly string playerId;
        string IPlayer.ConnectionId => connectionId;

        string IPlayer.PlayerId => playerId;

        public Player(string connectionId, string playerId)
        {
            this.connectionId = connectionId;
            this.playerId = playerId;
        }

        public Player(IPlayer player)
        {
            connectionId = player.ConnectionId;
            playerId = player.PlayerId;
        }
    }
}
