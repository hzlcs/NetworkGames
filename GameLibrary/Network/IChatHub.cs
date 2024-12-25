namespace GameLibrary.Network;

public interface IChatHub
{
    Task ReceiveMessage(ChatMessage[] message);
    
    Task SendMessageAsync(ChatMessage[] message);
}