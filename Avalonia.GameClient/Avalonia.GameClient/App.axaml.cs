using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.GameClient.Services;
using Avalonia.Markup.Xaml;
using Avalonia.GameClient.ViewModels;
using Avalonia.GameClient.ViewModels.Interfaces;
using Avalonia.GameClient.Views;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Avalonia.GameClient;

public partial class App(IServiceProvider services) : Application
{
    public static IServiceProvider Services { get; private set; } = null!;
    
    public override void Initialize()
    {
        Services = services;
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
            desktop.MainWindow = new MainWindow();
            desktop.MainWindow.DataContext = services.GetRequiredService<MainViewModel>();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView();
            singleViewPlatform.MainView.DataContext = services.GetRequiredService<MainViewModel>();
        }
        base.OnFrameworkInitializationCompleted();
    }


}
