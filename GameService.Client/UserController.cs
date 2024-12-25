using System.Diagnostics;
using GameLibrary.Core.Users;
using GameService.Abstraction;
using GameService.Abstraction.Controllers;
using GameService.Abstraction.Entity;

namespace GameService.Client;

public class UserController(IHttpClientFactory factory) : ControllerBase(factory), IUserController
{
    
    public async Task<ApiResult<LoginInfo>?> Login(string userCode, string password, CancellationToken token)
    {
        // if(Debugger.IsAttached)
        //     return new ApiResult<UserInfo>(0,null, new UserInfo(1, userCode, userCode));
        return await Client.PostApiResultAsync<LoginInfo>($"User/Login?userCode={userCode}&password={password}", null, token);
    }

    public async Task<ApiResult?> Register(string userName, string userCode, string password, CancellationToken token)
    {
        return await Client.PostApiResultAsync($"User/Register?userName={userName}&userCode={userCode}&password={password}", null, token);
    }

    public async Task<ApiResult?> Delete(long id, CancellationToken token)
    {
        return await Client.PostApiResultAsync($"User/Delete?id={id}", null, token);
    }

}