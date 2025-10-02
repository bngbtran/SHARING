using VoNguyenHoangKim.Data.Models;

namespace VoNguyenHoangKim.Data.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(string id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<string> GetLastIdAsync();
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(string id);
        Task<bool> HasNewsArticlesAsync(string id);
    }
}