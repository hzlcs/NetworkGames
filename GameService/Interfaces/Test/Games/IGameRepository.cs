namespace GameService.Interfaces.Test.Games
{
    public interface IGameRepository
    {
        IGameManager GameManager { get; }
        IPlayerManager PlayerManager { get; }
    }
}
