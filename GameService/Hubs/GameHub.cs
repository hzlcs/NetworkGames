using GameLibrary.Network;
using GameService.Interfaces.Test.Games;
using Microsoft.AspNetCore.SignalR;

namespace GameService.Hubs
{
    public class GameHub(IGameRepository gameRepository) : Hub
    {
        private readonly IGameRepository gameRepository = gameRepository;

        public override Task OnConnectedAsync()
        {
            

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            gameRepository.PlayerManager.RemovePlayerByConnectionId(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message)
        {
            await Clients.Others.SendAsync("ReceiveMessage", GetMessage(message));
        }

        private Message GetMessage(string message)
        {
            return new Message() { Id = Context.UserIdentifier ?? Context.ConnectionId, Msg = message, Type = MessageType.CommonMessage };
        }
    }
}