using System.Threading.Tasks;
using GameLibrary;

namespace Avalonia.GameClient.ViewModels.Interfaces;

public interface IGameLoadViewModel
{
    Task<Result> LoadAsync();
}