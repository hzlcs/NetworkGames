using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Core.Users
{
    public class UserInfo(long userId, string userName, string userCode)
    {
        public long UserId { get; set; } = userId;
        public string UserName { get; set; } = userName;
        public string UserCode { get; set; } = userCode;
    }
}
