using GameService.Entity.DBEntity;
using GameService.Interfaces.Test.Users;
using GameService.Entity.DBEntity.Entity;
using Microsoft.EntityFrameworkCore;

namespace GameService.Entity.Test.UserManage;

public class UserManager(MyDbContext context) : IUserManager
{
    private DbSet<User> Users => context.Set<User>();
    
    public async Task AddUser(User user, CancellationToken token)
    {
        var exist = await GetUser(user, token);
        if (exist is null)
        {
            await Users.AddAsync(user, token);
            await context.SaveChangesAsync(token);
        }

    }
    public async Task<User?> GetUser(User user, CancellationToken token)
    {
        if (user.Id > 0)
        {
            return await Users.FirstOrDefaultAsync(v => v.Id == user.Id, token);
        }

        if (!string.IsNullOrEmpty(user.UserCode))
            return await Users.FirstOrDefaultAsync(v => v.UserCode == user.UserCode, token);
        if (!string.IsNullOrEmpty(user.UserName))
            return await Users.FirstOrDefaultAsync(v => v.UserName == user.UserName, token);
        return null;
    }

    public async Task DeleteUser(User user, CancellationToken token)
    {
        var exist = await GetUser(user, token);
        if(exist is null)
            return;
        Users.Remove(user);
        await context.SaveChangesAsync(token);
    }
}