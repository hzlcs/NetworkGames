using GameService.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.Client;

public static class Extensions
{
    public static IServiceCollection AddGameServiceClient(this IServiceCollection services,Func<IServiceProvider, string> getBaseAddress)
    {
        services.AddHttpClient(IController.Name, (provider, client) =>
        {
            client.BaseAddress = new Uri(getBaseAddress(provider));
        });
        services.AddSingleton<IUserController, UserController>();
        return services;
    }
}