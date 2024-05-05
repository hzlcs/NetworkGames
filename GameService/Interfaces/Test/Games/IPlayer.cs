namespace GameService.Interfaces.Test.Games
{
    public interface IPlayer
    {
        long PlayerId { get; }
        string ConnectionId { get; set; }
        bool LoseConnection { get; set; }
    }
}
