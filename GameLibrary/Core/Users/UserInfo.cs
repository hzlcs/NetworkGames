using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Core.Users
{
    public class UserInfo(string userId, string userName, string userCode)
    {
        public string UserId { get; set; } = userId;
        public string UserName { get; set; } = userName;
        public string UserCode { get; set; } = userCode;
    }
}
