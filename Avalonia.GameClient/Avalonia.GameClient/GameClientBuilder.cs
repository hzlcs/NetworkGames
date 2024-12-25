using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.GameClient.Hubs;
using Avalonia.GameClient.Services;
using Avalonia.GameClient.ViewModels;
using Avalonia.GameClient.ViewModels.Interfaces;
using GameService.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Avalonia.GameClient;

public sealed class GameClientBuilder(AppBuilder builder)
{
    private readonly ServiceCollection services = new();

    private void RegisterServices()
    {
        RegisterViewModel();
        services.AddGameServiceClient(v => v.GetRequiredService<IConfig>().BaseAddress);
        services.AddSingleton<App>();
        services.AddSingleton<IChatHubClient, ChatHub>();
        services.AddSingleton<IGamePageManager, GamePageManager>();
        services.AddSingleton<IMessageBox, MessageBox>();
        services.AddSingleton<ILoginManager, LoginManager>();
        services.AddSingleton<IUserManager>(provider => provider.GetRequiredService<ILoginManager>());
        services.TryAddSingleton<IConfig, DefaultConfig>();
        services.TryAddSingleton<IPopupManager, DefaultPopupManager>();
    }

    private void RegisterViewModel()
    {
        var types = Assembly.GetAssembly(typeof(App))!.GetTypes();

        foreach (var type in types.Where(v => v.GetCustomAttribute<ViewModelAttribute>() is not null))
        {
            var i = type.GetInterfaces().FirstOrDefault(v => v.Name.EndsWith("ViewModel"));
            if (i is not null)
                services.AddSingleton(i, type);
            else
            {
                services.AddSingleton(type);
            }
        }

        foreach (var type in types.Where(v => v.IsClass && v.IsAssignableTo(typeof(IGamePage))))
        {
            var vm = types.First(v => v.Name == type.Name + "Model");
            var i = vm.GetInterface("I" + vm.Name);
            if (i is not null)
                vm = i;
            services.AddKeyedTransient<IGamePage>(vm, (provider, key) =>
            {
                var page = (IGamePage)Activator.CreateInstance(type)!;
                page.DataContext = provider.GetRequiredService((Type)key!);
                return page;
            });
        }
    }

    public GameClientBuilder SetConfig<T>() where T : class, IConfig
    {
        services.AddSingleton<IConfig, T>();
        return this;
    }

    public AppBuilder Build()
    {
        RegisterServices();
        var provider = services.BuildServiceProvider();

        builder.UnsafeAppFactory() = new Func<Application>(provider.GetRequiredService<App>);
        builder.UnsafeApplicationType() = typeof(App);
        return builder;
    }

    private void ConfigHttpClient()
    {
        services.AddHttpClient<HttpClient>("api",
            configureClient: (provider, client) =>
            {
                client.BaseAddress = new Uri(provider.GetRequiredService<IConfig>().BaseAddress);
            });
    }

    public GameClientBuilder SetCloseApplication<T>() where T : class, ICloseApplication
    {
        services.AddSingleton<ICloseApplication, T>();
        return this;
    }

    public GameClientBuilder SetCloseApplication<T>(T closeApplication) where T : class, ICloseApplication
    {
        services.AddSingleton<ICloseApplication>(closeApplication);
        return this;
    }
}

public static class GameClientBuilderExtensions
{
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_appFactory")]
    internal static extern ref Func<Application>? UnsafeAppFactory(this AppBuilder builder);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<ApplicationType>k__BackingField")]
    internal static extern ref Type? UnsafeApplicationType(this AppBuilder builder);

    public static GameClientBuilder UseGameClient(this AppBuilder builder)
    {
        return new GameClientBuilder(builder);
    }
}