namespace MessageHub.Abstraction;



public interface IMessageHub<out TClient, out TServer>  where TClient : IHubClient where TServer : IHubServer
{
    TClient Client { get; }
    
    TServer? Server { get; }
    
    Task ConnectAsync();
}