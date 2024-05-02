using GameService.Interfaces.Test.Games;
using System.Collections.Concurrent;

namespace GameService.Entity.Test.Games
{
    public class GameManager : IGameManager
    {
        private readonly ConcurrentDictionary<string, IGame> games = [];
        public IGame CreateGame(IPlayer[] players)
        {
            string id = Guid.NewGuid().ToString();
            var res = new Game(id, players);
            games.TryAdd(id, res);
            return res;
        }
    }
}
