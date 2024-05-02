using GameLibrary.Core.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameService.Entity.DBEntity.Entity
{
    [Table("Users")]
    public class Users
    {
        public long Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string UserCode { get; set; } = null!;
        public DateTime CreateTime { get; set; }

        public UserInfo ToUserInfo()
        {
            return new UserInfo(Id.ToString(), UserName, UserCode);
        }
    }
}
