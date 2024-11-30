using System.Text.RegularExpressions;
using GameLibrary.Core.Users;
using GameService.Abstraction;
using GameService.Abstraction.Entity;
using Microsoft.AspNetCore.Mvc;
using GameService.Entity.DBEntity.Entity;
using GameService.Entity.DBEntity;
using GameService.Interfaces.Test.Users;

namespace GameService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class UserController(IUserManager userManager) : Controller, IUserController
    {
        [HttpPost("[action]")]
        public async Task<ApiResult<UserInfo>?> Login(string userCode, string password, CancellationToken token)
        {
            ApiResult<UserInfo> res = new(0, "", null);
            if (!UserCodeRegex().IsMatch(userCode))
                return res with { Code = 1, Message = "用户不存在" };
            try
            {
                var users = await userManager.GetUser(new User(userCode), token);
                if (users is null)
                    return res with { Code = 1, Message = "用户不存在" };
                if (users.Password != password)
                    return res with { Code = 1, Message = "密码错误" };
                return res with { Message = "登录成功", Data = users.ToUserInfo() };
            }
            catch (Exception e)
            {
                return res with { Code = -1, Message = e.Message };
            }
        }

        [HttpPost("[action]")]
        public async Task<ApiResult?> Register(string userCode, string password,
            CancellationToken token)
        {
            ApiResult res = new(0, "");
            if (!UserCodeRegex().IsMatch(userCode))
                return res with { Code = 1, Message = "账号格式错误" };
            try
            {
                var users = await userManager.GetUser(new User(userCode), token);
                if (users is not null)
                    return res with { Code = 2, Message = "用户已存在" };
                await userManager.AddUser(new User(userCode, password), token);
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
            ApiResult res = new (0, "删除成功");
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

        [GeneratedRegex(@"^[a-zA-Z0-9]{5,12}")]
        private static partial Regex UserNameRegex();
    }
}