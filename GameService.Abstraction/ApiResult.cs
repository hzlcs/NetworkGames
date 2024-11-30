namespace GameService.Abstraction;

public record ApiResult(int Code, string? Message);


public record ApiResult<T>(int Code, string? Message, T? Data) : ApiResult(Code, Message);