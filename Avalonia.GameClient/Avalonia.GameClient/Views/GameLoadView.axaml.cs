using Avalonia;
using Avalonia.Controls;
using Avalonia.GameClient.ViewModels.Interfaces;
using Avalonia.Markup.Xaml;

namespace Avalonia.GameClient.Views;

public partial class GameLoadView : UserControl, IGamePage
{
    public GameLoadView()
    {
        InitializeComponent();
    }
}