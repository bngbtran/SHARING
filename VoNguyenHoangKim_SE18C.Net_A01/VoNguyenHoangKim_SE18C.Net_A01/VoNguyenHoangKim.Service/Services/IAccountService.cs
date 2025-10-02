using VoNguyenHoangKim.Service.DTOs;
using VoNguyenHoangKim.Service.ValidateReceive;
using VoNguyenHoangKim.Service.ValidateSend;

namespace VoNguyenHoangKim.Service.Services
{
    public interface IAccountService
    {
        Task<AccountResponse> GetByIdAsync(string id);
        Task<AccountResponse> GetByEmailAsync(string email);
        Task<IEnumerable<AccountDTO>> GetAllAsync();
        Task<AccountResponse> CreateAsync(AccountCreateRequest request);
        Task<AccountResponse> UpdateAsync(string id, AccountEditRequest request);
        Task<AccountResponse> DeleteAsync(string id);
    }
}