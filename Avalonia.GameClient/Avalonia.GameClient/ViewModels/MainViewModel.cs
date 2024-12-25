using Avalonia.GameClient.ViewModels.Interfaces;

namespace Avalonia.GameClient.ViewModels;

[ViewModel]
public class MainViewModel(IGamePageManager gamePageManager) : ViewModelBase
{
    public IGamePageManager GamePageManager { get; } = gamePageManager;
}