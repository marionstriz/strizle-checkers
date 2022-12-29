namespace DAL;

public interface IPlayerDbRepository : IDbRepository<DTO.Player>
{
    Task<DTO.Player?> GetByNameLazyAsync(string name);
}