using GameService.Interfaces.Test.Games;

namespace GameService.Entity.Test.Games
{
    public class GameRepository : IGameRepository
    {
        private readonly PlayerManager playerManager = new();

        public IGameManager GameManager => throw new NotImplementedException();

        public IPlayerManager PlayerManager => playerManager;
    }
}
