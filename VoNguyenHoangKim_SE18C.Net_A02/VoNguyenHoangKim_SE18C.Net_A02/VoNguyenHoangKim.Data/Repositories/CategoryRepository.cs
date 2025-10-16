using VoNguyenHoangKim.Common.Constants;
using VoNguyenHoangKim.Data.Models;
using Microsoft.EntityFrameworkCore;
using VoNguyenHoangKim.Data.Context;

namespace VoNguyenHoangKim.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FUNewsManagementDbContext _context;

        public CategoryRepository(FUNewsManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Category> GetByIdAsync(string id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<string> GetLastIdAsync()
        {
            var lastCategory = await _context.Categories
                .OrderByDescending(c => c.Id)
                .FirstOrDefaultAsync();
            return lastCategory?.Id ?? $"{IdPrefix.Category}00000000";
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var category = await GetByIdAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> HasNewsArticlesAsync(string id)
        {
            return await _context.NewsArticles.AnyAsync(n => n.CategoryId == id);
        }
    }
}