using VoNguyenHoangKim.Common.Constants;
using VoNguyenHoangKim.Data.Models;

namespace VoNguyenHoangKim.Data.Repositories
{
    public interface ITagRepository
    {
        Task<Tag> GetByIdAsync(string id);
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<string> GetLastIdAsync();
        Task AddAsync(Tag tag);
        Task UpdateAsync(Tag tag);
        Task DeleteAsync(string id);
    }
}