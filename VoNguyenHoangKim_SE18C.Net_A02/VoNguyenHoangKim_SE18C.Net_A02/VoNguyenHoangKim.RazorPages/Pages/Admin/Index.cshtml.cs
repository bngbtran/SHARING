using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VoNguyenHoangKim.Service.Services;
using VoNguyenHoangKim.Service.ValidateReceive;

namespace VoNguyenHoangKim.RazorPages.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;

        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IEnumerable<AccountResponse> Accounts { get; set; }

        public async Task OnGetAsync(string search)
        {
            var accountsDto = await _accountService.GetAllAsync();

            if (!string.IsNullOrEmpty(search))
            {
                accountsDto = accountsDto.Where(a =>
                    a.Email.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    a.FullName.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            Accounts = accountsDto.Select(a => new AccountResponse
            {
                Success = true,
                Account = a,
                Error = null
            }).ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            await _accountService.DeleteAsync(id);
            return RedirectToPage("/Admin/Index");
        }
    }
}