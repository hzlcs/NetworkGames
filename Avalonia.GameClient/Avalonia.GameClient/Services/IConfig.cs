namespace Avalonia.GameClient.Services;

public interface IConfig
{
    string BaseAddress { get; }
}

public class DefaultConfig : IConfig
{
    public string BaseAddress => "http://localhost:5000";
}