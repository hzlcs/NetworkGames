using GameLibrary.Core.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Utility.Interface
{
    public interface IGameHttpClient
    {
        Task<UserInfo?> GetUserInfoAsync(string userCode, string password);
    }
}
