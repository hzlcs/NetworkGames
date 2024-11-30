using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.GameClient.Services;
using Avalonia.GameClient.ViewModels;
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
            services.AddSingleton<IMessageBox, MessageBox>();
            services.AddSingleton<ILoginManager, LoginManager>();
            services.TryAddSingleton<IConfig, DefaultConfig>();
        }

        private void RegisterViewModel()
        {
            var types = Assembly.GetAssembly(typeof(App))!.GetTypes()
                .Where(v => v.GetCustomAttribute<ViewModelAttribute>() is not null).ToArray();
            foreach (var type in types)
            {
                var i = type.GetInterfaces().FirstOrDefault(v => v.Name.EndsWith("ViewModel"));
                if(i is not null)
                    services.AddSingleton(i, type);
                else
                {
                    services.AddSingleton(type);
                }
            }
        }

        public GameClientBuilder UseConfig<T>() where T : class, IConfig
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
        
        public GameClientBuilder AddPopup<T>() where T : class, IPopupManager
        {
            services.TryAddSingleton<TopLevel>(v=> v.GetRequiredService<App>().GetTopLevel()!);
            services.AddSingleton<IPopupManager, T>();
            return this;
        }
        public GameClientBuilder AddPopup<T>(T popupManager) where T : class, IPopupManager
        {
            services.AddSingleton<IPopupManager>(v=> popupManager.SetTopLevel(v.GetRequiredService<App>().GetTopLevel()));
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