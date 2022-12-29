using GameBrain;

namespace DAL;

public interface IOptionsDbRepository : IDbRepository<DTO.CheckersGameOptions>
{
    Task<DTO.CheckersGameOptions?> GetByOptionsAsync(GameOptions options);
}