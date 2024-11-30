using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using GameLibrary.Utility;
using GameService.Abstraction;
using GameLibrary;

namespace Avalonia.GameClient.Services;

public interface ILoginManager
{
    Task<BooleanResult> LoginAsync(string userCode, string password);
    
    Task<BooleanResult> RegisterAsync(string userCode, string password);
}

public class LoginManager(IUserController userController, IMessageBox messageBox) : ILoginManager
{
    
    public async Task<BooleanResult> LoginAsync(string userCode, string password)
    {
        CancellationTokenSource cts = new (3000);
        try
        {
            var res = await userController.Login(userCode, password, cts.Token);
            if (res?.Code == 0)
            {
                messageBox.Popup("登录成功", NotificationType.Success);
                return BooleanResult.True();
            }
            messageBox.Popup(res?.Message ?? "Unknown error", NotificationType.Warning);
            return BooleanResult.False();
        }
        catch (Exception e)
        {
            messageBox.Popup(e.Message, NotificationType.Error);
            return BooleanResult.Fail(e);
        }
        finally
        {
            cts.Dispose();
        }
    }

    public async Task<BooleanResult> RegisterAsync(string userCode, string password)
    {
        CancellationTokenSource cts = new (3000);
        try
        {
            var res = await userController.Register(userCode, password, cts.Token);
            if (res?.Code == 0)
            {
                messageBox.Popup("注册成功", NotificationType.Success);
                return BooleanResult.True();
            }
            messageBox.Popup(res?.Message ?? "Unknown error", NotificationType.Warning);
            return BooleanResult.False();
        }
        catch (Exception e)
        {
            messageBox.Popup(e.Message, NotificationType.Error);
            return BooleanResult.Fail(e);
        }
        finally
        {
            cts.Dispose();
        }
    }
}