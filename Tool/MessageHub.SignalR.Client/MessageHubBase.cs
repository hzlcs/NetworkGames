using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net.Sockets;
using GameLibrary.Network.MessageHub.ChatHub;
using MessageHub.Abstraction;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace MessageHub.SignalR.Client;

public abstract class MessageHubClientBase(string url)
{
    
    protected HubConnection? _connection;

    public virtual async Task ConnectAsync()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();
        await _connection.StartAsync();
        BuildServer(_connection);
    }
    
    protected IDisposable BuildClient<TClient>(TClient client)
        where TClient : class, IHubClient
    {
        if (_connection is null)
            throw new InvalidOperationException("Connection is not established");
        return _connection.ClientRegistration(client);
    }

    protected abstract IHubServer BuildServer(HubConnection connection);
}
