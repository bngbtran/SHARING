using VoNguyenHoangKim.Data.Models;

namespace VoNguyenHoangKim.Data.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> GetByIdAsync(string id);
        Task<Account> GetByEmailAsync(string email);
        Task<IEnumerable<Account>> GetAllAsync();
        Task<string> GetLastIdAsync();
        Task AddAsync(Account account);
        Task UpdateAsync(Account account);
        Task DeleteAsync(string id);
    }
}