using System.ComponentModel.DataAnnotations;
using GameLibrary.Core.Users;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameService.Entity.DBEntity.Entity
{
    [Table("Users")]
    [PrimaryKey(nameof(Id))]
    public class User
    {
        public long Id { get; set; }
        [MaxLength(30)] public string? UserName { get; set; }
        [MaxLength(30)] public string? Password { get; set; }
        [MaxLength(30)] public string? UserCode { get; set; }

        public DateTime CreateTime { get; set; }

        public User()
        {
        }

        public User(string userCode)
        {
            UserCode = userCode;
        }

        public User(long id)
        {
            Id = id;
        }

        public User(string userCode, string password)
        {
            UserCode = userCode;
            Password = password;
            CreateTime = DateTime.Now;
        }

        public User(string userName, string userCode, string password)
        {
            UserName = userName;
            UserCode = userCode;
            Password = password;
            CreateTime = DateTime.Now;
        }

        public UserInfo ToUserInfo()
        {
            return new UserInfo(Id, UserName!, UserCode!);
        }
    }
}