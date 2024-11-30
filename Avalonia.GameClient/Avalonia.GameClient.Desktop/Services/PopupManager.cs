using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.GameClient.Services;

namespace Avalonia.GameClient.Desktop.Services;

public class PopupManager : IPopupManager
{
    public PopupManager(TopLevel topLevel)
    {
        SetTopLevel(topLevel);
    }
    
    public PopupManager(){}
    
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
        if (topLevel is not null)
            manager = new WindowNotificationManager(topLevel)
            {
                Position = NotificationPosition.BottomRight,
                MaxItems = 3
            };
        return this;
    }
}