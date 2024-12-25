using System.Diagnostics;
using System.Reflection;
using MessageHub.Abstraction;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace MessageHub.SignalR.Client;

public static partial class Extensions
{
    public static IServiceCollection UseSignalRClient(this IServiceCollection services)
    {
        var assembly = Assembly.GetCallingAssembly();
        var types = assembly.GetTypes();
        foreach (var type in types)
        {
            if (!type.IsAssignableTo(typeof(IMessageHub<,>)))
                continue;
            var genericArguments = type.GetGenericArguments();
            
        }
        return services;
    }
    
    [HubClientProxy]
    public static partial IDisposable ClientRegistration<T>(this HubConnection connection, T provider);
    
    [HubServerProxy]
    public static partial T ServerProxy<T>(this HubConnection connection);

}

[AttributeUsage(AttributeTargets.Method)]
public class HubClientProxyAttribute : Attribute{}

[AttributeUsage(AttributeTargets.Method)]
public class HubServerProxyAttribute : Attribute{}
