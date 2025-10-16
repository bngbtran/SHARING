using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VoNguyenHoangKim.Service.Services;

namespace VoNguyenHoangKim.RazorPages.Pages.Account
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;

        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IList<VoNguyenHoangKim.Service.DTOs.AccountDTO> Accounts { get; set; }

        public async Task OnGetAsync()
        {
            Accounts = (IList<VoNguyenHoangKim.Service.DTOs.AccountDTO>)await _accountService.GetAllAsync();
        }
    }
}