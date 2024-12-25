using System.Diagnostics;
using GameLibrary.Network;
using GameService.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;

namespace GameService.Hubs;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ChatHub(IDatabase database, ILogger<ChatHub> logger) : Hub<IChatHub>, IChatHub
{
    private const string CacheKey = "ChatMessage";

    public override async Task OnConnectedAsync()
    {
        var context = Context;
        logger.LogInformation("UserIdentifier: {UserIdentifier}", context.UserIdentifier);
        await Clients.Caller.ReceiveMessage(await database.ListRange<ChatMessage>(CacheKey));
        await base.OnConnectedAsync();
    }

    public async Task ReceiveMessage(ChatMessage[] message)
    {
        await CacheMessage(message);
        await SendMessageAsync(message);
    }

    public async Task SendMessageAsync(ChatMessage[] message)
    {
        await Clients.All.ReceiveMessage(message);
    }


    private async Task CacheMessage(ChatMessage[] message)
    {
        long count = await database.EnqueueRange(CacheKey, message);
        if (count > 50)
            await database.Pop(CacheKey, 25);
    }
}