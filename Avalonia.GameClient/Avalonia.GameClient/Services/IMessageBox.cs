using Avalonia.Controls.Notifications;

namespace Avalonia.GameClient.Services;

public interface IMessageBox
{
    void Popup(string message, NotificationType type);
}

public class MessageBox(IPopupManager popupManager) : IMessageBox
{
    public void Popup(string message, NotificationType type)
    {
        popupManager.Popup(message, type);
    }
}