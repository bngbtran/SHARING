using DataAccess.Models;

public interface IFruitsService
{
    Task<List<Fruits>> GetAllFruitsAsync();
    Task<Fruits?> GetFruitByIdAsync(int id);
    Task AddFruitAsync(Fruits fruit);
    Task UpdateFruitAsync(Fruits fruit);
    Task DeleteFruitAsync(int id);
    Task<List<Fruits>> SearchFruitsAsync(string keyword);
    Task RestoreFruitAsync(int id);
    Task<List<Fruits>> GetDeletedFruitsAsync();
    Task HardDeleteFruitAsync(int id);
}
