using System;
using System.Threading.Tasks;
using Avalonia.GameClient.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using GameLibrary;

namespace Avalonia.GameClient.ViewModels;

[ViewModel]
public partial class GameLoadViewModel : ViewModelBase, IGameLoadViewModel
{
    [ObservableProperty]
    private bool visible;
    
    [ObservableProperty]
    private double value;

    [ObservableProperty]
    private double minimum;
    
    [ObservableProperty]
    private double maximum;

    public async Task<Result> LoadAsync()
    {
        Minimum = 0;
        Maximum = 20;
        for (int i = 0; i < 20; ++i)
        {
            Value += 1;
            await Task.Delay(100);
        }

        return Result.Success();
    }
}