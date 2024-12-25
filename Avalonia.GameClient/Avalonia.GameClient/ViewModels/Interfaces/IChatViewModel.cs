using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using GameLibrary.Network;

namespace Avalonia.GameClient.ViewModels.Interfaces;

public interface IChatViewModel
{
    ObservableCollection<ChatMessage> Messages { get; }
    
    string? Message { get; set; }
    
    IAsyncRelayCommand SendMessageCommand { get; }
    
    IRelayCommand ClearCommand { get; }
}