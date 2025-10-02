using VoNguyenHoangKim.Data.Models;

namespace VoNguyenHoangKim.Data.Repositories
{
    public interface INewsTagRepository
    {
        Task<NewsTag> GetByIdAsync(string id);
        Task<IEnumerable<NewsTag>> GetByNewsArticleIdAsync(string newsArticleId);
        Task<string> GetLastIdAsync();
        Task AddAsync(NewsTag newsTag);
        Task UpdateAsync(NewsTag newsTag);
        Task DeleteAsync(string id);
        Task<int> GetCountByTagAsync(string tagId);
        Task<IEnumerable<NewsTag>> GetAllAsync();
    }
}