using VoNguyenHoangKim.Common.Constants;
using VoNguyenHoangKim.Data.Models;
using Microsoft.EntityFrameworkCore;
using VoNguyenHoangKim.Data.Context;

namespace VoNguyenHoangKim.Data.Repositories
{
    public class NewsTagRepository : INewsTagRepository
    {
        private readonly FUNewsManagementDbContext _context;

        public NewsTagRepository(FUNewsManagementDbContext context)
        {
            _context = context;
        }

        public async Task<NewsTag> GetByIdAsync(string id)
        {
            return await _context.NewsTags.FindAsync(id);
        }

        public async Task<IEnumerable<NewsTag>> GetByNewsArticleIdAsync(string newsArticleId)
        {
            return await _context.NewsTags
                .Where(nt => nt.NewsArticleId == newsArticleId)
                .ToListAsync();
        }

        public async Task<string> GetLastIdAsync()
        {
            var lastNewsTag = await _context.NewsTags
                .OrderByDescending(nt => nt.Id)
                .FirstOrDefaultAsync();
            return lastNewsTag?.Id ?? $"{IdPrefix.NewsTag}00000000";
        }

        public async Task AddAsync(NewsTag newsTag)
        {
            await _context.NewsTags.AddAsync(newsTag);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NewsTag newsTag)
        {
            _context.NewsTags.Update(newsTag);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var newsTag = await GetByIdAsync(id);
            if (newsTag != null)
            {
                _context.NewsTags.Remove(newsTag);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetCountByTagAsync(string tagId)
        {
            return await _context.NewsTags.CountAsync(nt => nt.TagId == tagId);
        }

        public async Task<IEnumerable<NewsTag>> GetAllAsync()
        {
            return await _context.NewsTags.ToListAsync();
        }
    }
}