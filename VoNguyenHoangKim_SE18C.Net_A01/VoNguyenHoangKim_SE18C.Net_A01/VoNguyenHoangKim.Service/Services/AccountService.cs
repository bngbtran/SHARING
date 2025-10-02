using VoNguyenHoangKim.Common.Constants;
using VoNguyenHoangKim.Common.Validate;
using VoNguyenHoangKim.Data.Models;
using VoNguyenHoangKim.Data.Repositories;
using VoNguyenHoangKim.Service.DTOs;
using VoNguyenHoangKim.Service.ValidateReceive;
using VoNguyenHoangKim.Service.ValidateSend;

namespace VoNguyenHoangKim.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly IdGenerator _idGenerator;

        public AccountService(IAccountRepository accountRepository, INewsArticleRepository newsArticleRepository, IdGenerator idGenerator)
        {
            _accountRepository = accountRepository;
            _newsArticleRepository = newsArticleRepository;
            _idGenerator = idGenerator;
        }

        public async Task<AccountResponse> GetByIdAsync(string id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                return new AccountResponse { Success = false, Error = "Not Found !!!" };
            }

            var accountDTO = new AccountDTO
            {
                Id = account.Id,
                Email = account.Email,
                FullName = account.FullName,
                Role = account.Role
            };

            return new AccountResponse { Success = true, Account = accountDTO };
        }

        public async Task<AccountResponse> GetByEmailAsync(string email)
        {
            var account = await _accountRepository.GetByEmailAsync(email);
            if (account == null)
            {
                return new AccountResponse { Success = false, Error = "Not Found !!!" };
            }

            var accountDTO = new AccountDTO
            {
                Id = account.Id,
                Email = account.Email,
                FullName = account.FullName,
                Role = account.Role,
                Password = account.Password
            };

            return new AccountResponse { Success = true, Account = accountDTO };
        }

        public async Task<IEnumerable<AccountDTO>> GetAllAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return accounts.Select(a => new AccountDTO
            {
                Id = a.Id,
                Email = a.Email,
                FullName = a.FullName,
                Role = a.Role
            });
        }

        public async Task<AccountResponse> CreateAsync(AccountCreateRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.FullName))
            {
                return new AccountResponse { Success = false, Error = "Invalid Input !!!" };
            }

            var existingAccount = await _accountRepository.GetByEmailAsync(request.Email);
            if (existingAccount != null)
            {
                return new AccountResponse { Success = false, Error = "Duplicate Email !!!" };
            }

            var lastId = await _accountRepository.GetLastIdAsync();
            var newId = _idGenerator.GenerateId(IdPrefix.Account, lastId);

            var account = new Account
            {
                Id = newId,
                Email = request.Email,
                Password = request.Password,
                FullName = request.FullName,
                Role = request.Role
            };

            await _accountRepository.AddAsync(account);

            var accountDTO = new AccountDTO
            {
                Id = account.Id,
                Email = account.Email,
                FullName = account.FullName,
                Role = account.Role
            };

            return new AccountResponse { Success = true, Account = accountDTO };
        }

        public async Task<AccountResponse> UpdateAsync(string id, AccountEditRequest request)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                return new AccountResponse { Success = false, Error = "Not Found !!!" };
            }

            account.Email = request.Email;
            account.Password = request.Password ?? account.Password;
            account.FullName = request.FullName;
            account.Role = request.Role;

            await _accountRepository.UpdateAsync(account);

            var accountDTO = new AccountDTO
            {
                Id = account.Id,
                Email = account.Email,
                FullName = account.FullName,
                Role = account.Role
            };

            return new AccountResponse { Success = true, Account = accountDTO };
        }

        public async Task<AccountResponse> DeleteAsync(string id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                return new AccountResponse { Success = false, Error = "Not Found !!!" };
            }

            var articles = await _newsArticleRepository.GetByAccountIdAsync(id);
            if (articles.Any())
            {
                return new AccountResponse
                {
                    Success = false,
                    Error = "Staff Is In Use !!!"
                };
            }

            await _accountRepository.DeleteAsync(id);
            return new AccountResponse { Success = true };
        }
    }
}