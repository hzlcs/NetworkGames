using GameService.Interfaces.Test.Games;

namespace GameService.Interfaces.Test.Matchs
{
    public interface IMatchManager
    {
        IMatcher Add(IPlayer player);
        IMatcher Add(string connectionId, string playerId);
        void Remove(string connectionId);
        event GameMatchedEventHandler OnMatchSuccsee;
        string CreateMatchId();
    }

    public delegate void GameMatchedEventHandler(IMatcher[] matchers, string matchId);
}
