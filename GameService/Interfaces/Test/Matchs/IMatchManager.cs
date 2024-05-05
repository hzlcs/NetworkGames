using GameService.Interfaces.Test.Games;

namespace GameService.Interfaces.Test.Matchs
{
    public interface IMatchManager
    {
        IMatcher Add(IMatcher matcher);
        IMatcher Add(string connectionId, long playerId);
        void Remove(string connectionId);
        string CreateMatchId();
        void Confirm(string connectionId);
        void Cancel(string connectionId);
    }

    public delegate void GameMatchedCallback(IMatcher[] matchers);
}
