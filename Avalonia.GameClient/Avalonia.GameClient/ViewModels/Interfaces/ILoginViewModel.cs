using System;
using CommunityToolkit.Mvvm.Input;

namespace Avalonia.GameClient.ViewModels.Interfaces;

public interface ILoginViewModel
{
    string UserCode { get; set; }
    string Password { get; set; }
    IAsyncRelayCommand LoginCommand { get; } 
    IAsyncRelayCommand RegisterCommand { get; }
    
    event Action LoginSuccess;
}