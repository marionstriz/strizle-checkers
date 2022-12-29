namespace DAL;

public interface IGameDbRepository : IGameRepository, IDbRepository<DTO.CheckersGame>
{
    Task SaveBrainGameAsync(GameBrain.CheckersGame game);
    Task SaveBrainGameByNameAsync(GameBrain.CheckersGame game, string name);
    IStateDbRepository GetStateRepository();
    IPlayerDbRepository GetPlayerRepository();
    IOptionsDbRepository GetOptionsRepository();
    Task<DTO.CheckersGame?> GetByNameLazyAsync(string name);
    Task<DTO.CheckersGame?> GetByIdLazyAsync(int id);
}