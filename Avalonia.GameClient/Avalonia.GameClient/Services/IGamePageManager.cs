using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Avalonia.GameClient.ViewModels.Interfaces;

public interface IGamePageManager
{
    IGamePage CurrentPage { get; }
}

public class GamePageManager : IGamePageManager, INotifyPropertyChanged
{
    private readonly ILoginViewModel loginViewModel;
    private readonly IGameLoadViewModel gameLoadViewModel;
    private readonly ICloseApplication closeApplication;


    public GamePageManager(ILoginViewModel loginViewModel, IGameLoadViewModel gameLoadViewModel, ICloseApplication closeApplication)
    {
        this.loginViewModel = loginViewModel;
        this.gameLoadViewModel = gameLoadViewModel;
        this.closeApplication = closeApplication;
        CurrentPage = GetGamePage<ILoginViewModel>();
        this.loginViewModel.LoginSuccess += OnLoginSuccess;
    }

    private async void OnLoginSuccess()
    {
        CurrentPage = GetGamePage<IGameLoadViewModel>();
        var res = await gameLoadViewModel.LoadAsync();
        if (!res.IsSuccess)
        {
            closeApplication.ShunDown();
            return;
        }
        CurrentPage = GetGamePage<IGameMainViewModel>();
        
    }

    public IGamePage CurrentPage 
    { 
        get => field;
        private set
        {
            field = value; 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPage)));
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    
    private static IGamePage GetGamePage<T>() => App.Services.GetRequiredKeyedService<IGamePage>(typeof(T));
}