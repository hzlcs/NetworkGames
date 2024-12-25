using GameLibrary.Core.Users;

namespace Avalonia.GameClient.Services;

public interface IUserManager
{
    UserInfo CurrentUser { get; }
    
    string Token { get; }
}

