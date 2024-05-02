using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Network
{
    public interface IMatchHubService
    {
        void ConfirmMatch(string matchId);

        void CancelMatch(string matchId);
    }
}
