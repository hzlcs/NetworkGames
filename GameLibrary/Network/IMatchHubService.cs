﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Network
{
    public interface IMatchHubService
    {
        void Match(long playerId);

        void ConfirmMatch();

        void CancelMatch();
    }
}
