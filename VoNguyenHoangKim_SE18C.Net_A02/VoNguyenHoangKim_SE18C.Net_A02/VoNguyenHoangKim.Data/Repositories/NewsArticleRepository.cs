using VoNguyenHoangKim.Common.Constants;
using VoNguyenHoangKim.Data.Models;
using Microsoft.EntityFrameworkCore;
using VoNguyenHoangKim.Data.Context;

namespace VoNguyenHoangKim.Data.Repositories
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly FUNewsManagementDbContext _context;

        public NewsArticleRepository(FUNewsManagementDbContext context)
        {
            _context = context;
        }

        public async Task<NewsArticle?> GetByIdAsync(string id)
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.Account)
                .Include(n => n.UpdatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<IEnumerable<NewsArticle>> GetAllAsync()
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.Account)
                .Include(n => n.UpdatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .ToListAsync();
        }

        public async Task<string> GetLastIdAsync()
        {
            var lastArticle = await _context.NewsArticles
                .OrderByDescending(n => n.Id)
                .FirstOrDefaultAsync();
            return lastArticle?.Id ?? $"{IdPrefix.NewsArticle}00000000";
        }

        public async Task AddAsync(NewsArticle newsArticle)
        {
            await _context.NewsArticles.AddAsync(newsArticle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NewsArticle newsArticle)
        {
            _context.NewsArticles.Update(newsArticle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var newsArticle = await GetByIdAsync(id);
            if (newsArticle != null)
            {
                _context.NewsArticles.Remove(newsArticle);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetCountByCategoryAsync(string categoryId)
        {
            return await _context.NewsArticles.CountAsync(na => na.CategoryId == categoryId);
        }

        public async Task<IEnumerable<NewsArticle>> GetByAccountIdAsync(string accountId)
        {
            return await _context.NewsArticles
                .Where(n => n.AccountId == accountId)
                .ToListAsync();
        }
    }
}