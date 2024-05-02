using GameService.Entity.Test.Games;
using GameService.Interfaces.Test.Games;
using GameService.Interfaces.Test.Matchs;

namespace GameService.Entity.Test.Matchs
{
    public sealed class Matcher(IPlayer player) : IMatcher
    {
        private volatile bool disposed;

        IPlayer IMatcher.Player => player;

        public bool Matched { get; set; }

        bool IMatcher.Disposed => disposed;

        string IPlayer.ConnectionId => player.ConnectionId;
        string IPlayer.PlayerId => player.PlayerId;

        public bool Confirmed { get; set; }


        void IDisposable.Dispose()
        {
            disposed = true;
        }
    }
}
