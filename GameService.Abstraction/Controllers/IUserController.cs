using GameLibrary.Core.Users;
using GameService.Abstraction.Entity;

namespace GameService.Abstraction.Controllers;

public interface IUserController : IController
{
    
    Task<ApiResult<LoginInfo>?> Login(string userCode, string password, CancellationToken token);

    Task<ApiResult?> Register(string userName, string userCode, string password, CancellationToken token);

    Task<ApiResult?> Delete(long id, CancellationToken token);
}
