using VoNguyenHoangKim.Common.Constants;
using VoNguyenHoangKim.Data.Models;
using Microsoft.EntityFrameworkCore;
using VoNguyenHoangKim.Data.Context;

namespace VoNguyenHoangKim.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FUNewsManagementDbContext _context;

        public AccountRepository(FUNewsManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Account> GetByIdAsync(string id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<Account> GetByEmailAsync(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<string> GetLastIdAsync()
        {
            var lastAccount = await _context.Accounts
                .OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync();
            return lastAccount?.Id ?? $"{IdPrefix.Account}00000000";
        }

        public async Task AddAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var account = await GetByIdAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }
    }
}
