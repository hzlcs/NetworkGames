using GameLibrary.Core.Users;

namespace GameService.Abstraction.Entity;

public record LoginInfo(UserInfo UserInfo, string Token);
