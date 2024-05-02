using GameService.Interfaces.Test.Matchs;

namespace GameService.Interfaces.Test.Games
{
    public interface IMatchGroupManager
    {
        event MatchGroupCancelEventHandler OnMatchGroupCanceled;
        event MatchGroupConfirmEventHandler OnMatchGroupConfirmed;
        string Add(IMatcher[] matcher);
        void Confirm(string matchId, string connectionId);
        void Cancel(string matchId, string connectionId);
    }

    public delegate void MatchGroupConfirmEventHandler(IMatcher[] matchers, IGame game);
    public delegate void MatchGroupCancelEventHandler(IMatcher[] matchers, bool timeout);

}
