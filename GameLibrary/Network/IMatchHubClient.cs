﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Network
{
    public interface IMatchHubClient
    {
        void MatchConfirmed(string gameId);

        void MatchCanceled(bool timeout);

        void Matched(string matchId);
    }
}