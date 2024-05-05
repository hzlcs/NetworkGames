
using GameClient.Utility;
using GameClient.Utility.Interface;
using GameClient.Utility.Network;
using GameLibrary.Network;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Data;
using System.Windows;

namespace GameClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; }

        public App()
        {
            using IServiceScope scope = ServiceProvider.CreateScope();
            var loginWindow = ServiceProvider.GetRequiredService<GameViews.LoginWindow>();
            loginWindow.Show();
        }

        static App()
        {
            ServiceCollection services = new ();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        static void ConfigureServices(ServiceCollection services)
        {
            services.AddHttpClient<IGameHttpClient, GameHttpClient>();
            services.AddSingleton<IMessageDisplay,  MessageDisplay>();
            services.AddScoped<IMatchHubServiceManager, MatchService>();
            services.AddScoped<GameViews.LoginWindow>();
            services.AddScoped<GameViews.MainWindow>();
            services.AddScoped<ViewModels.LoginVM>();
            services.AddScoped<IMatchHubClientManager, ViewModels.MainVM>();
        }

        static void Log(string message)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " " + message);
        }
    }

}
