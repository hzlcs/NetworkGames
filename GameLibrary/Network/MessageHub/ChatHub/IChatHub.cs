using MessageHub.Abstraction;

namespace GameLibrary.Network.MessageHub.ChatHub;

public interface IChatHub : IMessageHub<IChatHubClient, IChatHubServer>
{
    
}