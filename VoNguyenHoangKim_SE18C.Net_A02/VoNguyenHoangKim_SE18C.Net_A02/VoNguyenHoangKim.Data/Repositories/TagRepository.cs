using VoNguyenHoangKim.Common.Constants;
using VoNguyenHoangKim.Data.Models;
using Microsoft.EntityFrameworkCore;
using VoNguyenHoangKim.Data.Context;

namespace VoNguyenHoangKim.Data.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly FUNewsManagementDbContext _context;

        public TagRepository(FUNewsManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Tag> GetByIdAsync(string id)
        {
            return await _context.Tags.FindAsync(id);
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<string> GetLastIdAsync()
        {
            var lastTag = await _context.Tags
                .OrderByDescending(t => t.Id)
                .FirstOrDefaultAsync();
            return lastTag?.Id ?? $"{IdPrefix.Tag}00000000";
        }

        public async Task AddAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tag tag)
        {
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var tag = await GetByIdAsync(id);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
        }
    }
}