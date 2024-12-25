using Avalonia;
using Avalonia.Controls;
using Avalonia.GameClient.ViewModels.Interfaces;
using Avalonia.Markup.Xaml;

namespace Avalonia.GameClient.Views;

public partial class GameMainView : UserControl, IGamePage
{
    public GameMainView()
    {
        InitializeComponent();
    }
}