namespace DAL;

public interface IDbRepository<T>
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task DeleteByIdAsync(int id);
    Task AddAsync(T obj);
    Task SaveAsync(T obj);
}