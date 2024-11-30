namespace GameService.Interfaces.Test.Users;
using Entity.DBEntity.Entity;

public interface IUserManager
{
    Task AddUser(User user, CancellationToken token);
    
    Task<User?> GetUser(User user, CancellationToken token);
    
    Task DeleteUser(User user, CancellationToken token);
}