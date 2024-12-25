using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.GameClient.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Avalonia.GameClient.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        App.Services.GetRequiredService<IPopupManager>().SetTopLevel(TopLevel.GetTopLevel(this));
    }
}