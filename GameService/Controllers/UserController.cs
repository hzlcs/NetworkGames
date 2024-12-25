using System.Text;
using System.Text.RegularExpressions;
using GameLibrary.Core.Users;
using GameService.Abstraction;
using GameService.Abstraction.Controllers;
using GameService.Abstraction.Entity;
using Microsoft.AspNetCore.Mvc;
using GameService.Entity.DBEntity.Entity;
using GameService.Entity.DBEntity;
using GameService.Interfaces.Test.Users;
using GameService.Utility;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace GameService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class UserController(IUserManager userManager, IDistributedCache cache, JwtHelper jwt) : Controller, IUserController
    {
        
        private static DistributedCacheEntryOptions UserCacheOption => new()
        {
            SlidingExpiration = TimeSpan.FromHours(1)
        };
        private static string GetUserCacheKey(string userCode) => $"user.{userCode}";

        [HttpPost("[action]")]
        public async Task<ApiResult<LoginInfo>?> Login(string userCode, string password, CancellationToken token)
        {
            ApiResult<LoginInfo> res = new(0, "", null);
            var cacheUser = await cache.GetObject<User>(GetUserCacheKey(userCode), token);
            if (cacheUser is not null)
            {
                if (cacheUser.Password != password)
                    return res with { Code = 1, Message = "密码错误" };
                return res with { Message = "登录成功", Data = new LoginInfo(cacheUser.ToUserInfo(), jwt.CreateToken(cacheUser.Id))};
            }
            if (!UserCodeRegex().IsMatch(userCode))
                return res with { Code = 1, Message = "用户不存在" };
            try
            {
                var users = await userManager.GetUser(new User(userCode), token);
                if (users is null)
                    return res with { Code = 1, Message = "用户不存在" };
                await cache.SetObject(GetUserCacheKey(userCode), users, UserCacheOption, token);
                if (users.Password != password)
                    return res with { Code = 1, Message = "密码错误" };
                return res with { Message = "登录成功", Data = new LoginInfo(users.ToUserInfo(), jwt.CreateToken(users.Id)) };
            }
            catch (Exception e)
            {
                return res with { Code = -1, Message = e.Message };
            }
        }

        [HttpPost("[action]")]
        public async Task<ApiResult?> Register(string userName, string userCode, string password,
            CancellationToken token)
        {
            ApiResult res = new(0, "");
            if (!UserCodeRegex().IsMatch(userCode))
                return res with { Code = 1, Message = "账号格式错误" };
            if (!UserNameRegex().IsMatch(userName))
                return res with { Code = 1, Message = "用户名格式错误" };
            try
            {
                var users = await userManager.GetUser(new User(userCode), token);
                if (users is not null)
                    return res with { Code = 2, Message = "用户已存在" };
                var user = new User(userName, userCode, password);
                await userManager.AddUser(user, token);
                await cache.SetObject(GetUserCacheKey(userCode), user, UserCacheOption, token);
                return res with { Message = "注册成功" };
            }
            catch (Exception e)
            {
                return res with { Code = -1, Message = e.Message };
            }
        }

        [HttpPost("[action]")]
        public async Task<ApiResult?> Delete(long id, CancellationToken token)
        {
            ApiResult res = new(0, "删除成功");
            var user = await userManager.GetUser(new User(id), token);
            if (user is null)
                return res;
            await userManager.DeleteUser(user, token);
            return res with { Message = "删除成功" };
        }


        [HttpGet("GetValue")]
        public async Task<string> GetValue(string value)
        {
            return await Task.FromResult(value);
        }

        [GeneratedRegex(@"^[a-zA-Z0-9]{5,12}")]
        private static partial Regex UserCodeRegex();

        [GeneratedRegex(@"^[\u4e00-\u9fa5a-zA-Z0-9]{6,12}$")]
        private static partial Regex UserNameRegex();
    }
}