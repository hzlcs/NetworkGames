using MessageHub.Abstraction;

namespace GameLibrary.Network.MessageHub.ChatHub;

public interface IChatHubServer : IHubServer
{
    Task ReceiveMessage(ChatMessage[] message);
}