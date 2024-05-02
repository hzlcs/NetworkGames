using System.Collections.Immutable;

namespace GameService.Interfaces.Test.Games
{
    public interface IGame
    {
        string GameId { get; }

        IEnumerable<IPlayer> Players { get; }
    }
}
