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
    public void Popup(string message, NotificationType type)
    {
        Debug.WriteLine($"{type}: {message}");
    }

    public IPopupManager SetTopLevel(TopLevel? topLevel)
    {
        return this;
    }
}