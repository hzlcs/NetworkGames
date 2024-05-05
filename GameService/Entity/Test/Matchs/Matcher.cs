using GameService.Entity.Test.Games;
using GameService.Interfaces.Test.Games;
using GameService.Interfaces.Test.Matchs;

namespace GameService.Entity.Test.Matchs
{
    public sealed class Matcher(string connectionId, long playerId) : IMatcher
    {
        private volatile bool disposed;

        public bool Matched { get; set; }

        bool IMatcher.Disposed => disposed;

        string IPlayer.ConnectionId
        {
            get => connectionId;
            set => connectionId = value;
        }
        long IPlayer.PlayerId => playerId;
        bool IPlayer.LoseConnection { get; set; }

        public bool Confirmed { get; set; }
        public string? MatchId { get; set; }

        void IDisposable.Dispose()
        {
            disposed = true;
            ((IPlayer)this).LoseConnection = true;
        }
    }
}
