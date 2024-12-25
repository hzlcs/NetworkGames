using System;
using System.Threading.Tasks;
using GameLibrary.Network;

namespace Avalonia.GameClient.Hubs;

public interface IChatHubClient : IChatHub
{
    event Action<ChatMessage[]>? MessageReceived; 
    
    Task SendMessageAsync(ChatMessage message);
}