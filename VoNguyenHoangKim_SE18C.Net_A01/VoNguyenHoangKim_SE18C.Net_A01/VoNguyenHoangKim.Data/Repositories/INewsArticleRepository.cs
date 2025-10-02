using VoNguyenHoangKim.Common.Constants;
using VoNguyenHoangKim.Data.Models;

namespace VoNguyenHoangKim.Data.Repositories
{
    public interface INewsArticleRepository
    {
        Task<NewsArticle?> GetByIdAsync(string id);
        Task<IEnumerable<NewsArticle>> GetAllAsync();
        Task<string> GetLastIdAsync();
        Task AddAsync(NewsArticle newsArticle);
        Task UpdateAsync(NewsArticle newsArticle);
        Task DeleteAsync(string id);
        Task<int> GetCountByCategoryAsync(string categoryId);
        Task<IEnumerable<NewsArticle>> GetByAccountIdAsync(string accountId);
    }
}