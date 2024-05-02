using GameService.Interfaces.Test.Games;

namespace GameService.Interfaces.Test.Matchs
{
    public interface IMatcher : IPlayer, IDisposable
    {
        IPlayer Player { get; }
        bool Disposed { get; }
        bool Matched { get; set; }
        bool Confirmed { get; set; }
    }
}
