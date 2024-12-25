using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.GameClient.Hubs;
using Avalonia.GameClient.Services;
using Avalonia.GameClient.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameLibrary.Network;

namespace Avalonia.GameClient.ViewModels;

[ViewModel]
public partial class ChatViewModel : ViewModelBase, IChatViewModel
{
    private readonly IChatHubClient chatHubClient;
    private readonly IUserManager userManager;

    public ChatViewModel(IChatHubClient chatHubClient, IUserManager userManager)
    {
        this.chatHubClient = chatHubClient;
        this.userManager = userManager;
        chatHubClient.MessageReceived += MessageReceived;
    }

    private void MessageReceived(ChatMessage[] objs)
    {
        foreach (var obj in objs)
        {
            Messages.Add(obj);
        }
    }

    public ObservableCollection<ChatMessage> Messages { get; } = [];

    [ObservableProperty] private string? message;

    [RelayCommand]
    private async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(Message))
        {
            return;
        }
        Debug.Assert(!string.IsNullOrEmpty(userManager.CurrentUser.UserName));
        var msg = new ChatMessage(userManager.CurrentUser.UserName, Message, DateTime.Now);
        //Messages.Add(msg);
        await chatHubClient.SendMessageAsync(new ChatMessage(userManager.CurrentUser.UserName, Message, DateTime.Now));
        Message = string.Empty;
    }
    
    [RelayCommand]
    private void Clear()
    {
        Messages.Clear(); 
    }
}