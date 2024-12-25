using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.GameClient.Services;
using GameLibrary.Network;
using Microsoft.AspNetCore.SignalR.Client;

namespace Avalonia.GameClient.Hubs;

public class ChatHub : IChatHubClient
{
    public event Action<ChatMessage[]>? MessageReceived;
    

    private readonly HubConnection connection;
    
    public ChatHub(IConfig config, IUserManager userManager)
    {
        connection = new HubConnectionBuilder()
            .WithUrl(config.BaseAddress + "/ChatHub", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult<string?>(userManager.Token);
            })
            .WithAutomaticReconnect()
            .WithKeepAliveInterval(TimeSpan.FromSeconds(5))
            
            .Build();
        connection.On<ChatMessage[]>(nameof(IChatHub.ReceiveMessage), ReceiveMessage);
        connection.StartAsync();
    }
    
    private async Task ReceiveMessage(object?[] data)
    {
        if (data.Length != 1)
            return;
        if(data[0] is not ChatMessage[] messages)
            return;
        await ((IChatHub)this).ReceiveMessage(messages);
    }
    
    Task IChatHub.ReceiveMessage(ChatMessage[] message)
    {
        MessageReceived?.Invoke(message);
        return Task.CompletedTask;
    }

    public async Task SendMessageAsync(ChatMessage[] message)
    {
        using CancellationTokenSource cts = new (3000);
        await connection.SendAsync(nameof(ReceiveMessage), message, cts.Token);
    }
    
    public async Task SendMessageAsync(ChatMessage message)
    {
        await SendMessageAsync([message]);
    }
}