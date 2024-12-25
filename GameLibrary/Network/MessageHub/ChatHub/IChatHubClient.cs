using MessageHub.Abstraction;

namespace GameLibrary.Network.MessageHub.ChatHub;

public interface IChatHubClient : IHubClient
{
    Task ReceiveMessage(ChatMessage[] message);
}