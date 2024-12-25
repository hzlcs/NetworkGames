using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;

namespace Avalonia.GameClient.Services;

public interface IPopupManager
{
    void Popup(string message, NotificationType type);

    IPopupManager SetTopLevel(TopLevel? topLevel);
}

public class DefaultPopupManager : IPopupManager
{
    private WindowNotificationManager manager = new()
    {
        Position = NotificationPosition.BottomRight,
        MaxItems = 3
    };
    
    public void Popup(string message, NotificationType type)
    {
        manager.Show(new Notification(type.ToString(), message, type, TimeSpan.FromSeconds(5)));
    }

    public IPopupManager SetTopLevel(TopLevel? topLevel)
    {
        manager = new WindowNotificationManager(topLevel)
        {
            Position = NotificationPosition.BottomRight,
            MaxItems = 3
        };
        return this;
    }
}