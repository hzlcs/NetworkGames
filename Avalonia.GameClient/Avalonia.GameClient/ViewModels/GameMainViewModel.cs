using Avalonia.GameClient.ViewModels.Interfaces;

namespace Avalonia.GameClient.ViewModels;

[ViewModel]
public class GameMainViewModel(IChatViewModel chatViewModel) : ViewModelBase, IGameMainViewModel
{
    public IChatViewModel ChatViewModel { get; } = chatViewModel;
}