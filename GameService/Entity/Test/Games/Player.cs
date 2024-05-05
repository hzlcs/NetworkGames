using GameService.Interfaces.Test.Games;

namespace GameService.Entity.Test.Games
{
    public class Player : IPlayer
    {
        private string connectionId;
        private readonly long playerId;

        string IPlayer.ConnectionId
        {
            get => connectionId;
            set => connectionId = value;
        }
        long IPlayer.PlayerId => playerId;
        bool IPlayer.LoseConnection { get; set; }

        public Player(string connectionId, long playerId)
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
