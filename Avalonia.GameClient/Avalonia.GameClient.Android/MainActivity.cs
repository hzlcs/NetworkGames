using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;
using Avalonia.GameClient.ViewModels.Interfaces;

namespace Avalonia.GameClient.Android;

[Activity(
    Label = "Avalonia.GameClient.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true, AllowEmbedded = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<Application>, ICloseApplication
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        builder.UseGameClient()
            .SetCloseApplication(this)
            .Build();
        return base.CustomizeAppBuilder(builder)
            .WithInterFont();
    }

    public void ShunDown()
    {
        FinishAffinity();
    }
}