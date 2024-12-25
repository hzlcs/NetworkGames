using GameLibrary.Network;
using GameLibrary.Network.MessageHub.ChatHub;
using MessageHub.Abstraction;
using Microsoft.AspNetCore.SignalR.Client;
using IChatHub = GameLibrary.Network.MessageHub.ChatHub.IChatHub;

namespace MessageHub.SignalR.Client;

public class ChatHub(string baseAddress)
    : MessageHubClientBase(baseAddress + "/" + nameof(ChatHub)), IChatHub, IChatHubClient
{
    public IChatHubClient Client => this;
    public IChatHubServer? Server { get; private set; }
    
    protected override IHubServer BuildServer(HubConnection connection)
    {
        Server = connection.ServerProxy<IChatHubServer>();
        return Server;
    }

    public Task ReceiveMessage(ChatMessage[] message)
    {
        throw new NotImplementedException();
    }
}