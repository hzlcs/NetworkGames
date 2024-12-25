using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.GameClient.Desktop.Services;
using Avalonia.GameClient.ViewModels.Interfaces;

namespace Avalonia.GameClient.Desktop;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args, LifeTimeBuilder);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<Application>()
            .UseGameClient()
            .SetCloseApplication<CloseApplication>()
            .Build()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

    class CloseApplication : ICloseApplication
    {
        public void ShunDown()
        {
            closeApplication.Invoke();
        }
    }

    private static Action closeApplication = null!;
    
    private static void LifeTimeBuilder(IClassicDesktopStyleApplicationLifetime desktop)
    {
        closeApplication = () => desktop.Shutdown(0);
    }
}