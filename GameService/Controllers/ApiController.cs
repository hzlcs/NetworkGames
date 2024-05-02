using GameLibrary.Core.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameService.Entity.DBEntity.Entity;
using GameService.Entity.DBEntity;
namespace GameService.Controllers
{
    [ApiController]
    [Route("/Api")]
    public class ApiController : Controller
    {
        private readonly MyDbContext context;

        public ApiController(MyDbContext context)
        {
            this.context = context;
        }

        [HttpGet("LoginIn")]
        public async Task<UserInfo?> LoginIn(string userCode, string password)
        {
            var users = await context.Set<Users>().FirstOrDefaultWithNoLockAsync(u => u.UserCode == userCode && u.Password == password);
            return users?.ToUserInfo();
        }

        [HttpGet("GetValue")]
        public async Task<string> GetValue(string value)
        {
            return await Task.FromResult(value);
        }
    }
}
