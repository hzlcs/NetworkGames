using GameService.Interfaces.Test.Games;

namespace GameService.Interfaces.Test.Matchs
{
    public interface IMatcher : IPlayer, IDisposable
    {
        bool Disposed { get; }
        bool Matched { get; set; }
        bool Confirmed { get; set; }
        string? MatchId { get; set; }
    }
}
