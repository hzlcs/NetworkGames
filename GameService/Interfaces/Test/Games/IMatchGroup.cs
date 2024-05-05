using GameService.Interfaces.Test.Matchs;

namespace GameService.Interfaces.Test.Games
{
    public interface IMatchGroupManager
    {
        event MatchGroupTimeoutEventHander? MatchGroupTimeoutCanceled;
        string Add(IMatcher[] matcher);
        IMatcher[]? Confirm(string connectionId);
        IMatcher[]? Cancel(string connectionId);
    }

    public delegate void MatchGroupTimeoutEventHander(IMatcher[] matchers);

}
