using GameService.Interfaces.Test.Games;
using System.Collections.Immutable;

namespace GameService.Entity.Test.Games
{
    public class Game(string gameId, IPlayer[] players) : IGame
    {
        string IGame.GameId => gameId;

        IEnumerable<IPlayer> IGame.Players { get; } = players;
    };
    
}
