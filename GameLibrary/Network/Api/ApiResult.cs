namespace GameLibrary.Network.Api;

public record ApiResult(int Code, string? Message);


public record ApiResult<T>(int Code, string? Message, T? Data) : ApiResult(Code, Message);
